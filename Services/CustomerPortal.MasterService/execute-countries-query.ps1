# Execute GetAllCountries GraphQL Operation
Write-Host "=== Executing GetAllCountries Operation ===" -ForegroundColor Green

# Wait a moment to ensure service is ready
Start-Sleep 3

# Your exact GraphQL query from the documentation
$graphqlQuery = @"
{
  "query": "query GetAllCountries { countries { id countryName countryCode currencyCode region isActive createdDate modifiedDate cities { id cityName stateProvince postalCode } } }"
}
"@

Write-Host "Sending GraphQL Query to Master Service..." -ForegroundColor Cyan
Write-Host "Endpoint: http://localhost:5067/graphql" -ForegroundColor Yellow
Write-Host ""

try {
    # Execute the GraphQL query
    $response = Invoke-RestMethod -Uri "http://localhost:5067/graphql" -Method POST -ContentType "application/json" -Body $graphqlQuery -TimeoutSec 30
    
    Write-Host "=== SUCCESS: Query Executed Successfully ===" -ForegroundColor Green
    Write-Host ""
    
    # Display the response in a formatted way
    Write-Host "Countries Retrieved:" -ForegroundColor Cyan
    $countries = $response.data.countries
    
    if ($countries -and $countries.Count -gt 0) {
        foreach ($country in $countries) {
            Write-Host "🌍 $($country.countryName) ($($country.countryCode))" -ForegroundColor White
            Write-Host "   Region: $($country.region)" -ForegroundColor Gray
            Write-Host "   Currency: $($country.currencyCode)" -ForegroundColor Gray
            Write-Host "   Active: $($country.isActive)" -ForegroundColor Gray
            
            if ($country.cities -and $country.cities.Count -gt 0) {
                Write-Host "   Cities:" -ForegroundColor Yellow
                foreach ($city in $country.cities) {
                    Write-Host "   🏙️  $($city.cityName), $($city.stateProvince) $($city.postalCode)" -ForegroundColor Cyan
                }
            }
            Write-Host ""
        }
        
        Write-Host "Total Countries Found: $($countries.Count)" -ForegroundColor Green
    } else {
        Write-Host "No countries found in the database." -ForegroundColor Yellow
    }
    
    Write-Host ""
    Write-Host "=== Full JSON Response ===" -ForegroundColor Magenta
    $response | ConvertTo-Json -Depth 6
    
} catch {
    Write-Host "=== ERROR: Failed to Execute Query ===" -ForegroundColor Red
    Write-Host "Error Message: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Please ensure the Master Service is running on port 5067" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "=== Operation Complete ===" -ForegroundColor Green