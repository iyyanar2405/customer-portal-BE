using AutoMapper;
using CustomerPortal.ContractsService.Repositories;

namespace CustomerPortal.ContractsService.GraphQL;

public class Query
{
    public async Task<IEnumerable<ContractGraphQLType>> GetContracts(
        [Service] IContractRepository contractRepository,
        [Service] IMapper mapper)
    {
        var contracts = await contractRepository.GetAllAsync();
        return mapper.Map<IEnumerable<ContractGraphQLType>>(contracts);
    }

    public async Task<ContractGraphQLType?> GetContract(
        int id,
        [Service] IContractRepository contractRepository,
        [Service] IMapper mapper)
    {
        var contract = await contractRepository.GetByIdAsync(id);
        return contract != null ? mapper.Map<ContractGraphQLType>(contract) : null;
    }

    public async Task<ContractGraphQLType?> GetContractByNumber(
        string contractNumber,
        [Service] IContractRepository contractRepository,
        [Service] IMapper mapper)
    {
        var contract = await contractRepository.GetByContractNumberAsync(contractNumber);
        return contract != null ? mapper.Map<ContractGraphQLType>(contract) : null;
    }

    public async Task<IEnumerable<ContractGraphQLType>> GetContractsByCompany(
        int companyId,
        [Service] IContractRepository contractRepository,
        [Service] IMapper mapper)
    {
        var contracts = await contractRepository.GetByCompanyIdAsync(companyId);
        return mapper.Map<IEnumerable<ContractGraphQLType>>(contracts);
    }

    public async Task<IEnumerable<ContractGraphQLType>> GetContractsByStatus(
        string status,
        [Service] IContractRepository contractRepository,
        [Service] IMapper mapper)
    {
        var contracts = await contractRepository.GetByStatusAsync(status);
        return mapper.Map<IEnumerable<ContractGraphQLType>>(contracts);
    }

    public async Task<IEnumerable<ExpiringContractGraphQLType>> GetExpiringContracts(
        int withinDays,
        [Service] IContractRepository contractRepository,
        [Service] IMapper mapper)
    {
        var contracts = await contractRepository.GetExpiringContractsAsync(withinDays);
        var expiringContracts = contracts.Select(c => new ExpiringContractGraphQLType
        {
            Id = c.Id,
            ContractNumber = c.ContractNumber,
            Company = mapper.Map<CompanyGraphQLType>(c.Company),
            ContractType = c.ContractType,
            EndDate = c.EndDate,
            DaysUntilExpiry = (int)(c.EndDate - DateTime.UtcNow).TotalDays,
            RenewalRequired = c.RenewalDate.HasValue,
            AutoRenewal = false, // This would need to be implemented based on business logic
            RenewalNotificationSent = false // This would need to be tracked separately
        });
        
        return expiringContracts;
    }

    public async Task<IEnumerable<ContractRenewalScheduleGraphQLType>> GetContractRenewalSchedule(
        DateTime startDate,
        DateTime endDate,
        [Service] IContractRenewalRepository renewalRepository)
    {
        var renewals = await renewalRepository.GetRenewalScheduleAsync(startDate, endDate);
        return renewals.Select(r => new ContractRenewalScheduleGraphQLType
        {
            ContractId = r.ContractId,
            ContractNumber = r.Contract?.ContractNumber ?? "",
            CompanyName = r.Contract?.Company?.CompanyName ?? "",
            ContractType = r.Contract?.ContractType ?? "",
            CurrentEndDate = r.Contract?.EndDate ?? DateTime.MinValue,
            RenewalDate = r.Contract?.RenewalDate,
            Status = r.Status,
            DaysUntilRenewal = r.Contract?.RenewalDate.HasValue == true 
                ? (int)(r.Contract.RenewalDate.Value - DateTime.UtcNow).TotalDays 
                : 0,
            AutoRenewal = r.AutoRenewal,
            RenewalValue = r.ProposedValue
        });
    }

    public async Task<IEnumerable<ContractTermGraphQLType>> GetContractTerms(
        int contractId,
        [Service] IContractTermRepository termRepository,
        [Service] IMapper mapper)
    {
        var terms = await termRepository.GetByContractIdAsync(contractId);
        return mapper.Map<IEnumerable<ContractTermGraphQLType>>(terms);
    }

    public async Task<IEnumerable<ContractAmendmentGraphQLType>> GetContractAmendments(
        int contractId,
        [Service] IContractAmendmentRepository amendmentRepository,
        [Service] IMapper mapper)
    {
        var amendments = await amendmentRepository.GetByContractIdAsync(contractId);
        return mapper.Map<IEnumerable<ContractAmendmentGraphQLType>>(amendments);
    }

    // Company queries
    public async Task<IEnumerable<CompanyGraphQLType>> GetCompanies(
        [Service] ICompanyRepository companyRepository,
        [Service] IMapper mapper)
    {
        var companies = await companyRepository.GetAllAsync();
        return mapper.Map<IEnumerable<CompanyGraphQLType>>(companies);
    }

    public async Task<CompanyGraphQLType?> GetCompany(
        int id,
        [Service] ICompanyRepository companyRepository,
        [Service] IMapper mapper)
    {
        var company = await companyRepository.GetByIdAsync(id);
        return company != null ? mapper.Map<CompanyGraphQLType>(company) : null;
    }

    // Service queries
    public async Task<IEnumerable<ServiceGraphQLType>> GetServices(
        [Service] IServiceRepository serviceRepository,
        [Service] IMapper mapper)
    {
        var services = await serviceRepository.GetAllAsync();
        return mapper.Map<IEnumerable<ServiceGraphQLType>>(services);
    }

    public async Task<ServiceGraphQLType?> GetService(
        int id,
        [Service] IServiceRepository serviceRepository,
        [Service] IMapper mapper)
    {
        var service = await serviceRepository.GetByIdAsync(id);
        return service != null ? mapper.Map<ServiceGraphQLType>(service) : null;
    }

    // Site queries
    public async Task<IEnumerable<SiteGraphQLType>> GetSites(
        [Service] ISiteRepository siteRepository,
        [Service] IMapper mapper)
    {
        var sites = await siteRepository.GetAllAsync();
        return mapper.Map<IEnumerable<SiteGraphQLType>>(sites);
    }

    public async Task<IEnumerable<SiteGraphQLType>> GetSitesByCompany(
        int companyId,
        [Service] ISiteRepository siteRepository,
        [Service] IMapper mapper)
    {
        var sites = await siteRepository.GetByCompanyIdAsync(companyId);
        return mapper.Map<IEnumerable<SiteGraphQLType>>(sites);
    }
}