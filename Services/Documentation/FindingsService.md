# Findings Service Documentation

**Base URL:** `http://localhost:5008`  
**GraphQL Endpoint:** `http://localhost:5008/graphql`  
**Status:** 🔄 In Progress

## 📊 Entities Overview

The Findings Service manages audit findings, non-conformities, and corrective actions:

1. **Findings** - Main findings records
2. **FindingCategories** - Classification of findings
3. **FindingStatuses** - Current status tracking
4. **FindingActions** - Corrective/preventive actions
5. **FindingEvidence** - Supporting evidence and documentation

---

## 🔍 Findings Operations

### Query: Get All Findings

**Operation:** `findings`

**GraphQL Query:**
```graphql
query GetAllFindings {
  findings {
    id
    findingNumber
    auditId
    siteId
    clauseId
    categoryId
    statusId
    severity
    findingTitle
    description
    requirement
    evidence
    recommendation
    rootCause
    raisedDate
    targetCloseDate
    actualCloseDate
    isActive
    createdDate
    modifiedDate
    audit {
      auditNumber
      auditTitle
    }
    site {
      siteName
      siteCode
    }
    clause {
      clauseName
      clauseNumber
    }
    category {
      categoryName
      description
    }
    status {
      statusName
      description
    }
    actions {
      id
      actionDescription
      responsiblePerson
      targetDate
      status
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "findings": [
      {
        "id": 1,
        "findingNumber": "FND-2024-001",
        "auditId": 1,
        "siteId": 1,
        "clauseId": 1,
        "categoryId": 1,
        "statusId": 1,
        "severity": "MAJOR",
        "findingTitle": "Inadequate Document Control",
        "description": "The organization lacks proper control of documented information as required by ISO 9001:2015 clause 7.5",
        "requirement": "Clause 7.5.3 - Control of documented information",
        "evidence": "Observed outdated procedures being used in production area",
        "recommendation": "Implement systematic document control procedure",
        "rootCause": "Lack of formal document review process",
        "raisedDate": "2024-10-01T14:30:00Z",
        "targetCloseDate": "2024-11-01T00:00:00Z",
        "actualCloseDate": null,
        "isActive": true,
        "createdDate": "2024-10-01T14:30:00Z",
        "modifiedDate": null,
        "audit": {
          "auditNumber": "AUD-2024-001",
          "auditTitle": "ISO 9001:2015 Annual Surveillance Audit"
        },
        "site": {
          "siteName": "Main Production Facility",
          "siteCode": "ACME-PROD-01"
        },
        "clause": {
          "clauseName": "Control of documented information",
          "clauseNumber": "7.5"
        },
        "category": {
          "categoryName": "Non-Conformity",
          "description": "Failure to meet specified requirements"
        },
        "status": {
          "statusName": "Open",
          "description": "Finding is open and requires action"
        },
        "actions": [
          {
            "id": 1,
            "actionDescription": "Review and update all production procedures",
            "responsiblePerson": "Quality Manager",
            "targetDate": "2024-10-15T00:00:00Z",
            "status": "IN_PROGRESS"
          }
        ]
      }
    ]
  }
}
```

### Mutation: Create Finding

**Operation:** `createFinding`

**GraphQL Mutation:**
```graphql
mutation CreateFinding($input: CreateFindingInput!) {
  createFinding(input: $input) {
    id
    findingNumber
    findingTitle
    severity
    raisedDate
    targetCloseDate
    createdDate
  }
}
```

**Input Payload:**
```json
{
  "input": {
    "auditId": 1,
    "siteId": 1,
    "clauseId": 2,
    "categoryId": 1,
    "severity": "MINOR",
    "findingTitle": "Incomplete Training Records",
    "description": "Some training records are missing signatures and dates",
    "requirement": "Clause 7.2 - Competence",
    "evidence": "Training matrix shows 3 employees without completion signatures",
    "recommendation": "Complete missing training records and implement verification process",
    "targetCloseDate": "2024-10-30T00:00:00Z",
    "actions": [
      {
        "actionDescription": "Complete missing training record signatures",
        "responsiblePerson": "HR Manager",
        "targetDate": "2024-10-20T00:00:00Z"
      }
    ]
  }
}
```

### Mutation: Update Finding Status

**Operation:** `updateFindingStatus`

**GraphQL Mutation:**
```graphql
mutation UpdateFindingStatus($findingId: Int!, $statusId: Int!, $comments: String) {
  updateFindingStatus(findingId: $findingId, statusId: $statusId, comments: $comments) {
    id
    statusId
    modifiedDate
    status {
      statusName
    }
  }
}
```

---

## 📊 Finding Categories Operations

### Query: Get All Finding Categories

**Operation:** `findingCategories`

**GraphQL Query:**
```graphql
query GetAllFindingCategories {
  findingCategories {
    id
    categoryName
    description
    severity
    colorCode
    isActive
    createdDate
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "findingCategories": [
      {
        "id": 1,
        "categoryName": "Non-Conformity",
        "description": "Failure to meet specified requirements",
        "severity": "HIGH",
        "colorCode": "#FF4444",
        "isActive": true,
        "createdDate": "2024-01-01T00:00:00Z"
      },
      {
        "id": 2,
        "categoryName": "Observation",
        "description": "Area for improvement",
        "severity": "LOW",
        "colorCode": "#FFAA00",
        "isActive": true,
        "createdDate": "2024-01-01T00:00:00Z"
      }
    ]
  }
}
```

---

## 📈 Finding Analytics

### Query: Get Finding Statistics

**Operation:** `findingStatistics`

**GraphQL Query:**
```graphql
query GetFindingStatistics($auditId: Int, $siteId: Int, $startDate: DateTime, $endDate: DateTime) {
  findingStatistics(auditId: $auditId, siteId: $siteId, startDate: $startDate, endDate: $endDate) {
    totalFindings
    criticalFindings
    majorFindings
    minorFindings
    observations
    openFindings
    closedFindings
    overdueFindings
    averageCloseTime
    findingsBySeverity {
      severity
      count
      percentage
    }
    findingsByCategory {
      categoryName
      count
      percentage
    }
    findingsByClause {
      clauseName
      count
    }
  }
}
```

### Query: Get Findings Trend

**Operation:** `findingsTrend`

**GraphQL Query:**
```graphql
query GetFindingsTrend($companyId: Int!, $period: String!) {
  findingsTrend(companyId: $companyId, period: $period) {
    period
    totalFindings
    criticalFindings
    majorFindings
    minorFindings
    closeRate
    averageCloseTime
  }
}
```

---

## ⚡ Finding Actions Operations

### Query: Get Finding Actions

**Operation:** `findingActions`

**GraphQL Query:**
```graphql
query GetFindingActions($findingId: Int!) {
  findingActions(findingId: $findingId) {
    id
    findingId
    actionType
    actionDescription
    responsiblePerson
    responsibleEmail
    targetDate
    completedDate
    status
    priority
    verificationRequired
    evidenceFiles
    comments
    createdDate
    modifiedDate
  }
}
```

### Mutation: Add Finding Action

**Operation:** `addFindingAction`

**GraphQL Mutation:**
```graphql
mutation AddFindingAction($input: AddFindingActionInput!) {
  addFindingAction(input: $input) {
    id
    findingId
    actionDescription
    responsiblePerson
    targetDate
    status
    createdDate
  }
}
```

**Input Payload:**
```json
{
  "input": {
    "findingId": 1,
    "actionType": "CORRECTIVE",
    "actionDescription": "Implement new document control procedure",
    "responsiblePerson": "Quality Manager",
    "responsibleEmail": "quality@acme.com",
    "targetDate": "2024-10-25T00:00:00Z",
    "priority": "HIGH",
    "verificationRequired": true
  }
}
```

### Mutation: Complete Finding Action

**Operation:** `completeFindingAction`

**GraphQL Mutation:**
```graphql
mutation CompleteFindingAction($actionId: Int!, $input: CompleteActionInput!) {
  completeFindingAction(actionId: $actionId, input: $input) {
    id
    status
    completedDate
    evidenceFiles
    comments
  }
}
```

---

## 📁 Finding Evidence Operations

### Mutation: Upload Finding Evidence

**Operation:** `uploadFindingEvidence`

**GraphQL Mutation:**
```graphql
mutation UploadFindingEvidence($input: UploadEvidenceInput!) {
  uploadFindingEvidence(input: $input) {
    id
    findingId
    fileName
    fileType
    fileSize
    uploadDate
    uploadedBy
  }
}
```

### Query: Get Finding Evidence

**Operation:** `findingEvidence`

**GraphQL Query:**
```graphql
query GetFindingEvidence($findingId: Int!) {
  findingEvidence(findingId: $findingId) {
    id
    fileName
    fileType
    fileSize
    uploadDate
    uploadedBy
    downloadUrl
  }
}
```

---

## 📊 Finding Reports

### Query: Generate Finding Report

**Operation:** `findingReport`

**GraphQL Query:**
```graphql
query GenerateFindingReport($input: FindingReportInput!) {
  findingReport(input: $input) {
    reportId
    reportType
    generatedDate
    parameters
    downloadUrl
    expiresAt
  }
}
```

**Input Types:**
- `AUDIT_FINDINGS_SUMMARY`
- `CORRECTIVE_ACTIONS_STATUS`
- `FINDINGS_TREND_ANALYSIS`
- `OVERDUE_ACTIONS_REPORT`

---

*📝 Note: This service is currently in development. The actual implementation may vary from this specification.*