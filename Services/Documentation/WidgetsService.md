# Widgets Service Documentation

**Base URL:** `http://localhost:5012`  
**GraphQL Endpoint:** `http://localhost:5012/graphql`  
**Status:** 🔄 In Progress

## 📊 Entities Overview

The Widgets Service manages UI components, dashboard widgets, and interactive elements:

1. **Widgets** - Widget definitions and configurations
2. **WidgetTypes** - Available widget types and templates
3. **WidgetInstances** - User-specific widget instances
4. **WidgetLayouts** - Dashboard layout configurations
5. **WidgetData** - Data sources and bindings
6. **WidgetPermissions** - Access control for widgets

---

## 🧩 Widget Operations

### Query: Get All Widgets

**Operation:** `widgets`

**GraphQL Query:**
```graphql
query GetAllWidgets {
  widgets {
    id
    widgetName
    widgetType
    description
    category
    version
    isActive
    isPublic
    createdBy
    createdDate
    modifiedDate
    configuration {
      title
      subtitle
      dataSource
      refreshInterval
      styling
      interactions
    }
    defaultSize {
      width
      height
      minWidth
      minHeight
      maxWidth
      maxHeight
    }
    permissions {
      view
      edit
      share
      delete
    }
    usage {
      instanceCount
      popularity
      averageRating
    }
    metadata {
      tags
      author
      documentation
      dependencies
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "widgets": [
      {
        "id": 1,
        "widgetName": "Audit Status Chart",
        "widgetType": "PIE_CHART",
        "description": "Shows distribution of audit statuses",
        "category": "ANALYTICS",
        "version": "1.2.0",
        "isActive": true,
        "isPublic": true,
        "createdBy": 1,
        "createdDate": "2024-01-15T00:00:00Z",
        "modifiedDate": "2024-08-20T00:00:00Z",
        "configuration": {
          "title": "Audit Status Distribution",
          "subtitle": "Current audit pipeline",
          "dataSource": "audits.status_summary",
          "refreshInterval": 300,
          "styling": {
            "colors": ["#4CAF50", "#FF9800", "#F44336"],
            "showLegend": true,
            "showLabels": true
          },
          "interactions": {
            "clickable": true,
            "drillDown": true,
            "filters": ["dateRange", "company"]
          }
        },
        "defaultSize": {
          "width": 6,
          "height": 4,
          "minWidth": 4,
          "minHeight": 3,
          "maxWidth": 12,
          "maxHeight": 8
        },
        "permissions": {
          "view": ["USER", "MANAGER", "ADMIN"],
          "edit": ["MANAGER", "ADMIN"],
          "share": ["MANAGER", "ADMIN"],
          "delete": ["ADMIN"]
        },
        "usage": {
          "instanceCount": 25,
          "popularity": 4.2,
          "averageRating": 4.5
        },
        "metadata": {
          "tags": ["audit", "status", "chart", "analytics"],
          "author": "System Administrator",
          "documentation": "/docs/widgets/audit-status-chart",
          "dependencies": ["chart.js", "audit-service"]
        }
      }
    ]
  }
}
```

### Query: Get Widget Types

**Operation:** `widgetTypes`

**GraphQL Query:**
```graphql
query GetWidgetTypes($category: String) {
  widgetTypes(category: $category) {
    id
    typeName
    displayName
    description
    category
    iconUrl
    previewUrl
    isBuiltIn
    configurationSchema
    dataRequirements
    supportedSizes
    features
    examples
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "widgetTypes": [
      {
        "id": 1,
        "typeName": "PIE_CHART",
        "displayName": "Pie Chart",
        "description": "Circular chart showing data distribution",
        "category": "CHARTS",
        "iconUrl": "/icons/pie-chart.svg",
        "previewUrl": "/previews/pie-chart.png",
        "isBuiltIn": true,
        "configurationSchema": {
          "title": "string",
          "dataSource": "string",
          "colors": "array",
          "showLegend": "boolean"
        },
        "dataRequirements": {
          "format": "key-value",
          "minRecords": 1,
          "maxRecords": 20,
          "requiredFields": ["label", "value"]
        },
        "supportedSizes": {
          "small": { "width": 4, "height": 3 },
          "medium": { "width": 6, "height": 4 },
          "large": { "width": 8, "height": 6 }
        },
        "features": ["responsive", "interactive", "exportable"],
        "examples": [
          {
            "name": "Audit Status Distribution",
            "config": { "dataSource": "audit.status" }
          }
        ]
      }
    ]
  }
}
```

### Mutation: Create Widget

**Operation:** `createWidget`

**GraphQL Mutation:**
```graphql
mutation CreateWidget($input: CreateWidgetInput!) {
  createWidget(input: $input) {
    id
    widgetName
    widgetType
    version
    isActive
    createdDate
  }
}
```

**Input Payload:**
```json
{
  "input": {
    "widgetName": "Certificate Expiry Timeline",
    "widgetType": "TIMELINE",
    "description": "Shows upcoming certificate expiry dates",
    "category": "COMPLIANCE",
    "isPublic": true,
    "configuration": {
      "title": "Certificate Expiry Timeline",
      "dataSource": "certificates.expiry_timeline",
      "timeRange": "6_months",
      "styling": {
        "showGrid": true,
        "highlightExpired": true,
        "colorScheme": "status_based"
      }
    },
    "defaultSize": {
      "width": 12,
      "height": 6
    },
    "permissions": {
      "view": ["USER", "MANAGER", "ADMIN"],
      "edit": ["ADMIN"]
    },
    "metadata": {
      "tags": ["certificate", "compliance", "timeline"]
    }
  }
}
```

### Mutation: Update Widget

**Operation:** `updateWidget`

**GraphQL Mutation:**
```graphql
mutation UpdateWidget($widgetId: Int!, $input: UpdateWidgetInput!) {
  updateWidget(widgetId: $widgetId, input: $input) {
    id
    widgetName
    version
    modifiedDate
    configuration
  }
}
```

---

## 📱 Widget Instances Operations

### Query: Get User Widget Instances

**Operation:** `userWidgetInstances`

**GraphQL Query:**
```graphql
query GetUserWidgetInstances($userId: Int!, $dashboardId: Int) {
  userWidgetInstances(userId: $userId, dashboardId: $dashboardId) {
    id
    widgetId
    userId
    dashboardId
    instanceName
    position {
      x
      y
      width
      height
    }
    customConfiguration
    filters
    isVisible
    createdDate
    lastAccessed
    widget {
      widgetName
      widgetType
      description
    }
    data {
      lastRefresh
      isLoading
      hasError
      errorMessage
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "userWidgetInstances": [
      {
        "id": 1,
        "widgetId": 1,
        "userId": 2,
        "dashboardId": 1,
        "instanceName": "My Audit Status",
        "position": {
          "x": 0,
          "y": 0,
          "width": 6,
          "height": 4
        },
        "customConfiguration": {
          "title": "My Team's Audits",
          "filters": {
            "assignedTo": "current_user",
            "status": ["PLANNED", "IN_PROGRESS"]
          }
        },
        "filters": {
          "dateRange": "last_30_days",
          "companyIds": [1, 2, 3]
        },
        "isVisible": true,
        "createdDate": "2024-09-01T00:00:00Z",
        "lastAccessed": "2024-09-20T14:30:00Z",
        "widget": {
          "widgetName": "Audit Status Chart",
          "widgetType": "PIE_CHART",
          "description": "Shows distribution of audit statuses"
        },
        "data": {
          "lastRefresh": "2024-09-20T14:25:00Z",
          "isLoading": false,
          "hasError": false,
          "errorMessage": null
        }
      }
    ]
  }
}
```

### Mutation: Create Widget Instance

**Operation:** `createWidgetInstance`

**GraphQL Mutation:**
```graphql
mutation CreateWidgetInstance($input: CreateWidgetInstanceInput!) {
  createWidgetInstance(input: $input) {
    id
    widgetId
    instanceName
    position
    isVisible
    createdDate
  }
}
```

### Mutation: Update Widget Position

**Operation:** `updateWidgetPosition`

**GraphQL Mutation:**
```graphql
mutation UpdateWidgetPosition($instanceId: Int!, $position: PositionInput!) {
  updateWidgetPosition(instanceId: $instanceId, position: $position) {
    id
    position {
      x
      y
      width
      height
    }
    modifiedDate
  }
}
```

### Mutation: Delete Widget Instance

**Operation:** `deleteWidgetInstance`

**GraphQL Mutation:**
```graphql
mutation DeleteWidgetInstance($instanceId: Int!) {
  deleteWidgetInstance(instanceId: $instanceId)
}
```

---

## 📊 Widget Data Operations

### Query: Get Widget Data

**Operation:** `widgetData`

**GraphQL Query:**
```graphql
query GetWidgetData($instanceId: Int!, $refresh: Boolean) {
  widgetData(instanceId: $instanceId, refresh: $refresh) {
    instanceId
    data
    metadata {
      totalRecords
      lastUpdated
      dataSource
      executionTime
    }
    schema {
      fields {
        name
        type
        format
        description
      }
    }
    status {
      isLoading
      hasError
      errorMessage
      cacheHit
    }
  }
}
```

### Query: Get Available Data Sources

**Operation:** `availableDataSources`

**GraphQL Query:**
```graphql
query GetAvailableDataSources($category: String) {
  availableDataSources(category: $category) {
    id
    sourceName
    displayName
    description
    category
    type
    endpoint
    parameters
    schema
    refreshRate
    permissions
    examples
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "availableDataSources": [
      {
        "id": 1,
        "sourceName": "audit.status_summary",
        "displayName": "Audit Status Summary",
        "description": "Aggregated audit status counts",
        "category": "AUDIT",
        "type": "AGGREGATION",
        "endpoint": "/api/audits/status-summary",
        "parameters": {
          "dateRange": "optional",
          "companyIds": "optional",
          "status": "optional"
        },
        "schema": {
          "status": "string",
          "count": "number",
          "percentage": "number"
        },
        "refreshRate": 300,
        "permissions": ["USER", "MANAGER", "ADMIN"],
        "examples": [
          {
            "description": "All audit statuses",
            "parameters": {}
          },
          {
            "description": "Last 30 days",
            "parameters": { "dateRange": "last_30_days" }
          }
        ]
      }
    ]
  }
}
```

### Mutation: Refresh Widget Data

**Operation:** `refreshWidgetData`

**GraphQL Mutation:**
```graphql
mutation RefreshWidgetData($instanceId: Int!) {
  refreshWidgetData(instanceId: $instanceId) {
    instanceId
    refreshed
    lastRefresh
    executionTime
    recordCount
  }
}
```

---

## 📱 Dashboard Layout Operations

### Query: Get Dashboard Layouts

**Operation:** `dashboardLayouts`

**GraphQL Query:**
```graphql
query GetDashboardLayouts($userId: Int!) {
  dashboardLayouts(userId: $userId) {
    id
    layoutName
    description
    isDefault
    isShared
    columns
    rowHeight
    widgets {
      instanceId
      position {
        x
        y
        width
        height
      }
    }
    settings {
      autoSave
      snapToGrid
      compactMode
      theme
    }
    createdDate
    modifiedDate
  }
}
```

### Mutation: Save Dashboard Layout

**Operation:** `saveDashboardLayout`

**GraphQL Mutation:**
```graphql
mutation SaveDashboardLayout($input: SaveLayoutInput!) {
  saveDashboardLayout(input: $input) {
    id
    layoutName
    isDefault
    modifiedDate
  }
}
```

### Mutation: Apply Layout Template

**Operation:** `applyLayoutTemplate`

**GraphQL Mutation:**
```graphql
mutation ApplyLayoutTemplate($templateId: Int!, $userId: Int!) {
  applyLayoutTemplate(templateId: $templateId, userId: $userId) {
    layoutId
    appliedWidgets
    createdInstances
    conflicts
  }
}
```

---

## 🎨 Widget Styling Operations

### Query: Get Widget Themes

**Operation:** `widgetThemes`

**GraphQL Query:**
```graphql
query GetWidgetThemes {
  widgetThemes {
    id
    themeName
    description
    colors {
      primary
      secondary
      background
      text
      accent
    }
    typography {
      fontFamily
      fontSize
      fontWeight
    }
    spacing {
      padding
      margin
      borderRadius
    }
    isDefault
    preview
  }
}
```

### Mutation: Update Widget Styling

**Operation:** `updateWidgetStyling`

**GraphQL Mutation:**
```graphql
mutation UpdateWidgetStyling($instanceId: Int!, $styling: WidgetStylingInput!) {
  updateWidgetStyling(instanceId: $instanceId, styling: $styling) {
    instanceId
    styling
    previewUrl
    modifiedDate
  }
}
```

---

## 📈 Widget Analytics

### Query: Get Widget Usage Analytics

**Operation:** `widgetUsageAnalytics`

**GraphQL Query:**
```graphql
query GetWidgetUsageAnalytics($timeframe: String!) {
  widgetUsageAnalytics(timeframe: $timeframe) {
    totalWidgets
    activeWidgets
    totalInstances
    averageWidgetsPerUser
    mostPopularWidgets {
      widgetName
      instanceCount
      averageRating
      usageFrequency
    }
    usageByType {
      widgetType
      count
      percentage
    }
    usageByCategory {
      category
      count
      averageEngagement
    }
    performanceMetrics {
      averageLoadTime
      errorRate
      cacheHitRatio
    }
  }
}
```

### Query: Get Widget Performance

**Operation:** `widgetPerformance`

**GraphQL Query:**
```graphql
query GetWidgetPerformance($widgetId: Int!, $period: String!) {
  widgetPerformance(widgetId: $widgetId, period: $period) {
    widgetId
    period
    viewCount
    uniqueUsers
    averageViewDuration
    interactionRate
    loadTime {
      average
      median
      p95
      p99
    }
    errors {
      count
      rate
      topErrors
    }
    userFeedback {
      averageRating
      reviewCount
      sentimentScore
    }
  }
}
```

---

## 🔧 Widget Configuration

### Query: Get Widget Configuration Schema

**Operation:** `widgetConfigurationSchema`

**GraphQL Query:**
```graphql
query GetWidgetConfigurationSchema($widgetTypeId: Int!) {
  widgetConfigurationSchema(widgetTypeId: $widgetTypeId) {
    widgetTypeId
    schema {
      properties {
        name
        type
        required
        default
        options
        validation
        description
        group
      }
      groups {
        name
        label
        order
        collapsible
      }
    }
    examples
    documentation
  }
}
```

### Mutation: Validate Widget Configuration

**Operation:** `validateWidgetConfiguration`

**GraphQL Mutation:**
```graphql
mutation ValidateWidgetConfiguration($widgetTypeId: Int!, $configuration: WidgetConfigurationInput!) {
  validateWidgetConfiguration(widgetTypeId: $widgetTypeId, configuration: $configuration) {
    isValid
    errors {
      field
      message
      code
    }
    warnings {
      field
      message
      suggestion
    }
    optimizations {
      field
      recommendation
      impact
    }
  }
}
```

---

## 🔄 Widget Automation

### Query: Get Widget Automation Rules

**Operation:** `widgetAutomationRules`

**GraphQL Query:**
```graphql
query GetWidgetAutomationRules($instanceId: Int) {
  widgetAutomationRules(instanceId: $instanceId) {
    id
    ruleName
    instanceId
    triggerType
    conditions
    actions
    isActive
    executionCount
    lastExecuted
    nextExecution
  }
}
```

### Mutation: Create Automation Rule

**Operation:** `createWidgetAutomationRule`

**GraphQL Mutation:**
```graphql
mutation CreateWidgetAutomationRule($input: CreateAutomationRuleInput!) {
  createWidgetAutomationRule(input: $input) {
    id
    ruleName
    triggerType
    isActive
    createdDate
  }
}
```

---

## 📤 Widget Export/Import

### Mutation: Export Widget

**Operation:** `exportWidget`

**GraphQL Mutation:**
```graphql
mutation ExportWidget($widgetId: Int!, $includeData: Boolean!) {
  exportWidget(widgetId: $widgetId, includeData: $includeData) {
    exportId
    downloadUrl
    format
    size
    expiryDate
  }
}
```

### Mutation: Import Widget

**Operation:** `importWidget`

**GraphQL Mutation:**
```graphql
mutation ImportWidget($importData: String!, $options: ImportOptionsInput!) {
  importWidget(importData: $importData, options: $options) {
    widgetId
    importedComponents
    conflicts
    success
    warnings
  }
}
```

---

*📝 Note: This service is currently in development. The actual implementation may vary from this specification.*