# Actions Service Documentation

**Base URL:** `http://localhost:5001`  
**GraphQL Endpoint:** `http://localhost:5001/graphql`  
**Status:** 🔄 In Progress

## 📊 Entities Overview

The Actions Service manages tasks, activities, and workflow automation:

1. **Actions** - Main action/task records
2. **ActionTypes** - Categories of actions
3. **ActionAssignments** - User assignments
4. **ActionDependencies** - Task dependencies
5. **ActionTemplates** - Reusable action templates
6. **Workflows** - Automated workflow processes

---

## ⚡ Actions Operations

### Query: Get All Actions

**Operation:** `actions`

**GraphQL Query:**
```graphql
query GetAllActions {
  actions {
    id
    actionNumber
    title
    description
    actionTypeId
    assignedToId
    createdById
    priority
    status
    startDate
    dueDate
    completedDate
    estimatedHours
    actualHours
    progress
    isActive
    createdDate
    modifiedDate
    actionType {
      typeName
      description
      defaultPriority
    }
    assignedTo {
      firstName
      lastName
      email
    }
    createdBy {
      firstName
      lastName
    }
    dependencies {
      dependentActionId
      dependsOn {
        actionNumber
        title
        status
      }
    }
    attachments {
      id
      fileName
      fileType
      uploadDate
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "actions": [
      {
        "id": 1,
        "actionNumber": "ACT-2024-001",
        "title": "Review Quality Manual",
        "description": "Conduct annual review of quality management system manual",
        "actionTypeId": 1,
        "assignedToId": 2,
        "createdById": 1,
        "priority": "HIGH",
        "status": "IN_PROGRESS",
        "startDate": "2024-10-01T09:00:00Z",
        "dueDate": "2024-10-15T17:00:00Z",
        "completedDate": null,
        "estimatedHours": 16,
        "actualHours": 8,
        "progress": 50,
        "isActive": true,
        "createdDate": "2024-09-25T00:00:00Z",
        "modifiedDate": "2024-10-05T10:30:00Z",
        "actionType": {
          "typeName": "Document Review",
          "description": "Review and update documentation",
          "defaultPriority": "MEDIUM"
        },
        "assignedTo": {
          "firstName": "Jane",
          "lastName": "Smith",
          "email": "jane.smith@acme.com"
        },
        "createdBy": {
          "firstName": "John",
          "lastName": "Manager"
        },
        "dependencies": [
          {
            "dependentActionId": 2,
            "dependsOn": {
              "actionNumber": "ACT-2024-002",
              "title": "Complete Procedure Updates",
              "status": "COMPLETED"
            }
          }
        ],
        "attachments": [
          {
            "id": 1,
            "fileName": "quality-manual-v2.pdf",
            "fileType": "PDF",
            "uploadDate": "2024-10-01T12:00:00Z"
          }
        ]
      }
    ]
  }
}
```

### Mutation: Create Action

**Operation:** `createAction`

**GraphQL Mutation:**
```graphql
mutation CreateAction($input: CreateActionInput!) {
  createAction(input: $input) {
    id
    actionNumber
    title
    priority
    status
    dueDate
    createdDate
  }
}
```

**Input Payload:**
```json
{
  "input": {
    "title": "Implement Corrective Action",
    "description": "Address non-conformity found during audit",
    "actionTypeId": 2,
    "assignedToId": 3,
    "priority": "HIGH",
    "startDate": "2024-10-10T09:00:00Z",
    "dueDate": "2024-10-25T17:00:00Z",
    "estimatedHours": 24,
    "relatedFindingId": 1,
    "dependencies": [2, 3],
    "tags": ["audit", "corrective-action", "quality"]
  }
}
```

### Mutation: Update Action Status

**Operation:** `updateActionStatus`

**GraphQL Mutation:**
```graphql
mutation UpdateActionStatus($actionId: Int!, $status: ActionStatus!, $comments: String) {
  updateActionStatus(actionId: $actionId, status: $status, comments: $comments) {
    id
    status
    modifiedDate
    progress
  }
}
```

**Status Values:**
- `NOT_STARTED`
- `IN_PROGRESS`
- `ON_HOLD`
- `COMPLETED`
- `CANCELLED`
- `OVERDUE`

### Mutation: Update Action Progress

**Operation:** `updateActionProgress`

**GraphQL Mutation:**
```graphql
mutation UpdateActionProgress($actionId: Int!, $progress: Int!, $actualHours: Float, $comments: String) {
  updateActionProgress(actionId: $actionId, progress: $progress, actualHours: $actualHours, comments: $comments) {
    id
    progress
    actualHours
    modifiedDate
  }
}
```

---

## 📋 Action Types Operations

### Query: Get All Action Types

**Operation:** `actionTypes`

**GraphQL Query:**
```graphql
query GetAllActionTypes {
  actionTypes {
    id
    typeName
    description
    category
    defaultPriority
    defaultEstimatedHours
    requiresApproval
    isActive
    colorCode
    iconName
    createdDate
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "actionTypes": [
      {
        "id": 1,
        "typeName": "Document Review",
        "description": "Review and update documentation",
        "category": "QUALITY",
        "defaultPriority": "MEDIUM",
        "defaultEstimatedHours": 8,
        "requiresApproval": true,
        "isActive": true,
        "colorCode": "#4CAF50",
        "iconName": "document-text",
        "createdDate": "2024-01-01T00:00:00Z"
      },
      {
        "id": 2,
        "typeName": "Corrective Action",
        "description": "Address non-conformities and issues",
        "category": "CORRECTIVE",
        "defaultPriority": "HIGH",
        "defaultEstimatedHours": 16,
        "requiresApproval": true,
        "isActive": true,
        "colorCode": "#FF9800",
        "iconName": "exclamation-triangle",
        "createdDate": "2024-01-01T00:00:00Z"
      }
    ]
  }
}
```

---

## 👥 Action Assignments Operations

### Query: Get User Actions

**Operation:** `userActions`

**GraphQL Query:**
```graphql
query GetUserActions($userId: Int!, $status: ActionStatus) {
  userActions(userId: $userId, status: $status) {
    id
    actionNumber
    title
    priority
    status
    dueDate
    progress
    isOverdue
    actionType {
      typeName
      colorCode
    }
  }
}
```

### Query: Get Team Actions

**Operation:** `teamActions`

**GraphQL Query:**
```graphql
query GetTeamActions($teamId: Int!, $startDate: DateTime, $endDate: DateTime) {
  teamActions(teamId: $teamId, startDate: $startDate, endDate: $endDate) {
    id
    actionNumber
    title
    assignedTo {
      firstName
      lastName
    }
    priority
    status
    dueDate
    progress
  }
}
```

### Mutation: Reassign Action

**Operation:** `reassignAction`

**GraphQL Mutation:**
```graphql
mutation ReassignAction($actionId: Int!, $newAssigneeId: Int!, $reason: String) {
  reassignAction(actionId: $actionId, newAssigneeId: $newAssigneeId, reason: $reason) {
    id
    assignedToId
    modifiedDate
    assignedTo {
      firstName
      lastName
      email
    }
  }
}
```

---

## 🔗 Action Dependencies Operations

### Query: Get Action Dependencies

**Operation:** `actionDependencies`

**GraphQL Query:**
```graphql
query GetActionDependencies($actionId: Int!) {
  actionDependencies(actionId: $actionId) {
    actionId
    dependsOnActionId
    dependencyType
    createdDate
    dependsOn {
      actionNumber
      title
      status
      completedDate
    }
  }
}
```

### Mutation: Add Dependency

**Operation:** `addActionDependency`

**GraphQL Mutation:**
```graphql
mutation AddActionDependency($input: AddDependencyInput!) {
  addActionDependency(input: $input) {
    actionId
    dependsOnActionId
    dependencyType
    createdDate
  }
}
```

**Input Payload:**
```json
{
  "input": {
    "actionId": 5,
    "dependsOnActionId": 3,
    "dependencyType": "FINISH_TO_START"
  }
}
```

**Dependency Types:**
- `FINISH_TO_START`
- `START_TO_START`
- `FINISH_TO_FINISH`
- `START_TO_FINISH`

---

## 📊 Action Analytics

### Query: Get Action Dashboard

**Operation:** `actionDashboard`

**GraphQL Query:**
```graphql
query GetActionDashboard($userId: Int, $teamId: Int, $period: String!) {
  actionDashboard(userId: $userId, teamId: $teamId, period: $period) {
    totalActions
    completedActions
    overdueActions
    inProgressActions
    completionRate
    averageCompletionTime
    actionsByPriority {
      priority
      count
    }
    actionsByType {
      typeName
      count
    }
    actionsByStatus {
      status
      count
    }
    upcomingDeadlines {
      actionId
      actionNumber
      title
      dueDate
      assignedTo {
        firstName
        lastName
      }
    }
  }
}
```

### Query: Get Action Performance

**Operation:** `actionPerformance`

**GraphQL Query:**
```graphql
query GetActionPerformance($userId: Int!, $startDate: DateTime!, $endDate: DateTime!) {
  actionPerformance(userId: $userId, startDate: $startDate, endDate: $endDate) {
    userId
    period
    totalActionsAssigned
    totalActionsCompleted
    averageCompletionTime
    onTimeCompletionRate
    qualityScore
    productivityTrend
    monthlyBreakdown {
      month
      completed
      onTime
      overdue
    }
  }
}
```

---

## 📝 Action Templates Operations

### Query: Get Action Templates

**Operation:** `actionTemplates`

**GraphQL Query:**
```graphql
query GetActionTemplates($category: String) {
  actionTemplates(category: $category) {
    id
    templateName
    description
    category
    actionTypeId
    defaultTitle
    defaultDescription
    defaultPriority
    defaultEstimatedHours
    checklist
    isActive
    usageCount
    actionType {
      typeName
    }
  }
}
```

### Mutation: Create Action from Template

**Operation:** `createActionFromTemplate`

**GraphQL Mutation:**
```graphql
mutation CreateActionFromTemplate($templateId: Int!, $input: CreateFromTemplateInput!) {
  createActionFromTemplate(templateId: $templateId, input: $input) {
    id
    actionNumber
    title
    priority
    dueDate
    createdDate
  }
}
```

---

## 🔄 Workflow Operations

### Query: Get Workflows

**Operation:** `workflows`

**GraphQL Query:**
```graphql
query GetWorkflows($isActive: Boolean) {
  workflows(isActive: $isActive) {
    id
    workflowName
    description
    triggerType
    isActive
    steps {
      stepNumber
      stepName
      actionTypeId
      assigneeRule
      approvalRequired
    }
  }
}
```

### Mutation: Start Workflow

**Operation:** `startWorkflow`

**GraphQL Mutation:**
```graphql
mutation StartWorkflow($workflowId: Int!, $input: StartWorkflowInput!) {
  startWorkflow(workflowId: $workflowId, input: $input) {
    workflowInstanceId
    status
    startedDate
    createdActions {
      id
      actionNumber
      title
    }
  }
}
```

---

*📝 Note: This service is currently in development. The actual implementation may vary from this specification.*