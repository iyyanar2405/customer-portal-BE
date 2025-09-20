# Certificates Service Documentation

**Base URL:** `http://localhost:5005`  
**GraphQL Endpoint:** `http://localhost:5005/graphql`  
**Status:** 🔄 In Progress

## 📊 Entities Overview

The Certificates Service manages certification records, renewal tracking, and compliance status:

1. **Certificates** - Main certificate records
2. **CertificateSites** - Sites covered by certificates
3. **CertificateServices** - Services covered by certificates
4. **CertificateAdditionalScopes** - Additional scope information
5. **CertificateRenewals** - Renewal history and tracking
6. **CertificateValidations** - Validation and verification records

---

## 📜 Certificates Operations

### Query: Get All Certificates

**Operation:** `certificates`

**GraphQL Query:**
```graphql
query GetAllCertificates {
  certificates {
    id
    certificateNumber
    companyId
    auditId
    certificateTypeId
    scope
    issueDate
    expiryDate
    renewalDate
    status
    isActive
    createdDate
    modifiedDate
    company {
      companyName
      companyCode
      address
      contactPerson
    }
    audit {
      auditNumber
      auditTitle
      leadAuditor {
        firstName
        lastName
      }
    }
    certificateType {
      typeName
      standard
      description
    }
    sites {
      siteId
      site {
        siteName
        siteCode
        address
      }
    }
    services {
      serviceId
      service {
        serviceName
        serviceCode
      }
    }
    additionalScopes {
      id
      scopeDescription
      isIncluded
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "certificates": [
      {
        "id": 1,
        "certificateNumber": "CERT-ISO9001-2024-001",
        "companyId": 1,
        "auditId": 1,
        "certificateTypeId": 1,
        "scope": "Design, manufacture and supply of industrial equipment",
        "issueDate": "2024-01-15T00:00:00Z",
        "expiryDate": "2027-01-14T23:59:59Z",
        "renewalDate": "2026-10-15T00:00:00Z",
        "status": "ACTIVE",
        "isActive": true,
        "createdDate": "2024-01-15T10:30:00Z",
        "modifiedDate": null,
        "company": {
          "companyName": "Acme Corporation",
          "companyCode": "ACME001",
          "address": "123 Business St, Business City",
          "contactPerson": "John Doe"
        },
        "audit": {
          "auditNumber": "AUD-2024-001",
          "auditTitle": "ISO 9001:2015 Initial Certification Audit",
          "leadAuditor": {
            "firstName": "Jane",
            "lastName": "Auditor"
          }
        },
        "certificateType": {
          "typeName": "ISO 9001:2015",
          "standard": "ISO 9001",
          "description": "Quality Management System"
        },
        "sites": [
          {
            "siteId": 1,
            "site": {
              "siteName": "Main Production Facility",
              "siteCode": "ACME-PROD-01",
              "address": "456 Factory Rd"
            }
          }
        ],
        "services": [
          {
            "serviceId": 1,
            "service": {
              "serviceName": "Manufacturing",
              "serviceCode": "MFG001"
            }
          }
        ],
        "additionalScopes": [
          {
            "id": 1,
            "scopeDescription": "Installation and maintenance services",
            "isIncluded": true
          }
        ]
      }
    ]
  }
}
```

### Mutation: Create Certificate

**Operation:** `createCertificate`

**GraphQL Mutation:**
```graphql
mutation CreateCertificate($input: CreateCertificateInput!) {
  createCertificate(input: $input) {
    id
    certificateNumber
    scope
    issueDate
    expiryDate
    status
    createdDate
  }
}
```

**Input Payload:**
```json
{
  "input": {
    "companyId": 1,
    "auditId": 2,
    "certificateTypeId": 2,
    "scope": "Environmental management for manufacturing operations",
    "issueDate": "2024-03-01T00:00:00Z",
    "expiryDate": "2027-02-28T23:59:59Z",
    "siteIds": [1, 2],
    "serviceIds": [1, 3],
    "additionalScopes": [
      {
        "scopeDescription": "Waste management services",
        "isIncluded": true
      }
    ]
  }
}
```

### Mutation: Update Certificate Status

**Operation:** `updateCertificateStatus`

**GraphQL Mutation:**
```graphql
mutation UpdateCertificateStatus($certificateId: Int!, $status: CertificateStatus!, $reason: String) {
  updateCertificateStatus(certificateId: $certificateId, status: $status, reason: $reason) {
    id
    status
    modifiedDate
    statusHistory {
      status
      changedDate
      reason
      changedBy
    }
  }
}
```

**Status Values:**
- `ACTIVE`
- `SUSPENDED`
- `WITHDRAWN`
- `EXPIRED`
- `CANCELLED`
- `UNDER_REVIEW`

---

## 📅 Certificate Renewal Operations

### Query: Get Renewal Schedule

**Operation:** `renewalSchedule`

**GraphQL Query:**
```graphql
query GetRenewalSchedule($startDate: DateTime!, $endDate: DateTime!) {
  renewalSchedule(startDate: $startDate, endDate: $endDate) {
    certificateId
    certificateNumber
    companyName
    certificateType
    currentExpiryDate
    renewalDate
    status
    daysUntilRenewal
    priority
  }
}
```

### Query: Get Expiring Certificates

**Operation:** `expiringCertificates`

**GraphQL Query:**
```graphql
query GetExpiringCertificates($withinDays: Int!) {
  expiringCertificates(withinDays: $withinDays) {
    id
    certificateNumber
    company {
      companyName
      contactPerson
      email
    }
    certificateType {
      typeName
    }
    expiryDate
    daysUntilExpiry
    renewalRequired
    hasActiveRenewalProcess
  }
}
```

### Mutation: Start Renewal Process

**Operation:** `startRenewalProcess`

**GraphQL Mutation:**
```graphql
mutation StartRenewalProcess($certificateId: Int!, $input: StartRenewalInput!) {
  startRenewalProcess(certificateId: $certificateId, input: $input) {
    renewalId
    certificateId
    plannedAuditDate
    targetCompletionDate
    status
    createdDate
  }
}
```

---

## 🔍 Certificate Validation Operations

### Query: Validate Certificate

**Operation:** `validateCertificate`

**GraphQL Query:**
```graphql
query ValidateCertificate($certificateNumber: String!) {
  validateCertificate(certificateNumber: $certificateNumber) {
    isValid
    certificateNumber
    companyName
    certificateType
    scope
    issueDate
    expiryDate
    status
    sites {
      siteName
      address
    }
    validationDate
    qrCodeUrl
  }
}
```

### Query: Get Certificate Verification

**Operation:** `certificateVerification`

**GraphQL Query:**
```graphql
query GetCertificateVerification($certificateId: Int!) {
  certificateVerification(certificateId: $certificateId) {
    certificateId
    verificationCode
    digitalSignature
    blockchainHash
    verificationUrl
    qrCodeData
    lastVerificationDate
    verificationHistory {
      verifiedDate
      verifiedBy
      verificationMethod
      result
    }
  }
}
```

---

## 📊 Certificate Analytics

### Query: Get Certificate Dashboard

**Operation:** `certificateDashboard`

**GraphQL Query:**
```graphql
query GetCertificateDashboard($companyId: Int, $period: String!) {
  certificateDashboard(companyId: $companyId, period: $period) {
    totalCertificates
    activeCertificates
    expiredCertificates
    suspendedCertificates
    expiringWithin30Days
    expiringWithin90Days
    renewalSuccessRate
    averageRenewalTime
    certificatesByType {
      typeName
      count
      activeCount
    }
    certificatesByStatus {
      status
      count
      percentage
    }
    monthlyIssuance {
      month
      issued
      renewed
      expired
    }
  }
}
```

### Query: Get Compliance Status

**Operation:** `complianceStatus`

**GraphQL Query:**
```graphql
query GetComplianceStatus($companyId: Int!, $siteId: Int) {
  complianceStatus(companyId: $companyId, siteId: $siteId) {
    companyId
    siteId
    overallComplianceScore
    activeCertifications
    requiredCertifications
    complianceGaps
    certificationsByStandard {
      standard
      status
      expiryDate
      complianceLevel
    }
    riskLevel
    recommendations
  }
}
```

---

## 📋 Certificate Types Operations

### Query: Get All Certificate Types

**Operation:** `certificateTypes`

**GraphQL Query:**
```graphql
query GetAllCertificateTypes {
  certificateTypes {
    id
    typeName
    standard
    description
    category
    validityPeriodMonths
    renewalNoticeDays
    isAccredited
    accreditationBody
    templateUrl
    requiresAnnualSurveillance
    isActive
    createdDate
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "certificateTypes": [
      {
        "id": 1,
        "typeName": "ISO 9001:2015",
        "standard": "ISO 9001",
        "description": "Quality Management System",
        "category": "MANAGEMENT_SYSTEMS",
        "validityPeriodMonths": 36,
        "renewalNoticeDays": 90,
        "isAccredited": true,
        "accreditationBody": "UKAS",
        "templateUrl": "/templates/iso9001-certificate.pdf",
        "requiresAnnualSurveillance": true,
        "isActive": true,
        "createdDate": "2024-01-01T00:00:00Z"
      }
    ]
  }
}
```

---

## 📍 Certificate Sites Operations

### Query: Get Certificate Sites

**Operation:** `certificateSites`

**GraphQL Query:**
```graphql
query GetCertificateSites($certificateId: Int!) {
  certificateSites(certificateId: $certificateId) {
    certificateId
    siteId
    includeDate
    excludeDate
    isActive
    scopeDescription
    site {
      siteName
      siteCode
      address
      city {
        cityName
        country {
          countryName
        }
      }
    }
  }
}
```

### Mutation: Add Site to Certificate

**Operation:** `addSiteToCertificate`

**GraphQL Mutation:**
```graphql
mutation AddSiteToCertificate($input: AddSiteToCertificateInput!) {
  addSiteToCertificate(input: $input) {
    certificateId
    siteId
    includeDate
    scopeDescription
  }
}
```

---

## 📄 Certificate Documents

### Query: Get Certificate Documents

**Operation:** `certificateDocuments`

**GraphQL Query:**
```graphql
query GetCertificateDocuments($certificateId: Int!) {
  certificateDocuments(certificateId: $certificateId) {
    id
    certificateId
    documentType
    fileName
    fileType
    fileSize
    uploadDate
    version
    isLatest
    downloadUrl
    digitalSignature
  }
}
```

### Mutation: Generate Certificate Document

**Operation:** `generateCertificateDocument`

**GraphQL Mutation:**
```graphql
mutation GenerateCertificateDocument($certificateId: Int!, $documentType: String!) {
  generateCertificateDocument(certificateId: $certificateId, documentType: $documentType) {
    documentId
    fileName
    downloadUrl
    generatedDate
    digitalSignature
    qrCodeData
  }
}
```

**Document Types:**
- `CERTIFICATE`
- `SCOPE_DOCUMENT`
- `ANNEXE`
- `VERIFICATION_LETTER`
- `QR_CODE`

---

*📝 Note: This service is currently in development. The actual implementation may vary from this specification.*