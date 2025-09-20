CREATE PROCEDURE Sp_ActionFilterSites
    @companies NVARCHAR(MAX) = NULL,   -- JSON array of company IDs
    @categories NVARCHAR(MAX) = NULL,  -- JSON array of category IDs
    @services NVARCHAR(MAX) = NULL     -- JSON array of service IDs
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
    
    -- Create temp table for service filter if provided
    DECLARE @serviceFilter TABLE (serviceName NVARCHAR(100));
    IF @services IS NOT NULL AND @services != '[]'
    BEGIN
        INSERT INTO @serviceFilter (serviceName)
        SELECT CAST(value AS NVARCHAR(100))
        FROM OPENJSON(@services);
    END
    
    -- Get filtered sites based on actions that match the criteria
    WITH FilteredSites AS SELECT DISTINCT 
            COALESCE(a.site, CAST(asit.siteId AS NVARCHAR(100))) AS siteName,
            asit.siteId
        FROM Audits au
        LEFT JOIN Actions a ON a.entityId = au.auditId
        LEFT JOIN AuditSites asit ON asit.auditId = au.auditId
        LEFT JOIN AuditServices aser ON aser.auditId = au.auditId
        WHERE 1=1
            AND (a.site IS NOT NULL OR asit.siteId IS NOT NULL)
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
            -- Filter by services if provided
            AND (
                SELECT COUNT(*) FROM @serviceFilter) = 0
                OR a.service IN SELECT serviceName FROM @serviceFilter)
                OR aser.service IN SELECT serviceName FROM @serviceFilter)
            )
    )
    
    -- Sample hierarchical data structure (this should be replaced with dynamic generation)
    -- In production, this would query from proper Countries, Cities, Sites tables
    -- and filter based on the FilteredSites CTE
    
    DECLARE @hasFilters BIT = 0;
    IF SELECT COUNT(*) FROM @companyFilter) > 0 
       OR SELECT COUNT(*) FROM @categoryFilter) > 0 
       OR SELECT COUNT(*) FROM @serviceFilter) > 0
        SET @hasFilters = 1;
    
    -- For now, return a subset of the hierarchical structure
    -- In production, you would dynamically build this based on filtered results
    SELECT 
                countryId as id,
                countryName as label,
                SELECT cityId as id,
                        cityName as label,
                        SELECT siteId as id,
                                siteName as label
                            FROM (VALUES 
                                (197294, 'INGRECOR S.A. - Molienda HÃºmeda TucumÃ¡n Planta II'),
                                (273539, 'Papel Misionero S.A.I.F.C.- Planta San Luis'),
                                (171089, 'MARELLI SISTEMAS AUTOMOTIVOS INDUSTRIA E COMERCIO BRASIL LTDA'),
                                (171123, 'MARELLI MÃ“VEIS PARA ESCRITÃ“RIO S/A'),
                                (173070, 'Ducati Motor Holding S.p.A.'),
                                (172446, 'EXPRIVIA S.p.A.'),
                                (186183, 'Kerry'),
                                (184938, 'Kerry')
                            ) AS Sites(siteId, siteName)
                            WHERE (@hasFilters = 0 OR EXISTS SELECT 1 FROM FilteredSites fs WHERE fs.siteId = Sites.siteId))
                        ) as children
                    FROM (VALUES 
                        (1, 'San Isidro de Lules'),
                        (1, 'San Luis'),
                        (1, 'Amparo'),
                        (1, 'Caxias do Sul'),
                        (1, 'Bologna'),
                        (1, 'Milano'),
                        (1, 'Albany'),
                        (1, 'Chicago')
                    ) AS Cities(cityId, cityName)
                ) as children
            FROM (VALUES 
                (265, 'Argentina'),
                (285, 'Brazil'),
                (357, 'Italy'),
                (255, 'USA')
            ) AS Countries(countryId, countryName)
        CAST(1 AS BIT) AS isSuccess,
        'Success' AS message;
END

