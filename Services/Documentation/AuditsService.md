# Audits Service Documentation

**Base URL:** `http://localhost:5002`  
**GraphQL Endpoint:** `http://localhost:5002/graphql`  
**Status:** 🔄 In Progress

## 📊 Entities Overview

The Audits Service manages audit planning, execution, and tracking:

1. **Audits** - Main audit records
2. **AuditSites** - Sites being audited
3. **AuditServices** - Services being audited
4. **AuditTeamMembers** - Audit team composition
5. **AuditTypes** - Types of audits
6. **AuditLogs** - Audit activity tracking
7. **AuditSiteAudits** - Site-specific audit details

---

## 🔍 Audits Operations

### Query: Get All Audits

**Operation:** `audits`

**GraphQL Query:**
```graphql
query GetAllAudits {
  audits {
    id
    auditNumber
    auditTitle
    companyId
    auditTypeId
    startDate
    endDate
    status
    leadAuditorId
    isActive
    createdDate
    modifiedDate
    company {
      companyName
      companyCode
    }
    auditType {
      auditTypeName
      description
    }
    leadAuditor {
      firstName
      lastName
      email
    }
    auditSites {
      siteId
      site {
        siteName
        siteCode
      }
    }
    auditTeamMembers {
      userId
      role
      user {
        firstName
        lastName
      }
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "audits": [
      {
        "id": 1,
        "auditNumber": "AUD-2024-001",
        "auditTitle": "ISO 9001:2015 Annual Surveillance Audit",
        "companyId": 1,
        "auditTypeId": 1,
        "startDate": "2024-10-01T09:00:00Z",
        "endDate": "2024-10-03T17:00:00Z",
        "status": "PLANNED",
        "leadAuditorId": 1,
        "isActive": true,
        "createdDate": "2024-09-15T00:00:00Z",
        "modifiedDate": null,
        "company": {
          "companyName": "Acme Corporation",
          "companyCode": "ACME001"
        },
        "auditType": {
          "auditTypeName": "Surveillance Audit",
          "description": "Regular surveillance audit for maintaining certification"
        },
        "leadAuditor": {
          "firstName": "John",
          "lastName": "Auditor",
          "email": "john.auditor@certbody.com"
        },
        "auditSites": [
          {
            "siteId": 1,
            "site": {
              "siteName": "Main Production Facility",
              "siteCode": "ACME-PROD-01"
            }
          }
        ],
        "auditTeamMembers": [
          {
            "userId": 1,
            "role": "LEAD_AUDITOR",
            "user": {
              "firstName": "John",
              "lastName": "Auditor"
            }
          }
        ]
      }
    ]
  }
}
```

### Mutation: Create Audit

**Operation:** `createAudit`

**GraphQL Mutation:**
```graphql
mutation CreateAudit($input: CreateAuditInput!) {
  createAudit(input: $input) {
    id
    auditNumber
    auditTitle
    startDate
    endDate
    status
    createdDate
  }
}
```

**Input Payload:**
```json
{
  "input": {
    "auditTitle": "ISO 14001:2015 Initial Certification Audit",
    "companyId": 1,
    "auditTypeId": 2,
    "startDate": "2024-11-15T09:00:00Z",
    "endDate": "2024-11-17T17:00:00Z",
    "leadAuditorId": 2,
    "siteIds": [1, 2],
    "serviceIds": [2, 3],
    "teamMembers": [
      {
        "userId": 2,
        "role": "LEAD_AUDITOR"
      },
      {
        "userId": 3,
        "role": "AUDITOR"
      }
    ]
  }
}
```

### Mutation: Update Audit Status

**Operation:** `updateAuditStatus`

**GraphQL Mutation:**
```graphql
mutation UpdateAuditStatus($auditId: Int!, $status: AuditStatus!) {
  updateAuditStatus(auditId: $auditId, status: $status) {
    id
    status
    modifiedDate
  }
}
```

**Status Values:**
- `PLANNED`
- `IN_PROGRESS`
- `COMPLETED`
- `CANCELLED`
- `ON_HOLD`

---

## 📅 Audit Planning Operations

### Query: Get Audit Schedule

**Operation:** `auditSchedule`

**GraphQL Query:**
```graphql
query GetAuditSchedule($startDate: DateTime!, $endDate: DateTime!) {
  auditSchedule(startDate: $startDate, endDate: $endDate) {
    id
    auditNumber
    auditTitle
    startDate
    endDate
    status
    company {
      companyName
    }
    leadAuditor {
      firstName
      lastName
    }
  }
}
```

### Query: Get Auditor Availability

**Operation:** `auditorAvailability`

**GraphQL Query:**
```graphql
query GetAuditorAvailability($auditorId: Int!, $startDate: DateTime!, $endDate: DateTime!) {
  auditorAvailability(auditorId: $auditorId, startDate: $startDate, endDate: $endDate) {
    auditorId
    availableDates
    conflictingAudits {
      auditNumber
      startDate
      endDate
    }
  }
}
```

---

## 👥 Audit Team Management

### Query: Get Audit Team

**Operation:** `auditTeam`

**GraphQL Query:**
```graphql
query GetAuditTeam($auditId: Int!) {
  auditTeam(auditId: $auditId) {
    auditId
    userId
    role
    assignedDate
    user {
      firstName
      lastName
      email
      qualifications
    }
  }
}
```

### Mutation: Assign Team Member

**Operation:** `assignTeamMember`

**GraphQL Mutation:**
```graphql
mutation AssignTeamMember($input: AssignTeamMemberInput!) {
  assignTeamMember(input: $input) {
    auditId
    userId
    role
    assignedDate
  }
}
```

---

## 📍 Audit Sites Management

### Query: Get Audit Sites

**Operation:** `auditSites`

**GraphQL Query:**
```graphql
query GetAuditSites($auditId: Int!) {
  auditSites(auditId: $auditId) {
    auditId
    siteId
    scheduledDate
    status
    site {
      siteName
      siteCode
      address
      contactPerson
    }
  }
}
```

### Mutation: Add Site to Audit

**Operation:** `addSiteToAudit`

**GraphQL Mutation:**
```graphql
mutation AddSiteToAudit($input: AddSiteToAuditInput!) {
  addSiteToAudit(input: $input) {
    auditId
    siteId
    scheduledDate
    status
  }
}
```

---

## 📊 Audit Reporting

### Query: Get Audit Summary

**Operation:** `auditSummary`

**GraphQL Query:**
```graphql
query GetAuditSummary($auditId: Int!) {
  auditSummary(auditId: $auditId) {
    auditId
    totalFindings
    criticalFindings
    majorFindings
    minorFindings
    observationsCount
    compliancePercentage
    recommendationForCertification
    auditDuration
    sitesAudited
    teamMembersCount
  }
}
```

---

*📝 Note: This service is currently in development. The actual implementation may vary from this specification.*