CREATE PROCEDURE Sp_ActionFilterServices
    @companies NVARCHAR(MAX) = NULL,   -- JSON array of company IDs
    @categories NVARCHAR(MAX) = NULL,  -- JSON array of category IDs
    @sites NVARCHAR(MAX) = NULL        -- JSON array of site IDs
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Create temp table for company filter if provided
    DECLARE @companyFilter TABLE (companyId INT);
    IF @companies IS NOT NULL AND @companies != '[]'
    BEGIN
        INSERT INTO @companyFilter (companyId)
        SELECT CAST(value AS INT)
        FROM OPENJSON(@companies);
    END
    
    -- Create temp table for category filter if provided
    DECLARE @categoryFilter TABLE (categoryId INT);
    IF @categories IS NOT NULL AND @categories != '[]'
    BEGIN
        INSERT INTO @categoryFilter (categoryId)
        SELECT CAST(value AS INT)
        FROM OPENJSON(@categories);
    END
    
    -- Create temp table for site filter if provided
    DECLARE @siteFilter TABLE (siteId INT);
    IF @sites IS NOT NULL AND @sites != '[]'
    BEGIN
        INSERT INTO @siteFilter (siteId)
        SELECT CAST(value AS INT)
        FROM OPENJSON(@sites);
    END
    
    -- Services data (this should ideally come from a Services table)
    -- For now, using a CTE with sample data matching the response
    WITH ServicesData AS SELECT * FROM (VALUES 
            (162, 'ISO 20121:2012'),
            (780, 'ISO 9001:2015'),
            (1060, 'ISO 14001:2015'),
            (1184, 'ISO 45001:2018'),
            (1389, 'SA 8000:2014'),
            (1867, 'GLOBAL STANDARD FOR STORAGE AND DISTRIBUTION Issue 4: November 2020'),
            (1887, 'SQF Food Safety Code: Food Manufacturing, Edition 9'),
            (1899, 'SQF Quality Code, Edition 9'),
            (2055, 'IFS Logistics Version 3 December 2023'),
            (2137, 'FSC-STD-40-004 V3-1 Chain of Custody Certification'),
            (2155, 'ISO 50001:2018')
        ) AS Services(id, label)
    ),
    
    -- Get filtered services based on actions that match the criteria
    FilteredServices AS SELECT DISTINCT 
            COALESCE(a.service, aser.service) AS serviceName
        FROM Audits au
        LEFT JOIN Actions a ON a.entityId = au.auditId
        LEFT JOIN AuditServices aser ON aser.auditId = au.auditId
        WHERE 1=1
            AND (a.service IS NOT NULL OR aser.service IS NOT NULL)
            -- Filter by companies if provided
            AND (
                SELECT COUNT(*) FROM @companyFilter) = 0
                OR au.companyId IN SELECT companyId FROM @companyFilter)
            )
            -- Filter by categories if provided (based on action entityType)
            AND (
                SELECT COUNT(*) FROM @categoryFilter) = 0
                OR (
                    EXISTS SELECT 1 FROM @categoryFilter WHERE categoryId = 2 AND a.entityType = 'Certificate')
                    OR EXISTS SELECT 1 FROM @categoryFilter WHERE categoryId = 3 AND a.entityType = 'Finding')
                    OR EXISTS SELECT 1 FROM @categoryFilter WHERE categoryId = 4 AND a.entityType = 'Schedule')
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
    
    -- Return services that have actions matching the filter criteria
    SELECT 
        sd.id,
        sd.label
    FROM ServicesData sd
    WHERE EXISTS SELECT 1 FROM FilteredServices fs 
        WHERE fs.serviceName = sd.label
           OR fs.serviceName LIKE '%' + sd.label + '%'
           OR sd.label LIKE '%' + fs.serviceName + '%'
    )
    OR (
        -- If no filters applied, return all services that have audits/actions
        SELECT COUNT(*) FROM @companyFilter) = 0 
        AND SELECT COUNT(*) FROM @categoryFilter) = 0
        AND SELECT COUNT(*) FROM @siteFilter) = 0
        AND (
            EXISTS SELECT 1 FROM Actions a 
                WHERE a.service = sd.label
                   OR a.service LIKE '%' + sd.label + '%'
                   OR sd.label LIKE '%' + a.service + '%'
            )
            OR EXISTS SELECT 1 FROM AuditServices aser 
                WHERE aser.service = sd.label
                   OR aser.service LIKE '%' + sd.label + '%'
                   OR sd.label LIKE '%' + aser.service + '%'
            )
        )
    )
    ORDER BY sd.label;
END
