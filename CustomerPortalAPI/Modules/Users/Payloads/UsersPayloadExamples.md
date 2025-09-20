# Users Module - JSON Payload Examples

## User Registration Request
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@acmecorp.com",
  "phone": "+1-555-123-4567",
  "password": "SecurePassword123!",
  "confirmPassword": "SecurePassword123!",
  "companyId": 1,
  "roleIds": [3, 5],
  "isActive": true,
  "createdBy": 1
}
```

## User Registration Response
```json
{
  "data": {
    "id": 15,
    "firstName": "John",
    "lastName": "Doe",
    "fullName": "John Doe",
    "email": "john.doe@acmecorp.com",
    "phone": "+1-555-123-4567",
    "companyId": 1,
    "companyName": "Acme Corporation",
    "isActive": true,
    "emailVerified": false,
    "lastLoginDate": null,
    "createdDate": "2025-09-19T10:00:00Z",
    "createdBy": 1,
    "createdByName": "System Admin",
    "roles": [
      {
        "id": 3,
        "roleName": "Company Admin",
        "permissions": "company.read,user.create,user.read,user.update"
      },
      {
        "id": 5,
        "roleName": "Site Manager",
        "permissions": "site.read,audit.read,certificate.read"
      }
    ],
    "temporaryPassword": true,
    "mustChangePassword": true
  },
  "isSuccess": true,
  "message": "User registered successfully. Verification email sent.",
  "errorCode": null
}
```

## User Login Request
```json
{
  "email": "john.doe@acmecorp.com",
  "password": "SecurePassword123!",
  "rememberMe": true,
  "clientInfo": {
    "userAgent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
    "ipAddress": "192.168.1.100",
    "deviceType": "Desktop"
  }
}
```

## User Login Response
```json
{
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "def50200a8f3b2c1d4e5f6a7b8c9d0e1f2a3b4c5...",
    "tokenType": "Bearer",
    "expiresIn": 3600,
    "refreshExpiresIn": 2592000,
    "user": {
      "id": 15,
      "firstName": "John",
      "lastName": "Doe",
      "email": "john.doe@acmecorp.com",
      "companyId": 1,
      "companyName": "Acme Corporation",
      "roles": [
        {
          "id": 3,
          "roleName": "Company Admin",
          "permissions": ["company.read", "user.create", "user.read", "user.update"]
        }
      ],
      "preferences": {
        "language": "en-US",
        "timeZone": "America/New_York",
        "dateFormat": "MM/dd/yyyy",
        "notifications": {
          "email": true,
          "browser": true,
          "mobile": false
        }
      },
      "mustChangePassword": false,
      "lastLoginDate": "2025-09-19T10:30:00Z"
    }
  },
  "isSuccess": true,
  "message": "Login successful",
  "errorCode": null
}
```

## Create User Profile Request
```json
{
  "firstName": "Jane",
  "lastName": "Smith",
  "email": "jane.smith@acmecorp.com",
  "phone": "+1-555-123-4568",
  "jobTitle": "Quality Manager",
  "department": "Quality Assurance",
  "employeeId": "EMP-001",
  "companyId": 1,
  "managerId": 10,
  "roleIds": [4, 6],
  "siteAccess": [1, 2],
  "serviceAccess": [1, 3, 5],
  "startDate": "2025-01-15T00:00:00Z",
  "isActive": true,
  "createdBy": 3
}
```

## User Profile Response
```json
{
  "data": {
    "id": 16,
    "firstName": "Jane",
    "lastName": "Smith",
    "fullName": "Jane Smith",
    "email": "jane.smith@acmecorp.com",
    "phone": "+1-555-123-4568",
    "jobTitle": "Quality Manager",
    "department": "Quality Assurance",
    "employeeId": "EMP-001",
    "companyId": 1,
    "companyName": "Acme Corporation",
    "managerId": 10,
    "managerName": "Michael Johnson",
    "isActive": true,
    "emailVerified": false,
    "startDate": "2025-01-15T00:00:00Z",
    "createdDate": "2025-09-19T10:00:00Z",
    "createdBy": 3,
    "createdByName": "John Doe",
    "roles": [
      {
        "id": 4,
        "roleName": "Auditor",
        "permissions": "audit.read,finding.create,finding.read"
      },
      {
        "id": 6,
        "roleName": "Site User",
        "permissions": "site.read,certificate.read"
      }
    ],
    "siteAccess": [
      {
        "siteId": 1,
        "siteName": "Main Manufacturing Plant",
        "accessLevel": "Full"
      },
      {
        "siteId": 2,
        "siteName": "West Coast Facility",
        "accessLevel": "Read Only"
      }
    ],
    "serviceAccess": [
      {
        "serviceId": 1,
        "serviceName": "ISO 9001 Certification"
      },
      {
        "serviceId": 3,
        "serviceName": "ISO 14001 Certification"
      }
    ]
  },
  "isSuccess": true,
  "message": "User profile created successfully",
  "errorCode": null
}
```

## Update User Password Request
```json
{
  "userId": 15,
  "currentPassword": "OldPassword123!",
  "newPassword": "NewSecurePassword456!",
  "confirmNewPassword": "NewSecurePassword456!"
}
```

## Update User Preferences Request
```json
{
  "userId": 15,
  "preferences": {
    "language": "en-US",
    "timeZone": "America/New_York",
    "dateFormat": "MM/dd/yyyy",
    "timeFormat": "12h",
    "currency": "USD",
    "notifications": {
      "email": true,
      "browser": true,
      "mobile": false,
      "auditReminders": true,
      "certificateExpiry": true,
      "findingUpdates": true
    },
    "dashboard": {
      "defaultView": "overview",
      "widgets": [
        "pending-audits",
        "certificate-status",
        "recent-findings",
        "upcoming-renewals"
      ]
    }
  }
}
```

## User Search Request
```json
{
  "searchTerm": "John",
  "companyIds": [1, 2],
  "roleIds": [3, 4, 5],
  "departments": ["Quality Assurance", "Operations"],
  "isActive": true,
  "emailVerified": true,
  "lastLoginFrom": "2025-01-01T00:00:00Z",
  "lastLoginTo": "2025-09-19T23:59:59Z",
  "pageNumber": 1,
  "pageSize": 10,
  "sortBy": "lastName",
  "sortDirection": "asc"
}
```

## Users List Response
```json
{
  "data": {
    "users": [
      {
        "id": 15,
        "firstName": "John",
        "lastName": "Doe",
        "email": "john.doe@acmecorp.com",
        "phone": "+1-555-123-4567",
        "jobTitle": "Operations Manager",
        "department": "Operations",
        "companyName": "Acme Corporation",
        "isActive": true,
        "emailVerified": true,
        "lastLoginDate": "2025-09-19T08:30:00Z",
        "roles": ["Company Admin", "Site Manager"],
        "sitesCount": 2,
        "auditsCount": 5
      },
      {
        "id": 16,
        "firstName": "Jane",
        "lastName": "Smith",
        "email": "jane.smith@acmecorp.com",
        "phone": "+1-555-123-4568",
        "jobTitle": "Quality Manager",
        "department": "Quality Assurance",
        "companyName": "Acme Corporation",
        "isActive": true,
        "emailVerified": false,
        "lastLoginDate": null,
        "roles": ["Auditor", "Site User"],
        "sitesCount": 2,
        "auditsCount": 0
      }
    ],
    "totalCount": 2,
    "pageNumber": 1,
    "pageSize": 10,
    "hasNextPage": false,
    "hasPreviousPage": false
  },
  "isSuccess": true,
  "message": "Users retrieved successfully",
  "errorCode": null
}
```

## Assign Role to User Request
```json
{
  "userId": 16,
  "roleId": 7,
  "effectiveDate": "2025-09-20T00:00:00Z",
  "expiryDate": "2026-09-20T00:00:00Z",
  "assignedBy": 3,
  "notes": "Promoted to Lead Auditor role"
}
```

## User Role Assignment Response
```json
{
  "data": {
    "userRoleId": 45,
    "userId": 16,
    "userName": "Jane Smith",
    "roleId": 7,
    "roleName": "Lead Auditor",
    "effectiveDate": "2025-09-20T00:00:00Z",
    "expiryDate": "2026-09-20T00:00:00Z",
    "isActive": true,
    "assignedDate": "2025-09-19T10:00:00Z",
    "assignedBy": 3,
    "assignedByName": "John Doe",
    "notes": "Promoted to Lead Auditor role",
    "permissions": [
      "audit.create",
      "audit.read",
      "audit.update",
      "finding.create",
      "finding.read",
      "finding.update",
      "certificate.read"
    ]
  },
  "isSuccess": true,
  "message": "Role assigned successfully",
  "errorCode": null
}
```

## User Training Assignment Request
```json
{
  "userId": 16,
  "trainingId": 3,
  "assignedDate": "2025-09-19T10:00:00Z",
  "dueDate": "2025-10-19T23:59:59Z",
  "isRequired": true,
  "assignedBy": 3,
  "notes": "Required for Lead Auditor certification"
}
```

## User Activity Log Response
```json
{
  "data": {
    "activities": [
      {
        "id": 1001,
        "userId": 15,
        "activityType": "Login",
        "description": "User logged in successfully",
        "timestamp": "2025-09-19T08:30:00Z",
        "ipAddress": "192.168.1.100",
        "userAgent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64)",
        "sessionId": "sess_abc123"
      },
      {
        "id": 1002,
        "userId": 15,
        "activityType": "Audit Created",
        "description": "Created audit AUD-2025-001 for Acme Corporation",
        "timestamp": "2025-09-19T09:15:00Z",
        "relatedEntityType": "Audit",
        "relatedEntityId": 25,
        "ipAddress": "192.168.1.100"
      },
      {
        "id": 1003,
        "userId": 15,
        "activityType": "Password Changed",
        "description": "User changed password",
        "timestamp": "2025-09-19T10:45:00Z",
        "ipAddress": "192.168.1.100"
      }
    ],
    "totalCount": 3,
    "pageNumber": 1,
    "pageSize": 10
  },
  "isSuccess": true,
  "message": "Activity log retrieved successfully",
  "errorCode": null
}
```

## User Permission Check Request
```json
{
  "userId": 15,
  "permissions": [
    "audit.create",
    "user.update",
    "certificate.read",
    "company.delete"
  ]
}
```

## User Permission Check Response
```json
{
  "data": {
    "userId": 15,
    "permissionResults": [
      {
        "permission": "audit.create",
        "hasPermission": true,
        "source": "Role: Company Admin"
      },
      {
        "permission": "user.update",
        "hasPermission": true,
        "source": "Role: Company Admin"
      },
      {
        "permission": "certificate.read",
        "hasPermission": true,
        "source": "Role: Site Manager"
      },
      {
        "permission": "company.delete",
        "hasPermission": false,
        "source": "No matching role or permission"
      }
    ]
  },
  "isSuccess": true,
  "message": "Permission check completed",
  "errorCode": null
}
```

## Reset Password Request
```json
{
  "email": "john.doe@acmecorp.com",
  "resetUrl": "https://portal.acmecorp.com/reset-password"
}
```

## Reset Password Response
```json
{
  "data": {
    "resetToken": "rst_abc123def456",
    "expiresAt": "2025-09-19T11:00:00Z",
    "emailSent": true
  },
  "isSuccess": true,
  "message": "Password reset instructions sent to email",
  "errorCode": null
}
```

## Bulk User Import Request
```json
{
  "users": [
    {
      "firstName": "Alice",
      "lastName": "Johnson",
      "email": "alice.johnson@acmecorp.com",
      "jobTitle": "Quality Specialist",
      "department": "Quality Assurance",
      "employeeId": "EMP-002",
      "roleIds": [4]
    },
    {
      "firstName": "Bob",
      "lastName": "Wilson",
      "email": "bob.wilson@acmecorp.com",
      "jobTitle": "Operations Supervisor",
      "department": "Operations",
      "employeeId": "EMP-003",
      "roleIds": [5, 6]
    }
  ],
  "companyId": 1,
  "sendWelcomeEmail": true,
  "requirePasswordReset": true,
  "importedBy": 3
}
```

## Deactivate User Request
```json
{
  "userId": 16,
  "reason": "Employee terminated",
  "effectiveDate": "2025-09-20T00:00:00Z",
  "transferResponsibilities": true,
  "transferToUserId": 15,
  "deactivatedBy": 3
}
```

## Error Response Examples
```json
{
  "data": null,
  "isSuccess": false,
  "message": "Email address already exists",
  "errorCode": "EMAIL_ALREADY_EXISTS"
}
```

```json
{
  "data": null,
  "isSuccess": false,
  "message": "Invalid login credentials",
  "errorCode": "INVALID_CREDENTIALS"
}
```

```json
{
  "data": null,
  "isSuccess": false,
  "message": "Password does not meet complexity requirements",
  "errorCode": "WEAK_PASSWORD"
}
```

```json
{
  "data": null,
  "isSuccess": false,
  "message": "User account is locked",
  "errorCode": "ACCOUNT_LOCKED"
}
```