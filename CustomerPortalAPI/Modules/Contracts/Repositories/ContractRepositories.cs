using Microsoft.EntityFrameworkCore;
using CustomerPortalAPI.Data;
using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Contracts.Entities;

namespace CustomerPortalAPI.Modules.Contracts.Repositories
{
    public class ContractRepository : Repository<Contract>, IContractRepository
    {
        public ContractRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Contract>> GetContractsByCompanyAsync(int companyId)
        {
            return await _dbSet.Where(c => c.CompanyId == companyId).ToListAsync();
        }

        public async Task<IEnumerable<Contract>> GetContractsByStatusAsync(string status)
        {
            return await _dbSet.Where(c => c.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<Contract>> GetContractsByTypeAsync(string contractType)
        {
            return await _dbSet.Where(c => c.ContractType == contractType).ToListAsync();
        }

        public async Task<IEnumerable<Contract>> GetActiveContractsAsync()
        {
            return await _dbSet.Where(c => c.IsActive && c.Status == "Active").ToListAsync();
        }

        public async Task<IEnumerable<Contract>> GetExpiredContractsAsync()
        {
            var today = DateTime.Today;
            return await _dbSet.Where(c => c.EndDate < today).ToListAsync();
        }

        public async Task<IEnumerable<Contract>> GetExpiringContractsAsync(int daysAhead)
        {
            var today = DateTime.Today;
            var futureDate = today.AddDays(daysAhead);
            return await _dbSet.Where(c => c.EndDate >= today && c.EndDate <= futureDate).ToListAsync();
        }

        public async Task<Contract?> GetContractWithDetailsAsync(int contractId)
        {
            return await _dbSet
                .Include(c => c.Company)
                .Include(c => c.ContractServices)
                .Include(c => c.ContractSites)
                .FirstOrDefaultAsync(c => c.Id == contractId);
        }

        public async Task<Contract?> GetByContractNumberAsync(string contractNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.ContractNumber == contractNumber);
        }

        public async Task<IEnumerable<Contract>> GetContractsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(c => c.StartDate >= startDate && c.StartDate <= endDate).ToListAsync();
        }

        public async Task<IEnumerable<Contract>> SearchContractsAsync(string searchTerm)
        {
            return await _dbSet.Where(c => 
                c.ContractName.Contains(searchTerm) ||
                c.ContractNumber!.Contains(searchTerm) ||
                c.Description!.Contains(searchTerm) ||
                c.ContractManager!.Contains(searchTerm)).ToListAsync();
        }

        public async Task UpdateContractStatusAsync(int contractId, string status, int modifiedBy)
        {
            var contract = await GetByIdAsync(contractId);
            if (contract != null)
            {
                contract.Status = status;
                contract.ModifiedBy = modifiedBy;
                contract.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(contract);
            }
        }

        public async Task<int> GetContractCountByStatusAsync(string status)
        {
            return await _dbSet.CountAsync(c => c.Status == status);
        }

        public async Task<decimal> GetTotalContractValueAsync()
        {
            return await _dbSet.Where(c => c.ContractValue.HasValue).SumAsync(c => c.ContractValue.Value);
        }

        public async Task<decimal> GetContractValueByCompanyAsync(int companyId)
        {
            return await _dbSet.Where(c => c.CompanyId == companyId && c.ContractValue.HasValue)
                .SumAsync(c => c.ContractValue.Value);
        }
    }

    public class ContractServiceRepository : Repository<ContractService>, IContractServiceRepository
    {
        public ContractServiceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ContractService>> GetServicesByContractAsync(int contractId)
        {
            return await _dbSet.Where(cs => cs.ContractId == contractId).ToListAsync();
        }

        public async Task<IEnumerable<ContractService>> GetContractsByServiceAsync(int serviceId)
        {
            return await _dbSet.Where(cs => cs.ServiceId == serviceId).ToListAsync();
        }

        public async Task<IEnumerable<ContractService>> GetContractServicesByStatusAsync(string status)
        {
            return await _dbSet.Where(cs => cs.Status == status).ToListAsync();
        }

        public async Task<ContractService?> GetContractServiceAsync(int contractId, int serviceId)
        {
            return await _dbSet.FirstOrDefaultAsync(cs => cs.ContractId == contractId && cs.ServiceId == serviceId);
        }

        public async Task AddServiceToContractAsync(int contractId, int serviceId, decimal? servicePrice, string? currency, string? description, int createdBy)
        {
            var existing = await GetContractServiceAsync(contractId, serviceId);
            if (existing == null)
            {
                var contractService = new ContractService
                {
                    ContractId = contractId,
                    ServiceId = serviceId,
                    ServicePrice = servicePrice,
                    Currency = currency,
                    ServiceDescription = description,
                    Status = "Active",
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.UtcNow
                };
                await AddAsync(contractService);
            }
        }

        public async Task RemoveServiceFromContractAsync(int contractId, int serviceId)
        {
            var contractService = await GetContractServiceAsync(contractId, serviceId);
            if (contractService != null)
            {
                await DeleteAsync(contractService);
            }
        }

        public async Task UpdateContractServiceAsync(int contractServiceId, decimal? servicePrice, string? description, string? status, int modifiedBy)
        {
            var contractService = await GetByIdAsync(contractServiceId);
            if (contractService != null)
            {
                contractService.ServicePrice = servicePrice;
                contractService.ServiceDescription = description;
                contractService.Status = status;
                contractService.ModifiedBy = modifiedBy;
                contractService.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(contractService);
            }
        }

        public async Task<decimal> GetTotalServiceValueByContractAsync(int contractId)
        {
            return await _dbSet.Where(cs => cs.ContractId == contractId && cs.ServicePrice.HasValue)
                .SumAsync(cs => cs.ServicePrice.Value);
        }
    }

    public class ContractSiteRepository : Repository<ContractSite>, IContractSiteRepository
    {
        public ContractSiteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ContractSite>> GetSitesByContractAsync(int contractId)
        {
            return await _dbSet.Where(cs => cs.ContractId == contractId).ToListAsync();
        }

        public async Task<IEnumerable<ContractSite>> GetContractsBySiteAsync(int siteId)
        {
            return await _dbSet.Where(cs => cs.SiteId == siteId).ToListAsync();
        }

        public async Task<IEnumerable<ContractSite>> GetContractSitesByStatusAsync(string status)
        {
            return await _dbSet.Where(cs => cs.Status == status).ToListAsync();
        }

        public async Task<ContractSite?> GetContractSiteAsync(int contractId, int siteId)
        {
            return await _dbSet.FirstOrDefaultAsync(cs => cs.ContractId == contractId && cs.SiteId == siteId);
        }

        public async Task AddSiteToContractAsync(int contractId, int siteId, string? siteSpecificTerms, int createdBy)
        {
            var existing = await GetContractSiteAsync(contractId, siteId);
            if (existing == null)
            {
                var contractSite = new ContractSite
                {
                    ContractId = contractId,
                    SiteId = siteId,
                    SiteSpecificTerms = siteSpecificTerms,
                    Status = "Active",
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.UtcNow
                };
                await AddAsync(contractSite);
            }
        }

        public async Task RemoveSiteFromContractAsync(int contractId, int siteId)
        {
            var contractSite = await GetContractSiteAsync(contractId, siteId);
            if (contractSite != null)
            {
                await DeleteAsync(contractSite);
            }
        }

        public async Task UpdateContractSiteAsync(int contractSiteId, string? siteSpecificTerms, string? status, int modifiedBy)
        {
            var contractSite = await GetByIdAsync(contractSiteId);
            if (contractSite != null)
            {
                contractSite.SiteSpecificTerms = siteSpecificTerms;
                contractSite.Status = status;
                contractSite.ModifiedBy = modifiedBy;
                contractSite.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(contractSite);
            }
        }
    }
}