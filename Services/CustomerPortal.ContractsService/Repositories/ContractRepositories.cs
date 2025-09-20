using Microsoft.EntityFrameworkCore;
using CustomerPortal.ContractsService.Data;
using CustomerPortal.ContractsService.Entities;

namespace CustomerPortal.ContractsService.Repositories;

public class ContractRepository : IContractRepository
{
    private readonly ContractsDbContext _context;

    public ContractRepository(ContractsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Contract>> GetAllAsync()
    {
        return await _context.Contracts
            .Include(c => c.Company)
            .Include(c => c.Services).ThenInclude(cs => cs.Service)
            .Include(c => c.Sites).ThenInclude(cs => cs.Site)
            .Include(c => c.Terms)
            .Include(c => c.Amendments)
            .Include(c => c.Renewals)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Contract?> GetByIdAsync(int id)
    {
        return await _context.Contracts
            .Include(c => c.Company)
            .Include(c => c.Services).ThenInclude(cs => cs.Service)
            .Include(c => c.Sites).ThenInclude(cs => cs.Site)
            .Include(c => c.Terms)
            .Include(c => c.Amendments)
            .Include(c => c.Renewals)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Contract?> GetByContractNumberAsync(string contractNumber)
    {
        return await _context.Contracts
            .Include(c => c.Company)
            .Include(c => c.Services).ThenInclude(cs => cs.Service)
            .Include(c => c.Sites).ThenInclude(cs => cs.Site)
            .Include(c => c.Terms)
            .Include(c => c.Amendments)
            .Include(c => c.Renewals)
            .FirstOrDefaultAsync(c => c.ContractNumber == contractNumber);
    }

    public async Task<IEnumerable<Contract>> GetByCompanyIdAsync(int companyId)
    {
        return await _context.Contracts
            .Include(c => c.Company)
            .Include(c => c.Services).ThenInclude(cs => cs.Service)
            .Include(c => c.Sites).ThenInclude(cs => cs.Site)
            .Include(c => c.Terms)
            .Include(c => c.Amendments)
            .Include(c => c.Renewals)
            .Where(c => c.CompanyId == companyId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Contract>> GetByStatusAsync(string status)
    {
        return await _context.Contracts
            .Include(c => c.Company)
            .Include(c => c.Services).ThenInclude(cs => cs.Service)
            .Include(c => c.Sites).ThenInclude(cs => cs.Site)
            .Where(c => c.Status == status)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Contract>> GetExpiringContractsAsync(int withinDays)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(withinDays);
        return await _context.Contracts
            .Include(c => c.Company)
            .Where(c => c.EndDate <= cutoffDate && c.Status == "ACTIVE")
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Contract> CreateAsync(Contract contract)
    {
        contract.CreatedDate = DateTime.UtcNow;
        contract.ModifiedDate = DateTime.UtcNow;
        
        _context.Contracts.Add(contract);
        await _context.SaveChangesAsync();
        return contract;
    }

    public async Task<Contract> UpdateAsync(Contract contract)
    {
        contract.ModifiedDate = DateTime.UtcNow;
        
        _context.Entry(contract).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return contract;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var contract = await _context.Contracts.FindAsync(id);
        if (contract == null) return false;

        _context.Contracts.Remove(contract);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateStatusAsync(int contractId, string status, string? reason = null)
    {
        var contract = await _context.Contracts.FindAsync(contractId);
        if (contract == null) return false;

        contract.Status = status;
        contract.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}

public class CompanyRepository : ICompanyRepository
{
    private readonly ContractsDbContext _context;

    public CompanyRepository(ContractsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Company>> GetAllAsync()
    {
        return await _context.Companies
            .Include(c => c.Sites)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Company?> GetByIdAsync(int id)
    {
        return await _context.Companies
            .Include(c => c.Sites)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Company?> GetByCompanyCodeAsync(string companyCode)
    {
        return await _context.Companies
            .Include(c => c.Sites)
            .FirstOrDefaultAsync(c => c.CompanyCode == companyCode);
    }

    public async Task<Company> CreateAsync(Company company)
    {
        company.CreatedDate = DateTime.UtcNow;
        
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        return company;
    }

    public async Task<Company> UpdateAsync(Company company)
    {
        _context.Entry(company).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return company;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company == null) return false;

        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();
        return true;
    }
}

public class ServiceRepository : IServiceRepository
{
    private readonly ContractsDbContext _context;

    public ServiceRepository(ContractsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Service>> GetAllAsync()
    {
        return await _context.Services
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Service?> GetByIdAsync(int id)
    {
        return await _context.Services.FindAsync(id);
    }

    public async Task<Service?> GetByServiceCodeAsync(string serviceCode)
    {
        return await _context.Services
            .FirstOrDefaultAsync(s => s.ServiceCode == serviceCode);
    }

    public async Task<IEnumerable<Service>> GetByCategoryAsync(string category)
    {
        return await _context.Services
            .Where(s => s.Category == category)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Service> CreateAsync(Service service)
    {
        service.CreatedDate = DateTime.UtcNow;
        
        _context.Services.Add(service);
        await _context.SaveChangesAsync();
        return service;
    }

    public async Task<Service> UpdateAsync(Service service)
    {
        _context.Entry(service).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return service;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service == null) return false;

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();
        return true;
    }
}