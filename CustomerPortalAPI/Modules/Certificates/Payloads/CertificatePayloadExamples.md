# Certificates Module - JSON Payload Examples

## Create Certificate Request
```json
{
  "certificateNumber": "CERT-ISO9001-2025-001",
  "companyId": 1,
  "siteId": 1,
  "standard": "ISO 9001:2015",
  "scope": "Design, manufacture and supply of industrial equipment",
  "status": "Active",
  "issueDate": "2025-01-15T00:00:00Z",
  "expiryDate": "2028-01-15T00:00:00Z",
  "validFrom": "2025-01-15T00:00:00Z",
  "validUntil": "2028-01-15T00:00:00Z",
  "certificationBody": "DNV Business Assurance",
  "auditId": 1,
  "certificationLevel": "Full",
  "accreditationBody": "UKAS",
  "accreditationNumber": "ACC-001-2025",
  "logo": "https://certificates.dnv.com/logos/iso9001.png",
  "certificateUrl": "https://certificates.dnv.com/CERT-ISO9001-2025-001.pdf",
  "isActive": true,
  "createdBy": 1
}
```

## Create Certificate Response
```json
{
  "data": {
    "id": 1,
    "certificateNumber": "CERT-ISO9001-2025-001",
    "companyId": 1,
    "companyName": "Acme Corporation",
    "siteId": 1,
    "siteName": "Main Manufacturing Plant",
    "standard": "ISO 9001:2015",
    "scope": "Design, manufacture and supply of industrial equipment",
    "status": "Active",
    "issueDate": "2025-01-15T00:00:00Z",
    "expiryDate": "2028-01-15T00:00:00Z",
    "validFrom": "2025-01-15T00:00:00Z",
    "validUntil": "2028-01-15T00:00:00Z",
    "certificationBody": "DNV Business Assurance",
    "auditId": 1,
    "auditNumber": "AUD-2025-001",
    "certificationLevel": "Full",
    "accreditationBody": "UKAS",
    "accreditationNumber": "ACC-001-2025",
    "logo": "https://certificates.dnv.com/logos/iso9001.png",
    "certificateUrl": "https://certificates.dnv.com/CERT-ISO9001-2025-001.pdf",
    "isActive": true,
    "createdDate": "2025-01-15T10:00:00Z",
    "createdBy": 1,
    "createdByName": "System Admin",
    "modifiedDate": "2025-01-15T10:00:00Z",
    "modifiedBy": 1,
    "modifiedByName": "System Admin",
    "services": [
      {
        "id": 1,
        "serviceName": "ISO 9001 Certification",
        "serviceCode": "ISO9001",
        "isActive": true
      }
    ],
    "additionalScopes": [
      {
        "id": 1,
        "scopeDescription": "Software development and maintenance",
        "effectiveDate": "2025-06-01T00:00:00Z",
        "isActive": true
      }
    ]
  },
  "isSuccess": true,
  "message": "Certificate created successfully",
  "errorCode": null
}
```

## Update Certificate Request
```json
{
  "id": 1,
  "status": "Suspended",
  "suspensionReason": "Non-compliance identified during surveillance audit",
  "suspensionDate": "2025-09-19T00:00:00Z",
  "modifiedBy": 5
}
```

## Certificate List Response
```json
{
  "data": {
    "certificates": [
      {
        "id": 1,
        "certificateNumber": "CERT-ISO9001-2025-001",
        "companyName": "Acme Corporation",
        "siteName": "Main Manufacturing Plant",
        "standard": "ISO 9001:2015",
        "status": "Active",
        "issueDate": "2025-01-15T00:00:00Z",
        "expiryDate": "2028-01-15T00:00:00Z",
        "certificationBody": "DNV Business Assurance",
        "daysUntilExpiry": 486,
        "expiryStatus": "Valid"
      },
      {
        "id": 2,
        "certificateNumber": "CERT-ISO14001-2025-002",
        "companyName": "TechFlow Industries Ltd",
        "siteName": "Technology Park",
        "standard": "ISO 14001:2015",
        "status": "Active",
        "issueDate": "2025-03-01T00:00:00Z",
        "expiryDate": "2028-03-01T00:00:00Z",
        "certificationBody": "DNV Business Assurance",
        "daysUntilExpiry": 528,
        "expiryStatus": "Valid"
      }
    ],
    "totalCount": 2,
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

## Certificate Filter Request
```json
{
  "companyIds": [1, 2],
  "siteIds": [1, 3, 5],
  "standards": ["ISO 9001:2015", "ISO 14001:2015"],
  "statuses": ["Active", "Suspended"],
  "issueDateFrom": "2025-01-01T00:00:00Z",
  "issueDateTo": "2025-12-31T23:59:59Z",
  "expiryDateFrom": "2026-01-01T00:00:00Z",
  "expiryDateTo": "2030-12-31T23:59:59Z",
  "certificationBodies": ["DNV Business Assurance"],
  "expiryStatus": ["Valid", "Expiring Soon"],
  "searchTerm": "ISO",
  "pageNumber": 1,
  "pageSize": 10,
  "sortBy": "expiryDate",
  "sortDirection": "asc"
}
```

## Renew Certificate Request
```json
{
  "certificateId": 1,
  "newExpiryDate": "2031-01-15T00:00:00Z",
  "renewalAuditId": 5,
  "renewalNotes": "Certificate renewed following successful surveillance audit",
  "renewedBy": 1
}
```

## Suspend Certificate Request
```json
{
  "certificateId": 1,
  "suspensionReason": "Major non-conformity identified during surveillance audit",
  "suspensionDate": "2025-09-19T00:00:00Z",
  "expectedResolutionDate": "2025-10-19T00:00:00Z",
  "suspendedBy": 5
}
```

## Restore Certificate Request
```json
{
  "certificateId": 1,
  "restorationDate": "2025-10-15T00:00:00Z",
  "restorationNotes": "Non-conformity resolved. Certificate restored to active status.",
  "restoredBy": 5
}
```

## Add Certificate Service Request
```json
{
  "certificateId": 1,
  "serviceId": 3,
  "effectiveDate": "2025-09-19T00:00:00Z",
  "notes": "Additional service scope added",
  "addedBy": 1
}
```

## Add Additional Scope Request
```json
{
  "certificateId": 1,
  "scopeDescription": "Software development and maintenance",
  "effectiveDate": "2025-09-19T00:00:00Z",
  "auditId": 6,
  "notes": "Scope extension approved following successful audit",
  "addedBy": 1
}
```

## Certificate Expiry Alert Response
```json
{
  "data": {
    "expiringCertificates": [
      {
        "id": 5,
        "certificateNumber": "CERT-ISO45001-2024-005",
        "companyName": "Safety First Corp",
        "standard": "ISO 45001:2018",
        "expiryDate": "2025-10-15T00:00:00Z",
        "daysUntilExpiry": 26,
        "expiryStatus": "Expiring Soon",
        "contactEmail": "quality@safetyfirst.com",
        "lastNotificationSent": "2025-09-01T00:00:00Z"
      }
    ],
    "alertLevels": {
      "critical": 2,
      "warning": 5,
      "info": 3
    }
  },
  "isSuccess": true,
  "message": "Expiry alerts retrieved successfully",
  "errorCode": null
}
```

## Error Response Examples
```json
{
  "data": null,
  "isSuccess": false,
  "message": "Certificate not found",
  "errorCode": "CERTIFICATE_NOT_FOUND"
}
```

```json
{
  "data": null,
  "isSuccess": false,
  "message": "Certificate number already exists",
  "errorCode": "DUPLICATE_CERTIFICATE_NUMBER"
}
```

```json
{
  "data": null,
  "isSuccess": false,
  "message": "Cannot suspend certificate: No active non-conformities found",
  "errorCode": "INVALID_SUSPENSION_REQUEST"
}
```