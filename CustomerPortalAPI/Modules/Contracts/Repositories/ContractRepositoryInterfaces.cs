using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Contracts.Entities;

namespace CustomerPortalAPI.Modules.Contracts.Repositories
{
    public interface IContractRepository : IRepository<Contract>
    {
        Task<IEnumerable<Contract>> GetContractsByCompanyAsync(int companyId);
        Task<IEnumerable<Contract>> GetContractsByStatusAsync(string status);
        Task<IEnumerable<Contract>> GetContractsByTypeAsync(string contractType);
        Task<IEnumerable<Contract>> GetActiveContractsAsync();
        Task<IEnumerable<Contract>> GetExpiredContractsAsync();
        Task<IEnumerable<Contract>> GetExpiringContractsAsync(int daysAhead);
        Task<Contract?> GetContractWithDetailsAsync(int contractId);
        Task<Contract?> GetByContractNumberAsync(string contractNumber);
        Task<IEnumerable<Contract>> GetContractsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Contract>> SearchContractsAsync(string searchTerm);
        Task UpdateContractStatusAsync(int contractId, string status, int modifiedBy);
        Task<int> GetContractCountByStatusAsync(string status);
        Task<decimal> GetTotalContractValueAsync();
        Task<decimal> GetContractValueByCompanyAsync(int companyId);
    }

    public interface IContractServiceRepository : IRepository<ContractService>
    {
        Task<IEnumerable<ContractService>> GetServicesByContractAsync(int contractId);
        Task<IEnumerable<ContractService>> GetContractsByServiceAsync(int serviceId);
        Task<IEnumerable<ContractService>> GetContractServicesByStatusAsync(string status);
        Task<ContractService?> GetContractServiceAsync(int contractId, int serviceId);
        Task AddServiceToContractAsync(int contractId, int serviceId, decimal? servicePrice, string? currency, string? description, int createdBy);
        Task RemoveServiceFromContractAsync(int contractId, int serviceId);
        Task UpdateContractServiceAsync(int contractServiceId, decimal? servicePrice, string? description, string? status, int modifiedBy);
        Task<decimal> GetTotalServiceValueByContractAsync(int contractId);
    }

    public interface IContractSiteRepository : IRepository<ContractSite>
    {
        Task<IEnumerable<ContractSite>> GetSitesByContractAsync(int contractId);
        Task<IEnumerable<ContractSite>> GetContractsBySiteAsync(int siteId);
        Task<IEnumerable<ContractSite>> GetContractSitesByStatusAsync(string status);
        Task<ContractSite?> GetContractSiteAsync(int contractId, int siteId);
        Task AddSiteToContractAsync(int contractId, int siteId, string? siteSpecificTerms, int createdBy);
        Task RemoveSiteFromContractAsync(int contractId, int siteId);
        Task UpdateContractSiteAsync(int contractSiteId, string? siteSpecificTerms, string? status, int modifiedBy);
    }
}