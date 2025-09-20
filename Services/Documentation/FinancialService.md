# Financial Service Documentation

**Base URL:** `http://localhost:5007`  
**GraphQL Endpoint:** `http://localhost:5007/graphql`  
**Status:** 🔄 In Progress

## 📊 Entities Overview

The Financial Service manages invoicing, payments, financial reporting, and revenue tracking:

1. **Invoices** - Invoice records and billing
2. **InvoiceItems** - Line items within invoices
3. **Payments** - Payment tracking and processing
4. **PaymentMethods** - Available payment options
5. **TaxRates** - Tax configuration and calculations
6. **FinancialReports** - Financial analytics and reporting

---

## 🧾 Invoices Operations

### Query: Get All Invoices

**Operation:** `invoices`

**GraphQL Query:**
```graphql
query GetAllInvoices {
  invoices {
    id
    invoiceNumber
    companyId
    contractId
    auditId
    invoiceDate
    dueDate
    paidDate
    subtotal
    taxAmount
    totalAmount
    currency
    status
    paymentTerms
    notes
    isActive
    createdDate
    modifiedDate
    company {
      companyName
      companyCode
      address
      email
      phone
    }
    contract {
      contractNumber
      title
    }
    audit {
      auditNumber
      auditTitle
    }
    items {
      id
      description
      quantity
      unitPrice
      taxRate
      lineTotal
      service {
        serviceName
        serviceCode
      }
    }
    payments {
      id
      paymentDate
      amount
      paymentMethod
      transactionId
      status
    }
    taxes {
      taxType
      taxRate
      taxAmount
      taxableAmount
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "invoices": [
      {
        "id": 1,
        "invoiceNumber": "INV-2024-001",
        "companyId": 1,
        "contractId": 1,
        "auditId": 1,
        "invoiceDate": "2024-09-01T00:00:00Z",
        "dueDate": "2024-10-01T00:00:00Z",
        "paidDate": null,
        "subtotal": 15000.00,
        "taxAmount": 1500.00,
        "totalAmount": 16500.00,
        "currency": "USD",
        "status": "SENT",
        "paymentTerms": "NET_30",
        "notes": "Payment for ISO 9001:2015 surveillance audit",
        "isActive": true,
        "createdDate": "2024-09-01T10:00:00Z",
        "modifiedDate": null,
        "company": {
          "companyName": "Acme Corporation",
          "companyCode": "ACME001",
          "address": "123 Business St, Business City",
          "email": "accounts@acme.com",
          "phone": "+1-555-0123"
        },
        "contract": {
          "contractNumber": "CNT-2024-001",
          "title": "ISO Management System Certification Contract"
        },
        "audit": {
          "auditNumber": "AUD-2024-001",
          "auditTitle": "ISO 9001:2015 Annual Surveillance Audit"
        },
        "items": [
          {
            "id": 1,
            "description": "ISO 9001:2015 Surveillance Audit - Main Production Facility",
            "quantity": 1,
            "unitPrice": 15000.00,
            "taxRate": 10.0,
            "lineTotal": 15000.00,
            "service": {
              "serviceName": "ISO 9001 Audit",
              "serviceCode": "ISO9001"
            }
          }
        ],
        "payments": [],
        "taxes": [
          {
            "taxType": "VAT",
            "taxRate": 10.0,
            "taxAmount": 1500.00,
            "taxableAmount": 15000.00
          }
        ]
      }
    ]
  }
}
```

### Mutation: Create Invoice

**Operation:** `createInvoice`

**GraphQL Mutation:**
```graphql
mutation CreateInvoice($input: CreateInvoiceInput!) {
  createInvoice(input: $input) {
    id
    invoiceNumber
    invoiceDate
    dueDate
    totalAmount
    status
    createdDate
  }
}
```

**Input Payload:**
```json
{
  "input": {
    "companyId": 1,
    "contractId": 1,
    "auditId": 2,
    "invoiceDate": "2024-10-01T00:00:00Z",
    "dueDate": "2024-11-01T00:00:00Z",
    "currency": "USD",
    "paymentTerms": "NET_30",
    "notes": "Payment for initial certification audit",
    "items": [
      {
        "serviceId": 1,
        "description": "ISO 9001:2015 Initial Certification Audit",
        "quantity": 1,
        "unitPrice": 20000.00,
        "taxRate": 10.0
      },
      {
        "serviceId": 2,
        "description": "ISO 14001:2015 Initial Certification Audit",
        "quantity": 1,
        "unitPrice": 18000.00,
        "taxRate": 10.0
      }
    ]
  }
}
```

### Mutation: Update Invoice Status

**Operation:** `updateInvoiceStatus`

**GraphQL Mutation:**
```graphql
mutation UpdateInvoiceStatus($invoiceId: Int!, $status: InvoiceStatus!, $notes: String) {
  updateInvoiceStatus(invoiceId: $invoiceId, status: $status, notes: $notes) {
    id
    status
    modifiedDate
    statusHistory {
      status
      changedDate
      changedBy
      notes
    }
  }
}
```

**Status Values:**
- `DRAFT`
- `SENT`
- `VIEWED`
- `PAID`
- `OVERDUE`
- `CANCELLED`
- `REFUNDED`

### Mutation: Send Invoice

**Operation:** `sendInvoice`

**GraphQL Mutation:**
```graphql
mutation SendInvoice($invoiceId: Int!, $recipientEmails: [String!]!) {
  sendInvoice(invoiceId: $invoiceId, recipientEmails: $recipientEmails) {
    id
    status
    sentDate
    sentTo
    deliveryStatus
  }
}
```

---

## 💳 Payments Operations

### Query: Get All Payments

**Operation:** `payments`

**GraphQL Query:**
```graphql
query GetAllPayments {
  payments {
    id
    invoiceId
    paymentDate
    amount
    currency
    paymentMethod
    transactionId
    status
    processingFee
    notes
    createdDate
    modifiedDate
    invoice {
      invoiceNumber
      company {
        companyName
      }
    }
    paymentMethodDetails {
      type
      last4
      expiryDate
      bankName
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "payments": [
      {
        "id": 1,
        "invoiceId": 1,
        "paymentDate": "2024-09-25T00:00:00Z",
        "amount": 16500.00,
        "currency": "USD",
        "paymentMethod": "CREDIT_CARD",
        "transactionId": "TXN-20240925-001",
        "status": "COMPLETED",
        "processingFee": 165.00,
        "notes": "Payment processed via Stripe",
        "createdDate": "2024-09-25T14:30:00Z",
        "modifiedDate": null,
        "invoice": {
          "invoiceNumber": "INV-2024-001",
          "company": {
            "companyName": "Acme Corporation"
          }
        },
        "paymentMethodDetails": {
          "type": "VISA",
          "last4": "1234",
          "expiryDate": "12/26",
          "bankName": "First National Bank"
        }
      }
    ]
  }
}
```

### Mutation: Record Payment

**Operation:** `recordPayment`

**GraphQL Mutation:**
```graphql
mutation RecordPayment($input: RecordPaymentInput!) {
  recordPayment(input: $input) {
    id
    invoiceId
    amount
    paymentMethod
    transactionId
    status
    paymentDate
  }
}
```

**Input Payload:**
```json
{
  "input": {
    "invoiceId": 1,
    "amount": 16500.00,
    "currency": "USD",
    "paymentMethod": "BANK_TRANSFER",
    "transactionId": "BT-20241001-456",
    "paymentDate": "2024-10-01T00:00:00Z",
    "notes": "Wire transfer from client bank account"
  }
}
```

### Mutation: Process Refund

**Operation:** `processRefund`

**GraphQL Mutation:**
```graphql
mutation ProcessRefund($paymentId: Int!, $amount: Float!, $reason: String!) {
  processRefund(paymentId: $paymentId, amount: $amount, reason: $reason) {
    refundId
    paymentId
    amount
    reason
    status
    processedDate
    transactionId
  }
}
```

---

## 📊 Financial Reporting Operations

### Query: Get Revenue Report

**Operation:** `revenueReport`

**GraphQL Query:**
```graphql
query GetRevenueReport($startDate: DateTime!, $endDate: DateTime!, $groupBy: String!) {
  revenueReport(startDate: $startDate, endDate: $endDate, groupBy: $groupBy) {
    totalRevenue
    recognizedRevenue
    deferredRevenue
    period
    growth
    revenueByService {
      serviceName
      amount
      percentage
      growth
    }
    revenueByCompany {
      companyName
      amount
      percentage
      invoiceCount
    }
    revenueByMonth {
      month
      amount
      invoiceCount
      averageInvoiceAmount
    }
    projectedRevenue {
      month
      projectedAmount
      confidence
    }
  }
}
```

### Query: Get Financial Dashboard

**Operation:** `financialDashboard`

**GraphQL Query:**
```graphql
query GetFinancialDashboard($period: String!) {
  financialDashboard(period: $period) {
    totalRevenue
    totalInvoices
    paidInvoices
    overdueInvoices
    averagePaymentTime
    outstandingAmount
    collectionRate
    monthlyRevenue {
      month
      revenue
      invoiceCount
    }
    topCustomers {
      companyName
      totalAmount
      invoiceCount
    }
    paymentMethods {
      method
      count
      totalAmount
      percentage
    }
    agingReport {
      current
      days30
      days60
      days90
      over90
    }
  }
}
```

### Query: Get Profit Loss Report

**Operation:** `profitLossReport`

**GraphQL Query:**
```graphql
query GetProfitLossReport($startDate: DateTime!, $endDate: DateTime!) {
  profitLossReport(startDate: $startDate, endDate: $endDate) {
    totalRevenue
    costOfSales
    grossProfit
    grossProfitMargin
    operatingExpenses
    operatingProfit
    operatingMargin
    netProfit
    netMargin
    revenueByService {
      serviceName
      revenue
      cost
      profit
      margin
    }
    monthlyBreakdown {
      month
      revenue
      expenses
      profit
      margin
    }
  }
}
```

---

## 💰 Tax Operations

### Query: Get Tax Rates

**Operation:** `taxRates`

**GraphQL Query:**
```graphql
query GetTaxRates($countryId: Int, $isActive: Boolean) {
  taxRates(countryId: $countryId, isActive: $isActive) {
    id
    taxType
    taxRate
    countryId
    region
    effectiveDate
    expiryDate
    isActive
    description
    country {
      countryName
      countryCode
    }
  }
}
```

**Response Schema:**
```json
{
  "data": {
    "taxRates": [
      {
        "id": 1,
        "taxType": "VAT",
        "taxRate": 10.0,
        "countryId": 1,
        "region": "Standard Rate",
        "effectiveDate": "2024-01-01T00:00:00Z",
        "expiryDate": null,
        "isActive": true,
        "description": "Standard VAT rate for services",
        "country": {
          "countryName": "United States",
          "countryCode": "US"
        }
      }
    ]
  }
}
```

### Query: Calculate Tax

**Operation:** `calculateTax`

**GraphQL Query:**
```graphql
query CalculateTax($amount: Float!, $countryId: Int!, $serviceId: Int!) {
  calculateTax(amount: $amount, countryId: $countryId, serviceId: $serviceId) {
    subtotal
    taxBreakdown {
      taxType
      taxRate
      taxableAmount
      taxAmount
    }
    totalTax
    totalAmount
  }
}
```

---

## 📈 Financial Analytics

### Query: Get Cash Flow Report

**Operation:** `cashFlowReport`

**GraphQL Query:**
```graphql
query GetCashFlowReport($startDate: DateTime!, $endDate: DateTime!) {
  cashFlowReport(startDate: $startDate, endDate: $endDate) {
    period
    openingBalance
    totalInflows
    totalOutflows
    netCashFlow
    closingBalance
    inflows {
      category
      amount
      description
    }
    outflows {
      category
      amount
      description
    }
    monthlyFlow {
      month
      inflows
      outflows
      netFlow
      balance
    }
    projectedFlow {
      month
      projectedInflows
      projectedOutflows
      projectedBalance
    }
  }
}
```

### Query: Get Accounts Receivable

**Operation:** `accountsReceivable`

**GraphQL Query:**
```graphql
query GetAccountsReceivable($asOfDate: DateTime!) {
  accountsReceivable(asOfDate: $asOfDate) {
    totalOutstanding
    averageDaysOutstanding
    agingBuckets {
      bucket
      amount
      count
      percentage
    }
    topDebtors {
      companyName
      amount
      oldestInvoiceDate
      invoiceCount
    }
    byCompany {
      companyId
      companyName
      totalOutstanding
      oldestAmount
      currentAmount
      overdue30
      overdue60
      overdue90
      over90
    }
    trends {
      month
      totalOutstanding
      collected
      newInvoices
    }
  }
}
```

---

## 💱 Currency Operations

### Query: Get Exchange Rates

**Operation:** `exchangeRates`

**GraphQL Query:**
```graphql
query GetExchangeRates($baseCurrency: String!, $targetCurrencies: [String!]!, $date: DateTime) {
  exchangeRates(baseCurrency: $baseCurrency, targetCurrencies: $targetCurrencies, date: $date) {
    baseCurrency
    rateDate
    rates {
      currency
      rate
      lastUpdated
    }
  }
}
```

### Mutation: Convert Currency

**Operation:** `convertCurrency`

**GraphQL Mutation:**
```graphql
mutation ConvertCurrency($amount: Float!, $fromCurrency: String!, $toCurrency: String!, $date: DateTime) {
  convertCurrency(amount: $amount, fromCurrency: $fromCurrency, toCurrency: $toCurrency, date: $date) {
    originalAmount
    convertedAmount
    fromCurrency
    toCurrency
    exchangeRate
    conversionDate
  }
}
```

---

## 📊 Budget Operations

### Query: Get Budgets

**Operation:** `budgets`

**GraphQL Query:**
```graphql
query GetBudgets($year: Int!, $departmentId: Int) {
  budgets(year: $year, departmentId: $departmentId) {
    id
    budgetYear
    departmentId
    category
    budgetedAmount
    actualAmount
    variance
    variancePercentage
    status
    department {
      departmentName
    }
    monthlyBreakdown {
      month
      budgeted
      actual
      variance
    }
  }
}
```

### Query: Get Budget Performance

**Operation:** `budgetPerformance`

**GraphQL Query:**
```graphql
query GetBudgetPerformance($year: Int!, $quarter: Int) {
  budgetPerformance(year: $year, quarter: $quarter) {
    totalBudgeted
    totalActual
    totalVariance
    performanceScore
    byCategory {
      category
      budgeted
      actual
      variance
      performance
    }
    byDepartment {
      departmentName
      budgeted
      actual
      variance
      performance
    }
    alerts {
      category
      message
      severity
      variance
    }
  }
}
```

---

*📝 Note: This service is currently in development. The actual implementation may vary from this specification.*