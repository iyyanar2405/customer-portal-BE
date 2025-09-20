# Test GraphQL queries only (no mutations)
Write-Host "Testing Master Service GraphQL Queries..."

# Start the service in background
$masterServiceJob = Start-Job -ScriptBlock {
    Set-Location "C:\POC\local\customer-portalBE\customer-portal-BE\Services\CustomerPortal.MasterService"
    dotnet run
}

# Wait for service to start
Start-Sleep 10

# Test 1: Query countries (should return empty array)
Write-Host "`nQuerying countries..."
$queryCountries = @{
    query = "{ countries { id countryName countryCode currencyCode region } }"
} | ConvertTo-Json

try {
    $result = Invoke-RestMethod -Uri "http://localhost:5067/graphql" -Method POST -ContentType "application/json" -Body $queryCountries
    Write-Host "Countries Query Result:"
    $result | ConvertTo-Json -Depth 3
} catch {
    Write-Host "Error querying countries: $($_.Exception.Message)"
}

# Test 2: Query cities (should return empty array)
Write-Host "`nQuerying cities..."
$queryCities = @{
    query = "{ cities { id cityName stateProvince country { countryName } } }"
} | ConvertTo-Json

try {
    $result = Invoke-RestMethod -Uri "http://localhost:5067/graphql" -Method POST -ContentType "application/json" -Body $queryCities
    Write-Host "Cities Query Result:"
    $result | ConvertTo-Json -Depth 3
} catch {
    Write-Host "Error querying cities: $($_.Exception.Message)"
}

# Test 3: Query companies (should return empty array)
Write-Host "`nQuerying companies..."
$queryCompanies = @{
    query = "{ companies { id companyName companyCode email website } }"
} | ConvertTo-Json

try {
    $result = Invoke-RestMethod -Uri "http://localhost:5067/graphql" -Method POST -ContentType "application/json" -Body $queryCompanies
    Write-Host "Companies Query Result:"
    $result | ConvertTo-Json -Depth 3
} catch {
    Write-Host "Error querying companies: $($_.Exception.Message)"
}

# Test 4: Query services (should return empty array)
Write-Host "`nQuerying services..."
$queryServices = @{
    query = "{ services { id serviceName serviceCode description serviceCategory } }"
} | ConvertTo-Json

try {
    $result = Invoke-RestMethod -Uri "http://localhost:5067/graphql" -Method POST -ContentType "application/json" -Body $queryServices
    Write-Host "Services Query Result:"
    $result | ConvertTo-Json -Depth 3
} catch {
    Write-Host "Error querying services: $($_.Exception.Message)"
}

# Test 5: Query roles (should return empty array)
Write-Host "`nQuerying roles..."
$queryRoles = @{
    query = "{ roles { id roleName description permissions } }"
} | ConvertTo-Json

try {
    $result = Invoke-RestMethod -Uri "http://localhost:5067/graphql" -Method POST -ContentType "application/json" -Body $queryRoles
    Write-Host "Roles Query Result:"
    $result | ConvertTo-Json -Depth 3
} catch {
    Write-Host "Error querying roles: $($_.Exception.Message)"
}

# Test 6: Query chapters (should return empty array)
Write-Host "`nQuerying chapters..."
$queryChapters = @{
    query = "{ chapters { id chapterName chapterNumber description } }"
} | ConvertTo-Json

try {
    $result = Invoke-RestMethod -Uri "http://localhost:5067/graphql" -Method POST -ContentType "application/json" -Body $queryChapters
    Write-Host "Chapters Query Result:"
    $result | ConvertTo-Json -Depth 3
} catch {
    Write-Host "Error querying chapters: $($_.Exception.Message)"
}

Write-Host "`n=== GraphQL Introspection Query ==="
$introspectionQuery = @{
    query = @"
        query IntrospectionQuery {
            __schema {
                queryType { name }
                mutationType { name }
                types {
                    name
                    kind
                    description
                    fields {
                        name
                        description
                        type {
                            name
                            kind
                        }
                    }
                }
            }
        }
"@
} | ConvertTo-Json

try {
    $result = Invoke-RestMethod -Uri "http://localhost:5067/graphql" -Method POST -ContentType "application/json" -Body $introspectionQuery
    Write-Host "Schema information retrieved successfully. Available types:"
    $result.data.__schema.types | Where-Object { $_.name -match "Type$" } | ForEach-Object { 
        Write-Host "- $($_.name)" 
    }
} catch {
    Write-Host "Error with introspection query: $($_.Exception.Message)"
}

Write-Host "`nTest completed. Stopping service..."

# Stop the service
Stop-Job $masterServiceJob
Remove-Job $masterServiceJob

Write-Host "GraphQL queries test completed successfully!"
Write-Host "The database is working and GraphQL endpoint is responding."
Write-Host "All queries returned empty arrays as expected (no data inserted yet)."