# Users Service Documentation

**Base URL:** `http://localhost:5004`  
**GraphQL Endpoint:** `http://localhost:5004/graphql`  
**Status:** 🔄 In Progress

## 📊 Entities Overview

The Users Service manages user authentication, authorization, and profile information:

1. **Users** - User account information
2. **UserRoles** - User role assignments
3. **UserCompanyAccess** - Company access permissions
4. **UserSiteAccess** - Site access permissions
5. **UserServiceAccess** - Service access permissions
6. **UserPreferences** - User settings and preferences
7. **UserTrainings** - User training records

---

## 👤 Users Operations

### Query: Get All Users

**Operation:** `users`

**GraphQL Query:**
```graphql
query GetAllUsers {
  users {
    id
    firstName
    lastName
    email
    username
    phoneNumber
    isActive
    lastLoginDate
    createdDate
    modifiedDate
    userRoles {
      roleId
      role {
        roleName
        description
      }
    }
    userCompanyAccess {
      companyId
      company {
        companyName
      }
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "users": [
      {
        "id": 1,
        "firstName": "John",
        "lastName": "Doe",
        "email": "john.doe@example.com",
        "username": "johndoe",
        "phoneNumber": "+1-555-0123",
        "isActive": true,
        "lastLoginDate": "2024-09-20T10:30:00Z",
        "createdDate": "2024-01-01T00:00:00Z",
        "modifiedDate": null,
        "userRoles": [
          {
            "roleId": 1,
            "role": {
              "roleName": "Administrator",
              "description": "Full system access"
            }
          }
        ],
        "userCompanyAccess": [
          {
            "companyId": 1,
            "company": {
              "companyName": "Acme Corporation"
            }
          }
        ]
      }
    ]
  }
}
```

### Mutation: Create User

**Operation:** `createUser`

**GraphQL Mutation:**
```graphql
mutation CreateUser($input: CreateUserInput!) {
  createUser(input: $input) {
    id
    firstName
    lastName
    email
    username
    isActive
    createdDate
  }
}
```

**Input Payload:**
```json
{
  "input": {
    "firstName": "Jane",
    "lastName": "Smith",
    "email": "jane.smith@example.com",
    "username": "janesmith",
    "phoneNumber": "+1-555-0124",
    "password": "SecurePassword123!",
    "roleIds": [2],
    "companyIds": [1],
    "siteIds": [1, 2]
  }
}
```

### Mutation: Update User

**Operation:** `updateUser`

**GraphQL Mutation:**
```graphql
mutation UpdateUser($id: Int!, $input: UpdateUserInput!) {
  updateUser(id: $id, input: $input) {
    id
    firstName
    lastName
    email
    modifiedDate
  }
}
```

### Mutation: Delete User

**Operation:** `deleteUser`

**GraphQL Mutation:**
```graphql
mutation DeleteUser($id: Int!) {
  deleteUser(id: $id)
}
```

---

## 🔑 Authentication Operations

### Mutation: Login

**Operation:** `login`

**GraphQL Mutation:**
```graphql
mutation Login($input: LoginInput!) {
  login(input: $input) {
    token
    refreshToken
    expiresAt
    user {
      id
      firstName
      lastName
      email
      roles {
        roleName
      }
    }
  }
}
```

**Input Payload:**
```json
{
  "input": {
    "email": "john.doe@example.com",
    "password": "SecurePassword123!"
  }
}
```

### Mutation: Refresh Token

**Operation:** `refreshToken`

**GraphQL Mutation:**
```graphql
mutation RefreshToken($refreshToken: String!) {
  refreshToken(refreshToken: $refreshToken) {
    token
    refreshToken
    expiresAt
  }
}
```

---

## 🏢 User Access Management

### Query: Get User Company Access

**Operation:** `userCompanyAccess`

**GraphQL Query:**
```graphql
query GetUserCompanyAccess($userId: Int!) {
  userCompanyAccess(userId: $userId) {
    userId
    companyId
    accessLevel
    grantedDate
    company {
      companyName
      companyCode
    }
  }
}
```

### Mutation: Grant Company Access

**Operation:** `grantCompanyAccess`

**GraphQL Mutation:**
```graphql
mutation GrantCompanyAccess($input: GrantCompanyAccessInput!) {
  grantCompanyAccess(input: $input) {
    userId
    companyId
    accessLevel
    grantedDate
  }
}
```

---

*📝 Note: This service is currently in development. The actual implementation may vary from this specification.*