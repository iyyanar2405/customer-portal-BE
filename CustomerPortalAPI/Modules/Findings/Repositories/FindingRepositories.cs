using Microsoft.EntityFrameworkCore;
using CustomerPortalAPI.Data;
using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Findings.Entities;

namespace CustomerPortalAPI.Modules.Findings.Repositories
{
    public class FindingRepository : Repository<Finding>, IFindingRepository
    {
        public FindingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Finding>> GetFindingsByAuditAsync(int auditId)
        {
            return await _dbSet.Where(f => f.AuditId == auditId).ToListAsync();
        }

        public async Task<IEnumerable<Finding>> GetFindingsBySiteAsync(int siteId)
        {
            return await _dbSet.Where(f => f.SiteId == siteId).ToListAsync();
        }

        public async Task<IEnumerable<Finding>> GetFindingsByStatusAsync(int statusId)
        {
            return await _dbSet.Where(f => f.FindingStatusId == statusId).ToListAsync();
        }

        public async Task<IEnumerable<Finding>> GetFindingsByCategoryAsync(int categoryId)
        {
            return await _dbSet.Where(f => f.FindingCategoryId == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<Finding>> GetFindingsByTypeAsync(string findingType)
        {
            return await _dbSet.Where(f => f.FindingType == findingType).ToListAsync();
        }

        public async Task<IEnumerable<Finding>> GetFindingsBySeverityAsync(string severity)
        {
            return await _dbSet.Where(f => f.Severity == severity).ToListAsync();
        }

        public async Task<IEnumerable<Finding>> GetOverdueFindingsAsync()
        {
            var today = DateTime.Today;
            return await _dbSet.Where(f => f.DueDate < today && f.ClosedDate == null && f.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Finding>> GetFindingsDueSoonAsync(int daysAhead)
        {
            var today = DateTime.Today;
            var futureDate = today.AddDays(daysAhead);
            return await _dbSet.Where(f => f.DueDate >= today && f.DueDate <= futureDate && f.ClosedDate == null && f.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Finding>> GetActiveFindingsAsync()
        {
            return await _dbSet.Where(f => f.IsActive && f.ClosedDate == null).ToListAsync();
        }

        public async Task<IEnumerable<Finding>> GetClosedFindingsAsync()
        {
            return await _dbSet.Where(f => f.ClosedDate != null).ToListAsync();
        }

        public async Task<Finding?> GetByFindingNumberAsync(string findingNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(f => f.FindingNumber == findingNumber);
        }

        public async Task<Finding?> GetFindingWithDetailsAsync(int findingId)
        {
            return await _dbSet
                .Include(f => f.Audit)
                .Include(f => f.Site)
                .Include(f => f.FindingStatus)
                .Include(f => f.FindingCategory)
                .Include(f => f.CreatedByUser)
                .Include(f => f.AssignedToUser)
                .Include(f => f.IdentifiedByUser)
                .Include(f => f.VerifiedByUser)
                .FirstOrDefaultAsync(f => f.Id == findingId);
        }

        public async Task<IEnumerable<Finding>> GetFindingsByAssigneeAsync(int assignedTo)
        {
            return await _dbSet.Where(f => f.AssignedTo == assignedTo && f.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Finding>> GetFindingsByIdentifierAsync(int identifiedBy)
        {
            return await _dbSet.Where(f => f.IdentifiedBy == identifiedBy).ToListAsync();
        }

        public async Task<IEnumerable<Finding>> GetFindingsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(f => f.IdentifiedDate >= startDate && f.IdentifiedDate <= endDate).ToListAsync();
        }

        public async Task<IEnumerable<Finding>> SearchFindingsAsync(string searchTerm)
        {
            return await _dbSet.Where(f => 
                f.FindingNumber.Contains(searchTerm) ||
                f.Title.Contains(searchTerm) ||
                f.Description.Contains(searchTerm) ||
                f.RootCause!.Contains(searchTerm) ||
                f.CorrectiveAction!.Contains(searchTerm)).ToListAsync();
        }

        public async Task UpdateFindingStatusAsync(int findingId, int statusId, int modifiedBy)
        {
            var finding = await GetByIdAsync(findingId);
            if (finding != null)
            {
                finding.FindingStatusId = statusId;
                finding.ModifiedBy = modifiedBy;
                finding.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(finding);
            }
        }

        public async Task AssignFindingAsync(int findingId, int assignedTo, int modifiedBy)
        {
            var finding = await GetByIdAsync(findingId);
            if (finding != null)
            {
                finding.AssignedTo = assignedTo;
                finding.ModifiedBy = modifiedBy;
                finding.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(finding);
            }
        }

        public async Task CloseFindingAsync(int findingId, int verifiedBy, string? verificationMethod)
        {
            var finding = await GetByIdAsync(findingId);
            if (finding != null)
            {
                finding.ClosedDate = DateTime.UtcNow;
                finding.VerifiedBy = verifiedBy;
                finding.VerificationDate = DateTime.UtcNow;
                finding.VerificationMethod = verificationMethod;
                finding.ModifiedBy = verifiedBy;
                finding.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(finding);
            }
        }

        public async Task<int> GetFindingCountByStatusAsync(int statusId)
        {
            return await _dbSet.CountAsync(f => f.FindingStatusId == statusId);
        }

        public async Task<int> GetFindingCountByTypeAsync(string findingType)
        {
            return await _dbSet.CountAsync(f => f.FindingType == findingType);
        }

        public async Task<IEnumerable<Finding>> GetFindingsTrendsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(f => f.IdentifiedDate >= startDate && f.IdentifiedDate <= endDate)
                .OrderBy(f => f.IdentifiedDate)
                .ToListAsync();
        }
    }

    public class FindingCategoryRepository : Repository<FindingCategory>, IFindingCategoryRepository
    {
        public FindingCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FindingCategory>> GetActiveCategoriesAsync()
        {
            return await _dbSet.Where(fc => fc.IsActive).OrderBy(fc => fc.DisplayOrder).ToListAsync();
        }

        public async Task<IEnumerable<FindingCategory>> GetRootCategoriesAsync()
        {
            return await _dbSet.Where(fc => fc.ParentCategoryId == null && fc.IsActive)
                .OrderBy(fc => fc.DisplayOrder).ToListAsync();
        }

        public async Task<IEnumerable<FindingCategory>> GetSubCategoriesAsync(int parentCategoryId)
        {
            return await _dbSet.Where(fc => fc.ParentCategoryId == parentCategoryId && fc.IsActive)
                .OrderBy(fc => fc.DisplayOrder).ToListAsync();
        }

        public async Task<FindingCategory?> GetCategoryWithSubCategoriesAsync(int categoryId)
        {
            return await _dbSet
                .Include(fc => fc.SubCategories)
                .Include(fc => fc.ParentCategory)
                .FirstOrDefaultAsync(fc => fc.Id == categoryId);
        }

        public async Task<IEnumerable<FindingCategory>> SearchCategoriesAsync(string searchTerm)
        {
            return await _dbSet.Where(fc => 
                fc.CategoryName.Contains(searchTerm) ||
                fc.CategoryCode.Contains(searchTerm) ||
                fc.Description!.Contains(searchTerm)).ToListAsync();
        }

        public async Task<FindingCategory?> GetByCategoryCodeAsync(string categoryCode)
        {
            return await _dbSet.FirstOrDefaultAsync(fc => fc.CategoryCode == categoryCode);
        }

        public async Task<IEnumerable<FindingCategory>> GetCategoriesByDisplayOrderAsync()
        {
            return await _dbSet.Where(fc => fc.IsActive).OrderBy(fc => fc.DisplayOrder).ToListAsync();
        }

        public async Task UpdateCategoryOrderAsync(int categoryId, int displayOrder, int modifiedBy)
        {
            var category = await GetByIdAsync(categoryId);
            if (category != null)
            {
                category.DisplayOrder = displayOrder;
                category.ModifiedBy = modifiedBy;
                category.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(category);
            }
        }
    }

    public class FindingStatusRepository : Repository<FindingStatus>, IFindingStatusRepository
    {
        public FindingStatusRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FindingStatus>> GetActiveStatusesAsync()
        {
            return await _dbSet.Where(fs => fs.IsActive).OrderBy(fs => fs.DisplayOrder).ToListAsync();
        }

        public async Task<IEnumerable<FindingStatus>> GetFinalStatusesAsync()
        {
            return await _dbSet.Where(fs => fs.IsFinal && fs.IsActive).OrderBy(fs => fs.DisplayOrder).ToListAsync();
        }

        public async Task<IEnumerable<FindingStatus>> GetNonFinalStatusesAsync()
        {
            return await _dbSet.Where(fs => !fs.IsFinal && fs.IsActive).OrderBy(fs => fs.DisplayOrder).ToListAsync();
        }

        public async Task<FindingStatus?> GetByStatusCodeAsync(string statusCode)
        {
            return await _dbSet.FirstOrDefaultAsync(fs => fs.StatusCode == statusCode);
        }

        public async Task<IEnumerable<FindingStatus>> GetStatusesByDisplayOrderAsync()
        {
            return await _dbSet.Where(fs => fs.IsActive).OrderBy(fs => fs.DisplayOrder).ToListAsync();
        }

        public async Task<IEnumerable<FindingStatus>> SearchStatusesAsync(string searchTerm)
        {
            return await _dbSet.Where(fs => 
                fs.StatusName.Contains(searchTerm) ||
                fs.StatusCode.Contains(searchTerm) ||
                fs.Description!.Contains(searchTerm)).ToListAsync();
        }

        public async Task UpdateStatusOrderAsync(int statusId, int displayOrder, int modifiedBy)
        {
            var status = await GetByIdAsync(statusId);
            if (status != null)
            {
                status.DisplayOrder = displayOrder;
                status.ModifiedBy = modifiedBy;
                status.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(status);
            }
        }

        public async Task<FindingStatus?> GetDefaultStatusAsync()
        {
            return await _dbSet.Where(fs => fs.IsActive)
                .OrderBy(fs => fs.DisplayOrder)
                .FirstOrDefaultAsync();
        }
    }
}