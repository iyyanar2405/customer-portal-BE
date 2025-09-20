# Audits Module - JSON Payload Examples

## Create Audit Request
```json
{
  "auditName": "Annual ISO 9001 Audit",
  "auditNumber": "AUD-2025-001",
  "companyId": 1,
  "siteId": 1,
  "auditTypeId": 1,
  "status": "Planned",
  "plannedStartDate": "2025-10-01T09:00:00Z",
  "plannedEndDate": "2025-10-03T17:00:00Z",
  "leadAuditorId": 5,
  "scope": "Quality Management System certification audit covering all processes",
  "objectives": "Verify compliance with ISO 9001:2015 standards",
  "summary": "Initial certification audit for new customer site",
  "recommendations": "",
  "createdBy": 1
}
```

## Create Audit Response
```json
{
  "data": {
    "id": 1,
    "auditName": "Annual ISO 9001 Audit",
    "auditNumber": "AUD-2025-001",
    "companyId": 1,
    "companyName": "Acme Corporation",
    "siteId": 1,
    "siteName": "Main Manufacturing Plant",
    "auditTypeId": 1,
    "auditTypeName": "Certification Audit",
    "status": "Planned",
    "plannedStartDate": "2025-10-01T09:00:00Z",
    "plannedEndDate": "2025-10-03T17:00:00Z",
    "actualStartDate": null,
    "actualEndDate": null,
    "leadAuditorId": 5,
    "leadAuditorName": "John Smith",
    "scope": "Quality Management System certification audit covering all processes",
    "objectives": "Verify compliance with ISO 9001:2015 standards",
    "summary": "Initial certification audit for new customer site",
    "recommendations": "",
    "isActive": true,
    "createdDate": "2025-09-19T10:00:00Z",
    "createdBy": 1,
    "createdByName": "Admin User",
    "modifiedDate": "2025-09-19T10:00:00Z",
    "modifiedBy": 1,
    "modifiedByName": "Admin User",
    "teamMembers": [],
    "auditServices": [],
    "auditSites": []
  },
  "isSuccess": true,
  "message": "Audit created successfully",
  "errorCode": null
}
```

## Update Audit Request
```json
{
  "id": 1,
  "status": "In Progress",
  "actualStartDate": "2025-10-01T09:30:00Z",
  "summary": "Audit started successfully. All team members present.",
  "modifiedBy": 5
}
```

## Audit List Response
```json
{
  "data": {
    "audits": [
      {
        "id": 1,
        "auditName": "Annual ISO 9001 Audit",
        "auditNumber": "AUD-2025-001",
        "companyName": "Acme Corporation",
        "siteName": "Main Manufacturing Plant",
        "status": "Planned",
        "plannedStartDate": "2025-10-01T09:00:00Z",
        "plannedEndDate": "2025-10-03T17:00:00Z",
        "leadAuditorName": "John Smith",
        "auditTypeName": "Certification Audit"
      }
    ],
    "totalCount": 1,
    "pageNumber": 1,
    "pageSize": 10,
    "hasNextPage": false,
    "hasPreviousPage": false
  },
  "isSuccess": true,
  "message": "Data retrieved successfully",
  "errorCode": null
}
```

## Audit Filter Request
```json
{
  "companyIds": [1, 2],
  "siteIds": [1, 3, 5],
  "statuses": ["Planned", "In Progress"],
  "startDateFrom": "2025-09-01T00:00:00Z",
  "startDateTo": "2025-12-31T23:59:59Z",
  "auditTypeIds": [1, 2],
  "leadAuditorIds": [5, 8],
  "searchTerm": "ISO",
  "pageNumber": 1,
  "pageSize": 10,
  "sortBy": "plannedStartDate",
  "sortDirection": "asc"
}
```

## Assign Team Member Request
```json
{
  "auditId": 1,
  "userId": 8,
  "role": "Technical Expert",
  "responsibilities": "Review technical documentation and processes",
  "isLead": false,
  "createdBy": 5
}
```

## Assign Team Member Response
```json
{
  "data": {
    "id": 1,
    "auditId": 1,
    "userId": 8,
    "userName": "Jane Doe",
    "role": "Technical Expert",
    "responsibilities": "Review technical documentation and processes",
    "isLead": false,
    "createdDate": "2025-09-19T10:15:00Z",
    "createdBy": 5
  },
  "isSuccess": true,
  "message": "Team member assigned successfully",
  "errorCode": null
}
```

## Complete Audit Request
```json
{
  "id": 1,
  "actualEndDate": "2025-10-03T16:30:00Z",
  "summary": "Audit completed successfully. All major non-conformities addressed.",
  "recommendations": "Continue current quality practices. Schedule surveillance audit in 6 months.",
  "completedBy": 5
}
```

## Error Response Example
```json
{
  "data": null,
  "isSuccess": false,
  "message": "Audit not found",
  "errorCode": "AUDIT_NOT_FOUND"
}
```