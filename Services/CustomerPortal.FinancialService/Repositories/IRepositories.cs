using CustomerPortal.FinancialService.Models;

namespace CustomerPortal.FinancialService.Repositories;

public interface IInvoiceRepository
{
    Task<IEnumerable<Invoice>> GetAllAsync();
    Task<Invoice?> GetByIdAsync(int id);
    Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber);
    Task<IEnumerable<Invoice>> GetByCompanyIdAsync(int companyId);
    Task<IEnumerable<Invoice>> GetByStatusAsync(string status);
    Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync();
    Task<IEnumerable<Invoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<Invoice> CreateAsync(Invoice invoice);
    Task<Invoice> UpdateAsync(Invoice invoice);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

public interface IPaymentRepository
{
    Task<IEnumerable<Payment>> GetAllAsync();
    Task<Payment?> GetByIdAsync(int id);
    Task<IEnumerable<Payment>> GetByInvoiceIdAsync(int invoiceId);
    Task<IEnumerable<Payment>> GetByTransactionIdAsync(string transactionId);
    Task<IEnumerable<Payment>> GetByStatusAsync(string status);
    Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<Payment> CreateAsync(Payment payment);
    Task<Payment> UpdateAsync(Payment payment);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllAsync();
    Task<Company?> GetByIdAsync(int id);
    Task<Company?> GetByCompanyCodeAsync(string companyCode);
    Task<IEnumerable<Company>> GetActiveCompaniesAsync();
    Task<Company> CreateAsync(Company company);
    Task<Company> UpdateAsync(Company company);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

public interface IServiceRepository
{
    Task<IEnumerable<Service>> GetAllAsync();
    Task<Service?> GetByIdAsync(int id);
    Task<Service?> GetByServiceCodeAsync(string serviceCode);
    Task<IEnumerable<Service>> GetByCategoryAsync(string category);
    Task<IEnumerable<Service>> GetActiveServicesAsync();
    Task<Service> CreateAsync(Service service);
    Task<Service> UpdateAsync(Service service);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

public interface ITaxRateRepository
{
    Task<IEnumerable<TaxRate>> GetAllAsync();
    Task<TaxRate?> GetByIdAsync(int id);
    Task<IEnumerable<TaxRate>> GetByCountryIdAsync(int countryId);
    Task<IEnumerable<TaxRate>> GetByTaxTypeAsync(string taxType);
    Task<IEnumerable<TaxRate>> GetActiveRatesAsync();
    Task<decimal> GetApplicableTaxRateAsync(int countryId, int serviceId);
    Task<TaxRate> CreateAsync(TaxRate taxRate);
    Task<TaxRate> UpdateAsync(TaxRate taxRate);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

public interface IFinancialReportingRepository
{
    Task<RevenueReportDto> GetRevenueReportAsync(DateTime startDate, DateTime endDate, string groupBy);
    Task<FinancialDashboardDto> GetFinancialDashboardAsync(string period);
    Task<ProfitLossReportDto> GetProfitLossReportAsync(DateTime startDate, DateTime endDate);
    Task<CashFlowReportDto> GetCashFlowReportAsync(DateTime startDate, DateTime endDate);
    Task<AccountsReceivableDto> GetAccountsReceivableAsync(DateTime asOfDate);
    Task<TaxCalculationDto> CalculateTaxAsync(decimal amount, int countryId, int serviceId);
}