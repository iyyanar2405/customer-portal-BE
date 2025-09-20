# Customer Portal API - Complete GraphQL Schema Definition

## GraphQL Schema Types and Structure

### 📋 Root Types

```graphql
type Query {
  # Actions
  actions(filter: ActionFilterInput, sort: ActionSortInput, skip: Int = 0, take: Int = 50): [ActionType!]!
  actionById(id: Int!): ActionType
  actionsByUser(userId: Int!): [ActionType!]!
  actionsByCompany(companyId: Int!): [ActionType!]!
  overdueActions: [ActionType!]!
  actionStatistics: ActionStatisticsType!

  # Audits
  audits(filter: AuditFilterInput, search: AuditSearchInput): AuditListResponse!
  auditById(auditId: Int!): AuditType
  auditsByCompany(companyId: Int!): [AuditType!]!
  auditsByLeadAuditor(auditorId: Int!): [AuditType!]!
  auditTypes: [AuditTypeInfo!]!

  # Certificates
  certificates(filter: CertificateFilterInput, search: CertificateSearchInput): CertificateListResponse!
  certificateById(id: Int!): CertificateType
  certificatesByCompany(companyId: Int!): [CertificateType!]!
  certificatesByService(serviceId: Int!): [CertificateType!]!
  expiringCertificates(daysAhead: Int = 90): [CertificateType!]!

  # Contracts
  contracts(filter: ContractFilterInput, search: ContractSearchInput): ContractListResponse!
  contractById(id: Int!): ContractType
  contractsByCompany(companyId: Int!): [ContractType!]!
  activeContracts: [ContractType!]!
  expiringContracts(daysAhead: Int = 90): [ContractType!]!

  # Financial
  invoices(filter: InvoiceFilterInput, search: InvoiceSearchInput): InvoiceListResponse!
  invoiceById(id: Int!): InvoiceType
  invoicesByCompany(companyId: Int!): [InvoiceType!]!
  invoicesByContract(contractId: Int!): [InvoiceType!]!
  overdueInvoices: [InvoiceType!]!
  financialSummary(companyId: Int, dateFrom: DateTime, dateTo: DateTime): FinancialSummaryType!

  # Users
  users(filter: UserFilterInput, search: UserSearchInput): UserListResponse!
  userById(id: Int!): UserType
  usersByCompany(companyId: Int!): [UserType!]!
  usersByRole(roleId: Int!): [UserType!]!
  roles: [RoleType!]!

  # Master Data
  companies(filter: CompanyFilterInput, search: CompanySearchInput): CompanyListResponse!
  companyById(id: Int!): CompanyType
  sites(companyId: Int): [SiteType!]!
  siteById(id: Int!): SiteType
  services: [ServiceType!]!
  serviceById(id: Int!): ServiceType
  countries: [CountryType!]!
  cities(countryId: Int): [CityType!]!
}

type Mutation {
  # Actions
  createAction(input: ActionInput!): CreateActionPayload!
  updateAction(input: UpdateActionInput!): UpdateActionPayload!
  completeAction(input: CompleteActionInput!): CompleteActionPayload!
  assignAction(input: AssignActionInput!): AssignActionPayload!
  deleteAction(id: Int!): DeleteActionPayload!

  # Audits
  createAudit(input: AuditInput!): CreateAuditPayload!
  updateAudit(input: UpdateAuditInput!): UpdateAuditPayload!
  scheduleAudit(input: AuditScheduleInput!): ScheduleAuditPayload!
  completeAudit(auditId: Int!, comments: String): CompleteAuditPayload!
  assignTeamMember(input: AuditTeamMemberInput!): AssignTeamMemberPayload!
  assignSite(input: AuditSiteInput!): AssignSitePayload!

  # Certificates
  createCertificate(input: CertificateInput!): CreateCertificatePayload!
  updateCertificate(input: UpdateCertificateInput!): UpdateCertificatePayload!
  renewCertificate(input: RenewCertificateInput!): RenewCertificatePayload!
  suspendCertificate(id: Int!, reason: String!): SuspendCertificatePayload!
  addAdditionalScope(input: CertificateAdditionalScopeInput!): AddAdditionalScopePayload!

  # Contracts
  createContract(input: ContractInput!): CreateContractPayload!
  updateContract(input: UpdateContractInput!): UpdateContractPayload!
  renewContract(input: RenewContractInput!): RenewContractPayload!
  terminateContract(id: Int!, reason: String!): TerminateContractPayload!
  addContractService(input: ContractServiceInput!): AddContractServicePayload!

  # Financial
  createInvoice(input: InvoiceInput!): CreateInvoicePayload!
  updateInvoice(input: UpdateInvoiceInput!): UpdateInvoicePayload!
  sendInvoice(id: Int!): SendInvoicePayload!
  recordPayment(input: PaymentInput!): RecordPaymentPayload!
  writeOffInvoice(id: Int!, reason: String!): WriteOffInvoicePayload!

  # Users
  createUser(input: UserInput!): CreateUserPayload!
  updateUser(input: UpdateUserInput!): UpdateUserPayload!
  deactivateUser(id: Int!): DeactivateUserPayload!
  assignRole(input: UserRoleInput!): AssignRolePayload!
  updateUserAccess(input: UserAccessInput!): UpdateUserAccessPayload!

  # Master Data
  createCompany(input: CompanyInput!): CreateCompanyPayload!
  updateCompany(input: UpdateCompanyInput!): UpdateCompanyPayload!
  createSite(input: SiteInput!): CreateSitePayload!
  updateSite(input: UpdateSiteInput!): UpdateSitePayload!
  createService(input: ServiceInput!): CreateServicePayload!
}
```

### 🎯 Core Entity Types

```graphql
# Actions Module
type ActionType {
  id: Int!
  actionName: String!
  description: String
  actionTypeValue: String
  priority: String
  status: String
  assignedToUserId: Int
  assignedToUserName: String
  companyId: Int
  companyName: String
  siteId: Int
  siteName: String
  auditId: Int
  auditName: String
  findingId: Int
  findingDescription: String
  dueDate: DateTime
  completedDate: DateTime
  createdDate: DateTime!
  createdBy: Int
  createdByName: String
  modifiedDate: DateTime
  modifiedBy: Int
  modifiedByName: String
  comments: String
  isActive: Boolean!
  isOverdue: Boolean!
  daysSinceDue: Int
  attachments: [AttachmentType!]
  history: [ActionHistoryType!]
}

type ActionStatisticsType {
  totalActions: Int!
  completedActions: Int!
  pendingActions: Int!
  overdueActions: Int!
  highPriorityActions: Int!
  mediumPriorityActions: Int!
  lowPriorityActions: Int!
  actionsByStatus: [ActionStatusStatType!]!
  actionsByPriority: [ActionPriorityStatType!]!
  averageCompletionDays: Float!
  completionRate: Float!
}

# Audits Module
type AuditType {
  auditId: Int!
  sites: String!
  services: String!
  companyId: Int!
  companyName: String
  status: String!
  startDate: DateTime!
  endDate: DateTime
  leadAuditor: String
  type: String
  auditNumber: String
  description: String
  auditTypeId: Int
  auditTypeName: String
  isActive: Boolean!
  createdDate: DateTime!
  modifiedDate: DateTime
  createdBy: Int
  modifiedBy: Int
  createdByName: String
  modifiedByName: String
  teamMembers: [AuditTeamMemberType!]
  auditSites: [AuditSiteType!]
  auditServices: [AuditServiceType!]
  findings: [FindingType!]
  documents: [DocumentType!]
  progress: AuditProgressType
}

type AuditTeamMemberType {
  id: Int!
  auditId: Int!
  userId: Int!
  userName: String
  userEmail: String
  role: String
  responsibilities: String
  isLead: Boolean!
  qualifications: [String!]
  createdDate: DateTime!
  createdBy: Int
  createdByName: String
}

# Certificates Module
type CertificateType {
  id: Int!
  certificateNumber: String!
  certificateName: String!
  companyId: Int!
  companyName: String
  siteId: Int!
  siteName: String
  siteAddress: String
  serviceId: Int!
  serviceName: String
  serviceDescription: String
  issueDate: DateTime!
  expiryDate: DateTime!
  status: String!
  issuingBody: String!
  scope: String!
  standardReference: String!
  auditId: Int
  auditName: String
  certificateFile: String
  notes: String
  isActive: Boolean!
  daysUntilExpiry: Int!
  isExpiringSoon: Boolean!
  isExpired: Boolean!
  createdDate: DateTime!
  modifiedDate: DateTime
  createdBy: Int
  modifiedBy: Int
  createdByName: String
  modifiedByName: String
  additionalScopes: [CertificateAdditionalScopeType!]
  certificateServices: [CertificateServiceType!]
  certificateSites: [CertificateSiteType!]
  renewalHistory: [CertificateRenewalType!]
}

# Contracts Module
type ContractType {
  id: Int!
  contractNumber: String!
  contractName: String!
  companyId: Int!
  companyName: String
  contractType: String!
  status: String!
  startDate: DateTime!
  endDate: DateTime!
  value: Decimal!
  currency: String!
  description: String
  contactPerson: String
  contactEmail: String
  contactPhone: String
  terms: String
  notes: String
  isActive: Boolean!
  daysUntilExpiry: Int!
  isExpiringSoon: Boolean!
  isExpired: Boolean!
  totalInvoiced: Decimal!
  totalPaid: Decimal!
  outstandingAmount: Decimal!
  createdDate: DateTime!
  modifiedDate: DateTime
  createdBy: Int
  modifiedBy: Int
  createdByName: String
  modifiedByName: String
  contractServices: [ContractServiceType!]
  contractSites: [ContractSiteType!]
  invoices: [InvoiceType!]
  amendments: [ContractAmendmentType!]
}

# Financial Module
type InvoiceType {
  id: Int!
  invoiceNumber: String!
  companyId: Int!
  companyName: String
  contractId: Int
  contractNumber: String
  issueDate: DateTime!
  dueDate: DateTime!
  paidDate: DateTime
  amount: Decimal!
  currency: String!
  status: String!
  paymentStatus: String!
  paymentMethod: String
  description: String
  taxRate: Decimal!
  taxAmount: Decimal!
  totalAmount: Decimal!
  discountAmount: Decimal!
  notes: String
  isActive: Boolean!
  daysOverdue: Int!
  isOverdue: Boolean!
  createdDate: DateTime!
  modifiedDate: DateTime
  createdBy: Int
  modifiedBy: Int
  createdByName: String
  modifiedByName: String
  invoiceItems: [InvoiceItemType!]!
  payments: [PaymentType!]
  reminders: [InvoiceReminderType!]
}

type FinancialSummaryType {
  totalRevenue: Decimal!
  totalInvoiced: Decimal!
  totalPaid: Decimal!
  totalOutstanding: Decimal!
  totalOverdue: Decimal!
  averagePaymentDays: Float!
  invoiceCount: Int!
  paidInvoiceCount: Int!
  overdueInvoiceCount: Int!
  topPayingCompanies: [TopPayingCompanyType!]!
  monthlyRevenue: [MonthlyRevenueType!]!
  paymentMethodStats: [PaymentMethodStatType!]!
  agingReport: [AgingReportType!]!
}

# Users Module
type UserType {
  id: Int!
  username: String!
  email: String!
  firstName: String
  lastName: String
  fullName: String
  phone: String
  jobTitle: String
  department: String
  companyId: Int
  companyName: String
  isActive: Boolean!
  lastLoginDate: DateTime
  loginCount: Int!
  failedLoginAttempts: Int!
  isLocked: Boolean!
  mustChangePassword: Boolean!
  createdDate: DateTime!
  modifiedDate: DateTime
  createdBy: Int
  modifiedBy: Int
  createdByName: String
  modifiedByName: String
  userRoles: [UserRoleType!]
  userCompanyAccess: [UserCompanyAccessType!]
  userSiteAccess: [UserSiteAccessType!]
  userServiceAccess: [UserServiceAccessType!]
  preferences: UserPreferencesType
  activity: [UserActivityType!]
}

# Master Data Module
type CompanyType {
  id: Int!
  companyName: String!
  companyCode: String
  industry: String
  address: String
  city: String
  state: String
  country: String
  postalCode: String
  phone: String
  email: String
  website: String
  contactPerson: String
  contactTitle: String
  contactEmail: String
  contactPhone: String
  taxId: String
  registrationNumber: String
  isActive: Boolean!
  createdDate: DateTime!
  modifiedDate: DateTime
  createdBy: Int
  modifiedBy: Int
  createdByName: String
  modifiedByName: String
  sites: [SiteType!]
  services: [ServiceType!]
  contracts: [ContractType!]
  users: [UserType!]
  audits: [AuditType!]
  certificates: [CertificateType!]
  invoices: [InvoiceType!]
  statistics: CompanyStatisticsType
}
```

### 🔧 Input Types

```graphql
# Action Inputs
input ActionInput {
  actionName: String!
  description: String
  actionType: String
  priority: String
  status: String
  assignedToUserId: Int
  companyId: Int
  siteId: Int
  auditId: Int
  findingId: Int
  dueDate: DateTime
  comments: String
}

input UpdateActionInput {
  id: Int!
  actionName: String
  description: String
  actionType: String
  priority: String
  status: String
  assignedToUserId: Int
  companyId: Int
  siteId: Int
  auditId: Int
  findingId: Int
  dueDate: DateTime
  comments: String
}

# Audit Inputs
input AuditInput {
  sites: String!
  services: String!
  companyId: Int!
  status: String!
  startDate: DateTime!
  endDate: DateTime
  leadAuditor: String
  type: String
  auditNumber: String
  description: String
  auditTypeId: Int
  createdBy: Int
  modifiedBy: Int
}

input UpdateAuditInput {
  auditId: Int!
  sites: String
  services: String
  companyId: Int
  status: String
  startDate: DateTime
  endDate: DateTime
  leadAuditor: String
  type: String
  auditNumber: String
  description: String
  auditTypeId: Int
  modifiedBy: Int
}

# Certificate Inputs
input CertificateInput {
  certificateNumber: String!
  certificateName: String!
  companyId: Int!
  siteId: Int!
  serviceId: Int!
  issueDate: DateTime!
  expiryDate: DateTime!
  status: String!
  issuingBody: String!
  scope: String!
  standardReference: String!
  auditId: Int
  certificateFile: String
  notes: String
  createdBy: Int
}

# Contract Inputs
input ContractInput {
  contractNumber: String!
  contractName: String!
  companyId: Int!
  contractType: String!
  status: String!
  startDate: DateTime!
  endDate: DateTime!
  value: Decimal!
  currency: String!
  description: String
  contactPerson: String
  contactEmail: String
  contactPhone: String
  terms: String
  notes: String
  createdBy: Int
}

# Invoice Inputs
input InvoiceInput {
  invoiceNumber: String!
  companyId: Int!
  contractId: Int
  issueDate: DateTime!
  dueDate: DateTime!
  amount: Decimal!
  currency: String!
  status: String!
  description: String
  taxRate: Decimal!
  taxAmount: Decimal!
  totalAmount: Decimal!
  discountAmount: Decimal
  notes: String
  items: [InvoiceItemInput!]!
  createdBy: Int
}

input InvoiceItemInput {
  description: String!
  quantity: Int!
  unitPrice: Decimal!
  amount: Decimal!
  serviceId: Int
}

# User Inputs
input UserInput {
  username: String!
  email: String!
  firstName: String
  lastName: String
  phone: String
  jobTitle: String
  department: String
  companyId: Int
  isActive: Boolean!
  roleIds: [Int!]
  companyAccess: [UserCompanyAccessInput!]
  siteAccess: [UserSiteAccessInput!]
  serviceAccess: [UserServiceAccessInput!]
  createdBy: Int
}

# Company Inputs
input CompanyInput {
  companyName: String!
  companyCode: String
  industry: String
  address: String
  city: String
  state: String
  country: String
  postalCode: String
  phone: String
  email: String
  website: String
  contactPerson: String
  contactTitle: String
  contactEmail: String
  contactPhone: String
  taxId: String
  registrationNumber: String
  isActive: Boolean!
  createdBy: Int
}
```

### 📊 Response/Payload Types

```graphql
# Success/Error Payload Pattern
type CreateActionPayload {
  action: ActionType
  errorMessage: String
}

type UpdateActionPayload {
  action: ActionType
  errorMessage: String
}

type DeleteActionPayload {
  success: Boolean!
  errorMessage: String
}

# List Response Pattern
type ActionListResponse {
  actions: [ActionType!]!
  totalCount: Int!
  pageNumber: Int!
  pageSize: Int!
  hasNextPage: Boolean!
  hasPreviousPage: Boolean!
}

type AuditListResponse {
  audits: [AuditType!]!
  totalCount: Int!
  pageNumber: Int!
  pageSize: Int!
  hasNextPage: Boolean!
  hasPreviousPage: Boolean!
}

# Similar patterns for all other modules...
```

### 🔍 Filter and Search Types

```graphql
# Search Input Pattern
input ActionSearchInput {
  searchTerm: String
  pageNumber: Int = 1
  pageSize: Int = 50
  sortBy: String
  sortDirection: SortDirection = Asc
  filters: ActionFilterInput
}

# Sort Enums
enum SortDirection {
  Asc
  Desc
}

enum ActionSortField {
  ActionName
  Priority
  Status
  DueDate
  CreatedDate
  ModifiedDate
}

# Filter Inputs (comprehensive filtering options)
input ActionFilterInput {
  actionName: String
  status: String
  priority: String
  assignedToUserId: Int
  companyId: Int
  siteId: Int
  dueDateFrom: DateTime
  dueDateTo: DateTime
  isOverdue: Boolean
  isCompleted: Boolean
}

# Similar patterns for all modules with their specific filter needs
```

### 🎯 Scalar Types

```graphql
scalar DateTime
scalar Decimal
scalar Upload  # For file uploads
scalar JSON    # For dynamic data
```

---

## 🛠️ GraphQL Operation Examples Summary

| Module | Queries | Mutations | Key Features |
|--------|---------|-----------|--------------|
| **Actions** | 6 queries | 5 mutations | Priority management, overdue tracking, statistics |
| **Audits** | 5 queries | 6 mutations | Team management, scheduling, completion tracking |
| **Certificates** | 5 queries | 5 mutations | Expiry tracking, renewal management, scope additions |
| **Contracts** | 5 queries | 5 mutations | Value tracking, renewal management, service associations |
| **Financial** | 6 queries | 5 mutations | Payment tracking, aging reports, financial summaries |
| **Users** | 5 queries | 5 mutations | Role management, access control, activity tracking |
| **Master Data** | 8 queries | 6 mutations | Company/site/service management, hierarchical data |

---

## 🔒 Authentication & Authorization

### Headers Required
```json
{
  "Authorization": "Bearer <JWT_TOKEN>",
  "Content-Type": "application/json",
  "X-Company-Context": "1"  // Optional company context
}
```

### Permission Levels
- **Read**: View data within assigned scope
- **Write**: Create/update data within assigned scope  
- **Delete**: Remove data within assigned scope
- **Admin**: Full access to all operations

---

*This complete GraphQL schema definition provides the foundation for all API operations in the Customer Portal, with comprehensive type safety, filtering, and business logic integration.*