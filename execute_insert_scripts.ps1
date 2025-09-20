# PowerShell script to execute all insert scripts
$serverName = "DESKTOP-6A76F24\MSSQLSERVER1"
$databaseName = "Customer_PortalI"
$insertScriptsPath = "C:\POC\customer-portal-BE\insert-scripts"

Write-Host "Executing all insert scripts..." -ForegroundColor Green

# Define execution order for insert scripts (dependencies first)
$insertOrder = @(
    "Countries_insert.sql",
    "Cities_insert.sql",
    "Roles_insert.sql",
    "Services_insert.sql",
    "Companies_insert.sql",
    "Sites_insert.sql",
    "Users_insert.sql",
    "UserRoles_insert.sql",
    "UserCompanyAccess_insert.sql",
    "UserServiceAccess_insert.sql",
    "UserSiteAccess_insert.sql",
    "UserPreferences_insert.sql",
    "UserTrainings_insert.sql",
    "AuditTypes_insert.sql",
    "Audits_insert.sql",
    "AuditServices_insert.sql",
    "AuditSites_insert.sql",
    "AuditSiteAudits_insert.sql",
    "AuditTeamMembers_insert.sql",
    "AuditLogs_insert.sql",
    "FindingCategories_insert.sql",
    "FindingStatuses_insert.sql",
    "Findings_insert.sql",
    "Certificates_insert.sql",
    "Invoices_insert.sql",
    "NotificationCategories_insert.sql",
    "Notifications_insert.sql",
    "Trainings_insert.sql"
)

$successCount = 0
$errorCount = 0

foreach ($insertFile in $insertOrder) {
    $filePath = Join-Path $insertScriptsPath $insertFile
    
    if (Test-Path $filePath) {
        Write-Host "Executing: $insertFile" -ForegroundColor Yellow
        
        try {
            $result = & sqlcmd -S $serverName -d $databaseName -E -i $filePath -b
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "✓ Successfully executed: $insertFile" -ForegroundColor Green
                $successCount++
            } else {
                Write-Host "✗ Error executing: $insertFile" -ForegroundColor Red
                if ($result) { Write-Host "Error details: $result" -ForegroundColor Red }
                $errorCount++
            }
        }
        catch {
            Write-Host "✗ Exception executing: $insertFile" -ForegroundColor Red
            Write-Host "Exception: $($_.Exception.Message)" -ForegroundColor Red
            $errorCount++
        }
    } else {
        Write-Host "⚠ File not found: $insertFile" -ForegroundColor Orange
    }
    
    Start-Sleep -Milliseconds 200
}

Write-Host "`nInsert scripts summary:" -ForegroundColor Cyan
Write-Host "✓ Successful: $successCount" -ForegroundColor Green
Write-Host "✗ Errors: $errorCount" -ForegroundColor Red
Write-Host "Total processed: $($successCount + $errorCount)" -ForegroundColor Cyan