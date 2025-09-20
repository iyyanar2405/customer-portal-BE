using CustomerPortal.FinancialService.Models;

namespace CustomerPortal.FinancialService.GraphQL;

public class CompanyGraphQLType
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyCode { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class ServiceGraphQLType
{
    public int Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string ServiceCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class ContractGraphQLType
{
    public int Id { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class AuditGraphQLType
{
    public int Id { get; set; }
    public string AuditNumber { get; set; } = string.Empty;
    public string AuditTitle { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class InvoiceGraphQLType
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public int? ContractId { get; set; }
    public int? AuditId { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string PaymentTerms { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }

    // Navigation properties
    public CompanyGraphQLType? Company { get; set; }
    public ContractGraphQLType? Contract { get; set; }
    public AuditGraphQLType? Audit { get; set; }
    public List<InvoiceItemGraphQLType> Items { get; set; } = new();
    public List<PaymentGraphQLType> Payments { get; set; } = new();
    public List<InvoiceTaxGraphQLType> Taxes { get; set; } = new();
}

public class InvoiceItemGraphQLType
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int? ServiceId { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TaxRate { get; set; }
    public decimal LineTotal { get; set; }
    public DateTime CreatedDate { get; set; }

    // Navigation properties
    public ServiceGraphQLType? Service { get; set; }
}

public class PaymentGraphQLType
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal ProcessingFee { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }

    // Navigation properties
    public InvoiceGraphQLType? Invoice { get; set; }
    public PaymentMethodGraphQLType? PaymentMethodDetails { get; set; }
}

public class PaymentMethodGraphQLType
{
    public int Id { get; set; }
    public int PaymentId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Last4 { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}

public class TaxRateGraphQLType
{
    public int Id { get; set; }
    public string TaxType { get; set; } = string.Empty;
    public decimal TaxRate { get; set; }
    public int CountryId { get; set; }
    public string Region { get; set; } = string.Empty;
    public DateTime EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }

    // Navigation properties
    public CountryGraphQLType? Country { get; set; }
}

public class CountryGraphQLType
{
    public int Id { get; set; }
    public string CountryName { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class InvoiceTaxGraphQLType
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public string TaxType { get; set; } = string.Empty;
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TaxableAmount { get; set; }
    public DateTime CreatedDate { get; set; }
}

// Reporting and Analytics Types
public class RevenueReportGraphQLType
{
    public decimal TotalRevenue { get; set; }
    public decimal RecognizedRevenue { get; set; }
    public decimal DeferredRevenue { get; set; }
    public string Period { get; set; } = string.Empty;
    public decimal Growth { get; set; }
    public List<RevenueByServiceGraphQLType> RevenueByService { get; set; } = new();
    public List<RevenueByCompanyGraphQLType> RevenueByCompany { get; set; } = new();
    public List<RevenueByMonthGraphQLType> RevenueByMonth { get; set; } = new();
    public List<ProjectedRevenueGraphQLType> ProjectedRevenue { get; set; } = new();
}

public class RevenueByServiceGraphQLType
{
    public string ServiceName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Percentage { get; set; }
    public decimal Growth { get; set; }
}

public class RevenueByCompanyGraphQLType
{
    public string CompanyName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Percentage { get; set; }
    public int InvoiceCount { get; set; }
}

public class RevenueByMonthGraphQLType
{
    public string Month { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int InvoiceCount { get; set; }
    public decimal AverageInvoiceAmount { get; set; }
}

public class ProjectedRevenueGraphQLType
{
    public string Month { get; set; } = string.Empty;
    public decimal ProjectedAmount { get; set; }
    public decimal Confidence { get; set; }
}

public class FinancialDashboardGraphQLType
{
    public decimal TotalRevenue { get; set; }
    public int TotalInvoices { get; set; }
    public int PaidInvoices { get; set; }
    public int OverdueInvoices { get; set; }
    public decimal AveragePaymentTime { get; set; }
    public decimal OutstandingAmount { get; set; }
    public decimal CollectionRate { get; set; }
    public List<MonthlyRevenueGraphQLType> MonthlyRevenue { get; set; } = new();
    public List<TopCustomerGraphQLType> TopCustomers { get; set; } = new();
    public List<PaymentMethodStatsGraphQLType> PaymentMethods { get; set; } = new();
    public AgingReportGraphQLType? AgingReport { get; set; }
}

public class MonthlyRevenueGraphQLType
{
    public string Month { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public int InvoiceCount { get; set; }
}

public class TopCustomerGraphQLType
{
    public string CompanyName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int InvoiceCount { get; set; }
}

public class PaymentMethodStatsGraphQLType
{
    public string Method { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Percentage { get; set; }
}

public class AgingReportGraphQLType
{
    public decimal Current { get; set; }
    public decimal Days30 { get; set; }
    public decimal Days60 { get; set; }
    public decimal Days90 { get; set; }
    public decimal Over90 { get; set; }
}

public class ProfitLossReportGraphQLType
{
    public decimal TotalRevenue { get; set; }
    public decimal CostOfSales { get; set; }
    public decimal GrossProfit { get; set; }
    public decimal GrossProfitMargin { get; set; }
    public decimal OperatingExpenses { get; set; }
    public decimal OperatingProfit { get; set; }
    public decimal OperatingMargin { get; set; }
    public decimal NetProfit { get; set; }
    public decimal NetMargin { get; set; }
    public List<RevenueByServiceProfitGraphQLType> RevenueByService { get; set; } = new();
    public List<MonthlyBreakdownGraphQLType> MonthlyBreakdown { get; set; } = new();
}

public class RevenueByServiceProfitGraphQLType
{
    public string ServiceName { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public decimal Cost { get; set; }
    public decimal Profit { get; set; }
    public decimal Margin { get; set; }
}

public class MonthlyBreakdownGraphQLType
{
    public string Month { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public decimal Expenses { get; set; }
    public decimal Profit { get; set; }
    public decimal Margin { get; set; }
}

public class CashFlowReportGraphQLType
{
    public string Period { get; set; } = string.Empty;
    public decimal OpeningBalance { get; set; }
    public decimal TotalInflows { get; set; }
    public decimal TotalOutflows { get; set; }
    public decimal NetCashFlow { get; set; }
    public decimal ClosingBalance { get; set; }
    public List<CashFlowItemGraphQLType> Inflows { get; set; } = new();
    public List<CashFlowItemGraphQLType> Outflows { get; set; } = new();
    public List<MonthlyFlowGraphQLType> MonthlyFlow { get; set; } = new();
    public List<ProjectedFlowGraphQLType> ProjectedFlow { get; set; } = new();
}

public class CashFlowItemGraphQLType
{
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class MonthlyFlowGraphQLType
{
    public string Month { get; set; } = string.Empty;
    public decimal Inflows { get; set; }
    public decimal Outflows { get; set; }
    public decimal NetFlow { get; set; }
    public decimal Balance { get; set; }
}

public class ProjectedFlowGraphQLType
{
    public string Month { get; set; } = string.Empty;
    public decimal ProjectedInflows { get; set; }
    public decimal ProjectedOutflows { get; set; }
    public decimal ProjectedBalance { get; set; }
}

public class AccountsReceivableGraphQLType
{
    public decimal TotalOutstanding { get; set; }
    public decimal AverageDaysOutstanding { get; set; }
    public List<AgingBucketGraphQLType> AgingBuckets { get; set; } = new();
    public List<TopDebtorGraphQLType> TopDebtors { get; set; } = new();
    public List<ReceivableByCompanyGraphQLType> ByCompany { get; set; } = new();
    public List<ReceivableTrendGraphQLType> Trends { get; set; } = new();
}

public class AgingBucketGraphQLType
{
    public string Bucket { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int Count { get; set; }
    public decimal Percentage { get; set; }
}

public class TopDebtorGraphQLType
{
    public string CompanyName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime OldestInvoiceDate { get; set; }
    public int InvoiceCount { get; set; }
}

public class ReceivableByCompanyGraphQLType
{
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public decimal TotalOutstanding { get; set; }
    public decimal OldestAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public decimal Overdue30 { get; set; }
    public decimal Overdue60 { get; set; }
    public decimal Overdue90 { get; set; }
    public decimal Over90 { get; set; }
}

public class ReceivableTrendGraphQLType
{
    public string Month { get; set; } = string.Empty;
    public decimal TotalOutstanding { get; set; }
    public decimal Collected { get; set; }
    public decimal NewInvoices { get; set; }
}

public class TaxCalculationGraphQLType
{
    public decimal Subtotal { get; set; }
    public List<TaxBreakdownGraphQLType> TaxBreakdown { get; set; } = new();
    public decimal TotalTax { get; set; }
    public decimal TotalAmount { get; set; }
}

public class TaxBreakdownGraphQLType
{
    public string TaxType { get; set; } = string.Empty;
    public decimal TaxRate { get; set; }
    public decimal TaxableAmount { get; set; }
    public decimal TaxAmount { get; set; }
}

public class CurrencyConversionGraphQLType
{
    public decimal OriginalAmount { get; set; }
    public decimal ConvertedAmount { get; set; }
    public string FromCurrency { get; set; } = string.Empty;
    public string ToCurrency { get; set; } = string.Empty;
    public decimal ExchangeRate { get; set; }
    public DateTime ConversionDate { get; set; }
}