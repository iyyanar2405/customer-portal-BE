# Master Module - JSON Payload Examples

## Create Country Request
```json
{
  "countryName": "United States",
  "countryCode": "US",
  "alpha2Code": "US",
  "alpha3Code": "USA",
  "currencyCode": "USD",
  "region": "North America",
  "isActive": true,
  "createdBy": 1
}
```

## Create Country Response
```json
{
  "data": {
    "id": 1,
    "countryName": "United States",
    "countryCode": "US",
    "alpha2Code": "US",
    "alpha3Code": "USA",
    "currencyCode": "USD",
    "region": "North America",
    "isActive": true,
    "createdDate": "2025-09-19T10:00:00Z",
    "createdBy": 1,
    "createdByName": "System Admin",
    "modifiedDate": "2025-09-19T10:00:00Z",
    "modifiedBy": 1,
    "modifiedByName": "System Admin",
    "citiesCount": 45,
    "companiesCount": 156
  },
  "isSuccess": true,
  "message": "Country created successfully",
  "errorCode": null
}
```

## Create City Request
```json
{
  "cityName": "New York",
  "countryId": 1,
  "stateProvince": "New York",
  "postalCode": "10001",
  "timeZone": "America/New_York",
  "isActive": true,
  "createdBy": 1
}
```

## Create City Response
```json
{
  "data": {
    "id": 1,
    "cityName": "New York",
    "countryId": 1,
    "countryName": "United States",
    "stateProvince": "New York",
    "postalCode": "10001",
    "timeZone": "America/New_York",
    "isActive": true,
    "createdDate": "2025-09-19T10:00:00Z",
    "createdBy": 1,
    "createdByName": "System Admin",
    "modifiedDate": "2025-09-19T10:00:00Z",
    "modifiedBy": 1,
    "modifiedByName": "System Admin",
    "sitesCount": 25
  },
  "isSuccess": true,
  "message": "City created successfully",
  "errorCode": null
}
```

## Create Company Request
```json
{
  "companyName": "Acme Corporation",
  "companyCode": "ACME-001",
  "description": "Leading manufacturing company specializing in industrial equipment",
  "address": "123 Industrial Blvd",
  "cityId": 1,
  "countryId": 1,
  "postalCode": "10001",
  "phone": "+1-555-123-4567",
  "email": "info@acmecorp.com",
  "website": "https://www.acmecorp.com",
  "contactPerson": "John Doe",
  "contactEmail": "john.doe@acmecorp.com",
  "contactPhone": "+1-555-123-4567",
  "industry": "Manufacturing",
  "employeeCount": 2500,
  "companyType": "Customer",
  "taxId": "12-3456789",
  "registrationNumber": "REG-NY-001",
  "isActive": true,
  "createdBy": 1
}
```

## Create Company Response
```json
{
  "data": {
    "id": 1,
    "companyName": "Acme Corporation",
    "companyCode": "ACME-001",
    "description": "Leading manufacturing company specializing in industrial equipment",
    "address": "123 Industrial Blvd",
    "cityId": 1,
    "cityName": "New York",
    "countryId": 1,
    "countryName": "United States",
    "postalCode": "10001",
    "phone": "+1-555-123-4567",
    "email": "info@acmecorp.com",
    "website": "https://www.acmecorp.com",
    "contactPerson": "John Doe",
    "contactEmail": "john.doe@acmecorp.com",
    "contactPhone": "+1-555-123-4567",
    "industry": "Manufacturing",
    "employeeCount": 2500,
    "companyType": "Customer",
    "taxId": "12-3456789",
    "registrationNumber": "REG-NY-001",
    "isActive": true,
    "createdDate": "2025-09-19T10:00:00Z",
    "createdBy": 1,
    "createdByName": "System Admin",
    "modifiedDate": "2025-09-19T10:00:00Z",
    "modifiedBy": 1,
    "modifiedByName": "System Admin",
    "sitesCount": 3,
    "contractsCount": 2,
    "certificatesCount": 5,
    "auditsCount": 8
  },
  "isSuccess": true,
  "message": "Company created successfully",
  "errorCode": null
}
```

## Create Site Request
```json
{
  "siteName": "Main Manufacturing Plant",
  "siteCode": "ACME-MAIN-001",
  "companyId": 1,
  "cityId": 1,
  "address": "123 Industrial Blvd",
  "postalCode": "10001",
  "phone": "+1-555-123-4567",
  "email": "plant@acmecorp.com",
  "contactPerson": "Jane Smith",
  "contactPhone": "+1-555-123-4568",
  "siteType": "Manufacturing",
  "operationalStatus": "Active",
  "establishedDate": "1995-03-15T00:00:00Z",
  "employeeCount": 450,
  "totalArea": 25000.0,
  "operatingHours": "Monday-Friday 7:00 AM - 6:00 PM",
  "isActive": true,
  "createdBy": 1
}
```

## Create Site Response
```json
{
  "data": {
    "id": 1,
    "siteName": "Main Manufacturing Plant",
    "siteCode": "ACME-MAIN-001",
    "companyId": 1,
    "companyName": "Acme Corporation",
    "cityId": 1,
    "cityName": "New York",
    "address": "123 Industrial Blvd",
    "postalCode": "10001",
    "phone": "+1-555-123-4567",
    "email": "plant@acmecorp.com",
    "contactPerson": "Jane Smith",
    "contactPhone": "+1-555-123-4568",
    "siteType": "Manufacturing",
    "operationalStatus": "Active",
    "establishedDate": "1995-03-15T00:00:00Z",
    "employeeCount": 450,
    "totalArea": 25000.0,
    "operatingHours": "Monday-Friday 7:00 AM - 6:00 PM",
    "isActive": true,
    "createdDate": "2025-09-19T10:00:00Z",
    "createdBy": 1,
    "createdByName": "System Admin",
    "modifiedDate": "2025-09-19T10:00:00Z",
    "modifiedBy": 1,
    "modifiedByName": "System Admin",
    "certificatesCount": 3,
    "auditsCount": 5,
    "contractsCount": 2
  },
  "isSuccess": true,
  "message": "Site created successfully",
  "errorCode": null
}
```

## Create Service Request
```json
{
  "serviceName": "ISO 9001 Management System Certification",
  "serviceCode": "ISO9001-CERT",
  "description": "Quality Management System certification according to ISO 9001:2015 standard",
  "serviceCategory": "Certification",
  "standard": "ISO 9001:2015",
  "duration": 3,
  "validityPeriod": 36,
  "basePrice": 15000.00,
  "currency": "USD",
  "isActive": true,
  "createdBy": 1
}
```

## Create Service Response
```json
{
  "data": {
    "id": 1,
    "serviceName": "ISO 9001 Management System Certification",
    "serviceCode": "ISO9001-CERT",
    "description": "Quality Management System certification according to ISO 9001:2015 standard",
    "serviceCategory": "Certification",
    "standard": "ISO 9001:2015",
    "duration": 3,
    "validityPeriod": 36,
    "basePrice": 15000.00,
    "currency": "USD",
    "isActive": true,
    "createdDate": "2025-09-19T10:00:00Z",
    "createdBy": 1,
    "createdByName": "System Admin",
    "modifiedDate": "2025-09-19T10:00:00Z",
    "modifiedBy": 1,
    "modifiedByName": "System Admin",
    "contractsCount": 15,
    "certificatesCount": 25,
    "auditsCount": 40
  },
  "isSuccess": true,
  "message": "Service created successfully",
  "errorCode": null
}
```

## Create Role Request
```json
{
  "roleName": "Lead Auditor",
  "description": "Certified lead auditor responsible for conducting certification and surveillance audits",
  "permissions": "audit.create,audit.read,audit.update,finding.create,finding.read,finding.update,certificate.read",
  "roleCategory": "Auditor",
  "isSystemRole": false,
  "isActive": true,
  "createdBy": 1
}
```

## Create Role Response
```json
{
  "data": {
    "id": 1,
    "roleName": "Lead Auditor",
    "description": "Certified lead auditor responsible for conducting certification and surveillance audits",
    "permissions": "audit.create,audit.read,audit.update,finding.create,finding.read,finding.update,certificate.read",
    "roleCategory": "Auditor",
    "isSystemRole": false,
    "isActive": true,
    "createdDate": "2025-09-19T10:00:00Z",
    "createdBy": 1,
    "createdByName": "System Admin",
    "modifiedDate": "2025-09-19T10:00:00Z",
    "modifiedBy": 1,
    "modifiedByName": "System Admin",
    "usersCount": 8
  },
  "isSuccess": true,
  "message": "Role created successfully",
  "errorCode": null
}
```

## Countries List Response
```json
{
  "data": {
    "countries": [
      {
        "id": 1,
        "countryName": "United States",
        "countryCode": "US",
        "alpha2Code": "US",
        "alpha3Code": "USA",
        "region": "North America",
        "isActive": true,
        "citiesCount": 45,
        "companiesCount": 156
      },
      {
        "id": 2,
        "countryName": "United Kingdom",
        "countryCode": "GB",
        "alpha2Code": "GB",
        "alpha3Code": "GBR",
        "region": "Europe",
        "isActive": true,
        "citiesCount": 28,
        "companiesCount": 89
      }
    ],
    "totalCount": 2,
    "pageNumber": 1,
    "pageSize": 10,
    "hasNextPage": false,
    "hasPreviousPage": false
  },
  "isSuccess": true,
  "message": "Data retrieved successfully",
  "errorCode": null
}
```

## Cities by Country Response
```json
{
  "data": {
    "cities": [
      {
        "id": 1,
        "cityName": "New York",
        "stateProvince": "New York",
        "postalCode": "10001",
        "isActive": true,
        "sitesCount": 25
      },
      {
        "id": 2,
        "cityName": "Los Angeles",
        "stateProvince": "California",
        "postalCode": "90001",
        "isActive": true,
        "sitesCount": 18
      }
    ],
    "countryName": "United States",
    "totalCount": 2
  },
  "isSuccess": true,
  "message": "Cities retrieved successfully",
  "errorCode": null
}
```

## Company Search Request
```json
{
  "searchTerm": "Acme",
  "countryIds": [1, 2],
  "cityIds": [1, 3, 5],
  "industries": ["Manufacturing", "Technology"],
  "companyTypes": ["Customer", "Prospect"],
  "employeeCountFrom": 100,
  "employeeCountTo": 5000,
  "isActive": true,
  "pageNumber": 1,
  "pageSize": 10,
  "sortBy": "companyName",
  "sortDirection": "asc"
}
```

## Sites by Company Response
```json
{
  "data": {
    "sites": [
      {
        "id": 1,
        "siteName": "Main Manufacturing Plant",
        "siteCode": "ACME-MAIN-001",
        "cityName": "New York",
        "siteType": "Manufacturing",
        "operationalStatus": "Active",
        "employeeCount": 450,
        "isActive": true,
        "certificatesCount": 3,
        "auditsCount": 5
      },
      {
        "id": 2,
        "siteName": "West Coast Facility",
        "siteCode": "ACME-WEST-002",
        "cityName": "Los Angeles",
        "siteType": "Distribution",
        "operationalStatus": "Active",
        "employeeCount": 125,
        "isActive": true,
        "certificatesCount": 1,
        "auditsCount": 2
      }
    ],
    "companyName": "Acme Corporation",
    "totalCount": 2
  },
  "isSuccess": true,
  "message": "Sites retrieved successfully",
  "errorCode": null
}
```

## Services by Category Response
```json
{
  "data": {
    "services": [
      {
        "id": 1,
        "serviceName": "ISO 9001 Management System Certification",
        "serviceCode": "ISO9001-CERT",
        "standard": "ISO 9001:2015",
        "basePrice": 15000.00,
        "currency": "USD",
        "validityPeriod": 36,
        "isActive": true
      },
      {
        "id": 2,
        "serviceName": "ISO 14001 Environmental Management Certification",
        "serviceCode": "ISO14001-CERT",
        "standard": "ISO 14001:2015",
        "basePrice": 12000.00,
        "currency": "USD",
        "validityPeriod": 36,
        "isActive": true
      }
    ],
    "category": "Certification",
    "totalCount": 2
  },
  "isSuccess": true,
  "message": "Services retrieved successfully",
  "errorCode": null
}
```

## Update Company Request
```json
{
  "id": 1,
  "companyName": "Acme Corporation Ltd",
  "employeeCount": 2750,
  "website": "https://www.acme-corp.com",
  "contactPerson": "John Smith",
  "contactEmail": "john.smith@acme-corp.com",
  "modifiedBy": 3
}
```

## Bulk Upload Request
```json
{
  "entityType": "companies",
  "data": [
    {
      "companyName": "Tech Innovations Inc",
      "companyCode": "TECH-001",
      "industry": "Technology",
      "countryId": 1,
      "email": "info@techinnovations.com"
    },
    {
      "companyName": "Green Solutions LLC",
      "companyCode": "GREEN-001",
      "industry": "Environmental",
      "countryId": 1,
      "email": "contact@greensolutions.com"
    }
  ],
  "validateOnly": false,
  "uploadedBy": 1
}
```

## Bulk Upload Response
```json
{
  "data": {
    "totalRecords": 2,
    "successfulImports": 2,
    "failedImports": 0,
    "warnings": 0,
    "results": [
      {
        "rowNumber": 1,
        "status": "Success",
        "entityId": 25,
        "companyName": "Tech Innovations Inc"
      },
      {
        "rowNumber": 2,
        "status": "Success",
        "entityId": 26,
        "companyName": "Green Solutions LLC"
      }
    ],
    "errors": [],
    "warnings": []
  },
  "isSuccess": true,
  "message": "Bulk import completed successfully",
  "errorCode": null
}
```

## Error Response Examples
```json
{
  "data": null,
  "isSuccess": false,
  "message": "Country not found",
  "errorCode": "COUNTRY_NOT_FOUND"
}
```

```json
{
  "data": null,
  "isSuccess": false,
  "message": "Company code already exists",
  "errorCode": "DUPLICATE_COMPANY_CODE"
}
```

```json
{
  "data": null,
  "isSuccess": false,
  "message": "Cannot delete country: Cities exist",
  "errorCode": "COUNTRY_HAS_DEPENDENCIES"
}
```