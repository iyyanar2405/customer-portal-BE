# PowerShell script to execute table creation scripts (handles circular dependencies)
$serverName = "DESKTOP-6A76F24\MSSQLSERVER1"
$databaseName = "Customer_PortalI"
$username = "sa"
$password = "Smart@#12"

Write-Host "Starting table creation process..." -ForegroundColor Green

$tablesPath = "C:\POC\customer-portal-BE\tables"

# Function to execute SQL command
function Execute-SqlCommand {
    param([string]$SqlFile)
    
    $filePath = Join-Path $tablesPath $SqlFile
    
    if (Test-Path $filePath) {
        Write-Host "Executing: $SqlFile" -ForegroundColor Yellow
        
        try {
            # Execute using sqlcmd with file input
            $result = & sqlcmd -S $serverName -d $databaseName -U $username -P $password -i $filePath -b
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "Successfully executed: $SqlFile" -ForegroundColor Green
                return $true
            } else {
                Write-Host "Error executing: $SqlFile" -ForegroundColor Red
                if ($result) { Write-Host "Output: $result" -ForegroundColor Red }
                return $false
            }
        }
        catch {
            Write-Host "Exception executing: $SqlFile" -ForegroundColor Red
            Write-Host "Exception: $($_.Exception.Message)" -ForegroundColor Red
            return $false
        }
    } else {
        Write-Host "File not found: $SqlFile" -ForegroundColor Orange
        return $false
    }
}

# Execute just Countries first
Write-Host "Creating Countries table..." -ForegroundColor Cyan
Execute-SqlCommand "Countries.sql"

Write-Host "Table creation completed!" -ForegroundColor Green