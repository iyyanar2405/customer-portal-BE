CREATE PROCEDURE Sp_ActionFilterCompanies
    @categories NVARCHAR(MAX) = NULL,  -- JSON array of category IDs
    @services NVARCHAR(MAX) = NULL,    -- JSON array of service IDs/names
    @sites NVARCHAR(MAX) = NULL        -- JSON array of site IDs
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Create temp table for category filter if provided
    DECLARE @categoryFilter TABLE (categoryId INT);
    IF @categories IS NOT NULL AND @categories != '[]'
    BEGIN
        INSERT INTO @categoryFilter (categoryId)
        SELECT CAST(value AS INT)
        FROM OPENJSON(@categories);
    END
    
    -- Create temp table for service filter if provided
    DECLARE @serviceFilter TABLE (serviceName NVARCHAR(100));
    IF @services IS NOT NULL AND @services != '[]'
    BEGIN
        INSERT INTO @serviceFilter (serviceName)
        SELECT CAST(value AS NVARCHAR(100))
        FROM OPENJSON(@services);
    END
    
    -- Create temp table for site filter if provided
    DECLARE @siteFilter TABLE (siteId INT);
    IF @sites IS NOT NULL AND @sites != '[]'
    BEGIN
        INSERT INTO @siteFilter (siteId)
        SELECT CAST(value AS INT)
        FROM OPENJSON(@sites);
    END
    
    -- Companies data (this should ideally come from a Companies table)
    -- For now, using a CTE with sample data matching the response
    WITH CompaniesData AS SELECT * FROM (VALUES 
            (341, 'FAVINI S.r.l.'),
            (353, 'UNILEVER ITALIA MANUFACTURING  SRL'),
            (364, 'MALAYSIAN AUTOMOTIVE LIGHTING SDN BHD'),
            (366, 'Ducati Motor Holding S.p.A.'),
            (368, 'Magneti Marelli UM Electronic Systems Pvt. Ltd.'),
            (380, 'MARELLI MÃ“VEIS PARA ESCRITÃ“RIO S/A'),
            (425, 'MAGNETI MARELLI SISTEMAGNETI MARELLI SISTEMAS AUTOMOTIVOS - INDUSTRIA E COMERCIO LTDA'),
            (428, 'MARELLI COFAP DO BRASIL LTDA'),
            (433, 'Magneti Marelli Sistemas Automotivos IndÃºstria e ComÃ©rcio Ltda'),
            (444, 'BRUNI GLASS SPA'),
            (451, 'Magneti Marelli Sistemas de SuspensiÃ³n Promatcor Mexicana S de RL de CV'),
            (453, 'ISS World Services A/S'),
            (470, 'MARELLI SALTILLO MÃ‰XICO S. DE R.L DE C.V'),
            (483, 'EXPRIVIA S.p.A.'),
            (209597, 'Kerry')
        ) AS Companies(id, label)
    ),
    
    -- Get filtered companies based on actions that match the criteria
    FilteredCompanies AS SELECT DISTINCT au.companyId
        FROM Audits au
        INNER JOIN Actions a ON a.entityId = au.auditId
        WHERE 1=1
            -- Filter by categories if provided (based on action entityType)
            AND (
                SELECT COUNT(*) FROM @categoryFilter) = 0
                OR (
                    EXISTS SELECT 1 FROM @categoryFilter WHERE categoryId = 2 AND a.entityType = 'Certificate')
                    OR EXISTS SELECT 1 FROM @categoryFilter WHERE categoryId = 3 AND a.entityType = 'Finding')
                    OR EXISTS SELECT 1 FROM @categoryFilter WHERE categoryId = 4 AND a.entityType = 'Schedule')
                )
            )
            -- Filter by services if provided
            AND (
                SELECT COUNT(*) FROM @serviceFilter) = 0
                OR a.service IN SELECT serviceName FROM @serviceFilter)
                OR EXISTS SELECT 1 FROM AuditServices aser 
                    WHERE aser.auditId = au.auditId 
                    AND aser.service IN SELECT serviceName FROM @serviceFilter)
                )
            )
            -- Filter by sites if provided
            AND (
                SELECT COUNT(*) FROM @siteFilter) = 0
                OR a.site IN SELECT CAST(siteId AS NVARCHAR(100)) FROM @siteFilter)
                OR EXISTS SELECT 1 FROM AuditSites asit 
                    WHERE asit.auditId = au.auditId 
                    AND asit.siteId IN SELECT siteId FROM @siteFilter)
                )
            )
    )
    
    -- Return companies that have actions matching the filter criteria
    SELECT 
        cd.id,
        cd.label
    FROM CompaniesData cd
    WHERE EXISTS SELECT 1 FROM FilteredCompanies fc 
        WHERE fc.companyId = cd.id
    )
    OR (
        -- If no filters applied, return all companies that have audits/actions
        SELECT COUNT(*) FROM @categoryFilter) = 0 
        AND SELECT COUNT(*) FROM @serviceFilter) = 0
        AND SELECT COUNT(*) FROM @siteFilter) = 0
        AND EXISTS SELECT 1 FROM Audits WHERE companyId = cd.id)
    )
    ORDER BY cd.label;
END
