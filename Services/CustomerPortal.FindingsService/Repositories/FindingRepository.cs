using CustomerPortal.FindingsService.Data;
using CustomerPortal.FindingsService.Entities;
using CustomerPortal.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.FindingsService.Repositories;

public class FindingRepository : BaseRepository<Finding>, IFindingRepository
{
    public FindingRepository(FindingsDbContext context) : base(context) { }

    public async Task<IEnumerable<Finding>> GetFindingsByStatusAsync(int statusId)
    {
        return await _context.Set<Finding>()
            .Include(f => f.Category)
            .Include(f => f.Status)
            .Where(f => f.StatusId == statusId)
            .OrderByDescending(f => f.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Finding>> GetFindingsByCategoryAsync(int categoryId)
    {
        return await _context.Set<Finding>()
            .Include(f => f.Category)
            .Include(f => f.Status)
            .Where(f => f.CategoryId == categoryId)
            .OrderByDescending(f => f.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Finding>> GetFindingsByAuditAsync(int auditId)
    {
        return await _context.Set<Finding>()
            .Include(f => f.Category)
            .Include(f => f.Status)
            .Where(f => f.AuditId == auditId)
            .OrderByDescending(f => f.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Finding>> GetFindingsBySiteAsync(int siteId)
    {
        return await _context.Set<Finding>()
            .Include(f => f.Category)
            .Include(f => f.Status)
            .Where(f => f.SiteId == siteId)
            .OrderByDescending(f => f.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Finding>> GetFindingsByCompanyAsync(int companyId)
    {
        return await _context.Set<Finding>()
            .Include(f => f.Category)
            .Include(f => f.Status)
            .Where(f => f.CompanyId == companyId)
            .OrderByDescending(f => f.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Finding>> GetFindingsByAssigneeAsync(string assignedTo)
    {
        return await _context.Set<Finding>()
            .Include(f => f.Category)
            .Include(f => f.Status)
            .Where(f => f.AssignedTo == assignedTo)
            .OrderByDescending(f => f.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Finding>> GetOverdueFindingsAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _context.Set<Finding>()
            .Include(f => f.Category)
            .Include(f => f.Status)
            .Where(f => f.RequiredCompletionDate.HasValue && 
                       f.RequiredCompletionDate.Value.Date < today &&
                       f.ActualCompletionDate == null &&
                       !f.Status.IsFinalStatus)
            .OrderBy(f => f.RequiredCompletionDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Finding>> GetFindingsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Set<Finding>()
            .Include(f => f.Category)
            .Include(f => f.Status)
            .Where(f => f.IdentifiedDate.HasValue && 
                       f.IdentifiedDate.Value.Date >= startDate.Date &&
                       f.IdentifiedDate.Value.Date <= endDate.Date)
            .OrderByDescending(f => f.IdentifiedDate)
            .ToListAsync();
    }

    public override async Task<IEnumerable<Finding>> GetAllAsync()
    {
        return await _context.Set<Finding>()
            .Include(f => f.Category)
            .Include(f => f.Status)
            .OrderByDescending(f => f.CreatedDate)
            .ToListAsync();
    }

    public override async Task<Finding?> GetByIdAsync(int id)
    {
        return await _context.Set<Finding>()
            .Include(f => f.Category)
            .Include(f => f.Status)
            .FirstOrDefaultAsync(f => f.Id == id);
    }
}