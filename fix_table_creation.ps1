# PowerShell script to execute remaining table creation with proper dependency handling
$serverName = "DESKTOP-6A76F24\MSSQLSERVER1"
$databaseName = "Customer_PortalI"

Write-Host "Starting remaining table creation process..." -ForegroundColor Green

# First, let's create Companies table without FK constraints
Write-Host "Creating Companies table without FK constraints..." -ForegroundColor Yellow

$companiesCreateScript = @"
-- Companies table for customer organizations (without FK constraints first)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Companies]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Companies]
    (
        [CompanyId] INT IDENTITY(1,1) NOT NULL,
        [CompanyName] NVARCHAR(200) NOT NULL,
        [CompanyCode] NVARCHAR(50) NOT NULL,
        [Description] NVARCHAR(500) NULL,
        [Address] NVARCHAR(255) NULL,
        [CityId] INT NULL,
        [CountryId] INT NULL,
        [PostalCode] NVARCHAR(20) NULL,
        [Phone] NVARCHAR(20) NULL,
        [Email] NVARCHAR(255) NULL,
        [Website] NVARCHAR(255) NULL,
        [ContactPerson] NVARCHAR(100) NULL,
        [ContactEmail] NVARCHAR(255) NULL,
        [ContactPhone] NVARCHAR(20) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [ModifiedDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [CreatedBy] INT NULL,
        [ModifiedBy] INT NULL,
        [Industry] NVARCHAR(100) NULL,
        [EmployeeCount] INT NULL,
        [TaxId] NVARCHAR(50) NULL,
        [RegistrationNumber] NVARCHAR(50) NULL,
        [LogoUrl] NVARCHAR(500) NULL,

        CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED ([CompanyId]),
        CONSTRAINT [UK_Companies_CompanyCode] UNIQUE ([CompanyCode]),
        CONSTRAINT [UK_Companies_CompanyName] UNIQUE ([CompanyName])
    );

    -- Indexes for performance
    CREATE NONCLUSTERED INDEX [IX_Companies_CompanyName] ON [dbo].[Companies] ([CompanyName]);
    CREATE NONCLUSTERED INDEX [IX_Companies_CompanyCode] ON [dbo].[Companies] ([CompanyCode]);
    CREATE NONCLUSTERED INDEX [IX_Companies_IsActive] ON [dbo].[Companies] ([IsActive]);
    CREATE NONCLUSTERED INDEX [IX_Companies_CountryId] ON [dbo].[Companies] ([CountryId]);
    CREATE NONCLUSTERED INDEX [IX_Companies_CityId] ON [dbo].[Companies] ([CityId]);
    
    PRINT 'Companies table created successfully without FK constraints'
END
ELSE
BEGIN
    PRINT 'Companies table already exists'
END
"@

try {
    $companiesCreateScript | sqlcmd -S $serverName -d $databaseName -E
    Write-Host "Companies table created successfully" -ForegroundColor Green
}
catch {
    Write-Host "Error creating Companies table: $($_.Exception.Message)" -ForegroundColor Red
}

# Now create Sites table without FK constraints
Write-Host "Creating Sites table without FK constraints..." -ForegroundColor Yellow

$sitesCreateScript = @"
-- Sites table 
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sites]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Sites]
    (
        [SiteId] INT IDENTITY(1,1) NOT NULL,
        [SiteName] NVARCHAR(200) NOT NULL,
        [SiteCode] NVARCHAR(50) NOT NULL,
        [CompanyId] INT NULL,
        [Address] NVARCHAR(255) NULL,
        [CityId] INT NULL,
        [CountryId] INT NULL,
        [PostalCode] NVARCHAR(20) NULL,
        [Phone] NVARCHAR(20) NULL,
        [Email] NVARCHAR(255) NULL,
        [ContactPerson] NVARCHAR(100) NULL,
        [ContactEmail] NVARCHAR(255) NULL,
        [ContactPhone] NVARCHAR(20) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [ModifiedDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [CreatedBy] INT NULL,
        [ModifiedBy] INT NULL,
        [SiteType] NVARCHAR(100) NULL,
        [Latitude] DECIMAL(10,8) NULL,
        [Longitude] DECIMAL(11,8) NULL,

        CONSTRAINT [PK_Sites] PRIMARY KEY CLUSTERED ([SiteId]),
        CONSTRAINT [UK_Sites_SiteCode] UNIQUE ([SiteCode])
    );

    -- Indexes for performance
    CREATE NONCLUSTERED INDEX [IX_Sites_SiteName] ON [dbo].[Sites] ([SiteName]);
    CREATE NONCLUSTERED INDEX [IX_Sites_SiteCode] ON [dbo].[Sites] ([SiteCode]);
    CREATE NONCLUSTERED INDEX [IX_Sites_CompanyId] ON [dbo].[Sites] ([CompanyId]);
    CREATE NONCLUSTERED INDEX [IX_Sites_IsActive] ON [dbo].[Sites] ([IsActive]);
    
    PRINT 'Sites table created successfully without FK constraints'
END
ELSE
BEGIN
    PRINT 'Sites table already exists'
END
"@

try {
    $sitesCreateScript | sqlcmd -S $serverName -d $databaseName -E
    Write-Host "Sites table created successfully" -ForegroundColor Green
}
catch {
    Write-Host "Error creating Sites table: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "Table creation completed. FK constraints will be added later." -ForegroundColor Cyan