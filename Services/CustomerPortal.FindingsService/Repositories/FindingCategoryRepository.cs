using CustomerPortal.FindingsService.Data;
using CustomerPortal.FindingsService.Entities;
using CustomerPortal.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.FindingsService.Repositories;

public class FindingCategoryRepository : BaseRepository<FindingCategory>, IFindingCategoryRepository
{
    public FindingCategoryRepository(FindingsDbContext context) : base(context) { }

    public async Task<FindingCategory?> GetByCodeAsync(string code)
    {
        return await _context.Set<FindingCategory>()
            .FirstOrDefaultAsync(fc => fc.Code == code);
    }

    public override async Task<IEnumerable<FindingCategory>> GetAllAsync()
    {
        return await _context.Set<FindingCategory>()
            .Where(fc => fc.IsActive)
            .OrderBy(fc => fc.Name)
            .ToListAsync();
    }
}

public class FindingStatusRepository : BaseRepository<FindingStatus>, IFindingStatusRepository
{
    public FindingStatusRepository(FindingsDbContext context) : base(context) { }

    public async Task<FindingStatus?> GetByCodeAsync(string code)
    {
        return await _context.Set<FindingStatus>()
            .FirstOrDefaultAsync(fs => fs.Code == code);
    }

    public async Task<IEnumerable<FindingStatus>> GetActiveStatusesAsync()
    {
        return await _context.Set<FindingStatus>()
            .Where(fs => fs.IsActive)
            .OrderBy(fs => fs.DisplayOrder)
            .ToListAsync();
    }

    public override async Task<IEnumerable<FindingStatus>> GetAllAsync()
    {
        return await _context.Set<FindingStatus>()
            .Where(fs => fs.IsActive)
            .OrderBy(fs => fs.DisplayOrder)
            .ToListAsync();
    }
}