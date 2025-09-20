# PowerShell script to execute stored procedures for each module
$serverName = "DESKTOP-6A76F24\MSSQLSERVER1"
$databaseName = "Customer_PortalI"
$storedProcPath = "C:\POC\customer-portal-BE\Stored-procedure"

Write-Host "Executing stored procedures for each module..." -ForegroundColor Green

# Define modules to process
$modules = @("Actions", "Audits", "Certificates", "Contracts", "Financial", "Findings", "Master", "Notifications", "Overview", "Settings", "Users", "Widgets")

$totalSuccess = 0
$totalErrors = 0

foreach ($module in $modules) {
    $modulePath = Join-Path $storedProcPath $module
    
    if (Test-Path $modulePath) {
        Write-Host "`nProcessing module: $module" -ForegroundColor Cyan
        
        # Get all SQL files in the module directory
        $sqlFiles = Get-ChildItem $modulePath -Filter "*.sql" -ErrorAction SilentlyContinue
        
        if ($sqlFiles.Count -eq 0) {
            Write-Host "  No SQL files found in $module module" -ForegroundColor Yellow
            continue
        }
        
        $moduleSuccess = 0
        $moduleErrors = 0
        
        foreach ($sqlFile in $sqlFiles) {
            Write-Host "  Executing: $($sqlFile.Name)" -ForegroundColor Yellow
            
            try {
                $result = & sqlcmd -S $serverName -d $databaseName -E -i $sqlFile.FullName -b
                
                if ($LASTEXITCODE -eq 0) {
                    Write-Host "  ✓ Successfully executed: $($sqlFile.Name)" -ForegroundColor Green
                    $moduleSuccess++
                    $totalSuccess++
                } else {
                    Write-Host "  ✗ Error executing: $($sqlFile.Name)" -ForegroundColor Red
                    if ($result) { 
                        Write-Host "  Error details: $($result -join ' ')" -ForegroundColor Red 
                    }
                    $moduleErrors++
                    $totalErrors++
                }
            }
            catch {
                Write-Host "  ✗ Exception executing: $($sqlFile.Name)" -ForegroundColor Red
                Write-Host "  Exception: $($_.Exception.Message)" -ForegroundColor Red
                $moduleErrors++
                $totalErrors++
            }
            
            Start-Sleep -Milliseconds 100
        }
        
        Write-Host "  Module $module summary: ✓$moduleSuccess ✗$moduleErrors" -ForegroundColor Cyan
    } else {
        Write-Host "`nModule directory not found: $module" -ForegroundColor Orange
    }
}

Write-Host "`n" + "="*60 -ForegroundColor Cyan
Write-Host "OVERALL STORED PROCEDURES SUMMARY:" -ForegroundColor Cyan
Write-Host "✓ Total Successful: $totalSuccess" -ForegroundColor Green
Write-Host "✗ Total Errors: $totalErrors" -ForegroundColor Red
Write-Host "Total processed: $($totalSuccess + $totalErrors)" -ForegroundColor Cyan

if ($totalErrors -eq 0) {
    Write-Host "All stored procedures executed successfully!" -ForegroundColor Green
} else {
    Write-Host "Some stored procedures failed. Please review the errors above." -ForegroundColor Yellow
}