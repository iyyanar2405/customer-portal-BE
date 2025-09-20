# PowerShell script to execute table creation scripts with Windows Authentication
$serverName = "DESKTOP-6A76F24\MSSQLSERVER1"
$databaseName = "Customer_PortalI"

Write-Host "Starting table creation process..." -ForegroundColor Green

# Define table creation order (avoiding circular FK dependencies)
$tableOrder = @(
    # Core reference tables first (no FKs)
    "Roles.sql",
    "Services.sql",
    "Chapters.sql", 
    "Clauses.sql",
    "FocusAreas.sql",
    "AuditTypes.sql",
    "FindingCategories.sql",
    "FindingStatuses.sql",
    "NotificationCategories.sql",
    
    # Tables with minimal dependencies
    "Countries.sql",
    "Companies.sql",
    "Cities.sql",
    "Sites.sql",
    
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
    
    # Business entity tables
    "Audits.sql",
    "Contracts.sql",
    "Certificates.sql",
    "Financials.sql",
    "Invoices.sql",
    "Findings.sql",
    "Notifications.sql",
    "Actions.sql",
    
    # Relationship/junction tables
    "AuditServices.sql",
    "AuditSites.sql",
    "AuditSiteAudits.sql",
    "AuditSiteRepresentatives.sql",
    "AuditSiteServices.sql",
    "AuditTeamMembers.sql",
    "ContractServices.sql",
    "ContractSites.sql",
    "CertificateServices.sql",
    "CertificateSites.sql",
    "CertificateAdditionalScopes.sql",
    "FindingClauses.sql",
    "FindingFocusAreas.sql",
    "FindingResponses.sql",
    
    # Log tables (typically have minimal constraints)
    "AuditLogs.sql",
    "InvoiceAuditLog.sql",
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
            # Execute using sqlcmd with Windows Authentication
            $result = & sqlcmd -S $serverName -d $databaseName -E -i $filePath -b
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "Successfully created table from: $tableFile" -ForegroundColor Green
                $successCount++
            } else {
                Write-Host "Error executing: $tableFile" -ForegroundColor Red
                if ($result) { Write-Host "Error details: $result" -ForegroundColor Red }
                $errorCount++
            }
        }
        catch {
            Write-Host "Exception executing: $tableFile" -ForegroundColor Red
            Write-Host "Exception: $($_.Exception.Message)" -ForegroundColor Red
            $errorCount++
        }
    } else {
        Write-Host "File not found: $tableFile" -ForegroundColor Orange
    }
    
    Start-Sleep -Milliseconds 200
}

Write-Host "Table creation summary:" -ForegroundColor Cyan
Write-Host "Successful: $successCount" -ForegroundColor Green
Write-Host "Errors: $errorCount" -ForegroundColor Red
Write-Host "Total processed: $($successCount + $errorCount)" -ForegroundColor Cyan

if ($errorCount -eq 0) {
    Write-Host "All tables created successfully!" -ForegroundColor Green
} else {
    Write-Host "Some tables failed to create. Please check the errors above." -ForegroundColor Yellow
}