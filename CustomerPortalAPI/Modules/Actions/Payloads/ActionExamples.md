# Actions Module - API Payload Examples

## GraphQL Queries

### Get All Actions
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
    isActive
  }
}
```

Variables:
```json
{
  "filter": {
    "status": "In Progress",
    "priority": "High",
    "isOverdue": false
  },
  "sort": {
    "field": "DueDate",
    "direction": "ASC"
  },
  "skip": 0,
  "take": 20
}
```

### Get Action by ID
```graphql
query GetAction($id: Int!) {
  actionById(id: $id) {
    id
    actionName
    description
    priority
    status
    assignedToUserId
    dueDate
    comments
    createdDate
    modifiedDate
  }
}
```

Variables:
```json
{
  "id": 1
}
```

### Get Actions Statistics
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

## GraphQL Mutations

### Create Action
```graphql
mutation CreateAction($input: ActionInput!) {
  createAction(input: $input) {
    action {
      id
      actionName
      description
      priority
      status
      dueDate
      createdDate
    }
    errorMessage
  }
}
```

Variables:
```json
{
  "input": {
    "actionName": "Fix critical security vulnerability",
    "description": "Address the SQL injection vulnerability in the user authentication module",
    "actionType": "Corrective",
    "priority": "High",
    "status": "New",
    "assignedToUserId": 15,
    "companyId": 3,
    "siteId": 8,
    "auditId": 12,
    "findingId": 45,
    "dueDate": "2024-01-15T00:00:00Z",
    "comments": "This is a critical security issue that needs immediate attention"
  }
}
```

### Update Action
```graphql
mutation UpdateAction($input: UpdateActionInput!) {
  updateAction(input: $input) {
    action {
      id
      actionName
      status
      priority
      modifiedDate
    }
    errorMessage
  }
}
```

Variables:
```json
{
  "input": {
    "id": 1,
    "status": "In Progress",
    "priority": "Critical",
    "comments": "Started working on this issue. Estimated completion by end of week."
  }
}
```

### Complete Action
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

Variables:
```json
{
  "input": {
    "id": 1,
    "comments": "Security vulnerability has been fixed and tested. Code deployed to production."
  }
}
```

### Assign Action
```graphql
mutation AssignAction($input: AssignActionInput!) {
  assignAction(input: $input) {
    action {
      id
      actionName
      assignedToUserId
      modifiedDate
    }
    errorMessage
  }
}
```

Variables:
```json
{
  "input": {
    "id": 1,
    "assignedToUserId": 22
  }
}
```

## REST API Examples

### GET /api/actions
```http
GET /api/actions?status=In Progress&priority=High&assignedToUserId=15&skip=0&take=20
Accept: application/json
```

Response:
```json
[
  {
    "id": 1,
    "actionName": "Fix critical security vulnerability",
    "description": "Address the SQL injection vulnerability in the user authentication module",
    "actionTypeValue": "Corrective",
    "priority": "High",
    "status": "In Progress",
    "assignedToUserId": 15,
    "companyId": 3,
    "siteId": 8,
    "auditId": 12,
    "findingId": 45,
    "dueDate": "2024-01-15T00:00:00Z",
    "completedDate": null,
    "createdDate": "2024-01-01T10:30:00Z",
    "modifiedDate": "2024-01-02T14:20:00Z",
    "comments": "This is a critical security issue that needs immediate attention",
    "isActive": true
  }
]
```

### POST /api/actions
```http
POST /api/actions
Content-Type: application/json

{
  "actionName": "Implement two-factor authentication",
  "description": "Add 2FA support to enhance security for user accounts",
  "actionType": "Preventive",
  "priority": "Medium",
  "status": "New",
  "assignedToUserId": 18,
  "companyId": 3,
  "siteId": 8,
  "dueDate": "2024-02-28T00:00:00Z",
  "comments": "Part of security enhancement initiative"
}
```

Response:
```json
{
  "id": 25,
  "actionName": "Implement two-factor authentication",
  "description": "Add 2FA support to enhance security for user accounts",
  "actionTypeValue": "Preventive",
  "priority": "Medium",
  "status": "New",
  "assignedToUserId": 18,
  "companyId": 3,
  "siteId": 8,
  "auditId": null,
  "findingId": null,
  "dueDate": "2024-02-28T00:00:00Z",
  "completedDate": null,
  "createdDate": "2024-01-03T09:15:00Z",
  "modifiedDate": null,
  "comments": "Part of security enhancement initiative",
  "isActive": true
}
```

### PUT /api/actions/1
```http
PUT /api/actions/1
Content-Type: application/json

{
  "id": 1,
  "status": "In Progress",
  "priority": "Critical",
  "comments": "Escalated to critical priority due to potential security breach"
}
```

### PATCH /api/actions/1/complete
```http
PATCH /api/actions/1/complete
Content-Type: application/json

{
  "id": 1,
  "comments": "Security vulnerability patched. Applied security update and conducted thorough testing."
}
```

### GET /api/actions/user/15
```http
GET /api/actions/user/15
Accept: application/json
```

### GET /api/actions/overdue
```http
GET /api/actions/overdue
Accept: application/json
```

Response:
```json
[
  {
    "id": 5,
    "actionName": "Update firewall rules",
    "priority": "High",
    "status": "New",
    "dueDate": "2023-12-20T00:00:00Z",
    "createdDate": "2023-12-01T08:00:00Z",
    "isActive": true
  }
]
```

## Common Response Codes

- **200 OK**: Successful GET, PUT, PATCH operations
- **201 Created**: Successful POST operation
- **204 No Content**: Successful DELETE operation
- **400 Bad Request**: Invalid input data
- **404 Not Found**: Resource not found
- **500 Internal Server Error**: Server error

## Filter Options

- `actionName`: Filter by action name (partial match)
- `status`: Filter by status (exact match)
- `priority`: Filter by priority (exact match)
- `assignedToUserId`: Filter by assigned user ID
- `companyId`: Filter by company ID
- `siteId`: Filter by site ID
- `dueDateFrom`: Filter actions due after this date
- `dueDateTo`: Filter actions due before this date
- `isOverdue`: Filter overdue actions (true/false)
- `isCompleted`: Filter completed actions (true/false)

## Sort Options

- `ActionName`: Sort by action name
- `Priority`: Sort by priority
- `Status`: Sort by status
- `DueDate`: Sort by due date
- `CreatedDate`: Sort by creation date
- `ModifiedDate`: Sort by modification date

Sort direction: `ASC` (ascending) or `DESC` (descending)