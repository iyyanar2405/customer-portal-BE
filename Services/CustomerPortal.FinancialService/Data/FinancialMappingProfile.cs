using AutoMapper;
using CustomerPortal.FinancialService.Models;
using CustomerPortal.FinancialService.GraphQL;

namespace CustomerPortal.FinancialService.Data;

public class FinancialMappingProfile : Profile
{
    public FinancialMappingProfile()
    {
        // Entity to GraphQL Type mappings
        CreateMap<Company, CompanyGraphQLType>();
        CreateMap<Service, ServiceGraphQLType>();
        CreateMap<Contract, ContractGraphQLType>();
        CreateMap<Audit, AuditGraphQLType>();
        CreateMap<Invoice, InvoiceGraphQLType>();
        CreateMap<InvoiceItem, InvoiceItemGraphQLType>();
        CreateMap<Payment, PaymentGraphQLType>();
        CreateMap<PaymentMethod, PaymentMethodGraphQLType>();
        CreateMap<TaxRate, TaxRateGraphQLType>()
            .ForMember(dest => dest.TaxRate, opt => opt.MapFrom(src => src.Rate));
        CreateMap<Country, CountryGraphQLType>();
        CreateMap<InvoiceTax, InvoiceTaxGraphQLType>();

        // DTO to GraphQL Type mappings for reporting
        CreateMap<RevenueReportDto, RevenueReportGraphQLType>();
        CreateMap<RevenueByServiceDto, RevenueByServiceGraphQLType>();
        CreateMap<RevenueByCompanyDto, RevenueByCompanyGraphQLType>();
        CreateMap<RevenueByMonthDto, RevenueByMonthGraphQLType>();
        CreateMap<ProjectedRevenueDto, ProjectedRevenueGraphQLType>();

        CreateMap<FinancialDashboardDto, FinancialDashboardGraphQLType>();
        CreateMap<MonthlyRevenueDto, MonthlyRevenueGraphQLType>();
        CreateMap<TopCustomerDto, TopCustomerGraphQLType>();
        CreateMap<PaymentMethodStatsDto, PaymentMethodStatsGraphQLType>();
        CreateMap<AgingReportDto, AgingReportGraphQLType>();

        CreateMap<ProfitLossReportDto, ProfitLossReportGraphQLType>();
        CreateMap<RevenueByServiceProfitDto, RevenueByServiceProfitGraphQLType>();
        CreateMap<MonthlyBreakdownDto, MonthlyBreakdownGraphQLType>();

        CreateMap<CashFlowReportDto, CashFlowReportGraphQLType>();
        CreateMap<CashFlowItemDto, CashFlowItemGraphQLType>();
        CreateMap<MonthlyFlowDto, MonthlyFlowGraphQLType>();
        CreateMap<ProjectedFlowDto, ProjectedFlowGraphQLType>();

        CreateMap<AccountsReceivableDto, AccountsReceivableGraphQLType>();
        CreateMap<AgingBucketDto, AgingBucketGraphQLType>();
        CreateMap<TopDebtorDto, TopDebtorGraphQLType>();
        CreateMap<ReceivableByCompanyDto, ReceivableByCompanyGraphQLType>();
        CreateMap<ReceivableTrendDto, ReceivableTrendGraphQLType>();

        CreateMap<TaxCalculationDto, TaxCalculationGraphQLType>();
        CreateMap<TaxBreakdownDto, TaxBreakdownGraphQLType>();
        CreateMap<CurrencyConversionDto, CurrencyConversionGraphQLType>();

        // Reverse mappings for input scenarios (if needed)
        CreateMap<CompanyGraphQLType, Company>();
        CreateMap<ServiceGraphQLType, Service>();
        CreateMap<InvoiceGraphQLType, Invoice>();
        CreateMap<PaymentGraphQLType, Payment>();
    }
}