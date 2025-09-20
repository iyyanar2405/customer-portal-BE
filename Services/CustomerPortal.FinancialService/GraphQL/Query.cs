using AutoMapper;
using CustomerPortal.FinancialService.Repositories;

namespace CustomerPortal.FinancialService.GraphQL;

public class Query
{
    private readonly IMapper _mapper;

    public Query(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<InvoiceGraphQLType>> GetInvoicesAsync(
        [Service] IInvoiceRepository invoiceRepository)
    {
        var invoices = await invoiceRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<InvoiceGraphQLType>>(invoices);
    }

    public async Task<InvoiceGraphQLType?> GetInvoiceByIdAsync(
        int id,
        [Service] IInvoiceRepository invoiceRepository)
    {
        var invoice = await invoiceRepository.GetByIdAsync(id);
        return _mapper.Map<InvoiceGraphQLType>(invoice);
    }

    public async Task<InvoiceGraphQLType?> GetInvoiceByNumberAsync(
        string invoiceNumber,
        [Service] IInvoiceRepository invoiceRepository)
    {
        var invoice = await invoiceRepository.GetByInvoiceNumberAsync(invoiceNumber);
        return _mapper.Map<InvoiceGraphQLType>(invoice);
    }

    public async Task<IEnumerable<InvoiceGraphQLType>> GetInvoicesByCompanyAsync(
        int companyId,
        [Service] IInvoiceRepository invoiceRepository)
    {
        var invoices = await invoiceRepository.GetByCompanyIdAsync(companyId);
        return _mapper.Map<IEnumerable<InvoiceGraphQLType>>(invoices);
    }

    public async Task<IEnumerable<InvoiceGraphQLType>> GetInvoicesByStatusAsync(
        string status,
        [Service] IInvoiceRepository invoiceRepository)
    {
        var invoices = await invoiceRepository.GetByStatusAsync(status);
        return _mapper.Map<IEnumerable<InvoiceGraphQLType>>(invoices);
    }

    public async Task<IEnumerable<InvoiceGraphQLType>> GetOverdueInvoicesAsync(
        [Service] IInvoiceRepository invoiceRepository)
    {
        var invoices = await invoiceRepository.GetOverdueInvoicesAsync();
        return _mapper.Map<IEnumerable<InvoiceGraphQLType>>(invoices);
    }

    public async Task<IEnumerable<PaymentGraphQLType>> GetPaymentsAsync(
        [Service] IPaymentRepository paymentRepository)
    {
        var payments = await paymentRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PaymentGraphQLType>>(payments);
    }

    public async Task<PaymentGraphQLType?> GetPaymentByIdAsync(
        int id,
        [Service] IPaymentRepository paymentRepository)
    {
        var payment = await paymentRepository.GetByIdAsync(id);
        return _mapper.Map<PaymentGraphQLType>(payment);
    }

    public async Task<IEnumerable<PaymentGraphQLType>> GetPaymentsByInvoiceAsync(
        int invoiceId,
        [Service] IPaymentRepository paymentRepository)
    {
        var payments = await paymentRepository.GetByInvoiceIdAsync(invoiceId);
        return _mapper.Map<IEnumerable<PaymentGraphQLType>>(payments);
    }

    public async Task<IEnumerable<PaymentGraphQLType>> GetPaymentsByStatusAsync(
        string status,
        [Service] IPaymentRepository paymentRepository)
    {
        var payments = await paymentRepository.GetByStatusAsync(status);
        return _mapper.Map<IEnumerable<PaymentGraphQLType>>(payments);
    }

    public async Task<IEnumerable<CompanyGraphQLType>> GetCompaniesAsync(
        [Service] ICompanyRepository companyRepository)
    {
        var companies = await companyRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CompanyGraphQLType>>(companies);
    }

    public async Task<CompanyGraphQLType?> GetCompanyByIdAsync(
        int id,
        [Service] ICompanyRepository companyRepository)
    {
        var company = await companyRepository.GetByIdAsync(id);
        return _mapper.Map<CompanyGraphQLType>(company);
    }

    public async Task<IEnumerable<ServiceGraphQLType>> GetServicesAsync(
        [Service] IServiceRepository serviceRepository)
    {
        var services = await serviceRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ServiceGraphQLType>>(services);
    }

    public async Task<ServiceGraphQLType?> GetServiceByIdAsync(
        int id,
        [Service] IServiceRepository serviceRepository)
    {
        var service = await serviceRepository.GetByIdAsync(id);
        return _mapper.Map<ServiceGraphQLType>(service);
    }

    public async Task<IEnumerable<ServiceGraphQLType>> GetServicesByCategoryAsync(
        string category,
        [Service] IServiceRepository serviceRepository)
    {
        var services = await serviceRepository.GetByCategoryAsync(category);
        return _mapper.Map<IEnumerable<ServiceGraphQLType>>(services);
    }

    public async Task<IEnumerable<TaxRateGraphQLType>> GetTaxRatesAsync(
        int? countryId,
        bool? isActive,
        [Service] ITaxRateRepository taxRateRepository)
    {
        IEnumerable<Models.TaxRate> taxRates;

        if (countryId.HasValue)
        {
            taxRates = await taxRateRepository.GetByCountryIdAsync(countryId.Value);
        }
        else if (isActive.HasValue && isActive.Value)
        {
            taxRates = await taxRateRepository.GetActiveRatesAsync();
        }
        else
        {
            taxRates = await taxRateRepository.GetAllAsync();
        }

        return _mapper.Map<IEnumerable<TaxRateGraphQLType>>(taxRates);
    }

    public async Task<TaxCalculationGraphQLType> GetTaxCalculationAsync(
        decimal amount,
        int countryId,
        int serviceId,
        [Service] IFinancialReportingRepository reportingRepository)
    {
        var calculation = await reportingRepository.CalculateTaxAsync(amount, countryId, serviceId);
        return _mapper.Map<TaxCalculationGraphQLType>(calculation);
    }

    // Financial Reporting Queries
    public async Task<RevenueReportGraphQLType> GetRevenueReportAsync(
        DateTime startDate,
        DateTime endDate,
        string groupBy,
        [Service] IFinancialReportingRepository reportingRepository)
    {
        var report = await reportingRepository.GetRevenueReportAsync(startDate, endDate, groupBy);
        return _mapper.Map<RevenueReportGraphQLType>(report);
    }

    public async Task<FinancialDashboardGraphQLType> GetFinancialDashboardAsync(
        string period,
        [Service] IFinancialReportingRepository reportingRepository)
    {
        var dashboard = await reportingRepository.GetFinancialDashboardAsync(period);
        return _mapper.Map<FinancialDashboardGraphQLType>(dashboard);
    }

    public async Task<ProfitLossReportGraphQLType> GetProfitLossReportAsync(
        DateTime startDate,
        DateTime endDate,
        [Service] IFinancialReportingRepository reportingRepository)
    {
        var report = await reportingRepository.GetProfitLossReportAsync(startDate, endDate);
        return _mapper.Map<ProfitLossReportGraphQLType>(report);
    }

    public async Task<CashFlowReportGraphQLType> GetCashFlowReportAsync(
        DateTime startDate,
        DateTime endDate,
        [Service] IFinancialReportingRepository reportingRepository)
    {
        var report = await reportingRepository.GetCashFlowReportAsync(startDate, endDate);
        return _mapper.Map<CashFlowReportGraphQLType>(report);
    }

    public async Task<AccountsReceivableGraphQLType> GetAccountsReceivableAsync(
        DateTime asOfDate,
        [Service] IFinancialReportingRepository reportingRepository)
    {
        var report = await reportingRepository.GetAccountsReceivableAsync(asOfDate);
        return _mapper.Map<AccountsReceivableGraphQLType>(report);
    }

    // Alias methods for compatibility with documentation
    public async Task<IEnumerable<InvoiceGraphQLType>> Invoices(
        [Service] IInvoiceRepository invoiceRepository) =>
        await GetInvoicesAsync(invoiceRepository);

    public async Task<IEnumerable<PaymentGraphQLType>> Payments(
        [Service] IPaymentRepository paymentRepository) =>
        await GetPaymentsAsync(paymentRepository);

    public async Task<RevenueReportGraphQLType> RevenueReport(
        DateTime startDate,
        DateTime endDate,
        string groupBy,
        [Service] IFinancialReportingRepository reportingRepository) =>
        await GetRevenueReportAsync(startDate, endDate, groupBy, reportingRepository);

    public async Task<FinancialDashboardGraphQLType> FinancialDashboard(
        string period,
        [Service] IFinancialReportingRepository reportingRepository) =>
        await GetFinancialDashboardAsync(period, reportingRepository);

    public async Task<ProfitLossReportGraphQLType> ProfitLossReport(
        DateTime startDate,
        DateTime endDate,
        [Service] IFinancialReportingRepository reportingRepository) =>
        await GetProfitLossReportAsync(startDate, endDate, reportingRepository);

    public async Task<CashFlowReportGraphQLType> CashFlowReport(
        DateTime startDate,
        DateTime endDate,
        [Service] IFinancialReportingRepository reportingRepository) =>
        await GetCashFlowReportAsync(startDate, endDate, reportingRepository);

    public async Task<AccountsReceivableGraphQLType> AccountsReceivable(
        DateTime asOfDate,
        [Service] IFinancialReportingRepository reportingRepository) =>
        await GetAccountsReceivableAsync(asOfDate, reportingRepository);

    public async Task<TaxCalculationGraphQLType> CalculateTax(
        decimal amount,
        int countryId,
        int serviceId,
        [Service] IFinancialReportingRepository reportingRepository) =>
        await GetTaxCalculationAsync(amount, countryId, serviceId, reportingRepository);

    public async Task<IEnumerable<TaxRateGraphQLType>> TaxRates(
        int? countryId,
        bool? isActive,
        [Service] ITaxRateRepository taxRateRepository) =>
        await GetTaxRatesAsync(countryId, isActive, taxRateRepository);
}