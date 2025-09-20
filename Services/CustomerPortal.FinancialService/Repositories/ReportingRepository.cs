using Microsoft.EntityFrameworkCore;
using CustomerPortal.FinancialService.Data;
using CustomerPortal.FinancialService.Models;

namespace CustomerPortal.FinancialService.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly FinancialDbContext _context;

    public ServiceRepository(FinancialDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Service>> GetAllAsync()
    {
        return await _context.Services
            .Where(s => s.IsActive)
            .OrderBy(s => s.ServiceName)
            .ToListAsync();
    }

    public async Task<Service?> GetByIdAsync(int id)
    {
        return await _context.Services
            .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
    }

    public async Task<Service?> GetByServiceCodeAsync(string serviceCode)
    {
        return await _context.Services
            .FirstOrDefaultAsync(s => s.ServiceCode == serviceCode && s.IsActive);
    }

    public async Task<IEnumerable<Service>> GetByCategoryAsync(string category)
    {
        return await _context.Services
            .Where(s => s.Category == category && s.IsActive)
            .OrderBy(s => s.ServiceName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Service>> GetActiveServicesAsync()
    {
        return await _context.Services
            .Where(s => s.IsActive)
            .OrderBy(s => s.ServiceName)
            .ToListAsync();
    }

    public async Task<Service> CreateAsync(Service service)
    {
        _context.Services.Add(service);
        await _context.SaveChangesAsync();
        return service;
    }

    public async Task<Service> UpdateAsync(Service service)
    {
        _context.Services.Update(service);
        await _context.SaveChangesAsync();
        return service;
    }

    public async Task DeleteAsync(int id)
    {
        var service = await GetByIdAsync(id);
        if (service != null)
        {
            service.IsActive = false;
            await UpdateAsync(service);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Services.AnyAsync(s => s.Id == id && s.IsActive);
    }
}

public class TaxRateRepository : ITaxRateRepository
{
    private readonly FinancialDbContext _context;

    public TaxRateRepository(FinancialDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaxRate>> GetAllAsync()
    {
        return await _context.TaxRates
            .Include(t => t.Country)
            .Where(t => t.IsActive)
            .OrderBy(t => t.Country.CountryName)
            .ThenBy(t => t.TaxType)
            .ToListAsync();
    }

    public async Task<TaxRate?> GetByIdAsync(int id)
    {
        return await _context.TaxRates
            .Include(t => t.Country)
            .FirstOrDefaultAsync(t => t.Id == id && t.IsActive);
    }

    public async Task<IEnumerable<TaxRate>> GetByCountryIdAsync(int countryId)
    {
        return await _context.TaxRates
            .Include(t => t.Country)
            .Where(t => t.CountryId == countryId && t.IsActive)
            .OrderBy(t => t.TaxType)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaxRate>> GetByTaxTypeAsync(string taxType)
    {
        return await _context.TaxRates
            .Include(t => t.Country)
            .Where(t => t.TaxType == taxType && t.IsActive)
            .OrderBy(t => t.Country.CountryName)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaxRate>> GetActiveRatesAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _context.TaxRates
            .Include(t => t.Country)
            .Where(t => t.IsActive && 
                       t.EffectiveDate <= today && 
                       (t.ExpiryDate == null || t.ExpiryDate >= today))
            .OrderBy(t => t.Country.CountryName)
            .ThenBy(t => t.TaxType)
            .ToListAsync();
    }

    public async Task<decimal> GetApplicableTaxRateAsync(int countryId, int serviceId)
    {
        var today = DateTime.UtcNow.Date;
        var taxRate = await _context.TaxRates
            .Where(t => t.CountryId == countryId && 
                       t.IsActive && 
                       t.EffectiveDate <= today && 
                       (t.ExpiryDate == null || t.ExpiryDate >= today))
            .OrderByDescending(t => t.Rate)
            .FirstOrDefaultAsync();

        return taxRate?.Rate ?? 0m;
    }

    public async Task<TaxRate> CreateAsync(TaxRate taxRate)
    {
        _context.TaxRates.Add(taxRate);
        await _context.SaveChangesAsync();
        return taxRate;
    }

    public async Task<TaxRate> UpdateAsync(TaxRate taxRate)
    {
        _context.TaxRates.Update(taxRate);
        await _context.SaveChangesAsync();
        return taxRate;
    }

    public async Task DeleteAsync(int id)
    {
        var taxRate = await GetByIdAsync(id);
        if (taxRate != null)
        {
            taxRate.IsActive = false;
            await UpdateAsync(taxRate);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.TaxRates.AnyAsync(t => t.Id == id && t.IsActive);
    }
}

public class FinancialReportingRepository : IFinancialReportingRepository
{
    private readonly FinancialDbContext _context;

    public FinancialReportingRepository(FinancialDbContext context)
    {
        _context = context;
    }

    public async Task<RevenueReportDto> GetRevenueReportAsync(DateTime startDate, DateTime endDate, string groupBy)
    {
        var invoices = await _context.Invoices
            .Include(i => i.Company)
            .Include(i => i.Items)
                .ThenInclude(item => item.Service)
            .Where(i => i.InvoiceDate >= startDate && i.InvoiceDate <= endDate && i.IsActive)
            .ToListAsync();

        var totalRevenue = invoices.Sum(i => i.TotalAmount);
        var recognizedRevenue = invoices.Where(i => i.Status == "PAID").Sum(i => i.TotalAmount);
        var deferredRevenue = totalRevenue - recognizedRevenue;

        var revenueByService = invoices
            .SelectMany(i => i.Items)
            .GroupBy(item => item.Service?.ServiceName ?? "Unknown")
            .Select(g => new RevenueByServiceDto
            {
                ServiceName = g.Key,
                Amount = g.Sum(item => item.LineTotal),
                Percentage = totalRevenue > 0 ? (g.Sum(item => item.LineTotal) / totalRevenue) * 100 : 0,
                Growth = 0 // Would need historical data to calculate
            })
            .OrderByDescending(r => r.Amount)
            .ToList();

        var revenueByCompany = invoices
            .GroupBy(i => i.Company.CompanyName)
            .Select(g => new RevenueByCompanyDto
            {
                CompanyName = g.Key,
                Amount = g.Sum(i => i.TotalAmount),
                Percentage = totalRevenue > 0 ? (g.Sum(i => i.TotalAmount) / totalRevenue) * 100 : 0,
                InvoiceCount = g.Count()
            })
            .OrderByDescending(r => r.Amount)
            .ToList();

        var revenueByMonth = invoices
            .GroupBy(i => new { i.InvoiceDate.Year, i.InvoiceDate.Month })
            .Select(g => new RevenueByMonthDto
            {
                Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                Amount = g.Sum(i => i.TotalAmount),
                InvoiceCount = g.Count(),
                AverageInvoiceAmount = g.Count() > 0 ? g.Sum(i => i.TotalAmount) / g.Count() : 0
            })
            .OrderBy(r => r.Month)
            .ToList();

        return new RevenueReportDto
        {
            TotalRevenue = totalRevenue,
            RecognizedRevenue = recognizedRevenue,
            DeferredRevenue = deferredRevenue,
            Period = $"{startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}",
            Growth = 0, // Would need historical data to calculate
            RevenueByService = revenueByService,
            RevenueByCompany = revenueByCompany,
            RevenueByMonth = revenueByMonth,
            ProjectedRevenue = new List<ProjectedRevenueDto>() // Would implement projection logic
        };
    }

    public async Task<FinancialDashboardDto> GetFinancialDashboardAsync(string period)
    {
        var currentDate = DateTime.UtcNow;
        var startDate = period switch
        {
            "month" => new DateTime(currentDate.Year, currentDate.Month, 1),
            "quarter" => new DateTime(currentDate.Year, ((currentDate.Month - 1) / 3) * 3 + 1, 1),
            "year" => new DateTime(currentDate.Year, 1, 1),
            _ => currentDate.AddDays(-30)
        };

        var invoices = await _context.Invoices
            .Include(i => i.Company)
            .Include(i => i.Payments)
            .Where(i => i.InvoiceDate >= startDate && i.IsActive)
            .ToListAsync();

        var payments = await _context.Payments
            .Include(p => p.Invoice)
            .Where(p => p.PaymentDate >= startDate)
            .ToListAsync();

        var totalRevenue = invoices.Sum(i => i.TotalAmount);
        var totalInvoices = invoices.Count;
        var paidInvoices = invoices.Count(i => i.Status == "PAID");
        var overdueInvoices = invoices.Count(i => i.DueDate < currentDate && i.Status != "PAID" && i.Status != "CANCELLED");
        var outstandingAmount = invoices.Where(i => i.Status != "PAID" && i.Status != "CANCELLED").Sum(i => i.TotalAmount);

        return new FinancialDashboardDto
        {
            TotalRevenue = totalRevenue,
            TotalInvoices = totalInvoices,
            PaidInvoices = paidInvoices,
            OverdueInvoices = overdueInvoices,
            AveragePaymentTime = 25, // Would calculate from actual payment data
            OutstandingAmount = outstandingAmount,
            CollectionRate = totalInvoices > 0 ? (decimal)paidInvoices / totalInvoices * 100 : 0,
            MonthlyRevenue = new List<MonthlyRevenueDto>(),
            TopCustomers = new List<TopCustomerDto>(),
            PaymentMethods = new List<PaymentMethodStatsDto>(),
            AgingReport = new AgingReportDto()
        };
    }

    public async Task<ProfitLossReportDto> GetProfitLossReportAsync(DateTime startDate, DateTime endDate)
    {
        var invoices = await _context.Invoices
            .Include(i => i.Items)
                .ThenInclude(item => item.Service)
            .Where(i => i.InvoiceDate >= startDate && i.InvoiceDate <= endDate && i.IsActive)
            .ToListAsync();

        var totalRevenue = invoices.Sum(i => i.TotalAmount);
        var costOfSales = totalRevenue * 0.3m; // Example: 30% cost ratio
        var grossProfit = totalRevenue - costOfSales;
        var operatingExpenses = totalRevenue * 0.2m; // Example: 20% operating expenses
        var operatingProfit = grossProfit - operatingExpenses;
        var netProfit = operatingProfit;

        return new ProfitLossReportDto
        {
            TotalRevenue = totalRevenue,
            CostOfSales = costOfSales,
            GrossProfit = grossProfit,
            GrossProfitMargin = totalRevenue > 0 ? (grossProfit / totalRevenue) * 100 : 0,
            OperatingExpenses = operatingExpenses,
            OperatingProfit = operatingProfit,
            OperatingMargin = totalRevenue > 0 ? (operatingProfit / totalRevenue) * 100 : 0,
            NetProfit = netProfit,
            NetMargin = totalRevenue > 0 ? (netProfit / totalRevenue) * 100 : 0,
            RevenueByService = new List<RevenueByServiceProfitDto>(),
            MonthlyBreakdown = new List<MonthlyBreakdownDto>()
        };
    }

    public async Task<CashFlowReportDto> GetCashFlowReportAsync(DateTime startDate, DateTime endDate)
    {
        var payments = await _context.Payments
            .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate && p.Status == "COMPLETED")
            .ToListAsync();

        var totalInflows = payments.Sum(p => p.Amount);
        var totalOutflows = payments.Sum(p => p.ProcessingFee); // Simplified example

        return new CashFlowReportDto
        {
            Period = $"{startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}",
            OpeningBalance = 0,
            TotalInflows = totalInflows,
            TotalOutflows = totalOutflows,
            NetCashFlow = totalInflows - totalOutflows,
            ClosingBalance = totalInflows - totalOutflows,
            Inflows = new List<CashFlowItemDto>(),
            Outflows = new List<CashFlowItemDto>(),
            MonthlyFlow = new List<MonthlyFlowDto>(),
            ProjectedFlow = new List<ProjectedFlowDto>()
        };
    }

    public async Task<AccountsReceivableDto> GetAccountsReceivableAsync(DateTime asOfDate)
    {
        var unpaidInvoices = await _context.Invoices
            .Include(i => i.Company)
            .Where(i => i.Status != "PAID" && i.Status != "CANCELLED" && i.IsActive && i.InvoiceDate <= asOfDate)
            .ToListAsync();

        var totalOutstanding = unpaidInvoices.Sum(i => i.TotalAmount);
        var averageDaysOutstanding = unpaidInvoices.Any() ? 
            unpaidInvoices.Average(i => (asOfDate - i.InvoiceDate).Days) : 0;

        return new AccountsReceivableDto
        {
            TotalOutstanding = totalOutstanding,
            AverageDaysOutstanding = (decimal)averageDaysOutstanding,
            AgingBuckets = new List<AgingBucketDto>(),
            TopDebtors = new List<TopDebtorDto>(),
            ByCompany = new List<ReceivableByCompanyDto>(),
            Trends = new List<ReceivableTrendDto>()
        };
    }

    public async Task<TaxCalculationDto> CalculateTaxAsync(decimal amount, int countryId, int serviceId)
    {
        var taxRates = await _context.TaxRates
            .Where(t => t.CountryId == countryId && t.IsActive)
            .ToListAsync();

        var taxBreakdown = taxRates.Select(t => new TaxBreakdownDto
        {
            TaxType = t.TaxType,
            TaxRate = t.Rate,
            TaxableAmount = amount,
            TaxAmount = amount * (t.Rate / 100)
        }).ToList();

        var totalTax = taxBreakdown.Sum(t => t.TaxAmount);

        return new TaxCalculationDto
        {
            Subtotal = amount,
            TaxBreakdown = taxBreakdown,
            TotalTax = totalTax,
            TotalAmount = amount + totalTax
        };
    }
}