using CustomerPortal.FindingsService.Entities;
using CustomerPortal.Shared.Interfaces;

namespace CustomerPortal.FindingsService.Repositories;

public interface IFindingRepository : IRepository<Finding>
{
    Task<IEnumerable<Finding>> GetFindingsByStatusAsync(int statusId);
    Task<IEnumerable<Finding>> GetFindingsByCategoryAsync(int categoryId);
    Task<IEnumerable<Finding>> GetFindingsByAuditAsync(int auditId);
    Task<IEnumerable<Finding>> GetFindingsBySiteAsync(int siteId);
    Task<IEnumerable<Finding>> GetFindingsByCompanyAsync(int companyId);
    Task<IEnumerable<Finding>> GetFindingsByAssigneeAsync(string assignedTo);
    Task<IEnumerable<Finding>> GetOverdueFindingsAsync();
    Task<IEnumerable<Finding>> GetFindingsByDateRangeAsync(DateTime startDate, DateTime endDate);
}

public interface IFindingCategoryRepository : IRepository<FindingCategory>
{
    Task<FindingCategory?> GetByCodeAsync(string code);
}

public interface IFindingStatusRepository : IRepository<FindingStatus>
{
    Task<FindingStatus?> GetByCodeAsync(string code);
    Task<IEnumerable<FindingStatus>> GetActiveStatusesAsync();
}