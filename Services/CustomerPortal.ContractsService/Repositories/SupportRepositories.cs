using Microsoft.EntityFrameworkCore;
using CustomerPortal.ContractsService.Data;
using CustomerPortal.ContractsService.Entities;

namespace CustomerPortal.ContractsService.Repositories;

public class SiteRepository : ISiteRepository
{
    private readonly ContractsDbContext _context;

    public SiteRepository(ContractsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Site>> GetAllAsync()
    {
        return await _context.Sites
            .Include(s => s.Company)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Site?> GetByIdAsync(int id)
    {
        return await _context.Sites
            .Include(s => s.Company)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Site?> GetBySiteCodeAsync(string siteCode)
    {
        return await _context.Sites
            .Include(s => s.Company)
            .FirstOrDefaultAsync(s => s.SiteCode == siteCode);
    }

    public async Task<IEnumerable<Site>> GetByCompanyIdAsync(int companyId)
    {
        return await _context.Sites
            .Include(s => s.Company)
            .Where(s => s.CompanyId == companyId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Site> CreateAsync(Site site)
    {
        site.CreatedDate = DateTime.UtcNow;
        
        _context.Sites.Add(site);
        await _context.SaveChangesAsync();
        return site;
    }

    public async Task<Site> UpdateAsync(Site site)
    {
        _context.Entry(site).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return site;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var site = await _context.Sites.FindAsync(id);
        if (site == null) return false;

        _context.Sites.Remove(site);
        await _context.SaveChangesAsync();
        return true;
    }
}

public class ContractTermRepository : IContractTermRepository
{
    private readonly ContractsDbContext _context;

    public ContractTermRepository(ContractsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ContractTerm>> GetByContractIdAsync(int contractId)
    {
        return await _context.ContractTerms
            .Where(ct => ct.ContractId == contractId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ContractTerm?> GetByIdAsync(int id)
    {
        return await _context.ContractTerms.FindAsync(id);
    }

    public async Task<ContractTerm> CreateAsync(ContractTerm contractTerm)
    {
        contractTerm.CreatedDate = DateTime.UtcNow;
        
        _context.ContractTerms.Add(contractTerm);
        await _context.SaveChangesAsync();
        return contractTerm;
    }

    public async Task<ContractTerm> UpdateAsync(ContractTerm contractTerm)
    {
        _context.Entry(contractTerm).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return contractTerm;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var contractTerm = await _context.ContractTerms.FindAsync(id);
        if (contractTerm == null) return false;

        _context.ContractTerms.Remove(contractTerm);
        await _context.SaveChangesAsync();
        return true;
    }
}

public class ContractAmendmentRepository : IContractAmendmentRepository
{
    private readonly ContractsDbContext _context;

    public ContractAmendmentRepository(ContractsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ContractAmendment>> GetByContractIdAsync(int contractId)
    {
        return await _context.ContractAmendments
            .Where(ca => ca.ContractId == contractId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ContractAmendment?> GetByIdAsync(int id)
    {
        return await _context.ContractAmendments.FindAsync(id);
    }

    public async Task<ContractAmendment?> GetByAmendmentNumberAsync(string amendmentNumber)
    {
        return await _context.ContractAmendments
            .FirstOrDefaultAsync(ca => ca.AmendmentNumber == amendmentNumber);
    }

    public async Task<ContractAmendment> CreateAsync(ContractAmendment amendment)
    {
        amendment.CreatedDate = DateTime.UtcNow;
        
        _context.ContractAmendments.Add(amendment);
        await _context.SaveChangesAsync();
        return amendment;
    }

    public async Task<ContractAmendment> UpdateAsync(ContractAmendment amendment)
    {
        _context.Entry(amendment).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return amendment;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var amendment = await _context.ContractAmendments.FindAsync(id);
        if (amendment == null) return false;

        _context.ContractAmendments.Remove(amendment);
        await _context.SaveChangesAsync();
        return true;
    }
}

public class ContractRenewalRepository : IContractRenewalRepository
{
    private readonly ContractsDbContext _context;

    public ContractRenewalRepository(ContractsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ContractRenewal>> GetByContractIdAsync(int contractId)
    {
        return await _context.ContractRenewals
            .Where(cr => cr.ContractId == contractId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ContractRenewal?> GetByIdAsync(int id)
    {
        return await _context.ContractRenewals.FindAsync(id);
    }

    public async Task<ContractRenewal?> GetByRenewalNumberAsync(string renewalNumber)
    {
        return await _context.ContractRenewals
            .FirstOrDefaultAsync(cr => cr.RenewalNumber == renewalNumber);
    }

    public async Task<IEnumerable<ContractRenewal>> GetRenewalScheduleAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.ContractRenewals
            .Include(cr => cr.Contract).ThenInclude(c => c!.Company)
            .Where(cr => cr.ProposedStartDate >= startDate && cr.ProposedStartDate <= endDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ContractRenewal> CreateAsync(ContractRenewal renewal)
    {
        renewal.CreatedDate = DateTime.UtcNow;
        
        _context.ContractRenewals.Add(renewal);
        await _context.SaveChangesAsync();
        return renewal;
    }

    public async Task<ContractRenewal> UpdateAsync(ContractRenewal renewal)
    {
        _context.Entry(renewal).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return renewal;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var renewal = await _context.ContractRenewals.FindAsync(id);
        if (renewal == null) return false;

        _context.ContractRenewals.Remove(renewal);
        await _context.SaveChangesAsync();
        return true;
    }
}