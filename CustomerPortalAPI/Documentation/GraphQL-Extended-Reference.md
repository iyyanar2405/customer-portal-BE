# Customer Portal API - Extended GraphQL Reference
## Certificates Module GraphQL Examples

### 🔍 **Query: Get All Certificates**
```graphql
query GetCertificates($filter: CertificateFilterInput, $search: CertificateSearchInput) {
  certificates(filter: $filter, search: $search) {
    certificates {
      id
      certificateNumber
      certificateName
      companyId
      companyName
      siteId
      siteName
      serviceId
      serviceName
      issueDate
      expiryDate
      status
      issuingBody
      scope
      standardReference
      auditId
      isActive
      createdDate
      modifiedDate
      createdBy
      modifiedBy
      additionalScopes {
        id
        certificateId
        scopeDescription
        isActive
      }
      certificateServices {
        id
        certificateId
        serviceId
        serviceName
        isActive
      }
    }
    totalCount
    pageNumber
    pageSize
    hasNextPage
    hasPreviousPage
  }
}
```

**Variables:**
```json
{
  "filter": {
    "companyIds": [1, 2],
    "siteIds": [5, 10],
    "serviceIds": [1, 3, 5],
    "statuses": ["Valid", "Pending"],
    "issueDateFrom": "2024-01-01T00:00:00Z",
    "issueDateTo": "2025-12-31T23:59:59Z",
    "expiryDateFrom": "2025-01-01T00:00:00Z",
    "expiryDateTo": "2026-12-31T23:59:59Z",
    "issuingBody": "BSI",
    "standardReference": "ISO 9001:2015",
    "isActive": true
  },
  "search": {
    "searchTerm": "quality",
    "pageNumber": 1,
    "pageSize": 20,
    "sortBy": "ExpiryDate",
    "sortDirection": "ASC"
  }
}
```

**Response:**
```json
{
  "data": {
    "certificates": {
      "certificates": [
        {
          "id": 1,
          "certificateNumber": "BSI-QMS-2024-001",
          "certificateName": "Quality Management System Certificate",
          "companyId": 1,
          "companyName": "ABC Manufacturing Ltd",
          "siteId": 5,
          "siteName": "Main Manufacturing Plant",
          "serviceId": 1,
          "serviceName": "ISO 9001",
          "issueDate": "2024-06-15T00:00:00Z",
          "expiryDate": "2027-06-14T23:59:59Z",
          "status": "Valid",
          "issuingBody": "BSI",
          "scope": "Design, manufacture and supply of industrial equipment",
          "standardReference": "ISO 9001:2015",
          "auditId": 101,
          "isActive": true,
          "createdDate": "2024-06-16T10:00:00Z",
          "modifiedDate": "2024-06-16T10:00:00Z",
          "createdBy": 100,
          "modifiedBy": 100,
          "additionalScopes": [
            {
              "id": 1,
              "certificateId": 1,
              "scopeDescription": "Including calibration services",
              "isActive": true
            }
          ],
          "certificateServices": [
            {
              "id": 1,
              "certificateId": 1,
              "serviceId": 1,
              "serviceName": "ISO 9001",
              "isActive": true
            }
          ]
        }
      ],
      "totalCount": 25,
      "pageNumber": 1,
      "pageSize": 20,
      "hasNextPage": true,
      "hasPreviousPage": false
    }
  }
}
```

### 🔍 **Query: Get Certificate by ID**
```graphql
query GetCertificateById($id: Int!) {
  certificateById(id: $id) {
    id
    certificateNumber
    certificateName
    companyId
    companyName
    siteId
    siteName
    serviceId
    serviceName
    issueDate
    expiryDate
    status
    issuingBody
    scope
    standardReference
    auditId
    auditName
    certificateFile
    notes
    isActive
    createdDate
    modifiedDate
    createdBy
    modifiedBy
    createdByName
    modifiedByName
    additionalScopes {
      id
      certificateId
      scopeDescription
      effectiveDate
      expiryDate
      isActive
      createdDate
      createdBy
    }
    certificateServices {
      id
      certificateId
      serviceId
      serviceName
      serviceDescription
      standardVersion
      isActive
      createdDate
      createdBy
    }
    certificateSites {
      id
      certificateId
      siteId
      siteName
      siteAddress
      isActive
      createdDate
      createdBy
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

**Response:**
```json
{
  "data": {
    "certificateById": {
      "id": 1,
      "certificateNumber": "BSI-QMS-2024-001",
      "certificateName": "Quality Management System Certificate",
      "companyId": 1,
      "companyName": "ABC Manufacturing Ltd",
      "siteId": 5,
      "siteName": "Main Manufacturing Plant",
      "serviceId": 1,
      "serviceName": "ISO 9001",
      "issueDate": "2024-06-15T00:00:00Z",
      "expiryDate": "2027-06-14T23:59:59Z",
      "status": "Valid",
      "issuingBody": "BSI",
      "scope": "Design, manufacture and supply of industrial equipment",
      "standardReference": "ISO 9001:2015",
      "auditId": 101,
      "auditName": "Annual Quality Management Audit",
      "certificateFile": "/certificates/BSI-QMS-2024-001.pdf",
      "notes": "Certificate includes all production lines",
      "isActive": true,
      "createdDate": "2024-06-16T10:00:00Z",
      "modifiedDate": "2024-06-16T10:00:00Z",
      "createdBy": 100,
      "modifiedBy": 100,
      "createdByName": "Admin User",
      "modifiedByName": "Admin User",
      "additionalScopes": [
        {
          "id": 1,
          "certificateId": 1,
          "scopeDescription": "Including calibration services for measuring equipment",
          "effectiveDate": "2024-06-15T00:00:00Z",
          "expiryDate": "2027-06-14T23:59:59Z",
          "isActive": true,
          "createdDate": "2024-06-16T10:05:00Z",
          "createdBy": 100
        }
      ],
      "certificateServices": [
        {
          "id": 1,
          "certificateId": 1,
          "serviceId": 1,
          "serviceName": "ISO 9001",
          "serviceDescription": "Quality Management System",
          "standardVersion": "ISO 9001:2015",
          "isActive": true,
          "createdDate": "2024-06-16T10:10:00Z",
          "createdBy": 100
        }
      ],
      "certificateSites": [
        {
          "id": 1,
          "certificateId": 1,
          "siteId": 5,
          "siteName": "Main Manufacturing Plant",
          "siteAddress": "123 Industrial Park, Manufacturing City",
          "isActive": true,
          "createdDate": "2024-06-16T10:15:00Z",
          "createdBy": 100
        }
      ]
    }
  }
}
```

### ✏️ **Mutation: Create Certificate**
```graphql
mutation CreateCertificate($input: CertificateInput!) {
  createCertificate(input: $input) {
    certificate {
      id
      certificateNumber
      certificateName
      companyId
      siteId
      serviceId
      issueDate
      expiryDate
      status
      issuingBody
      scope
      standardReference
      createdDate
    }
    errorMessage
  }
}
```

**Variables:**
```json
{
  "input": {
    "certificateNumber": "BSI-EMS-2024-002",
    "certificateName": "Environmental Management System Certificate",
    "companyId": 1,
    "siteId": 5,
    "serviceId": 2,
    "issueDate": "2024-08-01T00:00:00Z",
    "expiryDate": "2027-07-31T23:59:59Z",
    "status": "Valid",
    "issuingBody": "BSI",
    "scope": "Environmental management for manufacturing operations",
    "standardReference": "ISO 14001:2015",
    "auditId": 102,
    "notes": "Covers all environmental aspects of production",
    "createdBy": 100
  }
}
```

**Response:**
```json
{
  "data": {
    "createCertificate": {
      "certificate": {
        "id": 26,
        "certificateNumber": "BSI-EMS-2024-002",
        "certificateName": "Environmental Management System Certificate",
        "companyId": 1,
        "siteId": 5,
        "serviceId": 2,
        "issueDate": "2024-08-01T00:00:00Z",
        "expiryDate": "2027-07-31T23:59:59Z",
        "status": "Valid",
        "issuingBody": "BSI",
        "scope": "Environmental management for manufacturing operations",
        "standardReference": "ISO 14001:2015",
        "createdDate": "2025-01-20T11:30:00Z"
      },
      "errorMessage": null
    }
  }
}
```

---

## Contracts Module GraphQL Examples

### 🔍 **Query: Get All Contracts**
```graphql
query GetContracts($filter: ContractFilterInput, $search: ContractSearchInput) {
  contracts(filter: $filter, search: $search) {
    contracts {
      id
      contractNumber
      contractName
      companyId
      companyName
      contractType
      status
      startDate
      endDate
      value
      currency
      description
      contactPerson
      contactEmail
      contactPhone
      isActive
      createdDate
      modifiedDate
      createdBy
      modifiedBy
      contractServices {
        id
        contractId
        serviceId
        serviceName
        serviceValue
        isActive
      }
      contractSites {
        id
        contractId
        siteId
        siteName
        isActive
      }
    }
    totalCount
    pageNumber
    pageSize
    hasNextPage
    hasPreviousPage
  }
}
```

**Variables:**
```json
{
  "filter": {
    "companyIds": [1, 2],
    "contractTypes": ["Service Agreement", "Support Contract"],
    "statuses": ["Active", "Pending"],
    "startDateFrom": "2024-01-01T00:00:00Z",
    "startDateTo": "2025-12-31T23:59:59Z",
    "endDateFrom": "2025-01-01T00:00:00Z",
    "endDateTo": "2026-12-31T23:59:59Z",
    "valueFrom": 10000.00,
    "valueTo": 500000.00,
    "currency": "USD",
    "isActive": true
  },
  "search": {
    "searchTerm": "support",
    "pageNumber": 1,
    "pageSize": 25,
    "sortBy": "EndDate",
    "sortDirection": "ASC"
  }
}
```

**Response:**
```json
{
  "data": {
    "contracts": {
      "contracts": [
        {
          "id": 1,
          "contractNumber": "CON-2024-001",
          "contractName": "Annual Audit Services Agreement",
          "companyId": 1,
          "companyName": "ABC Manufacturing Ltd",
          "contractType": "Service Agreement",
          "status": "Active",
          "startDate": "2024-01-01T00:00:00Z",
          "endDate": "2024-12-31T23:59:59Z",
          "value": 75000.00,
          "currency": "USD",
          "description": "Comprehensive audit services for quality and environmental management systems",
          "contactPerson": "John Smith",
          "contactEmail": "john.smith@abcmanufacturing.com",
          "contactPhone": "+1-555-0123",
          "isActive": true,
          "createdDate": "2023-12-01T10:00:00Z",
          "modifiedDate": "2024-01-15T14:30:00Z",
          "createdBy": 100,
          "modifiedBy": 102,
          "contractServices": [
            {
              "id": 1,
              "contractId": 1,
              "serviceId": 1,
              "serviceName": "ISO 9001 Audit",
              "serviceValue": 25000.00,
              "isActive": true
            },
            {
              "id": 2,
              "contractId": 1,
              "serviceId": 2,
              "serviceName": "ISO 14001 Audit",
              "serviceValue": 20000.00,
              "isActive": true
            }
          ],
          "contractSites": [
            {
              "id": 1,
              "contractId": 1,
              "siteId": 5,
              "siteName": "Main Manufacturing Plant",
              "isActive": true
            },
            {
              "id": 2,
              "contractId": 1,
              "siteId": 6,
              "siteName": "Secondary Production Facility",
              "isActive": true
            }
          ]
        }
      ],
      "totalCount": 12,
      "pageNumber": 1,
      "pageSize": 25,
      "hasNextPage": false,
      "hasPreviousPage": false
    }
  }
}
```

### ✏️ **Mutation: Create Contract**
```graphql
mutation CreateContract($input: ContractInput!) {
  createContract(input: $input) {
    contract {
      id
      contractNumber
      contractName
      companyId
      contractType
      status
      startDate
      endDate
      value
      currency
      description
      contactPerson
      contactEmail
      createdDate
    }
    errorMessage
  }
}
```

**Variables:**
```json
{
  "input": {
    "contractNumber": "CON-2025-003",
    "contractName": "Safety Management System Support",
    "companyId": 2,
    "contractType": "Support Contract",
    "status": "Draft",
    "startDate": "2025-03-01T00:00:00Z",
    "endDate": "2026-02-28T23:59:59Z",
    "value": 45000.00,
    "currency": "USD",
    "description": "Ongoing support for ISO 45001 implementation and maintenance",
    "contactPerson": "Sarah Johnson",
    "contactEmail": "sarah.johnson@client.com",
    "contactPhone": "+1-555-0456",
    "terms": "Annual contract with quarterly reviews",
    "createdBy": 100
  }
}
```

**Response:**
```json
{
  "data": {
    "createContract": {
      "contract": {
        "id": 13,
        "contractNumber": "CON-2025-003",
        "contractName": "Safety Management System Support",
        "companyId": 2,
        "contractType": "Support Contract",
        "status": "Draft",
        "startDate": "2025-03-01T00:00:00Z",
        "endDate": "2026-02-28T23:59:59Z",
        "value": 45000.00,
        "currency": "USD",
        "description": "Ongoing support for ISO 45001 implementation and maintenance",
        "contactPerson": "Sarah Johnson",
        "contactEmail": "sarah.johnson@client.com",
        "createdDate": "2025-01-20T12:15:00Z"
      },
      "errorMessage": null
    }
  }
}
```

---

## Financial Module GraphQL Examples

### 🔍 **Query: Get All Invoices**
```graphql
query GetInvoices($filter: InvoiceFilterInput, $search: InvoiceSearchInput) {
  invoices(filter: $filter, search: $search) {
    invoices {
      id
      invoiceNumber
      companyId
      companyName
      contractId
      contractNumber
      issueDate
      dueDate
      paidDate
      amount
      currency
      status
      paymentStatus
      paymentMethod
      description
      taxAmount
      totalAmount
      discountAmount
      notes
      isActive
      createdDate
      modifiedDate
      createdBy
      modifiedBy
      invoiceItems {
        id
        invoiceId
        description
        quantity
        unitPrice
        amount
        serviceId
        serviceName
      }
      payments {
        id
        invoiceId
        paymentDate
        amount
        paymentMethod
        reference
        notes
      }
    }
    totalCount
    totalAmount
    paidAmount
    outstandingAmount
    pageNumber
    pageSize
    hasNextPage
    hasPreviousPage
  }
}
```

**Variables:**
```json
{
  "filter": {
    "companyIds": [1, 2],
    "contractIds": [1, 2],
    "statuses": ["Sent", "Overdue"],
    "paymentStatuses": ["Pending", "Partial"],
    "issueDateFrom": "2024-01-01T00:00:00Z",
    "issueDateTo": "2025-12-31T23:59:59Z",
    "dueDateFrom": "2024-12-01T00:00:00Z",
    "dueDateTo": "2025-03-31T23:59:59Z",
    "amountFrom": 1000.00,
    "amountTo": 100000.00,
    "currency": "USD",
    "isOverdue": true,
    "isActive": true
  },
  "search": {
    "searchTerm": "audit",
    "pageNumber": 1,
    "pageSize": 20,
    "sortBy": "DueDate",
    "sortDirection": "ASC"
  }
}
```

**Response:**
```json
{
  "data": {
    "invoices": {
      "invoices": [
        {
          "id": 1,
          "invoiceNumber": "INV-2024-001",
          "companyId": 1,
          "companyName": "ABC Manufacturing Ltd",
          "contractId": 1,
          "contractNumber": "CON-2024-001",
          "issueDate": "2024-12-01T00:00:00Z",
          "dueDate": "2024-12-31T23:59:59Z",
          "paidDate": null,
          "amount": 25000.00,
          "currency": "USD",
          "status": "Sent",
          "paymentStatus": "Pending",
          "paymentMethod": null,
          "description": "Q4 2024 Audit Services",
          "taxAmount": 3750.00,
          "totalAmount": 28750.00,
          "discountAmount": 0.00,
          "notes": "Payment terms: Net 30 days",
          "isActive": true,
          "createdDate": "2024-12-01T09:00:00Z",
          "modifiedDate": "2024-12-01T09:00:00Z",
          "createdBy": 100,
          "modifiedBy": 100,
          "invoiceItems": [
            {
              "id": 1,
              "invoiceId": 1,
              "description": "ISO 9001 Annual Surveillance Audit",
              "quantity": 1,
              "unitPrice": 15000.00,
              "amount": 15000.00,
              "serviceId": 1,
              "serviceName": "ISO 9001 Audit"
            },
            {
              "id": 2,
              "invoiceId": 1,
              "description": "ISO 14001 Annual Surveillance Audit",
              "quantity": 1,
              "unitPrice": 10000.00,
              "amount": 10000.00,
              "serviceId": 2,
              "serviceName": "ISO 14001 Audit"
            }
          ],
          "payments": []
        }
      ],
      "totalCount": 8,
      "totalAmount": 156000.00,
      "paidAmount": 89000.00,
      "outstandingAmount": 67000.00,
      "pageNumber": 1,
      "pageSize": 20,
      "hasNextPage": false,
      "hasPreviousPage": false
    }
  }
}
```

### 🔍 **Query: Get Financial Summary**
```graphql
query GetFinancialSummary($companyId: Int, $dateFrom: DateTime, $dateTo: DateTime) {
  financialSummary(companyId: $companyId, dateFrom: $dateFrom, dateTo: $dateTo) {
    totalRevenue
    totalInvoiced
    totalPaid
    totalOutstanding
    totalOverdue
    averagePaymentDays
    invoiceCount
    paidInvoiceCount
    overdueInvoiceCount
    topPayingCompanies {
      companyId
      companyName
      totalPaid
      invoiceCount
    }
    monthlyRevenue {
      month
      year
      revenue
      invoiceCount
    }
    paymentMethodStats {
      paymentMethod
      count
      totalAmount
      percentage
    }
  }
}
```

**Variables:**
```json
{
  "companyId": null,
  "dateFrom": "2024-01-01T00:00:00Z",
  "dateTo": "2024-12-31T23:59:59Z"
}
```

**Response:**
```json
{
  "data": {
    "financialSummary": {
      "totalRevenue": 450000.00,
      "totalInvoiced": 520000.00,
      "totalPaid": 378000.00,
      "totalOutstanding": 142000.00,
      "totalOverdue": 45000.00,
      "averagePaymentDays": 28.5,
      "invoiceCount": 156,
      "paidInvoiceCount": 123,
      "overdueInvoiceCount": 12,
      "topPayingCompanies": [
        {
          "companyId": 1,
          "companyName": "ABC Manufacturing Ltd",
          "totalPaid": 125000.00,
          "invoiceCount": 45
        },
        {
          "companyId": 2,
          "companyName": "XYZ Industries",
          "totalPaid": 89000.00,
          "invoiceCount": 32
        }
      ],
      "monthlyRevenue": [
        {
          "month": 1,
          "year": 2024,
          "revenue": 35000.00,
          "invoiceCount": 12
        },
        {
          "month": 2,
          "year": 2024,
          "revenue": 42000.00,
          "invoiceCount": 15
        }
      ],
      "paymentMethodStats": [
        {
          "paymentMethod": "Bank Transfer",
          "count": 89,
          "totalAmount": 267000.00,
          "percentage": 70.6
        },
        {
          "paymentMethod": "Check",
          "count": 34,
          "totalAmount": 111000.00,
          "percentage": 29.4
        }
      ]
    }
  }
}
```

### ✏️ **Mutation: Create Invoice**
```graphql
mutation CreateInvoice($input: InvoiceInput!) {
  createInvoice(input: $input) {
    invoice {
      id
      invoiceNumber
      companyId
      contractId
      issueDate
      dueDate
      amount
      currency
      status
      description
      taxAmount
      totalAmount
      createdDate
    }
    errorMessage
  }
}
```

**Variables:**
```json
{
  "input": {
    "invoiceNumber": "INV-2025-015",
    "companyId": 2,
    "contractId": 2,
    "issueDate": "2025-01-20T00:00:00Z",
    "dueDate": "2025-02-19T23:59:59Z",
    "amount": 18000.00,
    "currency": "USD",
    "status": "Draft",
    "description": "Q1 2025 Consulting Services",
    "taxRate": 0.15,
    "taxAmount": 2700.00,
    "totalAmount": 20700.00,
    "discountAmount": 0.00,
    "notes": "Net 30 payment terms",
    "items": [
      {
        "description": "Management System Consulting",
        "quantity": 20,
        "unitPrice": 900.00,
        "amount": 18000.00,
        "serviceId": 3
      }
    ],
    "createdBy": 100
  }
}
```

**Response:**
```json
{
  "data": {
    "createInvoice": {
      "invoice": {
        "id": 157,
        "invoiceNumber": "INV-2025-015",
        "companyId": 2,
        "contractId": 2,
        "issueDate": "2025-01-20T00:00:00Z",
        "dueDate": "2025-02-19T23:59:59Z",
        "amount": 18000.00,
        "currency": "USD",
        "status": "Draft",
        "description": "Q1 2025 Consulting Services",
        "taxAmount": 2700.00,
        "totalAmount": 20700.00,
        "createdDate": "2025-01-20T13:45:00Z"
      },
      "errorMessage": null
    }
  }
}
```

---

## Users Module GraphQL Examples

### 🔍 **Query: Get All Users**
```graphql
query GetUsers($filter: UserFilterInput, $search: UserSearchInput) {
  users(filter: $filter, search: $search) {
    users {
      id
      username
      email
      firstName
      lastName
      phone
      jobTitle
      department
      companyId
      companyName
      isActive
      lastLoginDate
      createdDate
      modifiedDate
      createdBy
      modifiedBy
      userRoles {
        id
        userId
        roleId
        roleName
        roleDescription
        isActive
        assignedDate
        assignedBy
      }
      userCompanyAccess {
        id
        userId
        companyId
        companyName
        accessLevel
        isActive
      }
      userSiteAccess {
        id
        userId
        siteId
        siteName
        accessLevel
        isActive
      }
      userServiceAccess {
        id
        userId
        serviceId
        serviceName
        accessLevel
        isActive
      }
    }
    totalCount
    pageNumber
    pageSize
    hasNextPage
    hasPreviousPage
  }
}
```

**Variables:**
```json
{
  "filter": {
    "companyIds": [1, 2],
    "departments": ["Quality", "Operations"],
    "roles": ["Auditor", "Manager"],
    "isActive": true,
    "lastLoginFrom": "2024-01-01T00:00:00Z",
    "lastLoginTo": "2025-01-20T23:59:59Z"
  },
  "search": {
    "searchTerm": "smith",
    "pageNumber": 1,
    "pageSize": 25,
    "sortBy": "LastName",
    "sortDirection": "ASC"
  }
}
```

**Response:**
```json
{
  "data": {
    "users": {
      "users": [
        {
          "id": 123,
          "username": "john.smith",
          "email": "john.smith@company.com",
          "firstName": "John",
          "lastName": "Smith",
          "phone": "+1-555-0123",
          "jobTitle": "Senior Auditor",
          "department": "Quality Assurance",
          "companyId": 1,
          "companyName": "ABC Manufacturing Ltd",
          "isActive": true,
          "lastLoginDate": "2025-01-19T14:30:00Z",
          "createdDate": "2023-06-01T09:00:00Z",
          "modifiedDate": "2024-12-15T16:45:00Z",
          "createdBy": 100,
          "modifiedBy": 102,
          "userRoles": [
            {
              "id": 1,
              "userId": 123,
              "roleId": 3,
              "roleName": "Lead Auditor",
              "roleDescription": "Authorized to lead audit teams and sign off on audit reports",
              "isActive": true,
              "assignedDate": "2023-06-01T09:00:00Z",
              "assignedBy": 100
            },
            {
              "id": 2,
              "userId": 123,
              "roleId": 5,
              "roleName": "Quality Manager",
              "roleDescription": "Responsible for quality management oversight",
              "isActive": true,
              "assignedDate": "2024-01-15T10:30:00Z",
              "assignedBy": 102
            }
          ],
          "userCompanyAccess": [
            {
              "id": 1,
              "userId": 123,
              "companyId": 1,
              "companyName": "ABC Manufacturing Ltd",
              "accessLevel": "Full",
              "isActive": true
            },
            {
              "id": 2,
              "userId": 123,
              "companyId": 2,
              "companyName": "XYZ Industries",
              "accessLevel": "Read",
              "isActive": true
            }
          ],
          "userSiteAccess": [
            {
              "id": 1,
              "userId": 123,
              "siteId": 5,
              "siteName": "Main Manufacturing Plant",
              "accessLevel": "Full",
              "isActive": true
            },
            {
              "id": 2,
              "userId": 123,
              "siteId": 6,
              "siteName": "Secondary Production Facility",
              "accessLevel": "Full",
              "isActive": true
            }
          ],
          "userServiceAccess": [
            {
              "id": 1,
              "userId": 123,
              "serviceId": 1,
              "serviceName": "ISO 9001",
              "accessLevel": "Full",
              "isActive": true
            },
            {
              "id": 2,
              "userId": 123,
              "serviceId": 2,
              "serviceName": "ISO 14001",
              "accessLevel": "Full",
              "isActive": true
            }
          ]
        }
      ],
      "totalCount": 45,
      "pageNumber": 1,
      "pageSize": 25,
      "hasNextPage": true,
      "hasPreviousPage": false
    }
  }
}
```

### ✏️ **Mutation: Create User**
```graphql
mutation CreateUser($input: UserInput!) {
  createUser(input: $input) {
    user {
      id
      username
      email
      firstName
      lastName
      phone
      jobTitle
      department
      companyId
      isActive
      createdDate
    }
    errorMessage
  }
}
```

**Variables:**
```json
{
  "input": {
    "username": "jane.doe",
    "email": "jane.doe@company.com",
    "firstName": "Jane",
    "lastName": "Doe",
    "phone": "+1-555-0456",
    "jobTitle": "Environmental Auditor",
    "department": "Environmental",
    "companyId": 1,
    "isActive": true,
    "roleIds": [4, 6],
    "companyAccess": [
      {
        "companyId": 1,
        "accessLevel": "Full"
      }
    ],
    "serviceAccess": [
      {
        "serviceId": 2,
        "accessLevel": "Full"
      },
      {
        "serviceId": 3,
        "accessLevel": "Read"
      }
    ],
    "createdBy": 100
  }
}
```

**Response:**
```json
{
  "data": {
    "createUser": {
      "user": {
        "id": 146,
        "username": "jane.doe",
        "email": "jane.doe@company.com",
        "firstName": "Jane",
        "lastName": "Doe",
        "phone": "+1-555-0456",
        "jobTitle": "Environmental Auditor",
        "department": "Environmental",
        "companyId": 1,
        "isActive": true,
        "createdDate": "2025-01-20T14:20:00Z"
      },
      "errorMessage": null
    }
  }
}
```

---

## Master Data Module GraphQL Examples

### 🔍 **Query: Get All Companies**
```graphql
query GetCompanies($filter: CompanyFilterInput, $search: CompanySearchInput) {
  companies(filter: $filter, search: $search) {
    companies {
      id
      companyName
      companyCode
      industry
      address
      city
      state
      country
      postalCode
      phone
      email
      website
      contactPerson
      contactTitle
      contactEmail
      contactPhone
      isActive
      createdDate
      modifiedDate
      createdBy
      modifiedBy
      sites {
        id
        siteName
        address
        city
        contactPerson
        phone
        isActive
      }
      services {
        id
        serviceName
        serviceDescription
        isActive
      }
    }
    totalCount
    pageNumber
    pageSize
    hasNextPage
    hasPreviousPage
  }
}
```

**Variables:**
```json
{
  "filter": {
    "industries": ["Manufacturing", "Technology"],
    "countries": ["United States", "Canada"],
    "states": ["California", "Texas"],
    "isActive": true
  },
  "search": {
    "searchTerm": "manufacturing",
    "pageNumber": 1,
    "pageSize": 20,
    "sortBy": "CompanyName",
    "sortDirection": "ASC"
  }
}
```

**Response:**
```json
{
  "data": {
    "companies": {
      "companies": [
        {
          "id": 1,
          "companyName": "ABC Manufacturing Ltd",
          "companyCode": "ABC001",
          "industry": "Manufacturing",
          "address": "123 Industrial Parkway",
          "city": "Manufacturing City",
          "state": "California",
          "country": "United States",
          "postalCode": "90210",
          "phone": "+1-555-0100",
          "email": "info@abcmanufacturing.com",
          "website": "https://www.abcmanufacturing.com",
          "contactPerson": "John Smith",
          "contactTitle": "Quality Manager",
          "contactEmail": "john.smith@abcmanufacturing.com",
          "contactPhone": "+1-555-0123",
          "isActive": true,
          "createdDate": "2023-01-15T10:00:00Z",
          "modifiedDate": "2024-06-20T14:30:00Z",
          "createdBy": 100,
          "modifiedBy": 102,
          "sites": [
            {
              "id": 5,
              "siteName": "Main Manufacturing Plant",
              "address": "123 Industrial Parkway",
              "city": "Manufacturing City",
              "contactPerson": "John Smith",
              "phone": "+1-555-0123",
              "isActive": true
            },
            {
              "id": 6,
              "siteName": "Secondary Production Facility",
              "address": "456 Production Lane",
              "city": "Manufacturing City",
              "contactPerson": "Sarah Johnson",
              "phone": "+1-555-0456",
              "isActive": true
            }
          ],
          "services": [
            {
              "id": 1,
              "serviceName": "ISO 9001",
              "serviceDescription": "Quality Management System",
              "isActive": true
            },
            {
              "id": 2,
              "serviceName": "ISO 14001",
              "serviceDescription": "Environmental Management System",
              "isActive": true
            }
          ]
        }
      ],
      "totalCount": 25,
      "pageNumber": 1,
      "pageSize": 20,
      "hasNextPage": true,
      "hasPreviousPage": false
    }
  }
}
```

---

## Complete GraphQL Schema Input Types

### Filter Input Types
```graphql
input ActionFilterInput {
  actionName: String
  status: String
  priority: String
  assignedToUserId: Int
  companyId: Int
  siteId: Int
  dueDateFrom: DateTime
  dueDateTo: DateTime
  isOverdue: Boolean
  isCompleted: Boolean
}

input AuditFilterInput {
  companyIds: [Int!]
  siteIds: [Int!]
  services: [String!]
  statuses: [String!]
  startDateFrom: DateTime
  startDateTo: DateTime
  endDateFrom: DateTime
  endDateTo: DateTime
  leadAuditor: String
  type: String
  auditNumber: String
  isActive: Boolean
}

input CertificateFilterInput {
  companyIds: [Int!]
  siteIds: [Int!]
  serviceIds: [Int!]
  statuses: [String!]
  issueDateFrom: DateTime
  issueDateTo: DateTime
  expiryDateFrom: DateTime
  expiryDateTo: DateTime
  issuingBody: String
  standardReference: String
  isActive: Boolean
}

input ContractFilterInput {
  companyIds: [Int!]
  contractTypes: [String!]
  statuses: [String!]
  startDateFrom: DateTime
  startDateTo: DateTime
  endDateFrom: DateTime
  endDateTo: DateTime
  valueFrom: Decimal
  valueTo: Decimal
  currency: String
  isActive: Boolean
}

input InvoiceFilterInput {
  companyIds: [Int!]
  contractIds: [Int!]
  statuses: [String!]
  paymentStatuses: [String!]
  issueDateFrom: DateTime
  issueDateTo: DateTime
  dueDateFrom: DateTime
  dueDateTo: DateTime
  amountFrom: Decimal
  amountTo: Decimal
  currency: String
  isOverdue: Boolean
  isActive: Boolean
}

input UserFilterInput {
  companyIds: [Int!]
  departments: [String!]
  roles: [String!]
  isActive: Boolean
  lastLoginFrom: DateTime
  lastLoginTo: DateTime
}

input CompanyFilterInput {
  industries: [String!]
  countries: [String!]
  states: [String!]
  isActive: Boolean
}
```

---

*This extended documentation provides comprehensive GraphQL examples for all modules in the Customer Portal API, including detailed request/response patterns, variables, and error handling examples.*