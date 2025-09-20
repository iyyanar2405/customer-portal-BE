namespace CustomerPortal.FinancialService.Models;

// DTOs for Financial Reporting and Analytics
public class RevenueReportDto
{
    public decimal TotalRevenue { get; set; }
    public decimal RecognizedRevenue { get; set; }
    public decimal DeferredRevenue { get; set; }
    public string Period { get; set; } = string.Empty;
    public decimal Growth { get; set; }
    public List<RevenueByServiceDto> RevenueByService { get; set; } = new();
    public List<RevenueByCompanyDto> RevenueByCompany { get; set; } = new();
    public List<RevenueByMonthDto> RevenueByMonth { get; set; } = new();
    public List<ProjectedRevenueDto> ProjectedRevenue { get; set; } = new();
}

public class RevenueByServiceDto
{
    public string ServiceName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Percentage { get; set; }
    public decimal Growth { get; set; }
}

public class RevenueByCompanyDto
{
    public string CompanyName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Percentage { get; set; }
    public int InvoiceCount { get; set; }
}

public class RevenueByMonthDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int InvoiceCount { get; set; }
    public decimal AverageInvoiceAmount { get; set; }
}

public class ProjectedRevenueDto
{
    public string Month { get; set; } = string.Empty;
    public decimal ProjectedAmount { get; set; }
    public decimal Confidence { get; set; }
}

public class FinancialDashboardDto
{
    public decimal TotalRevenue { get; set; }
    public int TotalInvoices { get; set; }
    public int PaidInvoices { get; set; }
    public int OverdueInvoices { get; set; }
    public decimal AveragePaymentTime { get; set; }
    public decimal OutstandingAmount { get; set; }
    public decimal CollectionRate { get; set; }
    public List<MonthlyRevenueDto> MonthlyRevenue { get; set; } = new();
    public List<TopCustomerDto> TopCustomers { get; set; } = new();
    public List<PaymentMethodStatsDto> PaymentMethods { get; set; } = new();
    public AgingReportDto AgingReport { get; set; } = new();
}

public class MonthlyRevenueDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public int InvoiceCount { get; set; }
}

public class TopCustomerDto
{
    public string CompanyName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int InvoiceCount { get; set; }
}

public class PaymentMethodStatsDto
{
    public string Method { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Percentage { get; set; }
}

public class AgingReportDto
{
    public decimal Current { get; set; }
    public decimal Days30 { get; set; }
    public decimal Days60 { get; set; }
    public decimal Days90 { get; set; }
    public decimal Over90 { get; set; }
}

public class ProfitLossReportDto
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
    public List<RevenueByServiceProfitDto> RevenueByService { get; set; } = new();
    public List<MonthlyBreakdownDto> MonthlyBreakdown { get; set; } = new();
}

public class RevenueByServiceProfitDto
{
    public string ServiceName { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public decimal Cost { get; set; }
    public decimal Profit { get; set; }
    public decimal Margin { get; set; }
}

public class MonthlyBreakdownDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public decimal Expenses { get; set; }
    public decimal Profit { get; set; }
    public decimal Margin { get; set; }
}

public class CashFlowReportDto
{
    public string Period { get; set; } = string.Empty;
    public decimal OpeningBalance { get; set; }
    public decimal TotalInflows { get; set; }
    public decimal TotalOutflows { get; set; }
    public decimal NetCashFlow { get; set; }
    public decimal ClosingBalance { get; set; }
    public List<CashFlowItemDto> Inflows { get; set; } = new();
    public List<CashFlowItemDto> Outflows { get; set; } = new();
    public List<MonthlyFlowDto> MonthlyFlow { get; set; } = new();
    public List<ProjectedFlowDto> ProjectedFlow { get; set; } = new();
}

public class CashFlowItemDto
{
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class MonthlyFlowDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Inflows { get; set; }
    public decimal Outflows { get; set; }
    public decimal NetFlow { get; set; }
    public decimal Balance { get; set; }
}

public class ProjectedFlowDto
{
    public string Month { get; set; } = string.Empty;
    public decimal ProjectedInflows { get; set; }
    public decimal ProjectedOutflows { get; set; }
    public decimal ProjectedBalance { get; set; }
}

public class AccountsReceivableDto
{
    public decimal TotalOutstanding { get; set; }
    public decimal AverageDaysOutstanding { get; set; }
    public List<AgingBucketDto> AgingBuckets { get; set; } = new();
    public List<TopDebtorDto> TopDebtors { get; set; } = new();
    public List<ReceivableByCompanyDto> ByCompany { get; set; } = new();
    public List<ReceivableTrendDto> Trends { get; set; } = new();
}

public class AgingBucketDto
{
    public string Bucket { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int Count { get; set; }
    public decimal Percentage { get; set; }
}

public class TopDebtorDto
{
    public string CompanyName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime OldestInvoiceDate { get; set; }
    public int InvoiceCount { get; set; }
}

public class ReceivableByCompanyDto
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

public class ReceivableTrendDto
{
    public string Month { get; set; } = string.Empty;
    public decimal TotalOutstanding { get; set; }
    public decimal Collected { get; set; }
    public decimal NewInvoices { get; set; }
}

public class TaxCalculationDto
{
    public decimal Subtotal { get; set; }
    public List<TaxBreakdownDto> TaxBreakdown { get; set; } = new();
    public decimal TotalTax { get; set; }
    public decimal TotalAmount { get; set; }
}

public class TaxBreakdownDto
{
    public string TaxType { get; set; } = string.Empty;
    public decimal TaxRate { get; set; }
    public decimal TaxableAmount { get; set; }
    public decimal TaxAmount { get; set; }
}

public class CurrencyConversionDto
{
    public decimal OriginalAmount { get; set; }
    public decimal ConvertedAmount { get; set; }
    public string FromCurrency { get; set; } = string.Empty;
    public string ToCurrency { get; set; } = string.Empty;
    public decimal ExchangeRate { get; set; }
    public DateTime ConversionDate { get; set; }
}