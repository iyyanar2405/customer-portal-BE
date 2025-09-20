# Contracts Service Documentation

**Base URL:** `http://localhost:6006`  
**GraphQL Endpoint:** `http://localhost:6006/graphql`  
**Status:** 🔄 In Progress

## 📊 Entities Overview

The Contracts Service manages client contracts, service agreements, and commercial relationships:

1. **Contracts** - Main contract records
2. **ContractServices** - Services covered by contracts
3. **ContractSites** - Sites covered by contracts
4. **ContractTerms** - Contract terms and conditions
5. **ContractRenewals** - Contract renewal tracking
6. **ContractAmendments** - Contract modifications

---

## 📄 Contracts Operations

### Query: Get All Contracts

**Operation:** `contracts`

**GraphQL Query:**
```graphql
query GetAllContracts {
  contracts {
    id
    contractNumber
    companyId
    contractType
    title
    description
    startDate
    endDate
    renewalDate
    value
    currency
    status
    paymentTerms
    isActive
    createdDate
    modifiedDate
    company {
      companyName
      companyCode
      contactPerson
      email
    }
    services {
      serviceId
      service {
        serviceName
        serviceCode
      }
      unitPrice
      quantity
      totalPrice
    }
    sites {
      siteId
      site {
        siteName
        siteCode
        address
      }
      isActive
    }
    terms {
      id
      termType
      description
      value
      unit
    }
    amendments {
      id
      amendmentNumber
      description
      effectiveDate
      amendmentType
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "contracts": [
      {
        "id": 1,
        "contractNumber": "CNT-2024-001",
        "companyId": 1,
        "contractType": "CERTIFICATION_SERVICES",
        "title": "ISO Management System Certification Contract",
        "description": "Multi-year certification contract for ISO 9001, 14001, and 45001",
        "startDate": "2024-01-01T00:00:00Z",
        "endDate": "2026-12-31T23:59:59Z",
        "renewalDate": "2026-09-30T00:00:00Z",
        "value": 50000.00,
        "currency": "USD",
        "status": "ACTIVE",
        "paymentTerms": "NET_30",
        "isActive": true,
        "createdDate": "2023-12-15T00:00:00Z",
        "modifiedDate": "2024-06-15T00:00:00Z",
        "company": {
          "companyName": "Acme Corporation",
          "companyCode": "ACME001",
          "contactPerson": "John Doe",
          "email": "john.doe@acme.com"
        },
        "services": [
          {
            "serviceId": 1,
            "service": {
              "serviceName": "ISO 9001 Certification",
              "serviceCode": "ISO9001"
            },
            "unitPrice": 15000.00,
            "quantity": 1,
            "totalPrice": 15000.00
          }
        ],
        "sites": [
          {
            "siteId": 1,
            "site": {
              "siteName": "Main Production Facility",
              "siteCode": "ACME-PROD-01",
              "address": "456 Factory Rd"
            },
            "isActive": true
          }
        ],
        "terms": [
          {
            "id": 1,
            "termType": "PAYMENT_SCHEDULE",
            "description": "Annual payment in advance",
            "value": "ANNUAL",
            "unit": "PAYMENT"
          }
        ],
        "amendments": [
          {
            "id": 1,
            "amendmentNumber": "AMD-001",
            "description": "Added ISO 45001 certification service",
            "effectiveDate": "2024-06-01T00:00:00Z",
            "amendmentType": "SERVICE_ADDITION"
          }
        ]
      }
    ]
  }
}
```

### Mutation: Create Contract

**Operation:** `createContract`

**GraphQL Mutation:**
```graphql
mutation CreateContract($input: CreateContractInput!) {
  createContract(input: $input) {
    id
    contractNumber
    title
    startDate
    endDate
    value
    status
    createdDate
  }
}
```

**Input Payload:**
```json
{
  "input": {
    "companyId": 2,
    "contractType": "TRAINING_SERVICES",
    "title": "Quality Management Training Contract",
    "description": "Annual training program for quality management",
    "startDate": "2024-10-01T00:00:00Z",
    "endDate": "2025-09-30T23:59:59Z",
    "value": 25000.00,
    "currency": "USD",
    "paymentTerms": "NET_30",
    "services": [
      {
        "serviceId": 3,
        "unitPrice": 5000.00,
        "quantity": 5
      }
    ],
    "sites": [1, 2],
    "terms": [
      {
        "termType": "CANCELLATION",
        "description": "30 days written notice required",
        "value": "30",
        "unit": "DAYS"
      }
    ]
  }
}
```

### Mutation: Update Contract Status

**Operation:** `updateContractStatus`

**GraphQL Mutation:**
```graphql
mutation UpdateContractStatus($contractId: Int!, $status: ContractStatus!, $reason: String) {
  updateContractStatus(contractId: $contractId, status: $status, reason: $reason) {
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
- `DRAFT`
- `PENDING_APPROVAL`
- `ACTIVE`
- `SUSPENDED`
- `EXPIRED`
- `TERMINATED`
- `RENEWED`

---

## 🔄 Contract Renewal Operations

### Query: Get Renewal Schedule

**Operation:** `contractRenewalSchedule`

**GraphQL Query:**
```graphql
query GetContractRenewalSchedule($startDate: DateTime!, $endDate: DateTime!) {
  contractRenewalSchedule(startDate: $startDate, endDate: $endDate) {
    contractId
    contractNumber
    companyName
    contractType
    currentEndDate
    renewalDate
    status
    daysUntilRenewal
    autoRenewal
    renewalValue
  }
}
```

### Query: Get Expiring Contracts

**Operation:** `expiringContracts`

**GraphQL Query:**
```graphql
query GetExpiringContracts($withinDays: Int!) {
  expiringContracts(withinDays: $withinDays) {
    id
    contractNumber
    company {
      companyName
      contactPerson
      email
    }
    contractType
    endDate
    daysUntilExpiry
    renewalRequired
    autoRenewal
    renewalNotificationSent
  }
}
```

### Mutation: Start Renewal Process

**Operation:** `startContractRenewal`

**GraphQL Mutation:**
```graphql
mutation StartContractRenewal($contractId: Int!, $input: StartRenewalInput!) {
  startContractRenewal(contractId: $contractId, input: $input) {
    renewalId
    contractId
    proposedStartDate
    proposedEndDate
    proposedValue
    status
    createdDate
  }
}
```

---

## 💰 Contract Financial Operations

### Query: Get Contract Financials

**Operation:** `contractFinancials`

**GraphQL Query:**
```graphql
query GetContractFinancials($contractId: Int!) {
  contractFinancials(contractId: $contractId) {
    contractId
    totalValue
    invoicedAmount
    paidAmount
    outstandingAmount
    currency
    paymentSchedule {
      dueDate
      amount
      status
      invoiceNumber
    }
    revenueRecognition {
      period
      recognizedAmount
      deferredAmount
    }
  }
}
```

### Query: Get Contract Revenue

**Operation:** `contractRevenue`

**GraphQL Query:**
```graphql
mutation GetContractRevenue($period: String!, $companyId: Int) {
  contractRevenue(period: $period, companyId: $companyId) {
    totalRevenue
    recognizedRevenue
    deferredRevenue
    contractCount
    averageContractValue
    revenueByType {
      contractType
      amount
      percentage
    }
    revenueByMonth {
      month
      amount
      contractCount
    }
  }
}
```

---

## 📝 Contract Terms Operations

### Query: Get Contract Terms

**Operation:** `contractTerms`

**GraphQL Query:**
```graphql
query GetContractTerms($contractId: Int!) {
  contractTerms(contractId: $contractId) {
    id
    contractId
    termType
    description
    value
    unit
    isRequired
    effectiveDate
    expiryDate
    isActive
  }
}
```

### Mutation: Add Contract Term

**Operation:** `addContractTerm`

**GraphQL Mutation:**
```graphql
mutation AddContractTerm($input: AddContractTermInput!) {
  addContractTerm(input: $input) {
    id
    contractId
    termType
    description
    value
    effectiveDate
  }
}
```

**Term Types:**
- `PAYMENT_SCHEDULE`
- `SERVICE_LEVEL`
- `CANCELLATION`
- `RENEWAL`
- `LIABILITY`
- `CONFIDENTIALITY`
- `INTELLECTUAL_PROPERTY`

---

## 📋 Contract Amendments Operations

### Query: Get Contract Amendments

**Operation:** `contractAmendments`

**GraphQL Query:**
```graphql
query GetContractAmendments($contractId: Int!) {
  contractAmendments(contractId: $contractId) {
    id
    contractId
    amendmentNumber
    description
    amendmentType
    effectiveDate
    valueChange
    status
    approvedBy
    approvedDate
    createdDate
    changes {
      fieldName
      oldValue
      newValue
      changeType
    }
  }
}
```

### Mutation: Create Amendment

**Operation:** `createContractAmendment`

**GraphQL Mutation:**
```graphql
mutation CreateContractAmendment($input: CreateAmendmentInput!) {
  createContractAmendment(input: $input) {
    id
    amendmentNumber
    description
    amendmentType
    effectiveDate
    status
    createdDate
  }
}
```

**Amendment Types:**
- `SERVICE_ADDITION`
- `SERVICE_REMOVAL`
- `PRICE_CHANGE`
- `TERM_EXTENSION`
- `TERM_MODIFICATION`
- `SITE_ADDITION`
- `SITE_REMOVAL`

---

## 📊 Contract Analytics

### Query: Get Contract Dashboard

**Operation:** `contractDashboard`

**GraphQL Query:**
```graphql
query GetContractDashboard($period: String!) {
  contractDashboard(period: $period) {
    totalContracts
    activeContracts
    expiringContracts
    totalValue
    averageContractValue
    renewalRate
    contractsByType {
      contractType
      count
      totalValue
    }
    contractsByStatus {
      status
      count
      percentage
    }
    monthlySignings {
      month
      count
      totalValue
    }
    topClients {
      companyName
      contractCount
      totalValue
    }
  }
}
```

### Query: Get Contract Performance

**Operation:** `contractPerformance`

**GraphQL Query:**
```graphql
query GetContractPerformance($contractId: Int!) {
  contractPerformance(contractId: $contractId) {
    contractId
    deliveryPerformance
    paymentPerformance
    clientSatisfaction
    profitMargin
    serviceMetrics {
      serviceName
      deliveredOnTime
      qualityScore
      clientFeedback
    }
    milestones {
      milestoneDate
      description
      status
      completedDate
    }
  }
}
```

---

## 🔍 Contract Compliance Operations

### Query: Get Contract Compliance

**Operation:** `contractCompliance`

**GraphQL Query:**
```graphql
query GetContractCompliance($contractId: Int!) {
  contractCompliance(contractId: $contractId) {
    contractId
    overallComplianceScore
    slaCompliance
    deliveryCompliance
    paymentCompliance
    documentCompliance
    complianceIssues {
      issueType
      description
      severity
      dueDate
      status
    }
    auditHistory {
      auditDate
      auditType
      result
      findings
    }
  }
}
```

### Mutation: Report Compliance Issue

**Operation:** `reportComplianceIssue`

**GraphQL Mutation:**
```graphql
mutation ReportComplianceIssue($input: ReportComplianceIssueInput!) {
  reportComplianceIssue(input: $input) {
    issueId
    contractId
    issueType
    severity
    description
    dueDate
    assignedTo
    createdDate
  }
}
```

---

*📝 Note: This service is currently in development. The actual implementation may vary from this specification.*