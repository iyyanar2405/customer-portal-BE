# Notifications Service Documentation

**Base URL:** `http://localhost:5009`  
**GraphQL Endpoint:** `http://localhost:5009/graphql`  
**Status:** 🔄 In Progress

## 📊 Entities Overview

The Notifications Service manages communication, alerts, and messaging across the system:

1. **Notifications** - Main notification records
2. **NotificationCategories** - Classification of notifications
3. **NotificationTemplates** - Reusable message templates
4. **NotificationChannels** - Delivery methods (email, SMS, push)
5. **NotificationSchedules** - Scheduled/recurring notifications
6. **NotificationSubscriptions** - User notification preferences

---

## 📢 Notifications Operations

### Query: Get All Notifications

**Operation:** `notifications`

**GraphQL Query:**
```graphql
query GetAllNotifications {
  notifications {
    id
    title
    message
    categoryId
    recipientId
    senderId
    priority
    status
    deliveryMethod
    scheduledDate
    sentDate
    readDate
    isRead
    isArchived
    expiryDate
    actionUrl
    metadata
    createdDate
    modifiedDate
    category {
      categoryName
      iconName
      colorCode
    }
    recipient {
      firstName
      lastName
      email
    }
    sender {
      firstName
      lastName
    }
    deliveryStatus {
      method
      status
      attemptCount
      lastAttempt
      errorMessage
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "notifications": [
      {
        "id": 1,
        "title": "Audit Scheduled",
        "message": "Your ISO 9001:2015 surveillance audit has been scheduled for October 15, 2024",
        "categoryId": 1,
        "recipientId": 2,
        "senderId": 1,
        "priority": "HIGH",
        "status": "DELIVERED",
        "deliveryMethod": "EMAIL",
        "scheduledDate": "2024-09-20T09:00:00Z",
        "sentDate": "2024-09-20T09:05:00Z",
        "readDate": null,
        "isRead": false,
        "isArchived": false,
        "expiryDate": "2024-10-20T00:00:00Z",
        "actionUrl": "/audits/1",
        "metadata": {
          "auditId": 1,
          "auditType": "surveillance",
          "reminderType": "initial"
        },
        "createdDate": "2024-09-20T09:00:00Z",
        "modifiedDate": null,
        "category": {
          "categoryName": "Audit Notifications",
          "iconName": "calendar-check",
          "colorCode": "#3B82F6"
        },
        "recipient": {
          "firstName": "Jane",
          "lastName": "Smith",
          "email": "jane.smith@acme.com"
        },
        "sender": {
          "firstName": "System",
          "lastName": "Admin"
        },
        "deliveryStatus": [
          {
            "method": "EMAIL",
            "status": "DELIVERED",
            "attemptCount": 1,
            "lastAttempt": "2024-09-20T09:05:00Z",
            "errorMessage": null
          }
        ]
      }
    ]
  }
}
```

### Query: Get User Notifications

**Operation:** `userNotifications`

**GraphQL Query:**
```graphql
query GetUserNotifications($userId: Int!, $isRead: Boolean, $categoryId: Int, $limit: Int, $offset: Int) {
  userNotifications(userId: $userId, isRead: $isRead, categoryId: $categoryId, limit: $limit, offset: $offset) {
    id
    title
    message
    priority
    sentDate
    isRead
    actionUrl
    category {
      categoryName
      iconName
      colorCode
    }
  }
}
```

### Mutation: Create Notification

**Operation:** `createNotification`

**GraphQL Mutation:**
```graphql
mutation CreateNotification($input: CreateNotificationInput!) {
  createNotification(input: $input) {
    id
    title
    priority
    status
    scheduledDate
    createdDate
  }
}
```

**Input Payload:**
```json
{
  "input": {
    "title": "Certificate Expiry Reminder",
    "message": "Your ISO 9001:2015 certificate will expire in 30 days. Please initiate renewal process.",
    "categoryId": 2,
    "recipientIds": [2, 3, 4],
    "priority": "HIGH",
    "deliveryMethods": ["EMAIL", "SMS"],
    "scheduledDate": "2024-09-25T10:00:00Z",
    "expiryDate": "2024-11-25T00:00:00Z",
    "actionUrl": "/certificates/renewal/1",
    "metadata": {
      "certificateId": 1,
      "daysUntilExpiry": 30,
      "reminderType": "30_day_warning"
    },
    "templateId": 3
  }
}
```

### Mutation: Mark as Read

**Operation:** `markNotificationAsRead`

**GraphQL Mutation:**
```graphql
mutation MarkNotificationAsRead($notificationId: Int!) {
  markNotificationAsRead(notificationId: $notificationId) {
    id
    isRead
    readDate
  }
}
```

### Mutation: Archive Notification

**Operation:** `archiveNotification`

**GraphQL Mutation:**
```graphql
mutation ArchiveNotification($notificationId: Int!) {
  archiveNotification(notificationId: $notificationId) {
    id
    isArchived
    modifiedDate
  }
}
```

---

## 📂 Notification Categories Operations

### Query: Get All Notification Categories

**Operation:** `notificationCategories`

**GraphQL Query:**
```graphql
query GetAllNotificationCategories {
  notificationCategories {
    id
    categoryName
    description
    iconName
    colorCode
    defaultPriority
    isActive
    allowUnsubscribe
    retentionDays
    createdDate
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "notificationCategories": [
      {
        "id": 1,
        "categoryName": "Audit Notifications",
        "description": "Notifications related to audit scheduling and activities",
        "iconName": "calendar-check",
        "colorCode": "#3B82F6",
        "defaultPriority": "MEDIUM",
        "isActive": true,
        "allowUnsubscribe": false,
        "retentionDays": 90,
        "createdDate": "2024-01-01T00:00:00Z"
      },
      {
        "id": 2,
        "categoryName": "Certificate Alerts",
        "description": "Certificate expiry and renewal notifications",
        "iconName": "certificate",
        "colorCode": "#F59E0B",
        "defaultPriority": "HIGH",
        "isActive": true,
        "allowUnsubscribe": false,
        "retentionDays": 365,
        "createdDate": "2024-01-01T00:00:00Z"
      }
    ]
  }
}
```

---

## 📄 Notification Templates Operations

### Query: Get All Notification Templates

**Operation:** `notificationTemplates`

**GraphQL Query:**
```graphql
query GetAllNotificationTemplates {
  notificationTemplates {
    id
    templateName
    categoryId
    subject
    bodyTemplate
    templateType
    variables
    isActive
    usageCount
    createdDate
    modifiedDate
    category {
      categoryName
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "notificationTemplates": [
      {
        "id": 1,
        "templateName": "Audit Reminder",
        "categoryId": 1,
        "subject": "Audit Scheduled - {{auditTitle}}",
        "bodyTemplate": "Dear {{recipientName}},\n\nYour {{auditType}} audit has been scheduled for {{auditDate}}.\n\nAudit Details:\n- Title: {{auditTitle}}\n- Date: {{auditDate}}\n- Lead Auditor: {{leadAuditor}}\n- Sites: {{sitesList}}\n\nPlease prepare the necessary documentation and ensure your team is available.\n\nBest regards,\nAudit Team",
        "templateType": "EMAIL",
        "variables": [
          "recipientName",
          "auditTitle",
          "auditType",
          "auditDate",
          "leadAuditor",
          "sitesList"
        ],
        "isActive": true,
        "usageCount": 45,
        "createdDate": "2024-01-01T00:00:00Z",
        "modifiedDate": "2024-06-15T00:00:00Z",
        "category": {
          "categoryName": "Audit Notifications"
        }
      }
    ]
  }
}
```

### Mutation: Create Template

**Operation:** `createNotificationTemplate`

**GraphQL Mutation:**
```graphql
mutation CreateNotificationTemplate($input: CreateTemplateInput!) {
  createNotificationTemplate(input: $input) {
    id
    templateName
    subject
    templateType
    createdDate
  }
}
```

---

## 🔔 Notification Subscriptions Operations

### Query: Get User Subscriptions

**Operation:** `userSubscriptions`

**GraphQL Query:**
```graphql
query GetUserSubscriptions($userId: Int!) {
  userSubscriptions(userId: $userId) {
    userId
    categoryId
    isSubscribed
    deliveryMethods
    frequency
    quietHours
    modifiedDate
    category {
      categoryName
      description
      allowUnsubscribe
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "userSubscriptions": [
      {
        "userId": 2,
        "categoryId": 1,
        "isSubscribed": true,
        "deliveryMethods": ["EMAIL", "PUSH"],
        "frequency": "IMMEDIATE",
        "quietHours": {
          "enabled": true,
          "startTime": "22:00",
          "endTime": "07:00",
          "timezone": "UTC"
        },
        "modifiedDate": "2024-09-01T00:00:00Z",
        "category": {
          "categoryName": "Audit Notifications",
          "description": "Notifications related to audit scheduling and activities",
          "allowUnsubscribe": false
        }
      }
    ]
  }
}
```

### Mutation: Update Subscription

**Operation:** `updateNotificationSubscription`

**GraphQL Mutation:**
```graphql
mutation UpdateNotificationSubscription($input: UpdateSubscriptionInput!) {
  updateNotificationSubscription(input: $input) {
    userId
    categoryId
    isSubscribed
    deliveryMethods
    frequency
    modifiedDate
  }
}
```

**Input Payload:**
```json
{
  "input": {
    "userId": 2,
    "categoryId": 2,
    "isSubscribed": true,
    "deliveryMethods": ["EMAIL"],
    "frequency": "DAILY_DIGEST",
    "quietHours": {
      "enabled": true,
      "startTime": "20:00",
      "endTime": "08:00",
      "timezone": "America/New_York"
    }
  }
}
```

**Frequency Options:**
- `IMMEDIATE`
- `HOURLY_DIGEST`
- `DAILY_DIGEST`
- `WEEKLY_DIGEST`
- `NEVER`

---

## 📊 Notification Analytics

### Query: Get Notification Dashboard

**Operation:** `notificationDashboard`

**GraphQL Query:**
```graphql
query GetNotificationDashboard($period: String!, $userId: Int) {
  notificationDashboard(period: $period, userId: $userId) {
    totalNotifications
    sentNotifications
    deliveredNotifications
    readNotifications
    failedNotifications
    deliveryRate
    readRate
    responseRate
    notificationsByCategory {
      categoryName
      count
      deliveryRate
    }
    notificationsByPriority {
      priority
      count
      readRate
    }
    notificationsByMethod {
      method
      count
      deliveryRate
    }
    dailyVolume {
      date
      sent
      delivered
      read
    }
  }
}
```

### Query: Get Delivery Statistics

**Operation:** `deliveryStatistics`

**GraphQL Query:**
```graphql
query GetDeliveryStatistics($startDate: DateTime!, $endDate: DateTime!) {
  deliveryStatistics(startDate: $startDate, endDate: $endDate) {
    totalAttempts
    successfulDeliveries
    failedDeliveries
    bounceRate
    openRate
    clickRate
    unsubscribeRate
    methodPerformance {
      method
      attempts
      delivered
      failed
      averageDeliveryTime
    }
    failureReasons {
      reason
      count
      percentage
    }
  }
}
```

---

## 📅 Scheduled Notifications Operations

### Query: Get Scheduled Notifications

**Operation:** `scheduledNotifications`

**GraphQL Query:**
```graphql
query GetScheduledNotifications($status: String, $categoryId: Int) {
  scheduledNotifications(status: $status, categoryId: $categoryId) {
    id
    title
    categoryId
    scheduledDate
    frequency
    nextRunDate
    lastRunDate
    status
    recipientCount
    isActive
    category {
      categoryName
    }
  }
}
```

### Mutation: Create Scheduled Notification

**Operation:** `createScheduledNotification`

**GraphQL Mutation:**
```graphql
mutation CreateScheduledNotification($input: CreateScheduledNotificationInput!) {
  createScheduledNotification(input: $input) {
    id
    title
    frequency
    scheduledDate
    nextRunDate
    status
    createdDate
  }
}
```

**Input Payload:**
```json
{
  "input": {
    "title": "Weekly Compliance Report",
    "templateId": 5,
    "categoryId": 3,
    "recipientQuery": "role:manager AND department:quality",
    "frequency": "WEEKLY",
    "scheduledDate": "2024-09-23T09:00:00Z",
    "endDate": "2024-12-31T23:59:59Z",
    "metadata": {
      "reportType": "compliance",
      "includeCharts": true
    }
  }
}
```

**Frequency Options:**
- `ONCE`
- `DAILY`
- `WEEKLY`
- `MONTHLY`
- `QUARTERLY`
- `YEARLY`

---

## 🔄 Notification Workflows

### Mutation: Trigger Workflow Notification

**Operation:** `triggerWorkflowNotification`

**GraphQL Mutation:**
```graphql
mutation TriggerWorkflowNotification($input: TriggerWorkflowInput!) {
  triggerWorkflowNotification(input: $input) {
    workflowId
    triggeredNotifications {
      id
      title
      recipientId
      status
    }
    executionTime
  }
}
```

### Query: Get Notification Workflows

**Operation:** `notificationWorkflows`

**GraphQL Query:**
```graphql
query GetNotificationWorkflows($isActive: Boolean) {
  notificationWorkflows(isActive: $isActive) {
    id
    workflowName
    triggerEvent
    conditions
    isActive
    steps {
      stepNumber
      templateId
      delay
      conditions
    }
  }
}
```

---

*📝 Note: This service is currently in development. The actual implementation may vary from this specification.*