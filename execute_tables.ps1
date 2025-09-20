# PowerShell script to execute table creation scripts in dependency order
$serverName = "DESKTOP-6A76F24\MSSQLSERVER1"
$databaseName = "Customer_PortalI"
$username = "sa"
$password = "Smart@#12"

# Connection string
$connectionString = "Server=$serverName;Database=$databaseName;User Id=$username;Password=$password;TrustServerCertificate=true"

Write-Host "Starting table creation process..." -ForegroundColor Green

# Define table creation order (dependencies first)
$tableOrder = @(
    # Master/Reference tables first
    "Countries.sql",
    "Cities.sql", 
    "Roles.sql",
    "Services.sql",
    "Companies.sql",
    "Sites.sql",
    "Chapters.sql",
    "Clauses.sql",
    "FocusAreas.sql",
    
    # User related tables
    "Users.sql",
    "UserRoles.sql",
    "UserCityAccess.sql",
    "UserCompanyAccess.sql",
    "UserCountryAccess.sql",
    "UserNotificationAccess.sql",
    "UserPreferences.sql",
    "UserServiceAccess.sql",
    "UserSiteAccess.sql",
    "UserTrainings.sql",
    
    # Audit related tables
    "AuditTypes.sql",
    "Audits.sql",
    "AuditLogs.sql",
    "AuditServices.sql",
    "AuditSites.sql",
    "AuditSiteAudits.sql",
    "AuditSiteRepresentatives.sql",
    "AuditSiteServices.sql",
    "AuditTeamMembers.sql",
    
    # Contract related tables
    "Contracts.sql",
    "ContractServices.sql",
    "ContractSites.sql",
    
    # Certificate related tables
    "Certificates.sql",
    "CertificateServices.sql",
    "CertificateSites.sql",
    "CertificateAdditionalScopes.sql",
    
    # Finding related tables
    "FindingCategories.sql",
    "FindingStatuses.sql",
    "FindingFocusAreas.sql",
    "FindingClauses.sql",
    "Findings.sql",
    "FindingResponses.sql",
    
    # Financial tables
    "Financials.sql",
    "Invoices.sql",
    "InvoiceAuditLog.sql",
    
    # Notification tables
    "NotificationCategories.sql",
    "Notifications.sql",
    
    # Action and other tables
    "Actions.sql",
    "ErrorLogs.sql",
    "Trainings.sql"
)

$tablesPath = "C:\POC\customer-portal-BE\tables"
$successCount = 0
$errorCount = 0

foreach ($tableFile in $tableOrder) {
    $filePath = Join-Path $tablesPath $tableFile
    
    if (Test-Path $filePath) {
        Write-Host "Executing: $tableFile" -ForegroundColor Yellow
        
        try {
            $sqlContent = Get-Content $filePath -Raw
            
            # Execute SQL using sqlcmd
            $result = & sqlcmd -S $serverName -d $databaseName -U $username -P $password -Q $sqlContent -b
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "✓ Successfully created table from: $tableFile" -ForegroundColor Green
                $successCount++
            } else {
                Write-Host "✗ Error executing: $tableFile" -ForegroundColor Red
                Write-Host "Error details: $result" -ForegroundColor Red
                $errorCount++
            }
        }
        catch {
            Write-Host "✗ Exception executing: $tableFile" -ForegroundColor Red
            Write-Host "Exception: $($_.Exception.Message)" -ForegroundColor Red
            $errorCount++
        }
    } else {
        Write-Host "⚠ File not found: $tableFile" -ForegroundColor Orange
    }
    
    Start-Sleep -Milliseconds 500  # Brief pause between executions
}

Write-Host "`nTable creation summary:" -ForegroundColor Cyan
Write-Host "✓ Successful: $successCount" -ForegroundColor Green
Write-Host "✗ Errors: $errorCount" -ForegroundColor Red
Write-Host "Total processed: $($successCount + $errorCount)" -ForegroundColor Cyan

if ($errorCount -eq 0) {
    Write-Host "All tables created successfully!" -ForegroundColor Green
} else {
    Write-Host "Some tables failed to create. Please check the errors above." -ForegroundColor Yellow
}