# PowerShell script to create all tables without foreign key constraints first
$serverName = "DESKTOP-6A76F24\MSSQLSERVER1"
$databaseName = "Customer_PortalI"

Write-Host "Creating all database tables..." -ForegroundColor Green

# Function to create table without foreign key constraints
function Create-TableWithoutFKs {
    param([string]$TableFile)
    
    $filePath = Join-Path "C:\POC\customer-portal-BE\tables" $TableFile
    
    if (Test-Path $filePath) {
        Write-Host "Processing: $TableFile" -ForegroundColor Yellow
        
        # Read the file and remove foreign key constraints
        $sqlContent = Get-Content $filePath -Raw
        
        # Remove foreign key constraint lines but keep the table structure
        $modifiedSql = $sqlContent -replace '(?m)^\s*CONSTRAINT\s+\[FK_[^\]]+\]\s+FOREIGN\s+KEY.*?REFERENCES.*?,?\s*$', ''
        
        # Clean up any trailing commas before closing parenthesis
        $modifiedSql = $modifiedSql -replace ',(\s*\r?\n\s*\);)', '$1'
        
        try {
            # Execute the modified SQL
            $result = $modifiedSql | sqlcmd -S $serverName -d $databaseName -E -b
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "✓ Created table: $TableFile" -ForegroundColor Green
                return $true
            } else {
                Write-Host "✗ Error creating: $TableFile" -ForegroundColor Red
                if ($result) { Write-Host "Error: $result" -ForegroundColor Red }
                return $false
            }
        }
        catch {
            Write-Host "✗ Exception creating: $TableFile" -ForegroundColor Red
            Write-Host "Exception: $($_.Exception.Message)" -ForegroundColor Red
            return $false
        }
    } else {
        Write-Host "⚠ File not found: $TableFile" -ForegroundColor Orange
        return $false
    }
}

# List all SQL files in the tables directory
$tableFiles = Get-ChildItem "C:\POC\customer-portal-BE\tables" -Filter "*.sql" | Select-Object -ExpandProperty Name

$successCount = 0
$errorCount = 0

Write-Host "Found $($tableFiles.Count) table files to process..." -ForegroundColor Cyan

foreach ($tableFile in $tableFiles) {
    if (Create-TableWithoutFKs $tableFile) {
        $successCount++
    } else {
        $errorCount++
    }
    Start-Sleep -Milliseconds 100
}

Write-Host "`nTable creation summary:" -ForegroundColor Cyan
Write-Host "✓ Successful: $successCount" -ForegroundColor Green
Write-Host "✗ Errors: $errorCount" -ForegroundColor Red
Write-Host "Total processed: $($successCount + $errorCount)" -ForegroundColor Cyan

if ($errorCount -eq 0) {
    Write-Host "All tables created successfully!" -ForegroundColor Green
} else {
    Write-Host "Some tables failed to create. Continuing with data insertion..." -ForegroundColor Yellow
}