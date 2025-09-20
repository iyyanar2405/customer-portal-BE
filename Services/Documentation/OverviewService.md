# Overview Service Documentation

**Base URL:** `http://localhost:5011`  
**GraphQL Endpoint:** `http://localhost:5011/graphql`  
**Status:** 🔄 In Progress

## 📊 Entities Overview

The Overview Service provides dashboard analytics, KPIs, reporting, and business intelligence:

1. **Dashboards** - Dashboard configurations and widgets
2. **KPIs** - Key Performance Indicators tracking
3. **Reports** - Business reports and analytics
4. **Metrics** - System and business metrics
5. **Trends** - Historical trend analysis
6. **Insights** - AI-powered business insights

---

## 📈 Dashboard Operations

### Query: Get Main Dashboard

**Operation:** `mainDashboard`

**GraphQL Query:**
```graphql
query GetMainDashboard($userId: Int!, $timeframe: String!) {
  mainDashboard(userId: $userId, timeframe: $timeframe) {
    summary {
      totalCompanies
      activeCertificates
      upcomingAudits
      pendingActions
      overdueItems
      monthlyRevenue
      complianceScore
      userActivity
    }
    charts {
      auditTrend {
        period
        planned
        completed
        cancelled
      }
      certificateStatus {
        status
        count
        percentage
      }
      revenueByService {
        serviceName
        amount
        growth
      }
      complianceByCompany {
        companyName
        score
        trend
      }
    }
    alerts {
      type
      message
      severity
      count
      actionUrl
    }
    recentActivity {
      activityType
      description
      timestamp
      userId
      userName
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "mainDashboard": {
      "summary": {
        "totalCompanies": 45,
        "activeCertificates": 128,
        "upcomingAudits": 12,
        "pendingActions": 23,
        "overdueItems": 5,
        "monthlyRevenue": 145000.00,
        "complianceScore": 94.5,
        "userActivity": 89
      },
      "charts": {
        "auditTrend": [
          {
            "period": "2024-08",
            "planned": 15,
            "completed": 12,
            "cancelled": 1
          },
          {
            "period": "2024-09",
            "planned": 18,
            "completed": 16,
            "cancelled": 0
          }
        ],
        "certificateStatus": [
          {
            "status": "ACTIVE",
            "count": 98,
            "percentage": 76.6
          },
          {
            "status": "EXPIRING_SOON",
            "count": 20,
            "percentage": 15.6
          },
          {
            "status": "EXPIRED",
            "count": 10,
            "percentage": 7.8
          }
        ],
        "revenueByService": [
          {
            "serviceName": "ISO 9001",
            "amount": 65000.00,
            "growth": 12.5
          },
          {
            "serviceName": "ISO 14001",
            "amount": 45000.00,
            "growth": 8.3
          }
        ],
        "complianceByCompany": [
          {
            "companyName": "Acme Corp",
            "score": 96.2,
            "trend": "UP"
          }
        ]
      },
      "alerts": [
        {
          "type": "CERTIFICATE_EXPIRY",
          "message": "5 certificates expiring within 30 days",
          "severity": "HIGH",
          "count": 5,
          "actionUrl": "/certificates/expiring"
        }
      ],
      "recentActivity": [
        {
          "activityType": "AUDIT_COMPLETED",
          "description": "ISO 9001 audit completed for Acme Corp",
          "timestamp": "2024-09-20T14:30:00Z",
          "userId": 12,
          "userName": "Jane Auditor"
        }
      ]
    }
  }
}
```

### Query: Get Executive Dashboard

**Operation:** `executiveDashboard`

**GraphQL Query:**
```graphql
query GetExecutiveDashboard($timeframe: String!) {
  executiveDashboard(timeframe: $timeframe) {
    financialMetrics {
      totalRevenue
      profitMargin
      revenueGrowth
      averageContractValue
      customerRetentionRate
    }
    operationalMetrics {
      totalAudits
      auditSuccessRate
      averageAuditDuration
      customerSatisfaction
      teamUtilization
    }
    strategicMetrics {
      marketShare
      newCustomers
      serviceExpansion
      competitorAnalysis
    }
    trends {
      revenueChart
      auditVolumeChart
      customerGrowthChart
      profitabilityChart
    }
    forecasts {
      revenueProjection
      growthProjection
      capacityProjection
    }
  }
}
```

### Query: Get Operational Dashboard

**Operation:** `operationalDashboard`

**GraphQL Query:**
```graphql
query GetOperationalDashboard($departmentId: Int, $timeframe: String!) {
  operationalDashboard(departmentId: $departmentId, timeframe: $timeframe) {
    workload {
      totalTasks
      completedTasks
      overdueTasks
      averageCompletionTime
      teamCapacity
    }
    performance {
      qualityScore
      onTimeDelivery
      customerFeedback
      errorRate
      reworkRate
    }
    resources {
      teamUtilization
      skillsMatrix
      trainingNeeds
      equipmentStatus
    }
    schedule {
      upcomingDeadlines
      resourceConflicts
      availableCapacity
    }
  }
}
```

---

## 📊 KPI Operations

### Query: Get All KPIs

**Operation:** `kpis`

**GraphQL Query:**
```graphql
query GetAllKPIs($category: String, $period: String!) {
  kpis(category: $category, period: $period) {
    id
    kpiName
    category
    description
    currentValue
    targetValue
    unit
    trend
    changePercent
    status
    lastUpdated
    historicalData {
      period
      value
      target
    }
    benchmarks {
      industry
      bestPractice
      internal
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "kpis": [
      {
        "id": 1,
        "kpiName": "Customer Satisfaction Score",
        "category": "CUSTOMER",
        "description": "Average customer satisfaction rating",
        "currentValue": 4.2,
        "targetValue": 4.5,
        "unit": "RATING",
        "trend": "UP",
        "changePercent": 5.2,
        "status": "IMPROVING",
        "lastUpdated": "2024-09-20T10:00:00Z",
        "historicalData": [
          {
            "period": "2024-08",
            "value": 4.0,
            "target": 4.5
          },
          {
            "period": "2024-09",
            "value": 4.2,
            "target": 4.5
          }
        ],
        "benchmarks": {
          "industry": 4.1,
          "bestPractice": 4.8,
          "internal": 4.2
        }
      }
    ]
  }
}
```

### Query: Get KPI Performance

**Operation:** `kpiPerformance`

**GraphQL Query:**
```graphql
query GetKPIPerformance($kpiId: Int!, $startDate: DateTime!, $endDate: DateTime!) {
  kpiPerformance(kpiId: $kpiId, startDate: $startDate, endDate: $endDate) {
    kpiId
    kpiName
    overallPerformance
    achievementRate
    trendAnalysis
    periodBreakdown {
      period
      value
      target
      achievement
      variance
    }
    correlations {
      relatedKpi
      correlationScore
      relationship
    }
    insights {
      insight
      confidence
      actionable
    }
  }
}
```

### Mutation: Update KPI Target

**Operation:** `updateKPITarget`

**GraphQL Mutation:**
```graphql
mutation UpdateKPITarget($kpiId: Int!, $newTarget: Float!, $effectiveDate: DateTime!, $reason: String) {
  updateKPITarget(kpiId: $kpiId, newTarget: $newTarget, effectiveDate: $effectiveDate, reason: $reason) {
    kpiId
    newTarget
    previousTarget
    effectiveDate
    updatedBy
    reason
  }
}
```

---

## 📈 Analytics Operations

### Query: Get Business Analytics

**Operation:** `businessAnalytics`

**GraphQL Query:**
```graphql
query GetBusinessAnalytics($analysisType: String!, $parameters: AnalyticsParametersInput!) {
  businessAnalytics(analysisType: $analysisType, parameters: $parameters) {
    analysisType
    executedDate
    summary {
      totalRecords
      timeframe
      dataQuality
    }
    metrics {
      metricName
      value
      previousValue
      change
      significance
    }
    visualizations {
      chartType
      title
      data
      insights
    }
    recommendations {
      priority
      recommendation
      impact
      effort
      timeline
    }
  }
}
```

### Query: Get Trend Analysis

**Operation:** `trendAnalysis`

**GraphQL Query:**
```graphql
query GetTrendAnalysis($metric: String!, $timeframe: String!, $granularity: String!) {
  trendAnalysis(metric: $metric, timeframe: $timeframe, granularity: $granularity) {
    metric
    timeframe
    trendDirection
    seasonality
    volatility
    dataPoints {
      timestamp
      value
      prediction
      confidence
    }
    patterns {
      patternType
      strength
      description
    }
    forecasts {
      period
      predictedValue
      lowerBound
      upperBound
      confidence
    }
  }
}
```

### Query: Get Comparative Analysis

**Operation:** `comparativeAnalysis`

**GraphQL Query:**
```graphql
query GetComparativeAnalysis($baselineEntity: String!, $comparisonEntities: [String!]!, $metrics: [String!]!) {
  comparativeAnalysis(baselineEntity: $baselineEntity, comparisonEntities: $comparisonEntities, metrics: $metrics) {
    baselineEntity
    comparisonResults {
      entity
      metrics {
        metricName
        baselineValue
        comparisonValue
        variance
        percentageChange
        significance
      }
      overallPerformance
      ranking
    }
    insights {
      bestPerformer
      worstPerformer
      keyDifferences
      recommendations
    }
  }
}
```

---

## 📋 Reporting Operations

### Query: Get Available Reports

**Operation:** `availableReports`

**GraphQL Query:**
```graphql
query GetAvailableReports($category: String) {
  availableReports(category: $category) {
    id
    reportName
    category
    description
    parameters
    outputFormats
    scheduleOptions
    accessLevel
    lastRun
    averageExecutionTime
    popularity
  }
}
```

### Query: Generate Report

**Operation:** `generateReport`

**GraphQL Query:**
```graphql
query GenerateReport($reportId: Int!, $parameters: ReportParametersInput!, $format: String!) {
  generateReport(reportId: $reportId, parameters: $parameters, format: $format) {
    reportId
    executionId
    status
    generatedDate
    downloadUrl
    expiryDate
    parameters
    recordCount
    fileSize
  }
}
```

### Query: Get Scheduled Reports

**Operation:** `scheduledReports`

**GraphQL Query:**
```graphql
query GetScheduledReports($userId: Int) {
  scheduledReports(userId: $userId) {
    id
    reportName
    schedule
    nextRun
    lastRun
    isActive
    recipients
    deliveryMethod
    parameters
    status
  }
}
```

### Mutation: Schedule Report

**Operation:** `scheduleReport`

**GraphQL Mutation:**
```graphql
mutation ScheduleReport($input: ScheduleReportInput!) {
  scheduleReport(input: $input) {
    scheduleId
    reportId
    schedule
    nextRun
    isActive
    createdDate
  }
}
```

---

## 🎯 Performance Metrics

### Query: Get Performance Metrics

**Operation:** `performanceMetrics`

**GraphQL Query:**
```graphql
query GetPerformanceMetrics($entityType: String!, $entityId: Int, $period: String!) {
  performanceMetrics(entityType: $entityType, entityId: $entityId, period: $period) {
    entityType
    entityId
    period
    overallScore
    efficiency
    quality
    compliance
    customerSatisfaction
    financialPerformance
    detailedMetrics {
      category
      metrics {
        name
        value
        target
        trend
        benchmark
      }
    }
    rankings {
      criterion
      rank
      total
      percentile
    }
  }
}
```

### Query: Get Team Performance

**Operation:** `teamPerformance`

**GraphQL Query:**
```graphql
query GetTeamPerformance($teamId: Int!, $period: String!) {
  teamPerformance(teamId: $teamId, period: $period) {
    teamId
    teamName
    period
    productivity
    quality
    collaboration
    skills
    memberPerformance {
      userId
      userName
      individualScore
      contributions
      strengths
      improvementAreas
    }
    teamMetrics {
      tasksCompleted
      averageDeliveryTime
      errorRate
      customerFeedback
      utilization
    }
  }
}
```

---

## 🔍 Business Intelligence

### Query: Get Business Insights

**Operation:** `businessInsights`

**GraphQL Query:**
```graphql
query GetBusinessInsights($timeframe: String!, $focusArea: String) {
  businessInsights(timeframe: $timeframe, focusArea: $focusArea) {
    executionDate
    focusArea
    insights {
      insightType
      title
      description
      confidence
      impact
      urgency
      dataPoints
      recommendations {
        action
        priority
        estimatedImpact
        resources
        timeline
      }
    }
    patterns {
      patternName
      strength
      frequency
      business_impact
      examples
    }
    anomalies {
      anomalyType
      severity
      description
      possibleCauses
      recommendedActions
    }
  }
}
```

### Query: Get Predictive Analytics

**Operation:** `predictiveAnalytics`

**GraphQL Query:**
```graphql
query GetPredictiveAnalytics($predictionType: String!, $horizon: String!, $confidence: Float!) {
  predictiveAnalytics(predictionType: $predictionType, horizon: $horizon, confidence: $confidence) {
    predictionType
    horizon
    confidence
    modelAccuracy
    predictions {
      period
      predictedValue
      lowerBound
      upperBound
      factors {
        factor
        influence
        direction
      }
    }
    scenarios {
      scenarioName
      assumptions
      outcomes
      probability
    }
    recommendations {
      scenario
      actions
      expectedOutcome
      riskLevel
    }
  }
}
```

---

## 📊 Data Visualization

### Query: Get Chart Data

**Operation:** `chartData`

**GraphQL Query:**
```graphql
query GetChartData($chartType: String!, $dataSource: String!, $filters: ChartFiltersInput!) {
  chartData(chartType: $chartType, dataSource: $dataSource, filters: $filters) {
    chartType
    title
    subtitle
    dataSource
    lastUpdated
    series {
      name
      data {
        x
        y
        label
        color
        metadata
      }
    }
    axes {
      xAxis {
        label
        type
        format
      }
      yAxis {
        label
        type
        format
        min
        max
      }
    }
    configuration {
      colors
      styling
      interactivity
    }
  }
}
```

### Query: Get Dashboard Widgets

**Operation:** `dashboardWidgets`

**GraphQL Query:**
```graphql
query GetDashboardWidgets($dashboardId: Int!, $userId: Int!) {
  dashboardWidgets(dashboardId: $dashboardId, userId: $userId) {
    widgetId
    widgetType
    title
    position {
      x
      y
      width
      height
    }
    configuration
    data
    refreshInterval
    lastUpdated
    permissions {
      canEdit
      canMove
      canDelete
    }
  }
}
```

### Mutation: Update Widget Configuration

**Operation:** `updateWidgetConfiguration`

**GraphQL Mutation:**
```graphql
mutation UpdateWidgetConfiguration($widgetId: Int!, $configuration: WidgetConfigurationInput!) {
  updateWidgetConfiguration(widgetId: $widgetId, configuration: $configuration) {
    widgetId
    configuration
    lastUpdated
  }
}
```

---

*📝 Note: This service is currently in development. The actual implementation may vary from this specification.*