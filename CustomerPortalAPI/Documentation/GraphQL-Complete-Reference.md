# Customer Portal API - Complete GraphQL Reference

## Table of Contents
1. [Actions Module](#actions-module)
2. [Audits Module](#audits-module)
3. [Certificates Module](#certificates-module)
4. [Contracts Module](#contracts-module)
5. [Financial Module](#financial-module)
6. [Master Data Module](#master-data-module)
7. [Users Module](#users-module)
8. [Common Types](#common-types)
9. [Error Handling](#error-handling)

---

## Actions Module

### 🔍 **Query: Get All Actions**
```graphql
query GetActions($filter: ActionFilterInput, $sort: ActionSortInput, $skip: Int, $take: Int) {
  actions(filter: $filter, sort: $sort, skip: $skip, take: $take) {
    id
    actionName
    description
    actionTypeValue
    priority
    status
    assignedToUserId
    companyId
    siteId
    auditId
    findingId
    dueDate
    completedDate
    createdDate
    createdBy
    modifiedDate
    modifiedBy
    comments
    isActive
  }
}
```

**Variables:**
```json
{
  "filter": {
    "actionName": "string",
    "status": "In Progress",
    "priority": "High",
    "assignedToUserId": 123,
    "companyId": 1,
    "siteId": 5,
    "dueDateFrom": "2025-01-01T00:00:00Z",
    "dueDateTo": "2025-12-31T23:59:59Z",
    "isOverdue": false,
    "isCompleted": false
  },
  "sort": {
    "field": "DUEDATE",
    "direction": "ASC"
  },
  "skip": 0,
  "take": 50
}
```

**Response:**
```json
{
  "data": {
    "actions": [
      {
        "id": 1,
        "actionName": "Fix Compliance Issue",
        "description": "Address the non-compliance finding in audit report",
        "actionTypeValue": "Corrective",
        "priority": "High",
        "status": "In Progress",
        "assignedToUserId": 123,
        "companyId": 1,
        "siteId": 5,
        "auditId": 101,
        "findingId": 202,
        "dueDate": "2025-01-15T23:59:59Z",
        "completedDate": null,
        "createdDate": "2025-01-01T10:00:00Z",
        "createdBy": 100,
        "modifiedDate": "2025-01-05T14:30:00Z",
        "modifiedBy": 100,
        "comments": "Requires immediate attention",
        "isActive": true
      }
    ]
  }
}
```

### 🔍 **Query: Get Action by ID**
```graphql
query GetActionById($id: Int!) {
  actionById(id: $id) {
    id
    actionName
    description
    actionTypeValue
    priority
    status
    assignedToUserId
    companyId
    siteId
    auditId
    findingId
    dueDate
    completedDate
    createdDate
    createdBy
    modifiedDate
    modifiedBy
    comments
    isActive
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
    "actionById": {
      "id": 1,
      "actionName": "Fix Compliance Issue",
      "description": "Address the non-compliance finding in audit report",
      "actionTypeValue": "Corrective",
      "priority": "High",
      "status": "In Progress",
      "assignedToUserId": 123,
      "companyId": 1,
      "siteId": 5,
      "auditId": 101,
      "findingId": 202,
      "dueDate": "2025-01-15T23:59:59Z",
      "completedDate": null,
      "createdDate": "2025-01-01T10:00:00Z",
      "createdBy": 100,
      "modifiedDate": "2025-01-05T14:30:00Z",
      "modifiedBy": 100,
      "comments": "Requires immediate attention",
      "isActive": true
    }
  }
}
```

### 🔍 **Query: Get Actions by User**
```graphql
query GetActionsByUser($userId: Int!) {
  actionsByUser(userId: $userId) {
    id
    actionName
    priority
    status
    dueDate
    isOverdue
  }
}
```

**Variables:**
```json
{
  "userId": 123
}
```

**Response:**
```json
{
  "data": {
    "actionsByUser": [
      {
        "id": 1,
        "actionName": "Fix Compliance Issue",
        "priority": "High",
        "status": "In Progress",
        "dueDate": "2025-01-15T23:59:59Z",
        "isOverdue": false
      },
      {
        "id": 2,
        "actionName": "Update Documentation",
        "priority": "Medium",
        "status": "Pending",
        "dueDate": "2025-01-20T23:59:59Z",
        "isOverdue": false
      }
    ]
  }
}
```

### 🔍 **Query: Get Action Statistics**
```graphql
query GetActionStatistics {
  actionStatistics {
    totalActions
    completedActions
    pendingActions
    overdueActions
    highPriorityActions
    mediumPriorityActions
    lowPriorityActions
  }
}
```

**Variables:** None

**Response:**
```json
{
  "data": {
    "actionStatistics": {
      "totalActions": 150,
      "completedActions": 89,
      "pendingActions": 45,
      "overdueActions": 16,
      "highPriorityActions": 23,
      "mediumPriorityActions": 67,
      "lowPriorityActions": 60
    }
  }
}
```

### ✏️ **Mutation: Create Action**
```graphql
mutation CreateAction($input: ActionInput!) {
  createAction(input: $input) {
    action {
      id
      actionName
      description
      priority
      status
      assignedToUserId
      dueDate
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
    "actionName": "Fix Security Vulnerability",
    "description": "Address critical security finding from audit",
    "actionType": "Corrective",
    "priority": "Critical",
    "status": "Assigned",
    "assignedToUserId": 123,
    "companyId": 1,
    "siteId": 5,
    "auditId": 101,
    "findingId": 203,
    "dueDate": "2025-01-10T23:59:59Z",
    "comments": "Security team needs immediate attention"
  }
}
```

**Response:**
```json
{
  "data": {
    "createAction": {
      "action": {
        "id": 151,
        "actionName": "Fix Security Vulnerability",
        "description": "Address critical security finding from audit",
        "priority": "Critical",
        "status": "Assigned",
        "assignedToUserId": 123,
        "dueDate": "2025-01-10T23:59:59Z",
        "createdDate": "2025-01-01T15:30:00Z"
      },
      "errorMessage": null
    }
  }
}
```

### ✏️ **Mutation: Update Action**
```graphql
mutation UpdateAction($input: UpdateActionInput!) {
  updateAction(input: $input) {
    action {
      id
      actionName
      status
      priority
      assignedToUserId
      dueDate
      modifiedDate
    }
    errorMessage
  }
}
```

**Variables:**
```json
{
  "input": {
    "id": 151,
    "actionName": "Fix Critical Security Vulnerability",
    "status": "In Progress",
    "priority": "Critical",
    "assignedToUserId": 124,
    "dueDate": "2025-01-08T23:59:59Z",
    "comments": "Escalated to senior security engineer"
  }
}
```

**Response:**
```json
{
  "data": {
    "updateAction": {
      "action": {
        "id": 151,
        "actionName": "Fix Critical Security Vulnerability",
        "status": "In Progress",
        "priority": "Critical",
        "assignedToUserId": 124,
        "dueDate": "2025-01-08T23:59:59Z",
        "modifiedDate": "2025-01-02T09:15:00Z"
      },
      "errorMessage": null
    }
  }
}
```

### ✏️ **Mutation: Complete Action**
```graphql
mutation CompleteAction($input: CompleteActionInput!) {
  completeAction(input: $input) {
    action {
      id
      actionName
      status
      completedDate
      comments
    }
    errorMessage
  }
}
```

**Variables:**
```json
{
  "input": {
    "id": 151,
    "comments": "Security vulnerability patched and tested successfully"
  }
}
```

**Response:**
```json
{
  "data": {
    "completeAction": {
      "action": {
        "id": 151,
        "actionName": "Fix Critical Security Vulnerability",
        "status": "Completed",
        "completedDate": "2025-01-07T16:45:00Z",
        "comments": "Security vulnerability patched and tested successfully"
      },
      "errorMessage": null
    }
  }
}
```

---

## Audits Module

### 🔍 **Query: Get All Audits**
```graphql
query GetAudits($filter: AuditFilterInput, $search: AuditSearchInput) {
  audits(filter: $filter, search: $search) {
    audits {
      auditId
      sites
      services
      companyId
      companyName
      status
      startDate
      endDate
      leadAuditor
      type
      auditNumber
      description
      auditTypeId
      auditTypeName
      isActive
      createdDate
      modifiedDate
      createdBy
      modifiedBy
      createdByName
      modifiedByName
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
    "services": ["ISO 9001", "ISO 14001"],
    "statuses": ["Planned", "In Progress"],
    "startDateFrom": "2025-01-01T00:00:00Z",
    "startDateTo": "2025-12-31T23:59:59Z",
    "leadAuditor": "John Smith",
    "type": "Internal",
    "isActive": true
  },
  "search": {
    "searchTerm": "compliance",
    "pageNumber": 1,
    "pageSize": 20,
    "sortBy": "StartDate",
    "sortDirection": "DESC"
  }
}
```

**Response:**
```json
{
  "data": {
    "audits": {
      "audits": [
        {
          "auditId": 101,
          "sites": "Main Manufacturing Plant",
          "services": "ISO 9001, ISO 14001",
          "companyId": 1,
          "companyName": "ABC Manufacturing Ltd",
          "status": "In Progress",
          "startDate": "2025-01-15T08:00:00Z",
          "endDate": "2025-01-17T17:00:00Z",
          "leadAuditor": "John Smith",
          "type": "Internal",
          "auditNumber": "AUD-2025-001",
          "description": "Annual compliance audit for quality and environmental management systems",
          "auditTypeId": 1,
          "auditTypeName": "Internal Audit",
          "isActive": true,
          "createdDate": "2024-12-01T10:00:00Z",
          "modifiedDate": "2025-01-10T14:30:00Z",
          "createdBy": 100,
          "modifiedBy": 102,
          "createdByName": "Admin User",
          "modifiedByName": "Audit Manager"
        }
      ],
      "totalCount": 45,
      "pageNumber": 1,
      "pageSize": 20,
      "hasNextPage": true,
      "hasPreviousPage": false
    }
  }
}
```

### 🔍 **Query: Get Audit by ID**
```graphql
query GetAuditById($auditId: Int!) {
  auditById(auditId: $auditId) {
    auditId
    sites
    services
    companyId
    companyName
    status
    startDate
    endDate
    leadAuditor
    type
    auditNumber
    description
    auditTypeId
    auditTypeName
    teamMembers {
      id
      auditId
      userId
      userName
      role
      responsibilities
      isLead
      createdDate
      createdBy
    }
    auditSites {
      id
      auditId
      siteId
      siteName
      plannedStartDate
      plannedEndDate
      actualStartDate
      actualEndDate
      status
      notes
      createdDate
      createdBy
    }
    auditServices {
      id
      auditId
      service
      serviceDescription
      isActive
      createdDate
      createdBy
    }
    isActive
    createdDate
    modifiedDate
    createdBy
    modifiedBy
  }
}
```

**Variables:**
```json
{
  "auditId": 101
}
```

**Response:**
```json
{
  "data": {
    "auditById": {
      "auditId": 101,
      "sites": "Main Manufacturing Plant",
      "services": "ISO 9001, ISO 14001",
      "companyId": 1,
      "companyName": "ABC Manufacturing Ltd",
      "status": "In Progress",
      "startDate": "2025-01-15T08:00:00Z",
      "endDate": "2025-01-17T17:00:00Z",
      "leadAuditor": "John Smith",
      "type": "Internal",
      "auditNumber": "AUD-2025-001",
      "description": "Annual compliance audit for quality and environmental management systems",
      "auditTypeId": 1,
      "auditTypeName": "Internal Audit",
      "teamMembers": [
        {
          "id": 1,
          "auditId": 101,
          "userId": 123,
          "userName": "John Smith",
          "role": "Lead Auditor",
          "responsibilities": "Overall audit coordination and reporting",
          "isLead": true,
          "createdDate": "2024-12-01T10:00:00Z",
          "createdBy": 100
        },
        {
          "id": 2,
          "auditId": 101,
          "userId": 124,
          "userName": "Jane Doe",
          "role": "Technical Auditor",
          "responsibilities": "Environmental management system review",
          "isLead": false,
          "createdDate": "2024-12-01T10:05:00Z",
          "createdBy": 100
        }
      ],
      "auditSites": [
        {
          "id": 1,
          "auditId": 101,
          "siteId": 5,
          "siteName": "Main Manufacturing Plant",
          "plannedStartDate": "2025-01-15T08:00:00Z",
          "plannedEndDate": "2025-01-17T17:00:00Z",
          "actualStartDate": "2025-01-15T08:15:00Z",
          "actualEndDate": null,
          "status": "In Progress",
          "notes": "Production areas and quality control lab",
          "createdDate": "2024-12-01T10:10:00Z",
          "createdBy": 100
        }
      ],
      "auditServices": [
        {
          "id": 1,
          "auditId": 101,
          "service": "ISO 9001",
          "serviceDescription": "Quality Management System",
          "isActive": true,
          "createdDate": "2024-12-01T10:15:00Z",
          "createdBy": 100
        },
        {
          "id": 2,
          "auditId": 101,
          "service": "ISO 14001",
          "serviceDescription": "Environmental Management System",
          "isActive": true,
          "createdDate": "2024-12-01T10:16:00Z",
          "createdBy": 100
        }
      ],
      "isActive": true,
      "createdDate": "2024-12-01T10:00:00Z",
      "modifiedDate": "2025-01-10T14:30:00Z",
      "createdBy": 100,
      "modifiedBy": 102
    }
  }
}
```

### ✏️ **Mutation: Create Audit**
```graphql
mutation CreateAudit($input: AuditInput!) {
  createAudit(input: $input) {
    audit {
      auditId
      sites
      services
      companyId
      status
      startDate
      endDate
      leadAuditor
      auditNumber
      description
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
    "sites": "Secondary Production Facility",
    "services": "ISO 9001, ISO 45001",
    "companyId": 1,
    "status": "Planned",
    "startDate": "2025-02-01T08:00:00Z",
    "endDate": "2025-02-03T17:00:00Z",
    "leadAuditor": "Sarah Johnson",
    "type": "Internal",
    "auditNumber": "AUD-2025-002",
    "description": "Quality and safety management systems audit",
    "auditTypeId": 1,
    "createdBy": 100
  }
}
```

**Response:**
```json
{
  "data": {
    "createAudit": {
      "audit": {
        "auditId": 102,
        "sites": "Secondary Production Facility",
        "services": "ISO 9001, ISO 45001",
        "companyId": 1,
        "status": "Planned",
        "startDate": "2025-02-01T08:00:00Z",
        "endDate": "2025-02-03T17:00:00Z",
        "leadAuditor": "Sarah Johnson",
        "auditNumber": "AUD-2025-002",
        "description": "Quality and safety management systems audit",
        "createdDate": "2025-01-20T10:30:00Z"
      },
      "errorMessage": null
    }
  }
}
```

### ✏️ **Mutation: Update Audit**
```graphql
mutation UpdateAudit($input: UpdateAuditInput!) {
  updateAudit(input: $input) {
    audit {
      auditId
      sites
      services
      status
      startDate
      endDate
      leadAuditor
      description
      modifiedDate
    }
    errorMessage
  }
}
```

**Variables:**
```json
{
  "input": {
    "auditId": 102,
    "status": "In Progress",
    "startDate": "2025-02-01T08:00:00Z",
    "endDate": "2025-02-04T17:00:00Z",
    "description": "Quality and safety management systems audit - Extended to include additional safety processes",
    "modifiedBy": 102
  }
}
```

**Response:**
```json
{
  "data": {
    "updateAudit": {
      "audit": {
        "auditId": 102,
        "sites": "Secondary Production Facility",
        "services": "ISO 9001, ISO 45001",
        "status": "In Progress",
        "startDate": "2025-02-01T08:00:00Z",
        "endDate": "2025-02-04T17:00:00Z",
        "leadAuditor": "Sarah Johnson",
        "description": "Quality and safety management systems audit - Extended to include additional safety processes",
        "modifiedDate": "2025-01-25T14:20:00Z"
      },
      "errorMessage": null
    }
  }
}
```

### ✏️ **Mutation: Schedule Audit**
```graphql
mutation ScheduleAudit($input: AuditScheduleInput!) {
  scheduleAudit(input: $input) {
    audit {
      auditId
      status
      startDate
      endDate
      modifiedDate
    }
    errorMessage
  }
}
```

**Variables:**
```json
{
  "input": {
    "auditId": 102,
    "startDate": "2025-02-05T08:00:00Z",
    "endDate": "2025-02-07T17:00:00Z",
    "comments": "Rescheduled due to client availability",
    "modifiedBy": 102
  }
}
```

**Response:**
```json
{
  "data": {
    "scheduleAudit": {
      "audit": {
        "auditId": 102,
        "status": "Scheduled",
        "startDate": "2025-02-05T08:00:00Z",
        "endDate": "2025-02-07T17:00:00Z",
        "modifiedDate": "2025-01-26T11:15:00Z"
      },
      "errorMessage": null
    }
  }
}
```

### ✏️ **Mutation: Complete Audit**
```graphql
mutation CompleteAudit($auditId: Int!, $comments: String) {
  completeAudit(auditId: $auditId, comments: $comments) {
    audit {
      auditId
      status
      endDate
      description
      modifiedDate
    }
    errorMessage
  }
}
```

**Variables:**
```json
{
  "auditId": 101,
  "comments": "Audit completed successfully. All findings documented and action items assigned."
}
```

**Response:**
```json
{
  "data": {
    "completeAudit": {
      "audit": {
        "auditId": 101,
        "status": "Completed",
        "endDate": "2025-01-17T16:30:00Z",
        "description": "Annual compliance audit for quality and environmental management systems\n\nCompletion Comments: Audit completed successfully. All findings documented and action items assigned.",
        "modifiedDate": "2025-01-17T16:30:00Z"
      },
      "errorMessage": null
    }
  }
}
```

---

## Common Error Responses

### GraphQL Error Structure
```json
{
  "errors": [
    {
      "message": "Action not found",
      "locations": [
        {
          "line": 2,
          "column": 3
        }
      ],
      "path": ["actionById"],
      "extensions": {
        "code": "NOT_FOUND",
        "details": "Action with ID 999 does not exist"
      }
    }
  ],
  "data": {
    "actionById": null
  }
}
```

### Validation Error Response
```json
{
  "data": {
    "createAction": {
      "action": null,
      "errorMessage": "Action name is required and cannot be empty"
    }
  }
}
```

### Authorization Error Response
```json
{
  "errors": [
    {
      "message": "Access denied. User does not have permission to perform this operation.",
      "extensions": {
        "code": "UNAUTHORIZED"
      }
    }
  ]
}
```

---

## GraphQL Schema Types Reference

### Filter Enums
```graphql
enum SortDirection {
  ASC
  DESC
}

enum ActionSortField {
  ACTIONNAME
  PRIORITY
  STATUS
  DUEDATE
  CREATEDDATE
  MODIFIEDDATE
}
```

### Input Types Summary
- `ActionInput` - For creating new actions
- `UpdateActionInput` - For updating existing actions
- `ActionFilterInput` - For filtering action queries
- `ActionSortInput` - For sorting action results
- `AuditInput` - For creating new audits
- `UpdateAuditInput` - For updating existing audits
- `AuditScheduleInput` - For scheduling audits

### Output Types Summary
- `ActionType` - Complete action details
- `ActionSummaryType` - Simplified action view
- `ActionStatisticsType` - Action metrics and counts
- `AuditType` - Complete audit details with related data
- Payload types for all mutations (success/error responses)

---

*This documentation provides comprehensive GraphQL examples for the Customer Portal API. Each query and mutation includes complete request/response examples with realistic data.*