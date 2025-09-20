# Add Sample Data and Test GraphQL
Write-Host "Adding sample data to Master Service database..."

# Connect to LocalDB and add sample data
$connectionString = "Server=(localdb)\mssqllocaldb;Database=CustomerPortal_Master;Trusted_Connection=true;TrustServerCertificate=true;"

# Sample SQL to insert test data
$insertCountries = @"
INSERT INTO Countries (CountryName, CountryCode, CurrencyCode, Region, CreatedDate, ModifiedDate, IsActive)
VALUES 
('United States', 'US', 'USD', 'North America', GETDATE(), GETDATE(), 1),
('Canada', 'CA', 'CAD', 'North America', GETDATE(), GETDATE(), 1),
('United Kingdom', 'GB', 'GBP', 'Europe', GETDATE(), GETDATE(), 1),
('Germany', 'DE', 'EUR', 'Europe', GETDATE(), GETDATE(), 1),
('Australia', 'AU', 'AUD', 'Oceania', GETDATE(), GETDATE(), 1);
"@

$insertCities = @"
INSERT INTO Cities (CityName, CountryId, StateProvince, PostalCode, CreatedDate, ModifiedDate, IsActive)
VALUES 
('New York', 1, 'NY', '10001', GETDATE(), GETDATE(), 1),
('Los Angeles', 1, 'CA', '90001', GETDATE(), GETDATE(), 1),
('Toronto', 2, 'ON', 'M5V', GETDATE(), GETDATE(), 1),
('London', 3, 'England', 'SW1A', GETDATE(), GETDATE(), 1),
('Berlin', 4, 'Berlin', '10115', GETDATE(), GETDATE(), 1),
('Sydney', 5, 'NSW', '2000', GETDATE(), GETDATE(), 1);
"@

$insertCompanies = @"
INSERT INTO Companies (CompanyName, CompanyCode, Address, CountryId, Phone, Email, Website, ContactPerson, CompanyType, CreatedDate, ModifiedDate, IsActive)
VALUES 
('Acme Corporation', 'ACME001', '123 Business St', 1, '+1-555-0123', 'contact@acme.com', 'https://www.acme.com', 'John Doe', 'Manufacturing', GETDATE(), GETDATE(), 1),
('TechCorp Ltd', 'TECH001', '456 Tech Ave', 1, '+1-555-0124', 'info@techcorp.com', 'https://www.techcorp.com', 'Jane Smith', 'Technology', GETDATE(), GETDATE(), 1),
('Global Industries', 'GLOB001', '789 Global Blvd', 3, '+44-20-7123-4567', 'hello@globalind.com', 'https://www.globalind.com', 'Mike Johnson', 'Consulting', GETDATE(), GETDATE(), 1);
"@

$insertServices = @"
INSERT INTO Services (ServiceName, ServiceCode, Description, ServiceCategory, CreatedDate, ModifiedDate, IsActive)
VALUES 
('ISO 9001 Audit', 'ISO9001', 'Quality Management System Audit', 'Quality', GETDATE(), GETDATE(), 1),
('ISO 14001 Audit', 'ISO14001', 'Environmental Management System Audit', 'Environmental', GETDATE(), GETDATE(), 1),
('ISO 45001 Audit', 'ISO45001', 'Occupational Health and Safety Management System Audit', 'Safety', GETDATE(), GETDATE(), 1),
('Training Services', 'TRAIN001', 'Professional training and development', 'Training', GETDATE(), GETDATE(), 1);
"@

$insertRoles = @"
INSERT INTO Roles (RoleName, Description, Permissions, CreatedDate, ModifiedDate, IsActive)
VALUES 
('Administrator', 'Full system access and control', 'CREATE,READ,UPDATE,DELETE,ADMIN', GETDATE(), GETDATE(), 1),
('Auditor', 'Audit management and execution', 'CREATE,READ,UPDATE', GETDATE(), GETDATE(), 1),
('Client User', 'Client portal access', 'READ,UPDATE', GETDATE(), GETDATE(), 1),
('Viewer', 'Read-only access', 'READ', GETDATE(), GETDATE(), 1);
"@

try {
    # Use sqlcmd to execute the SQL commands
    Write-Host "Inserting Countries..."
    $insertCountries | sqlcmd -S "(localdb)\mssqllocaldb" -d "CustomerPortal_Master" -Q
    
    Write-Host "Inserting Cities..."
    $insertCities | sqlcmd -S "(localdb)\mssqllocaldb" -d "CustomerPortal_Master" -Q
    
    Write-Host "Inserting Companies..."
    $insertCompanies | sqlcmd -S "(localdb)\mssqllocaldb" -d "CustomerPortal_Master" -Q
    
    Write-Host "Inserting Services..."
    $insertServices | sqlcmd -S "(localdb)\mssqllocaldb" -d "CustomerPortal_Master" -Q
    
    Write-Host "Inserting Roles..."
    $insertRoles | sqlcmd -S "(localdb)\mssqllocaldb" -d "CustomerPortal_Master" -Q
    
    Write-Host "Sample data inserted successfully!"
} catch {
    Write-Host "Error inserting data: $($_.Exception.Message)"
    Write-Host "Trying alternative approach..."
}

# Wait a moment then test GraphQL
Start-Sleep 3

Write-Host "`n=== Testing GraphQL Queries ==="

# Test 1: Countries query
Write-Host "`nTesting Countries Query..."
$countriesQuery = @{
    query = "query GetAllCountries { countries { id countryName countryCode currencyCode region isActive createdDate cities { id cityName stateProvince postalCode } } }"
} | ConvertTo-Json

try {
    $result = Invoke-RestMethod -Uri "http://localhost:5067/graphql" -Method POST -ContentType "application/json" -Body $countriesQuery
    Write-Host "Countries Result:"
    $result | ConvertTo-Json -Depth 4
} catch {
    Write-Host "Error querying countries: $($_.Exception.Message)"
    Write-Host "Response: $($_.Exception.Response)"
}

# Test 2: Companies query
Write-Host "`nTesting Companies Query..."
$companiesQuery = @{
    query = "query GetAllCompanies { companies { id companyName companyCode email website country { countryName } } }"
} | ConvertTo-Json

try {
    $result = Invoke-RestMethod -Uri "http://localhost:5067/graphql" -Method POST -ContentType "application/json" -Body $companiesQuery
    Write-Host "Companies Result:"
    $result | ConvertTo-Json -Depth 4
} catch {
    Write-Host "Error querying companies: $($_.Exception.Message)"
}

# Test 3: Services query
Write-Host "`nTesting Services Query..."
$servicesQuery = @{
    query = "query GetAllServices { services { id serviceName serviceCode description serviceCategory } }"
} | ConvertTo-Json

try {
    $result = Invoke-RestMethod -Uri "http://localhost:5067/graphql" -Method POST -ContentType "application/json" -Body $servicesQuery
    Write-Host "Services Result:"
    $result | ConvertTo-Json -Depth 4
} catch {
    Write-Host "Error querying services: $($_.Exception.Message)"
}

Write-Host "`nGraphQL testing completed!"