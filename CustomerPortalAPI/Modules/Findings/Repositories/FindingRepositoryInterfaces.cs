using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Findings.Entities;

namespace CustomerPortalAPI.Modules.Findings.Repositories
{
    public interface IFindingRepository : IRepository<Finding>
    {
        Task<IEnumerable<Finding>> GetFindingsByAuditAsync(int auditId);
        Task<IEnumerable<Finding>> GetFindingsBySiteAsync(int siteId);
        Task<IEnumerable<Finding>> GetFindingsByStatusAsync(int statusId);
        Task<IEnumerable<Finding>> GetFindingsByCategoryAsync(int categoryId);
        Task<IEnumerable<Finding>> GetFindingsByTypeAsync(string findingType);
        Task<IEnumerable<Finding>> GetFindingsBySeverityAsync(string severity);
        Task<IEnumerable<Finding>> GetOverdueFindingsAsync();
        Task<IEnumerable<Finding>> GetFindingsDueSoonAsync(int daysAhead);
        Task<IEnumerable<Finding>> GetActiveFindingsAsync();
        Task<IEnumerable<Finding>> GetClosedFindingsAsync();
        Task<Finding?> GetByFindingNumberAsync(string findingNumber);
        Task<Finding?> GetFindingWithDetailsAsync(int findingId);
        Task<IEnumerable<Finding>> GetFindingsByAssigneeAsync(int assignedTo);
        Task<IEnumerable<Finding>> GetFindingsByIdentifierAsync(int identifiedBy);
        Task<IEnumerable<Finding>> GetFindingsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Finding>> SearchFindingsAsync(string searchTerm);
        Task UpdateFindingStatusAsync(int findingId, int statusId, int modifiedBy);
        Task AssignFindingAsync(int findingId, int assignedTo, int modifiedBy);
        Task CloseFindingAsync(int findingId, int verifiedBy, string? verificationMethod);
        Task<int> GetFindingCountByStatusAsync(int statusId);
        Task<int> GetFindingCountByTypeAsync(string findingType);
        Task<IEnumerable<Finding>> GetFindingsTrendsByPeriodAsync(DateTime startDate, DateTime endDate);
    }

    public interface IFindingCategoryRepository : IRepository<FindingCategory>
    {
        Task<IEnumerable<FindingCategory>> GetActiveCategoriesAsync();
        Task<IEnumerable<FindingCategory>> GetRootCategoriesAsync();
        Task<IEnumerable<FindingCategory>> GetSubCategoriesAsync(int parentCategoryId);
        Task<FindingCategory?> GetCategoryWithSubCategoriesAsync(int categoryId);
        Task<IEnumerable<FindingCategory>> SearchCategoriesAsync(string searchTerm);
        Task<FindingCategory?> GetByCategoryCodeAsync(string categoryCode);
        Task<IEnumerable<FindingCategory>> GetCategoriesByDisplayOrderAsync();
        Task UpdateCategoryOrderAsync(int categoryId, int displayOrder, int modifiedBy);
    }

    public interface IFindingStatusRepository : IRepository<FindingStatus>
    {
        Task<IEnumerable<FindingStatus>> GetActiveStatusesAsync();
        Task<IEnumerable<FindingStatus>> GetFinalStatusesAsync();
        Task<IEnumerable<FindingStatus>> GetNonFinalStatusesAsync();
        Task<FindingStatus?> GetByStatusCodeAsync(string statusCode);
        Task<IEnumerable<FindingStatus>> GetStatusesByDisplayOrderAsync();
        Task<IEnumerable<FindingStatus>> SearchStatusesAsync(string searchTerm);
        Task UpdateStatusOrderAsync(int statusId, int displayOrder, int modifiedBy);
        Task<FindingStatus?> GetDefaultStatusAsync();
    }
}