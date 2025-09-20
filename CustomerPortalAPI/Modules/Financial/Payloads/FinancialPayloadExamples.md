# Financial Module - JSON Payload Examples

## Create Invoice Request
```json
{
  "invoiceNumber": "INV-2025-001",
  "contractId": 1,
  "companyId": 1,
  "invoiceDate": "2025-09-19T00:00:00Z",
  "dueDate": "2025-10-19T00:00:00Z",
  "currency": "USD",
  "exchangeRate": 1.0,
  "subtotal": 15000.00,
  "taxAmount": 1500.00,
  "totalAmount": 16500.00,
  "taxRate": 10.0,
  "paymentTerms": "Net 30 days",
  "description": "ISO 9001 Initial Certification Audit",
  "purchaseOrderNumber": "PO-2025-001",
  "billingAddress": "123 Industrial Blvd, New York, NY 10001",
  "notes": "Payment due within 30 days of invoice date",
  "status": "Sent",
  "isActive": true,
  "createdBy": 1
}
```

## Create Invoice Response
```json
{
  "data": {
    "id": 1,
    "invoiceNumber": "INV-2025-001",
    "contractId": 1,
    "contractNumber": "CONTRACT-2025-001",
    "companyId": 1,
    "companyName": "Acme Corporation",
    "invoiceDate": "2025-09-19T00:00:00Z",
    "dueDate": "2025-10-19T00:00:00Z",
    "currency": "USD",
    "exchangeRate": 1.0,
    "subtotal": 15000.00,
    "taxAmount": 1500.00,
    "totalAmount": 16500.00,
    "taxRate": 10.0,
    "paymentTerms": "Net 30 days",
    "description": "ISO 9001 Initial Certification Audit",
    "purchaseOrderNumber": "PO-2025-001",
    "billingAddress": "123 Industrial Blvd, New York, NY 10001",
    "notes": "Payment due within 30 days of invoice date",
    "status": "Sent",
    "sentDate": "2025-09-19T14:30:00Z",
    "daysOverdue": 0,
    "isOverdue": false,
    "isActive": true,
    "createdDate": "2025-09-19T10:00:00Z",
    "createdBy": 1,
    "createdByName": "Finance Manager",
    "modifiedDate": "2025-09-19T14:30:00Z",
    "modifiedBy": 1,
    "modifiedByName": "Finance Manager",
    "lineItems": [
      {
        "id": 1,
        "description": "ISO 9001 Stage 1 Audit",
        "quantity": 2,
        "unitPrice": 2500.00,
        "totalPrice": 5000.00,
        "auditId": 1
      },
      {
        "id": 2,
        "description": "ISO 9001 Stage 2 Audit",
        "quantity": 4,
        "unitPrice": 2500.00,
        "totalPrice": 10000.00,
        "auditId": 1
      }
    ],
    "payments": []
  },
  "isSuccess": true,
  "message": "Invoice created successfully",
  "errorCode": null
}
```

## Record Payment Request
```json
{
  "invoiceId": 1,
  "paymentAmount": 16500.00,
  "paymentDate": "2025-10-15T00:00:00Z",
  "paymentMethod": "Bank Transfer",
  "referenceNumber": "TXN-2025-001234",
  "notes": "Full payment received via wire transfer",
  "receivedBy": 3
}
```

## Record Payment Response
```json
{
  "data": {
    "id": 1,
    "invoiceId": 1,
    "invoiceNumber": "INV-2025-001",
    "paymentAmount": 16500.00,
    "paymentDate": "2025-10-15T00:00:00Z",
    "paymentMethod": "Bank Transfer",
    "referenceNumber": "TXN-2025-001234",
    "notes": "Full payment received via wire transfer",
    "status": "Completed",
    "isActive": true,
    "createdDate": "2025-10-15T09:30:00Z",
    "receivedBy": 3,
    "receivedByName": "Accounts Receivable",
    "updatedInvoiceStatus": "Paid",
    "remainingBalance": 0.00
  },
  "isSuccess": true,
  "message": "Payment recorded successfully",
  "errorCode": null
}
```

## Invoice List Response
```json
{
  "data": {
    "invoices": [
      {
        "id": 1,
        "invoiceNumber": "INV-2025-001",
        "companyName": "Acme Corporation",
        "contractNumber": "CONTRACT-2025-001",
        "invoiceDate": "2025-09-19T00:00:00Z",
        "dueDate": "2025-10-19T00:00:00Z",
        "totalAmount": 16500.00,
        "currency": "USD",
        "status": "Paid",
        "daysOverdue": 0,
        "isOverdue": false,
        "paidAmount": 16500.00,
        "remainingBalance": 0.00
      },
      {
        "id": 2,
        "invoiceNumber": "INV-2025-002",
        "companyName": "TechFlow Industries Ltd",
        "contractNumber": "CONTRACT-2025-002",
        "invoiceDate": "2025-08-15T00:00:00Z",
        "dueDate": "2025-09-14T00:00:00Z",
        "totalAmount": 12000.00,
        "currency": "EUR",
        "status": "Overdue",
        "daysOverdue": 5,
        "isOverdue": true,
        "paidAmount": 0.00,
        "remainingBalance": 12000.00
      }
    ],
    "totalCount": 2,
    "pageNumber": 1,
    "pageSize": 10,
    "hasNextPage": false,
    "hasPreviousPage": false,
    "summaryData": {
      "totalInvoiced": 28500.00,
      "totalPaid": 16500.00,
      "totalOutstanding": 12000.00,
      "overdueAmount": 12000.00,
      "overdueCount": 1
    }
  },
  "isSuccess": true,
  "message": "Data retrieved successfully",
  "errorCode": null
}
```

## Financial Dashboard Response
```json
{
  "data": {
    "summary": {
      "totalRevenue": 245000.00,
      "totalOutstanding": 48500.00,
      "overdueAmount": 23000.00,
      "projectedRevenue": 320000.00,
      "averagePaymentDays": 28,
      "collectionRate": 94.2
    },
    "monthlyData": [
      {
        "month": "2025-01",
        "invoiced": 45000.00,
        "collected": 42000.00,
        "outstanding": 3000.00
      },
      {
        "month": "2025-02",
        "invoiced": 38000.00,
        "collected": 38000.00,
        "outstanding": 0.00
      },
      {
        "month": "2025-03",
        "invoiced": 52000.00,
        "collected": 48000.00,
        "outstanding": 4000.00
      }
    ],
    "topCustomers": [
      {
        "companyId": 1,
        "companyName": "Acme Corporation",
        "totalRevenue": 75000.00,
        "outstandingAmount": 15000.00,
        "lastPaymentDate": "2025-09-15T00:00:00Z"
      },
      {
        "companyId": 2,
        "companyName": "TechFlow Industries Ltd",
        "totalRevenue": 65000.00,
        "outstandingAmount": 12000.00,
        "lastPaymentDate": "2025-08-10T00:00:00Z"
      }
    ],
    "agingReport": {
      "current": 25500.00,
      "days1to30": 15000.00,
      "days31to60": 8000.00,
      "days61to90": 0.00,
      "over90Days": 0.00
    }
  },
  "isSuccess": true,
  "message": "Financial dashboard data retrieved successfully",
  "errorCode": null
}
```

## Financial Filter Request
```json
{
  "companyIds": [1, 2],
  "contractIds": [1, 3, 5],
  "statuses": ["Sent", "Overdue"],
  "invoiceDateFrom": "2025-01-01T00:00:00Z",
  "invoiceDateTo": "2025-12-31T23:59:59Z",
  "dueDateFrom": "2025-09-01T00:00:00Z",
  "dueDateTo": "2025-10-31T23:59:59Z",
  "amountFrom": 1000.00,
  "amountTo": 50000.00,
  "currencies": ["USD", "EUR"],
  "overdueDaysFrom": 0,
  "overdueDaysTo": 90,
  "searchTerm": "ISO",
  "pageNumber": 1,
  "pageSize": 10,
  "sortBy": "dueDate",
  "sortDirection": "asc"
}
```

## Send Invoice Reminder Request
```json
{
  "invoiceId": 2,
  "reminderType": "Final Notice",
  "customMessage": "This is a final notice. Please remit payment immediately to avoid collection proceedings.",
  "includeLateFees": true,
  "lateFeeAmount": 120.00,
  "sentBy": 3
}
```

## Write Off Invoice Request
```json
{
  "invoiceId": 5,
  "writeOffAmount": 2500.00,
  "writeOffReason": "Customer bankruptcy - uncollectible",
  "writeOffDate": "2025-09-19T00:00:00Z",
  "approvedBy": 10,
  "notes": "Customer filed for Chapter 11. Amount deemed uncollectible after legal review.",
  "writtenOffBy": 3
}
```

## Credit Note Request
```json
{
  "originalInvoiceId": 1,
  "creditNoteNumber": "CN-2025-001",
  "creditAmount": 1500.00,
  "reason": "Service delivery issue - partial refund approved",
  "description": "Credit for delayed audit completion",
  "issueDate": "2025-09-19T00:00:00Z",
  "approvedBy": 10,
  "issuedBy": 3
}
```

## Recurring Invoice Setup Request
```json
{
  "contractId": 1,
  "templateName": "Monthly Surveillance Audit",
  "frequency": "Monthly",
  "startDate": "2025-10-01T00:00:00Z",
  "endDate": "2027-12-31T00:00:00Z",
  "amount": 2500.00,
  "description": "Monthly surveillance audit - ISO 9001",
  "dueDaysAfterIssue": 30,
  "autoSend": true,
  "createdBy": 1
}
```

## Payment Terms Update Request
```json
{
  "companyId": 1,
  "newPaymentTerms": "Net 15 days",
  "creditLimit": 50000.00,
  "effectiveDate": "2025-10-01T00:00:00Z",
  "reason": "Improved payment history - terms upgraded",
  "approvedBy": 10,
  "updatedBy": 3
}
```

## Error Response Examples
```json
{
  "data": null,
  "isSuccess": false,
  "message": "Invoice not found",
  "errorCode": "INVOICE_NOT_FOUND"
}
```

```json
{
  "data": null,
  "isSuccess": false,
  "message": "Payment amount exceeds outstanding balance",
  "errorCode": "INVALID_PAYMENT_AMOUNT"
}
```

```json
{
  "data": null,
  "isSuccess": false,
  "message": "Cannot modify paid invoice",
  "errorCode": "INVOICE_ALREADY_PAID"
}
```