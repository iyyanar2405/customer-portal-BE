# Settings Service Documentation

**Base URL:** `http://localhost:5010`  
**GraphQL Endpoint:** `http://localhost:5010/graphql`  
**Status:** 🔄 In Progress

## 📊 Entities Overview

The Settings Service manages application configuration, user preferences, and system settings:

1. **SystemSettings** - Global system configuration
2. **UserPreferences** - Individual user settings
3. **CompanySettings** - Company-specific configurations
4. **EmailTemplates** - Email template management
5. **NotificationSettings** - Notification preferences
6. **SecuritySettings** - Security and authentication settings

---

## ⚙️ System Settings Operations

### Query: Get All System Settings

**Operation:** `systemSettings`

**GraphQL Query:**
```graphql
query GetAllSystemSettings {
  systemSettings {
    id
    settingKey
    settingValue
    dataType
    category
    description
    isEditable
    isVisible
    defaultValue
    validationRules
    lastModified
    modifiedBy
    order
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "systemSettings": [
      {
        "id": 1,
        "settingKey": "system.timezone",
        "settingValue": "UTC",
        "dataType": "STRING",
        "category": "GENERAL",
        "description": "Default system timezone",
        "isEditable": true,
        "isVisible": true,
        "defaultValue": "UTC",
        "validationRules": "timezone",
        "lastModified": "2024-09-01T00:00:00Z",
        "modifiedBy": "system_admin",
        "order": 1
      },
      {
        "id": 2,
        "settingKey": "audit.default_duration_days",
        "settingValue": "3",
        "dataType": "INTEGER",
        "category": "AUDIT",
        "description": "Default audit duration in days",
        "isEditable": true,
        "isVisible": true,
        "defaultValue": "3",
        "validationRules": "min:1,max:30",
        "lastModified": "2024-09-01T00:00:00Z",
        "modifiedBy": "quality_manager",
        "order": 10
      }
    ]
  }
}
```

### Query: Get Settings by Category

**Operation:** `settingsByCategory`

**GraphQL Query:**
```graphql
query GetSettingsByCategory($category: String!) {
  settingsByCategory(category: $category) {
    id
    settingKey
    settingValue
    dataType
    description
    isEditable
    validationRules
    options
  }
}
```

### Mutation: Update System Setting

**Operation:** `updateSystemSetting`

**GraphQL Mutation:**
```graphql
mutation UpdateSystemSetting($settingKey: String!, $settingValue: String!) {
  updateSystemSetting(settingKey: $settingKey, settingValue: $settingValue) {
    id
    settingKey
    settingValue
    lastModified
    modifiedBy
  }
}
```

**Input Example:**
```json
{
  "settingKey": "audit.default_duration_days",
  "settingValue": "5"
}
```

---

## 👤 User Preferences Operations

### Query: Get User Preferences

**Operation:** `userPreferences`

**GraphQL Query:**
```graphql
query GetUserPreferences($userId: Int!) {
  userPreferences(userId: $userId) {
    userId
    timezone
    language
    dateFormat
    timeFormat
    currency
    theme
    dashboardLayout
    notificationPreferences {
      email
      sms
      push
      inApp
    }
    privacySettings {
      profileVisibility
      activityTracking
      dataSharing
    }
    workflowPreferences {
      defaultView
      itemsPerPage
      autoSave
      confirmActions
    }
    lastModified
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "userPreferences": {
      "userId": 1,
      "timezone": "America/New_York",
      "language": "en-US",
      "dateFormat": "MM/DD/YYYY",
      "timeFormat": "12h",
      "currency": "USD",
      "theme": "light",
      "dashboardLayout": "grid",
      "notificationPreferences": {
        "email": true,
        "sms": false,
        "push": true,
        "inApp": true
      },
      "privacySettings": {
        "profileVisibility": "company",
        "activityTracking": true,
        "dataSharing": false
      },
      "workflowPreferences": {
        "defaultView": "list",
        "itemsPerPage": 25,
        "autoSave": true,
        "confirmActions": true
      },
      "lastModified": "2024-09-15T10:30:00Z"
    }
  }
}
```

### Mutation: Update User Preferences

**Operation:** `updateUserPreferences`

**GraphQL Mutation:**
```graphql
mutation UpdateUserPreferences($userId: Int!, $input: UpdateUserPreferencesInput!) {
  updateUserPreferences(userId: $userId, input: $input) {
    userId
    timezone
    language
    theme
    lastModified
  }
}
```

**Input Payload:**
```json
{
  "userId": 1,
  "input": {
    "timezone": "Europe/London",
    "language": "en-GB",
    "theme": "dark",
    "dateFormat": "DD/MM/YYYY",
    "notificationPreferences": {
      "email": true,
      "sms": true,
      "push": false,
      "inApp": true
    }
  }
}
```

---

## 🏢 Company Settings Operations

### Query: Get Company Settings

**Operation:** `companySettings`

**GraphQL Query:**
```graphql
query GetCompanySettings($companyId: Int!) {
  companySettings(companyId: $companyId) {
    companyId
    businessHours {
      timezone
      workDays
      startTime
      endTime
      lunchBreak
    }
    auditSettings {
      defaultAuditDuration
      advanceNotificationDays
      reminderFrequency
      requireApproval
    }
    invoiceSettings {
      defaultPaymentTerms
      taxRate
      currency
      invoicePrefix
      nextInvoiceNumber
      logoUrl
    }
    certificateSettings {
      renewalNoticeDays
      autoRenewal
      digitalSignature
      templateSettings
    }
    integrationSettings {
      enabledIntegrations
      apiKeys
      webhookUrls
    }
    lastModified
    modifiedBy
  }
}
```

### Mutation: Update Company Settings

**Operation:** `updateCompanySettings`

**GraphQL Mutation:**
```graphql
mutation UpdateCompanySettings($companyId: Int!, $input: UpdateCompanySettingsInput!) {
  updateCompanySettings(companyId: $companyId, input: $input) {
    companyId
    lastModified
    modifiedBy
  }
}
```

---

## 📧 Email Template Operations

### Query: Get Email Templates

**Operation:** `emailTemplates`

**GraphQL Query:**
```graphql
query GetEmailTemplates($category: String, $isActive: Boolean) {
  emailTemplates(category: $category, isActive: $isActive) {
    id
    templateName
    category
    subject
    bodyHtml
    bodyText
    variables
    isActive
    isDefault
    usageCount
    createdDate
    modifiedDate
    previewUrl
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "emailTemplates": [
      {
        "id": 1,
        "templateName": "Audit Notification",
        "category": "AUDIT",
        "subject": "Audit Scheduled - {{companyName}}",
        "bodyHtml": "<html><body><h2>Audit Notification</h2><p>Dear {{contactPerson}},</p><p>Your {{auditType}} audit has been scheduled for {{auditDate}}.</p><p>Best regards,<br>Audit Team</p></body></html>",
        "bodyText": "Dear {{contactPerson}},\n\nYour {{auditType}} audit has been scheduled for {{auditDate}}.\n\nBest regards,\nAudit Team",
        "variables": ["companyName", "contactPerson", "auditType", "auditDate"],
        "isActive": true,
        "isDefault": true,
        "usageCount": 125,
        "createdDate": "2024-01-01T00:00:00Z",
        "modifiedDate": "2024-08-15T00:00:00Z",
        "previewUrl": "/templates/preview/1"
      }
    ]
  }
}
```

### Mutation: Create Email Template

**Operation:** `createEmailTemplate`

**GraphQL Mutation:**
```graphql
mutation CreateEmailTemplate($input: CreateEmailTemplateInput!) {
  createEmailTemplate(input: $input) {
    id
    templateName
    category
    subject
    isActive
    createdDate
  }
}
```

### Mutation: Update Email Template

**Operation:** `updateEmailTemplate`

**GraphQL Mutation:**
```graphql
mutation UpdateEmailTemplate($templateId: Int!, $input: UpdateEmailTemplateInput!) {
  updateEmailTemplate(templateId: $templateId, input: $input) {
    id
    templateName
    modifiedDate
  }
}
```

---

## 🔔 Notification Settings Operations

### Query: Get Notification Settings

**Operation:** `notificationSettings`

**GraphQL Query:**
```graphql
query GetNotificationSettings($userId: Int, $companyId: Int) {
  notificationSettings(userId: $userId, companyId: $companyId) {
    id
    entityType
    entityId
    eventType
    isEnabled
    channels
    frequency
    conditions
    quietHours
    escalationRules
    lastModified
  }
}
```

### Query: Get Notification Events

**Operation:** `notificationEvents`

**GraphQL Query:**
```graphql
query GetNotificationEvents {
  notificationEvents {
    eventType
    description
    category
    defaultChannels
    allowedChannels
    variables
    isConfigurable
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "notificationEvents": [
      {
        "eventType": "AUDIT_SCHEDULED",
        "description": "Triggered when an audit is scheduled",
        "category": "AUDIT",
        "defaultChannels": ["EMAIL", "IN_APP"],
        "allowedChannels": ["EMAIL", "SMS", "PUSH", "IN_APP"],
        "variables": ["auditTitle", "auditDate", "companyName", "leadAuditor"],
        "isConfigurable": true
      },
      {
        "eventType": "CERTIFICATE_EXPIRING",
        "description": "Triggered when certificate is approaching expiry",
        "category": "CERTIFICATE",
        "defaultChannels": ["EMAIL", "SMS"],
        "allowedChannels": ["EMAIL", "SMS", "PUSH"],
        "variables": ["certificateNumber", "companyName", "expiryDate", "daysRemaining"],
        "isConfigurable": true
      }
    ]
  }
}
```

### Mutation: Update Notification Setting

**Operation:** `updateNotificationSetting`

**GraphQL Mutation:**
```graphql
mutation UpdateNotificationSetting($input: UpdateNotificationSettingInput!) {
  updateNotificationSetting(input: $input) {
    id
    eventType
    isEnabled
    channels
    lastModified
  }
}
```

---

## 🔒 Security Settings Operations

### Query: Get Security Settings

**Operation:** `securitySettings`

**GraphQL Query:**
```graphql
query GetSecuritySettings($companyId: Int) {
  securitySettings(companyId: $companyId) {
    passwordPolicy {
      minLength
      requireUppercase
      requireLowercase
      requireNumbers
      requireSpecialChars
      maxAge
      preventReuse
    }
    sessionSettings {
      sessionTimeout
      maxConcurrentSessions
      requireReauth
      rememberMe
    }
    loginSettings {
      maxFailedAttempts
      lockoutDuration
      twoFactorRequired
      allowedIpRanges
    }
    auditSettings {
      logUserActions
      logDataChanges
      retentionPeriod
      encryptLogs
    }
    lastModified
    modifiedBy
  }
}
```

### Mutation: Update Security Settings

**Operation:** `updateSecuritySettings`

**GraphQL Mutation:**
```graphql
mutation UpdateSecuritySettings($input: UpdateSecuritySettingsInput!) {
  updateSecuritySettings(input: $input) {
    passwordPolicy {
      minLength
      maxAge
    }
    sessionSettings {
      sessionTimeout
    }
    lastModified
  }
}
```

---

## 🔧 Configuration Management

### Query: Get Configuration Schema

**Operation:** `configurationSchema`

**GraphQL Query:**
```graphql
query GetConfigurationSchema($category: String) {
  configurationSchema(category: $category) {
    category
    settings {
      key
      dataType
      defaultValue
      description
      isRequired
      validationRules
      options
      dependencies
    }
  }
}
```

### Query: Validate Configuration

**Operation:** `validateConfiguration`

**GraphQL Query:**
```graphql
query ValidateConfiguration($settings: [SettingInput!]!) {
  validateConfiguration(settings: $settings) {
    isValid
    errors {
      settingKey
      errorMessage
      errorCode
    }
    warnings {
      settingKey
      warningMessage
    }
  }
}
```

### Mutation: Backup Settings

**Operation:** `backupSettings`

**GraphQL Mutation:**
```graphql
mutation BackupSettings($scope: String!, $entityId: Int) {
  backupSettings(scope: $scope, entityId: $entityId) {
    backupId
    scope
    entityId
    backupDate
    settingsCount
    downloadUrl
  }
}
```

### Mutation: Restore Settings

**Operation:** `restoreSettings`

**GraphQL Mutation:**
```graphql
mutation RestoreSettings($backupId: String!) {
  restoreSettings(backupId: $backupId) {
    success
    restoredCount
    skippedCount
    errors
    restoreDate
  }
}
```

---

## 🎨 UI Customization Operations

### Query: Get UI Settings

**Operation:** `uiSettings`

**GraphQL Query:**
```graphql
query GetUISettings($companyId: Int, $userId: Int) {
  uiSettings(companyId: $companyId, userId: $userId) {
    theme {
      primaryColor
      secondaryColor
      backgroundColor
      textColor
      fontFamily
      fontSize
    }
    layout {
      sidebarCollapsed
      headerHeight
      footerVisible
      breadcrumbsVisible
    }
    dashboard {
      widgets {
        widgetType
        position
        size
        isVisible
        configuration
      }
      refreshInterval
      autoRefresh
    }
    customizations {
      logoUrl
      faviconUrl
      customCss
      customJs
    }
  }
}
```

### Mutation: Update UI Settings

**Operation:** `updateUISettings`

**GraphQL Mutation:**
```graphql
mutation UpdateUISettings($input: UpdateUISettingsInput!) {
  updateUISettings(input: $input) {
    theme {
      primaryColor
      secondaryColor
    }
    lastModified
  }
}
```

---

## 📊 Settings Analytics

### Query: Get Settings Usage

**Operation:** `settingsUsage`

**GraphQL Query:**
```graphql
query GetSettingsUsage($period: String!) {
  settingsUsage(period: $period) {
    totalSettings
    modifiedSettings
    activeUsers
    mostChangedSettings {
      settingKey
      changeCount
      lastChanged
    }
    userActivity {
      userId
      userName
      changesCount
      lastActivity
    }
    categoryBreakdown {
      category
      settingsCount
      modificationCount
    }
  }
}
```

### Query: Get Audit Trail

**Operation:** `settingsAuditTrail`

**GraphQL Query:**
```graphql
query GetSettingsAuditTrail($settingKey: String, $userId: Int, $startDate: DateTime, $endDate: DateTime) {
  settingsAuditTrail(settingKey: $settingKey, userId: $userId, startDate: $startDate, endDate: $endDate) {
    id
    settingKey
    oldValue
    newValue
    changedBy
    changedDate
    changeReason
    ipAddress
    userAgent
    rollbackId
  }
}
```

---

*📝 Note: This service is currently in development. The actual implementation may vary from this specification.*