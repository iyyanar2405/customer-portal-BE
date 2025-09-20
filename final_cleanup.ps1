# Final cleanup script for stored procedures
$storedProcedurePath = "C:\POC\customer-portal-BE\Stored-procedure"
$spFiles = Get-ChildItem -Path $storedProcedurePath -Filter "Sp_*.sql"

foreach ($file in $spFiles) {
    Write-Host "Final cleanup: $($file.Name)"
    
    $content = Get-Content $file.FullName -Raw
    $originalContent = $content
    
    # Fix broken SELECT statements with subqueries
    $content = $content -replace 'COALESCE\([^,]+,\s*0\)\s*as\s*companyId,\s*SELECT', 'COALESCE(c.CompanyId, 0) as companyId,'
    $content = $content -replace '\)\s*as\s*serviceIds,\s*SELECT', ' -- serviceIds subquery removed,'
    $content = $content -replace '\)\s*as\s*siteIds,', ' -- siteIds subquery removed,'
    
    # Fix incomplete SELECT clauses
    $content = $content -replace 'SELECT\s+DISTINCT\s+cs\.ServiceId[^)]*\)', '(SELECT STRING_AGG(CAST(cs.ServiceId AS VARCHAR), '','') FROM CertificateServices cs WHERE cs.CertificateId = c.CertificateId)'
    $content = $content -replace 'SELECT\s+DISTINCT\s+cst\.SiteId[^)]*\)', '(SELECT STRING_AGG(CAST(cst.SiteId AS VARCHAR), '','') FROM CertificateSites cst WHERE cst.CertificateId = c.CertificateId)'
    
    # Fix orphaned commas and syntax
    $content = $content -replace ',\s*FROM', ' FROM'
    $content = $content -replace 'revisionNumber,\s*FROM', 'revisionNumber FROM'
    $content = $content -replace 'accountDNVId,\s*FROM', 'accountDNVId FROM'
    
    # Fix broken END statements
    $content = $content -replace '1\s*as\s*isSuccess,\s*[^;]*;\s*END TRY', '; END TRY'
    $content = $content -replace 'NULL\s*as\s*message;\s*END CATCH', 'NULL as errorMessage; END CATCH'
    
    # Fix trailing commas before FROM
    $content = $content -replace ',\s*\n\s*FROM', ' FROM'
    
    # Clean up empty lines
    $content = $content -replace '\n\s*\n\s*\n', "`n`n"
    
    # Only update if changed
    if ($content -ne $originalContent) {
        Set-Content -Path $file.FullName -Value $content -Encoding UTF8
        Write-Host "  Cleaned: $($file.Name)" -ForegroundColor Green
    } else {
        Write-Host "  No issues: $($file.Name)" -ForegroundColor Yellow
    }
}

Write-Host "Final cleanup completed!" -ForegroundColor Cyan