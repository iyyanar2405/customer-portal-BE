# Test Master Service GraphQL API
Write-Host "Starting Master Service..."

# Start the service in background
$masterServiceJob = Start-Job -ScriptBlock {
    Set-Location "C:\POC\local\customer-portalBE\customer-portal-BE\Services\CustomerPortal.MasterService"
    dotnet run
}

# Wait for service to start
Start-Sleep 10

Write-Host "Testing GraphQL endpoint..."

# Test 1: Add a country
Write-Host "`nAdding a country..."
$addCountryMutation = @{
    query = @"
        mutation {
            addCountry(input: {
                countryName: "United States"
                countryCode: "USA"
                currencyCode: "USD"
                region: "North America"
            }) {
                id
                countryName
                countryCode
                currencyCode
                region
            }
        }
"@
} | ConvertTo-Json

try {
    $addResult = Invoke-RestMethod -Uri "http://localhost:5067/graphql" -Method POST -ContentType "application/json" -Body $addCountryMutation
    Write-Host "Add Country Result:"
    $addResult | ConvertTo-Json -Depth 3
} catch {
    Write-Host "Error adding country: $($_.Exception.Message)"
}

# Test 2: Query countries
Write-Host "`nQuerying countries..."
$queryCountries = @{
    query = "{ countries { id countryName countryCode currencyCode region } }"
} | ConvertTo-Json

try {
    $queryResult = Invoke-RestMethod -Uri "http://localhost:5067/graphql" -Method POST -ContentType "application/json" -Body $queryCountries
    Write-Host "Query Countries Result:"
    $queryResult | ConvertTo-Json -Depth 3
} catch {
    Write-Host "Error querying countries: $($_.Exception.Message)"
}

# Test 3: Add a city
Write-Host "`nAdding a city..."
$addCityMutation = @{
    query = @"
        mutation {
            addCity(input: {
                cityName: "New York"
                countryId: 1
                stateProvince: "NY"
                postalCode: "10001"
            }) {
                id
                cityName
                stateProvince
                postalCode
                country {
                    countryName
                }
            }
        }
"@
} | ConvertTo-Json

try {
    $addCityResult = Invoke-RestMethod -Uri "http://localhost:5067/graphql" -Method POST -ContentType "application/json" -Body $addCityMutation
    Write-Host "Add City Result:"
    $addCityResult | ConvertTo-Json -Depth 3
} catch {
    Write-Host "Error adding city: $($_.Exception.Message)"
}

# Test 4: Query cities
Write-Host "`nQuerying cities..."
$queryCities = @{
    query = "{ cities { id cityName stateProvince country { countryName } } }"
} | ConvertTo-Json

try {
    $queryCitiesResult = Invoke-RestMethod -Uri "http://localhost:5067/graphql" -Method POST -ContentType "application/json" -Body $queryCities
    Write-Host "Query Cities Result:"
    $queryCitiesResult | ConvertTo-Json -Depth 3
} catch {
    Write-Host "Error querying cities: $($_.Exception.Message)"
}

Write-Host "`nTest completed. Stopping service..."

# Stop the service
Stop-Job $masterServiceJob
Remove-Job $masterServiceJob

Write-Host "Master Service tests completed!"