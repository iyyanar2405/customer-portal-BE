# Contracts Module - JSON Payload Examples

## Create Contract Request
```json
{
  "contractNumber": "CONTRACT-2025-001",
  "contractName": "ISO Certification Services Agreement",
  "companyId": 1,
  "contractType": "Certification",
  "status": "Active",
  "startDate": "2025-01-01T00:00:00Z",
  "endDate": "2027-12-31T00:00:00Z",
  "signedDate": "2024-12-15T00:00:00Z",
  "totalValue": 75000.00,
  "currency": "USD",
  "paymentTerms": "Net 30 days",
  "description": "Comprehensive ISO certification services including initial audits and ongoing surveillance",
  "terms": "Standard terms and conditions apply. Subject to DNV certification procedures.",
  "accountManagerId": 5,
  "salesRepId": 8,
  "isFrameworkAgreement": false,
  "parentContractId": null,
  "renewalDate": "2027-10-01T00:00:00Z",
  "noticePeriod": 90,
  "autoRenewal": true,
  "isActive": true,
  "createdBy": 1
}
```

## Create Contract Response
```json
{
  "data": {
    "id": 1,
    "contractNumber": "CONTRACT-2025-001",
    "contractName": "ISO Certification Services Agreement",
    "companyId": 1,
    "companyName": "Acme Corporation",
    "contractType": "Certification",
    "status": "Active",
    "startDate": "2025-01-01T00:00:00Z",
    "endDate": "2027-12-31T00:00:00Z",
    "signedDate": "2024-12-15T00:00:00Z",
    "totalValue": 75000.00,
    "currency": "USD",
    "paymentTerms": "Net 30 days",
    "description": "Comprehensive ISO certification services including initial audits and ongoing surveillance",
    "terms": "Standard terms and conditions apply. Subject to DNV certification procedures.",
    "accountManagerId": 5,
    "accountManagerName": "John Smith",
    "salesRepId": 8,
    "salesRepName": "Jane Doe",
    "isFrameworkAgreement": false,
    "parentContractId": null,
    "parentContractNumber": null,
    "renewalDate": "2027-10-01T00:00:00Z",
    "noticePeriod": 90,
    "autoRenewal": true,
    "contractValue": {
      "totalValue": 75000.00,
      "invoicedAmount": 25000.00,
      "remainingValue": 50000.00,
      "utilizationPercentage": 33.33
    },
    "isActive": true,
    "createdDate": "2024-12-15T10:00:00Z",
    "createdBy": 1,
    "createdByName": "System Admin",
    "modifiedDate": "2024-12-15T10:00:00Z",
    "modifiedBy": 1,
    "modifiedByName": "System Admin",
    "services": [
      {
        "id": 1,
        "serviceName": "ISO 9001 Certification",
        "quantity": 1,
        "unitPrice": 15000.00,
        "totalPrice": 15000.00
      },
      {
        "id": 2,
        "serviceName": "ISO 14001 Certification",
        "quantity": 1,
        "unitPrice": 12000.00,
        "totalPrice": 12000.00
      }
    ],
    "sites": [
      {
        "id": 1,
        "siteName": "Main Manufacturing Plant",
        "address": "123 Industrial Blvd",
        "isActive": true
      }
    ]
  },
  "isSuccess": true,
  "message": "Contract created successfully",
  "errorCode": null
}
```

## Update Contract Request
```json
{
  "id": 1,
  "status": "Amended",
  "totalValue": 85000.00,
  "endDate": "2028-12-31T00:00:00Z",
  "amendmentReason": "Additional scope added - ISO 45001 certification",
  "amendmentDate": "2025-09-19T00:00:00Z",
  "modifiedBy": 5
}
```

## Contract List Response
```json
{
  "data": {
    "contracts": [
      {
        "id": 1,
        "contractNumber": "CONTRACT-2025-001",
        "contractName": "ISO Certification Services Agreement",
        "companyName": "Acme Corporation",
        "contractType": "Certification",
        "status": "Active",
        "startDate": "2025-01-01T00:00:00Z",
        "endDate": "2027-12-31T00:00:00Z",
        "totalValue": 75000.00,
        "currency": "USD",
        "utilizationPercentage": 33.33,
        "accountManagerName": "John Smith",
        "daysUntilExpiry": 829
      },
      {
        "id": 2,
        "contractNumber": "CONTRACT-2025-002",
        "contractName": "Environmental Management Services",
        "companyName": "Green Energy Solutions GmbH",
        "contractType": "Consulting",
        "status": "Active",
        "startDate": "2025-02-01T00:00:00Z",
        "endDate": "2026-01-31T00:00:00Z",
        "totalValue": 45000.00,
        "currency": "EUR",
        "utilizationPercentage": 67.22,
        "accountManagerName": "Peter White",
        "daysUntilExpiry": 499
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

## Contract Filter Request
```json
{
  "companyIds": [1, 2],
  "contractTypes": ["Certification", "Consulting"],
  "statuses": ["Active", "Pending"],
  "startDateFrom": "2025-01-01T00:00:00Z",
  "startDateTo": "2025-12-31T23:59:59Z",
  "endDateFrom": "2026-01-01T00:00:00Z",
  "endDateTo": "2030-12-31T23:59:59Z",
  "accountManagerIds": [5, 8],
  "valueFrom": 10000.00,
  "valueTo": 100000.00,
  "currencies": ["USD", "EUR"],
  "expiringWithinDays": 90,
  "searchTerm": "ISO",
  "pageNumber": 1,
  "pageSize": 10,
  "sortBy": "endDate",
  "sortDirection": "asc"
}
```

## Add Contract Service Request
```json
{
  "contractId": 1,
  "serviceId": 3,
  "serviceName": "ISO 45001 Certification",
  "quantity": 1,
  "unitPrice": 18000.00,
  "totalPrice": 18000.00,
  "effectiveDate": "2025-09-19T00:00:00Z",
  "description": "Additional scope - Occupational Health and Safety Management",
  "addedBy": 5
}
```

## Add Contract Site Request
```json
{
  "contractId": 1,
  "siteId": 3,
  "effectiveDate": "2025-09-19T00:00:00Z",
  "notes": "Additional site added to contract scope",
  "addedBy": 5
}
```

## Contract Amendment Request
```json
{
  "contractId": 1,
  "amendmentType": "Scope Extension",
  "amendmentDescription": "Added ISO 45001 certification service and additional site",
  "valueChange": 10000.00,
  "newTotalValue": 85000.00,
  "endDateChange": "2028-12-31T00:00:00Z",
  "effectiveDate": "2025-09-19T00:00:00Z",
  "reason": "Customer requested additional scope",
  "approvedBy": 10,
  "amendedBy": 5
}
```

## Contract Renewal Request
```json
{
  "contractId": 1,
  "newStartDate": "2028-01-01T00:00:00Z",
  "newEndDate": "2030-12-31T00:00:00Z",
  "newTotalValue": 90000.00,
  "renewalTerms": "Updated terms with 2024 rates and additional services",
  "renewalNotes": "Automatic renewal triggered 90 days before expiry",
  "renewedBy": 5
}
```

## Contract Termination Request
```json
{
  "contractId": 1,
  "terminationDate": "2025-12-31T00:00:00Z",
  "terminationReason": "Customer requested early termination",
  "noticePeriod": 90,
  "penaltyAmount": 5000.00,
  "finalInvoiceAmount": 15000.00,
  "terminatedBy": 5
}
```

## Contract Financial Summary Response
```json
{
  "data": {
    "contractId": 1,
    "financialSummary": {
      "totalValue": 75000.00,
      "invoicedAmount": 25000.00,
      "pendingInvoices": 8000.00,
      "remainingValue": 42000.00,
      "utilizationPercentage": 44.00,
      "paymentsReceived": 17000.00,
      "outstandingAmount": 8000.00,
      "overdueDays": 15
    },
    "invoiceHistory": [
      {
        "invoiceNumber": "INV-2025-001",
        "amount": 15000.00,
        "issueDate": "2025-01-15T00:00:00Z",
        "dueDate": "2025-02-14T00:00:00Z",
        "status": "Paid",
        "paidDate": "2025-02-10T00:00:00Z"
      },
      {
        "invoiceNumber": "INV-2025-045",
        "amount": 10000.00,
        "issueDate": "2025-07-15T00:00:00Z",
        "dueDate": "2025-08-14T00:00:00Z",
        "status": "Paid",
        "paidDate": "2025-08-12T00:00:00Z"
      }
    ]
  },
  "isSuccess": true,
  "message": "Financial summary retrieved successfully",
  "errorCode": null
}
```

## Contract Expiry Alert Response
```json
{
  "data": {
    "expiringContracts": [
      {
        "id": 5,
        "contractNumber": "CONTRACT-2022-015",
        "contractName": "Maritime Safety Certification",
        "companyName": "Maritime Solutions AS",
        "endDate": "2025-10-31T00:00:00Z",
        "daysUntilExpiry": 42,
        "totalValue": 65000.00,
        "autoRenewal": false,
        "accountManagerName": "Tor Hansen",
        "lastRenewalContact": "2025-08-15T00:00:00Z"
      }
    ],
    "renewalOpportunities": {
      "totalValue": 285000.00,
      "contractCount": 6,
      "averageValue": 47500.00
    }
  },
  "isSuccess": true,
  "message": "Contract expiry alerts retrieved successfully",
  "errorCode": null
}
```

## Error Response Examples
```json
{
  "data": null,
  "isSuccess": false,
  "message": "Contract not found",
  "errorCode": "CONTRACT_NOT_FOUND"
}
```

```json
{
  "data": null,
  "isSuccess": false,
  "message": "Contract number already exists",
  "errorCode": "DUPLICATE_CONTRACT_NUMBER"
}
```

```json
{
  "data": null,
  "isSuccess": false,
  "message": "Cannot terminate contract: Active audits in progress",
  "errorCode": "INVALID_TERMINATION_REQUEST"
}
```