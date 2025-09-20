# Customer Portal API Documentation Index

## 🏗️ Architecture Overview

The Customer Portal has been architected as a microservices-based system with 12 specialized services, each responsible for specific business domains. All services expose GraphQL endpoints for consistent API access.

---

## 📚 Service Documentation

### ✅ **Implemented Services**

| Service | Port | Documentation | Status |
|---------|------|---------------|---------|
| [**Master Service**](./MasterService.md) | 5003 | Core master data management | ✅ **Running** |

### 🔄 **Services In Development**

| Service | Port | Documentation | Responsibility |
|---------|------|---------------|----------------|
| [**Users Service**](./UsersService.md) | 5004 | User management, authentication, authorization |
| [**Audits Service**](./AuditsService.md) | 5002 | Audit planning, execution, and tracking |
| [**Actions Service**](./ActionsService.md) | 5001 | Task and action management |
| [**Certificates Service**](./CertificatesService.md) | 5005 | Certificate management and tracking |
| [**Contracts Service**](./ContractsService.md) | 6006 | Contract management |
| [**Financial Service**](./FinancialService.md) | 5007 | Invoicing and financial operations |
| [**Findings Service**](./FindingsService.md) | 5008 | Audit findings management |
| [**Notifications Service**](./NotificationsService.md) | 5009 | Notification and messaging system |
| [**Settings Service**](./SettingsService.md) | 5010 | Application configuration and settings |
| [**Overview Service**](./OverviewService.md) | 5011 | Dashboard and reporting |
| [**Widgets Service**](./WidgetsService.md) | 5012 | UI widgets and components |

### 🔄 **Planned Services**

| Service | Port | Responsibility |
|---------|------|----------------|
| **API Gateway** | 5000 | Unified entry point and request routing |

---

## 🚀 Quick Start Guide

### 1. **Master Service (Currently Running)**

The Master Service is fully operational and can be tested immediately:

```bash
# Service URL
http://localhost:5003

# GraphQL Playground
http://localhost:5003/graphql
```

**Sample Query:**
```graphql
query {
  countries {
    id
    countryName
    countryCode
    cities {
      cityName
    }
  }
}
```

### 2. **Testing Infrastructure**

Use the provided testing tools:

- **PowerShell Script:** `test-graphql.ps1`
- **Batch Script:** `start-services.bat` 
- **HTML Dashboard:** `graphql-test.html`

---

## 📊 Service Architecture Patterns

All services follow consistent architectural patterns:

### **GraphQL Schema Structure**
- **Queries:** Read operations (Get, Search, Filter)
- **Mutations:** Write operations (Create, Update, Delete)
- **Subscriptions:** Real-time updates (planned)

### **Entity Relationships**
- **Base Entity:** Common fields (Id, CreatedDate, ModifiedDate, IsActive)
- **Repository Pattern:** Standardized data access
- **Service Layer:** Business logic abstraction

### **Error Handling**
- **Consistent Error Codes:** Standardized across all services
- **Structured Responses:** Predictable error format
- **Validation:** Input validation with detailed messages

---

## 🔧 Development Standards

### **GraphQL Conventions**
- **Queries:** Use descriptive names (e.g., `usersByRole`, `auditsByStatus`)
- **Mutations:** Use action verbs (e.g., `createUser`, `updateAudit`)
- **Types:** Use PascalCase for types, camelCase for fields
- **Input Types:** Suffix with `Input` (e.g., `CreateUserInput`)

### **Response Patterns**
- **Pagination:** Consistent offset/limit pattern
- **Filtering:** Standardized filter parameters
- **Sorting:** Common sort field patterns
- **Metadata:** Include execution time, record counts

### **Security Standards**
- **Authentication:** JWT token-based
- **Authorization:** Role-based access control
- **Data Protection:** Field-level security
- **Audit Logging:** Track all mutations

---

## 📈 API Usage Patterns

### **Common Query Patterns**

#### 1. **List with Pagination**
```graphql
query GetEntities($offset: Int, $limit: Int) {
  entities(offset: $offset, limit: $limit) {
    id
    name
    # ... other fields
  }
}
```

#### 2. **Search with Filters**
```graphql
query SearchEntities($filters: EntityFiltersInput!) {
  searchEntities(filters: $filters) {
    totalCount
    items {
      id
      name
    }
  }
}
```

#### 3. **Entity with Relations**
```graphql
query GetEntityDetails($id: Int!) {
  entityById(id: $id) {
    id
    name
    relatedEntities {
      id
      name
    }
  }
}
```

### **Common Mutation Patterns**

#### 1. **Create Entity**
```graphql
mutation CreateEntity($input: CreateEntityInput!) {
  createEntity(input: $input) {
    id
    name
    createdDate
  }
}
```

#### 2. **Update Entity**
```graphql
mutation UpdateEntity($id: Int!, $input: UpdateEntityInput!) {
  updateEntity(id: $id, input: $input) {
    id
    name
    modifiedDate
  }
}
```

#### 3. **Delete Entity**
```graphql
mutation DeleteEntity($id: Int!) {
  deleteEntity(id: $id)
}
```

---

## 🔍 Testing Guidelines

### **GraphQL Playground Testing**
1. Open service GraphQL endpoint in browser
2. Use schema explorer to browse available operations
3. Write queries in left panel
4. Execute and view results in right panel

### **PowerShell Testing**
```powershell
$headers = @{'Content-Type' = 'application/json'}
$query = '{"query": "{ entities { id name } }"}'
Invoke-RestMethod -Uri 'http://localhost:PORT/graphql' -Method POST -Headers $headers -Body $query
```

### **cURL Testing**
```bash
curl -X POST http://localhost:PORT/graphql \
  -H "Content-Type: application/json" \
  -d '{"query": "{ entities { id name } }"}'
```

---

## 📋 Implementation Roadmap

### **Phase 1: Core Services** ✅
- [x] Master Service (Countries, Cities, Companies, Sites, Services, Roles)
- [x] Shared Library (Base entities, repositories, services)
- [x] Testing Infrastructure

### **Phase 2: User Management** 🔄
- [ ] Users Service (Authentication, authorization, profiles)
- [ ] Settings Service (User preferences, system configuration)
- [ ] Notifications Service (Communication, alerts)

### **Phase 3: Audit Management** 🔄
- [ ] Audits Service (Planning, execution, tracking)
- [ ] Findings Service (Non-conformities, corrective actions)
- [ ] Actions Service (Task management, workflows)

### **Phase 4: Compliance & Finance** 🔄
- [ ] Certificates Service (Certificate management, renewals)
- [ ] Contracts Service (Contract management, renewals)
- [ ] Financial Service (Invoicing, payments, reporting)

### **Phase 5: Analytics & UI** 🔄
- [ ] Overview Service (Dashboards, KPIs, reporting)
- [ ] Widgets Service (UI components, customization)
- [ ] API Gateway (Unified entry point, routing)

---

## 🛠️ Development Environment

### **Prerequisites**
- .NET 9.0 SDK
- SQL Server / SQL Server Express
- Visual Studio 2022 or VS Code
- GraphQL client (GraphQL Playground, Insomnia, Postman)

### **Project Structure**
```
customer-portal-BE/
├── CustomerPortal.Shared/          # Common library
├── Services/
│   ├── CustomerPortal.MasterService/    # ✅ Implemented
│   ├── CustomerPortal.UsersService/     # 🔄 In Progress  
│   ├── CustomerPortal.AuditsService/    # 🔄 In Progress
│   └── ... (other services)
├── Documentation/                  # 📚 API Documentation
└── Tests/                         # 🧪 Unit & Integration Tests
```

### **Getting Started**
1. Clone repository
2. Restore NuGet packages: `dotnet restore`
3. Build solution: `dotnet build`
4. Run Master Service: `dotnet run --project Services/CustomerPortal.MasterService`
5. Open GraphQL Playground: `http://localhost:5003/graphql`

---

## 📞 Support & Resources

### **Documentation**
- [Master Service Details](./MasterService.md) - Complete API reference
- [Testing Guide](../graphql-test.html) - Interactive testing dashboard
- [PowerShell Scripts](../test-graphql.ps1) - Automated testing

### **Development Tools**
- **GraphQL Playground:** Interactive query editor
- **Postman Collections:** Pre-built API requests
- **PowerShell Scripts:** Automated testing and deployment

### **Contact Information**
- **Development Team:** [Team Contact]
- **Technical Lead:** [Lead Contact]
- **Documentation:** [Doc Maintainer]

---

## 📝 Contributing

### **Development Workflow**
1. Create feature branch from `main`
2. Implement service following established patterns
3. Add comprehensive tests
4. Update documentation
5. Submit pull request for review

### **Documentation Standards**
- Update service documentation for any API changes
- Include payload examples for all operations
- Document error scenarios and responses
- Maintain consistent formatting and structure

### **Testing Requirements**
- Unit tests for all business logic
- Integration tests for GraphQL endpoints
- Performance tests for data operations
- Documentation tests for examples

---

*Last Updated: September 20, 2024*  
*Documentation Version: 1.0*  
*API Version: 1.0.0*