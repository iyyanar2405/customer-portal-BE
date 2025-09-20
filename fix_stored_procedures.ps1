# PowerShell script to fix remaining syntax issues in stored procedures
$storedProcedurePath = "C:\POC\customer-portal-BE\Stored-procedure"
$spFiles = Get-ChildItem -Path $storedProcedurePath -Filter "Sp_*.sql"

foreach ($file in $spFiles) {
    Write-Host "Fixing: $($file.Name)"
    
    $content = Get-Content $file.FullName -Raw
    $originalContent = $content
    
    # Fix remaining JSON issues
    $content = $content -replace 'JSON_QUERY\([^)]+\)', 'NULL'
    $content = $content -replace '\)\s*as items,', ''
    $content = $content -replace ',\s*$', ''
    $content = $content -replace ',\s*\n\s*$', ''
    
    # Fix SELECT statement issues
    $content = $content -replace 'SELECT\s*\(\s*SELECT', 'SELECT'
    $content = $content -replace '\(\s*SELECT\s+', 'SELECT '
    
    # Fix trailing commas before END TRY/CATCH
    $content = $content -replace ',\s*END TRY', '; END TRY'
    $content = $content -replace ',\s*END CATCH', '; END CATCH'
    
    # Fix duplicate SELECT statements
    $content = $content -replace 'SELECT\s+SELECT', 'SELECT'
    
    # Fix orphaned parentheses
    $content = $content -replace '\)\s*;\s*\n\s*\)\s*;', ';'
    $content = $content -replace '\)\s*\n\s*END TRY', 'END TRY'
    
    # Clean up whitespace
    $content = $content -replace '\n\s*\n\s*\n', "`n`n"
    
    # Only update if changed
    if ($content -ne $originalContent) {
        Set-Content -Path $file.FullName -Value $content -Encoding UTF8
        Write-Host "  Fixed: $($file.Name)" -ForegroundColor Green
    } else {
        Write-Host "  No issues: $($file.Name)" -ForegroundColor Yellow
    }
}

Write-Host "Completed fixing stored procedures!" -ForegroundColor Cyan