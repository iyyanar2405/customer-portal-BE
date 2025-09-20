# PowerShell script to remove JSON formatting from all stored procedures
# This script will modify stored procedures to return table results instead of JSON

$storedProcedurePath = "C:\POC\customer-portal-BE\Stored-procedure"
$spFiles = Get-ChildItem -Path $storedProcedurePath -Filter "Sp_*.sql"

Write-Host "Found $($spFiles.Count) stored procedure files to process"

foreach ($file in $spFiles) {
    Write-Host "Processing: $($file.Name)"
    
    $content = Get-Content $file.FullName -Raw
    $originalContent = $content
    
    # Remove FOR JSON PATH patterns
    $content = $content -replace '\s+FOR JSON PATH,\s*WITHOUT_ARRAY_WRAPPER', ''
    $content = $content -replace '\s+FOR JSON PATH', ''
    $content = $content -replace 'FOR JSON PATH,\s*WITHOUT_ARRAY_WRAPPER', ''
    $content = $content -replace 'FOR JSON PATH', ''
    
    # Remove common JSON response wrapper patterns
    $content = $content -replace "'[^']*'\s+as __typename", ""
    $content = $content -replace ",\s*'[^']*'\s+as __typename", ""
    
    # Remove GraphQL response structure patterns - simple approach
    $content = $content -replace '\)\s*as data,\s*', ''
    $content = $content -replace '\)\s*as data', ''
    $content = $content -replace ',\s*as errorCode[^;]*;', ';'
    $content = $content -replace ',\s*as __typename[^;]*;', ';'
    
    # Clean up any remaining response wrapper patterns
    $content = $content -replace ",\s*''\s*as errorCode", ""
    $content = $content -replace ",\s*1\s*as isSuccess", ""
    $content = $content -replace ",\s*0\s*as isSuccess", ""
    $content = $content -replace ",\s*''\s*as message", ""
    
    # Clean up extra commas and whitespace
    $content = $content -replace ',\s*;', ';'
    $content = $content -replace ',\s*\)', ')'
    
    # Only update file if content changed
    if ($content -ne $originalContent) {
        Set-Content -Path $file.FullName -Value $content -Encoding UTF8
        Write-Host "  Updated: $($file.Name)" -ForegroundColor Green
    } else {
        Write-Host "  No changes: $($file.Name)" -ForegroundColor Yellow
    }
}

Write-Host "`nCompleted processing all stored procedure files!" -ForegroundColor Cyan