# GraphQL Testing Script for Customer Portal Services
# Run this script to test GraphQL endpoints

$masterServiceUrl = "http://localhost:5003/graphql"

Write-Host "Testing Customer Portal GraphQL Services" -ForegroundColor Green
Write-Host "=" * 50

# Test if Master Service is running
Write-Host "Testing Master Service at $masterServiceUrl..." -ForegroundColor Yellow

try {
    # Test basic connectivity
    $response = Invoke-WebRequest -Uri $masterServiceUrl -Method GET -TimeoutSec 5
    Write-Host "Master Service is responding (Status: $($response.StatusCode))" -ForegroundColor Green
    
    # Test GraphQL Schema Query
    Write-Host "`nTesting GraphQL Schema..." -ForegroundColor Yellow
    
    $schemaQuery = @{
        query = "query { __schema { types { name } } }"
    } | ConvertTo-Json
    
    $headers = @{
        "Content-Type" = "application/json"
    }
    
    $schemaResponse = Invoke-RestMethod -Uri $masterServiceUrl -Method POST -Body $schemaQuery -Headers $headers
    
    if ($schemaResponse.__schema) {
        Write-Host "GraphQL Schema is accessible" -ForegroundColor Green
        Write-Host "Available Types:" -ForegroundColor Cyan
        $schemaResponse.__schema.types | Where-Object { $_.name -notlike "__*" } | ForEach-Object { 
            Write-Host "   - $($_.name)" -ForegroundColor White
        }
    }
    
    # Test Countries Query
    Write-Host "`nTesting Countries Query..." -ForegroundColor Yellow
    
    $countriesQuery = @{
        query = @"
        query {
          countries {
            id
            countryName
            countryCode
            region
          }
        }
"@
    } | ConvertTo-Json
    
    $countriesResponse = Invoke-RestMethod -Uri $masterServiceUrl -Method POST -Body $countriesQuery -Headers $headers
    
    if ($countriesResponse.data.countries) {
        Write-Host "Countries query successful" -ForegroundColor Green
        Write-Host "Found $($countriesResponse.data.countries.Count) countries" -ForegroundColor Cyan
        
        if ($countriesResponse.data.countries.Count -gt 0) {
            Write-Host "Sample countries:" -ForegroundColor Cyan
            $countriesResponse.data.countries | Select-Object -First 3 | ForEach-Object {
                Write-Host "   - $($_.countryName) ($($_.countryCode))" -ForegroundColor White
            }
        } else {
            Write-Host "No countries found in database (database might need seeding)" -ForegroundColor Yellow
        }
    }
    
} catch {
    if ($_.Exception.Message -like "*refused*" -or $_.Exception.Message -like "*timeout*") {
        Write-Host "Master Service is not running or not accessible" -ForegroundColor Red
        Write-Host "Start it with: dotnet run --project Services\CustomerPortal.MasterService --urls http://localhost:5003" -ForegroundColor Yellow
    } else {
        Write-Host "Error testing Master Service: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "`n" + "=" * 50
Write-Host "🔗 GraphQL Endpoints:" -ForegroundColor Green
Write-Host "   Master Service:    http://localhost:5003/graphql" -ForegroundColor White
Write-Host "   Users Service:     http://localhost:5004/graphql" -ForegroundColor White
Write-Host "   Audits Service:    http://localhost:5002/graphql" -ForegroundColor White
Write-Host "   Actions Service:   http://localhost:5001/graphql" -ForegroundColor White

Write-Host "`n📖 Sample Queries for Master Service:" -ForegroundColor Green
Write-Host @"
# Get all countries
query {
  countries {
    id
    countryName
    countryCode
    cities {
      id
      cityName
    }
  }
}

# Get all companies
query {
  companies {
    id
    companyName
    companyCode
    sites {
      id
      siteName
    }
  }
}

# Get all services
query {
  services {
    id
    serviceName
    serviceCode
    serviceCategory
  }
}
"@ -ForegroundColor Cyan

Write-Host "`n🌐 Open in browser: http://localhost:5003/graphql" -ForegroundColor Green