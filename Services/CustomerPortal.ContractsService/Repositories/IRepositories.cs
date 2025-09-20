using CustomerPortal.ContractsService.Entities;

namespace CustomerPortal.ContractsService.Repositories;

public interface IContractRepository
{
    Task<IEnumerable<Contract>> GetAllAsync();
    Task<Contract?> GetByIdAsync(int id);
    Task<Contract?> GetByContractNumberAsync(string contractNumber);
    Task<IEnumerable<Contract>> GetByCompanyIdAsync(int companyId);
    Task<IEnumerable<Contract>> GetByStatusAsync(string status);
    Task<IEnumerable<Contract>> GetExpiringContractsAsync(int withinDays);
    Task<Contract> CreateAsync(Contract contract);
    Task<Contract> UpdateAsync(Contract contract);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateStatusAsync(int contractId, string status, string? reason = null);
}

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllAsync();
    Task<Company?> GetByIdAsync(int id);
    Task<Company?> GetByCompanyCodeAsync(string companyCode);
    Task<Company> CreateAsync(Company company);
    Task<Company> UpdateAsync(Company company);
    Task<bool> DeleteAsync(int id);
}

public interface IServiceRepository
{
    Task<IEnumerable<Service>> GetAllAsync();
    Task<Service?> GetByIdAsync(int id);
    Task<Service?> GetByServiceCodeAsync(string serviceCode);
    Task<IEnumerable<Service>> GetByCategoryAsync(string category);
    Task<Service> CreateAsync(Service service);
    Task<Service> UpdateAsync(Service service);
    Task<bool> DeleteAsync(int id);
}

public interface ISiteRepository
{
    Task<IEnumerable<Site>> GetAllAsync();
    Task<Site?> GetByIdAsync(int id);
    Task<Site?> GetBySiteCodeAsync(string siteCode);
    Task<IEnumerable<Site>> GetByCompanyIdAsync(int companyId);
    Task<Site> CreateAsync(Site site);
    Task<Site> UpdateAsync(Site site);
    Task<bool> DeleteAsync(int id);
}

public interface IContractTermRepository
{
    Task<IEnumerable<ContractTerm>> GetByContractIdAsync(int contractId);
    Task<ContractTerm?> GetByIdAsync(int id);
    Task<ContractTerm> CreateAsync(ContractTerm contractTerm);
    Task<ContractTerm> UpdateAsync(ContractTerm contractTerm);
    Task<bool> DeleteAsync(int id);
}

public interface IContractAmendmentRepository
{
    Task<IEnumerable<ContractAmendment>> GetByContractIdAsync(int contractId);
    Task<ContractAmendment?> GetByIdAsync(int id);
    Task<ContractAmendment?> GetByAmendmentNumberAsync(string amendmentNumber);
    Task<ContractAmendment> CreateAsync(ContractAmendment amendment);
    Task<ContractAmendment> UpdateAsync(ContractAmendment amendment);
    Task<bool> DeleteAsync(int id);
}

public interface IContractRenewalRepository
{
    Task<IEnumerable<ContractRenewal>> GetByContractIdAsync(int contractId);
    Task<ContractRenewal?> GetByIdAsync(int id);
    Task<ContractRenewal?> GetByRenewalNumberAsync(string renewalNumber);
    Task<IEnumerable<ContractRenewal>> GetRenewalScheduleAsync(DateTime startDate, DateTime endDate);
    Task<ContractRenewal> CreateAsync(ContractRenewal renewal);
    Task<ContractRenewal> UpdateAsync(ContractRenewal renewal);
    Task<bool> DeleteAsync(int id);
}