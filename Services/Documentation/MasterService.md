# Customer Portal API Documentation

## 🏗️ Microservices Architecture Overview

The Customer Portal has been transformed from a monolithic application into a microservices architecture with the following services:

| Service | Port | Responsibility | Status |
|---------|------|---------------|---------|
| Master Service | 5067 | Core master data (Countries, Cities, Companies, Sites, Services, Roles, Chapters, Clauses, Focus Areas) | ✅ **Implemented** |
| Users Service | 5105 | User management, authentication, authorization | ⚠️ **Basic Structure** |
| Audits Service | 5009 | Audit management and tracking | ⚠️ **Basic Structure** |
| Actions Service | 5209 | Task and action management | ⚠️ **Basic Structure** |
| Certificates Service | 5277 | Certificate management and tracking | ⚠️ **Basic Structure** |
| Contracts Service | 5149 | Contract management | ⚠️ **Basic Structure** |
| Financial Service | 5010 | Invoicing and financial operations | ⚠️ **Basic Structure** |
| Findings Service | 5218 | Audit findings management | ⚠️ **Basic Structure** |
| Notifications Service | 5202 | Notification and messaging system | ⚠️ **Basic Structure** |
| Settings Service | 5075 | Application configuration and settings | ⚠️ **Basic Structure** |
| Overview Service | 5181 | Dashboard and reporting | ⚠️ **Basic Structure** |
| Widgets Service | 5266 | UI widgets and components | ⚠️ **Basic Structure** |
| API Gateway | 5271 | Unified entry point and request routing | ⚠️ **Basic Structure** |

---

## 🏠 Master Service Documentation

**Base URL:** `http://localhost:5067`  
**GraphQL Endpoint:** `http://localhost:5067/graphql`  
**Status:** ✅ Running

### 📊 Entities Overview

The Master Service manages core reference data used across the entire system:

1. **Countries** - Geographic country information
2. **Cities** - City information linked to countries
3. **Companies** - Client company information
4. **Sites** - Company site/location information
5. **Services** - Business services offered
6. **Roles** - User roles and permissions
7. **Chapters** - Audit framework chapters
8. **Clauses** - Audit clauses within chapters
9. **Focus Areas** - Audit focus categories

---

## 🌍 Countries Operations

### Query: Get All Countries

**Operation:** `countries`

**GraphQL Query:**
```graphql
query GetAllCountries {
  countries {
    id
    countryName
    countryCode
    currencyCode
    region
    isActive
    createdDate
    modifiedDate
    cities {
      id
      cityName
      stateProvince
      postalCode
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "countries": [
      {
        "id": 1,
        "countryName": "United States",
        "countryCode": "US",
        "currencyCode": "USD",
        "region": "North America",
        "isActive": true,
        "createdDate": "2024-01-01T00:00:00Z",
        "modifiedDate": null,
        "cities": [
          {
            "id": 1,
            "cityName": "New York",
            "stateProvince": "NY",
            "postalCode": "10001"
          }
        ]
      }
    ]
  }
}
```

### Query: Get Country by ID

**Operation:** `countryById`

**GraphQL Query:**
```graphql
query GetCountryById($id: Int!) {
  countryById(id: $id) {
    id
    countryName
    countryCode
    currencyCode
    region
    isActive
    cities {
      id
      cityName
      stateProvince
    }
  }
}
```

**Variables:**
```json
{
  "id": 1
}
```

**Response Schema:**
```json
{
  "data": {
    "countryById": {
      "id": 1,
      "countryName": "United States",
      "countryCode": "US",
      "currencyCode": "USD",
      "region": "North America",
      "isActive": true,
      "cities": [...]
    }
  }
}
```

### Query: Get Country by Code

**Operation:** `countryByCode`

**GraphQL Query:**
```graphql
query GetCountryByCode($countryCode: String!) {
  countryByCode(countryCode: $countryCode) {
    id
    countryName
    countryCode
    currencyCode
    region
  }
}
```

**Variables:**
```json
{
  "countryCode": "US"
}
```

### Query: Get Countries by Region

**Operation:** `countriesByRegion`

**GraphQL Query:**
```graphql
query GetCountriesByRegion($region: String!) {
  countriesByRegion(region: $region) {
    id
    countryName
    countryCode
    region
  }
}
```

**Variables:**
```json
{
  "region": "North America"
}
```

---

## 🏙️ Cities Operations

### Query: Get All Cities

**Operation:** `cities`

**GraphQL Query:**
```graphql
query GetAllCities {
  cities {
    id
    cityName
    countryId
    stateProvince
    postalCode
    isActive
    createdDate
    country {
      id
      countryName
      countryCode
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "cities": [
      {
        "id": 1,
        "cityName": "New York",
        "countryId": 1,
        "stateProvince": "NY",
        "postalCode": "10001",
        "isActive": true,
        "createdDate": "2024-01-01T00:00:00Z",
        "country": {
          "id": 1,
          "countryName": "United States",
          "countryCode": "US"
        }
      }
    ]
  }
}
```

### Query: Get Cities by Country

**Operation:** `citiesByCountry`

**GraphQL Query:**
```graphql
query GetCitiesByCountry($countryId: Int!) {
  citiesByCountry(countryId: $countryId) {
    id
    cityName
    stateProvince
    postalCode
    country {
      countryName
    }
  }
}
```

**Variables:**
```json
{
  "countryId": 1
}
```

---

## 🏢 Companies Operations

### Query: Get All Companies

**Operation:** `companies`

**GraphQL Query:**
```graphql
query GetAllCompanies {
  companies {
    id
    companyName
    companyCode
    address
    countryId
    phone
    email
    website
    contactPerson
    companyTypeName
    isActive
    createdDate
    modifiedDate
    country {
      id
      countryName
      countryCode
    }
    sites {
      id
      siteName
      siteCode
      address
      phone
      email
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "companies": [
      {
        "id": 1,
        "companyName": "Acme Corporation",
        "companyCode": "ACME001",
        "address": "123 Business St, Business City",
        "countryId": 1,
        "phone": "+1-555-0123",
        "email": "contact@acme.com",
        "website": "https://www.acme.com",
        "contactPerson": "John Doe",
        "companyTypeName": "Manufacturing",
        "isActive": true,
        "createdDate": "2024-01-01T00:00:00Z",
        "modifiedDate": null,
        "country": {
          "id": 1,
          "countryName": "United States",
          "countryCode": "US"
        },
        "sites": [
          {
            "id": 1,
            "siteName": "Main Production Facility",
            "siteCode": "ACME-PROD-01",
            "address": "456 Factory Rd",
            "phone": "+1-555-0124",
            "email": "production@acme.com"
          }
        ]
      }
    ]
  }
}
```

### Query: Get Company by Code

**Operation:** `companyByCode`

**GraphQL Query:**
```graphql
query GetCompanyByCode($companyCode: String!) {
  companyByCode(companyCode: $companyCode) {
    id
    companyName
    companyCode
    email
    website
    sites {
      id
      siteName
      siteCode
    }
  }
}
```

**Variables:**
```json
{
  "companyCode": "ACME001"
}
```

---

## 🏭 Sites Operations

### Query: Get All Sites

**Operation:** `sites`

**GraphQL Query:**
```graphql
query GetAllSites {
  sites {
    id
    siteName
    siteCode
    companyId
    cityId
    address
    phone
    email
    contactPerson
    siteTypeName
    isActive
    createdDate
    company {
      id
      companyName
      companyCode
    }
    city {
      id
      cityName
      country {
        countryName
      }
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "sites": [
      {
        "id": 1,
        "siteName": "Main Production Facility",
        "siteCode": "ACME-PROD-01",
        "companyId": 1,
        "cityId": 1,
        "address": "456 Factory Rd",
        "phone": "+1-555-0124",
        "email": "production@acme.com",
        "contactPerson": "Jane Smith",
        "siteTypeName": "Manufacturing",
        "isActive": true,
        "createdDate": "2024-01-01T00:00:00Z",
        "company": {
          "id": 1,
          "companyName": "Acme Corporation",
          "companyCode": "ACME001"
        },
        "city": {
          "id": 1,
          "cityName": "New York",
          "country": {
            "countryName": "United States"
          }
        }
      }
    ]
  }
}
```

### Query: Get Sites by Company

**Operation:** `sitesByCompany`

**GraphQL Query:**
```graphql
query GetSitesByCompany($companyId: Int!) {
  sitesByCompany(companyId: $companyId) {
    id
    siteName
    siteCode
    address
    phone
    email
    siteTypeName
  }
}
```

**Variables:**
```json
{
  "companyId": 1
}
```

---

## ⚙️ Services Operations

### Query: Get All Services

**Operation:** `services`

**GraphQL Query:**
```graphql
query GetAllServices {
  services {
    id
    serviceName
    serviceCode
    description
    serviceCategory
    isActive
    createdDate
    modifiedDate
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "services": [
      {
        "id": 1,
        "serviceName": "ISO 9001 Audit",
        "serviceCode": "ISO9001",
        "description": "Quality Management System Audit",
        "serviceCategory": "Quality",
        "isActive": true,
        "createdDate": "2024-01-01T00:00:00Z",
        "modifiedDate": null
      }
    ]
  }
}
```

---

## 👥 Roles Operations

### Query: Get All Roles

**Operation:** `roles`

**GraphQL Query:**
```graphql
query GetAllRoles {
  roles {
    id
    roleName
    description
    permissions
    isActive
    createdDate
    modifiedDate
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "roles": [
      {
        "id": 1,
        "roleName": "Administrator",
        "description": "Full system access",
        "permissions": "CREATE,READ,UPDATE,DELETE,ADMIN",
        "isActive": true,
        "createdDate": "2024-01-01T00:00:00Z",
        "modifiedDate": null
      }
    ]
  }
}
```

---

## 📚 Chapters Operations

### Query: Get All Chapters

**Operation:** `chapters`

**GraphQL Query:**
```graphql
query GetAllChapters {
  chapters {
    id
    chapterName
    chapterNumber
    description
    isActive
    createdDate
    clauses {
      id
      clauseName
      clauseNumber
      description
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "chapters": [
      {
        "id": 1,
        "chapterName": "Quality Management",
        "chapterNumber": "4.0",
        "description": "Quality management system requirements",
        "isActive": true,
        "createdDate": "2024-01-01T00:00:00Z",
        "clauses": [
          {
            "id": 1,
            "clauseName": "Context of the organization",
            "clauseNumber": "4.1",
            "description": "Understanding the organization and its context"
          }
        ]
      }
    ]
  }
}
```

---

## 📋 Clauses Operations

### Query: Get All Clauses

**Operation:** `clauses`

**GraphQL Query:**
```graphql
query GetAllClauses {
  clauses {
    id
    clauseName
    clauseNumber
    chapterId
    description
    isActive
    createdDate
    chapter {
      id
      chapterName
      chapterNumber
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "clauses": [
      {
        "id": 1,
        "clauseName": "Context of the organization",
        "clauseNumber": "4.1",
        "chapterId": 1,
        "description": "Understanding the organization and its context",
        "isActive": true,
        "createdDate": "2024-01-01T00:00:00Z",
        "chapter": {
          "id": 1,
          "chapterName": "Quality Management",
          "chapterNumber": "4.0"
        }
      }
    ]
  }
}
```

---

## 🎯 Focus Areas Operations

### Query: Get All Focus Areas

**Operation:** `focusAreas`

**GraphQL Query:**
```graphql
query GetAllFocusAreas {
  focusAreas {
    id
    focusAreaName
    description
    isActive
    createdDate
    modifiedDate
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "focusAreas": [
      {
        "id": 1,
        "focusAreaName": "Document Control",
        "description": "Management of documented information",
        "isActive": true,
        "createdDate": "2024-01-01T00:00:00Z",
        "modifiedDate": null
      }
    ]
  }
}
```

---

## 🔧 Error Handling

All GraphQL operations follow standard error handling patterns:

### Success Response
```json
{
  "data": {
    "operationName": [...]
  }
}
```

### Error Response
```json
{
  "errors": [
    {
      "message": "Entity not found",
      "extensions": {
        "code": "ENTITY_NOT_FOUND",
        "entityType": "Country",
        "entityId": 999
      }
    }
  ],
  "data": null
}
```

### Common Error Codes

| Code | Description |
|------|-------------|
| `ENTITY_NOT_FOUND` | Requested entity does not exist |
| `VALIDATION_ERROR` | Input validation failed |
| `DATABASE_ERROR` | Database operation failed |
| `UNAUTHORIZED_ACCESS` | User lacks required permissions |
| `SERVICE_UNAVAILABLE` | Service is temporarily unavailable |

---

## 📊 Testing Examples

### Using GraphQL Playground
1. Open `http://localhost:5067/graphql` in browser
2. Use the schema explorer to browse available operations
3. Write queries in the left panel
4. Execute and see results in the right panel

### Using PowerShell
```powershell
$headers = @{'Content-Type' = 'application/json'}
$query = '{"query": "query { countries { id countryName countryCode } }"}'
Invoke-RestMethod -Uri 'http://localhost:5067/graphql' -Method POST -Headers $headers -Body $query
```

### Using cURL
```bash
curl -X POST http://localhost:5067/graphql \
  -H "Content-Type: application/json" \
  -d '{"query": "query { countries { id countryName countryCode } }"}'
```

---
