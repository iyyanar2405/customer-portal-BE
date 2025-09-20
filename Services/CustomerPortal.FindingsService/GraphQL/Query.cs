using CustomerPortal.FindingsService.Entities;
using CustomerPortal.FindingsService.Repositories;

namespace CustomerPortal.FindingsService.GraphQL;

public class Query
{
    public async Task<IEnumerable<Finding>> GetFindings([Service] IFindingRepository repository)
        => await repository.GetAllAsync();

    public async Task<Finding?> GetFinding(int id, [Service] IFindingRepository repository)
        => await repository.GetByIdAsync(id);

    public async Task<IEnumerable<Finding>> GetFindingsByStatus(int statusId, [Service] IFindingRepository repository)
        => await repository.GetFindingsByStatusAsync(statusId);

    public async Task<IEnumerable<Finding>> GetFindingsByCategory(int categoryId, [Service] IFindingRepository repository)
        => await repository.GetFindingsByCategoryAsync(categoryId);

    public async Task<IEnumerable<Finding>> GetFindingsByAudit(int auditId, [Service] IFindingRepository repository)
        => await repository.GetFindingsByAuditAsync(auditId);

    public async Task<IEnumerable<Finding>> GetFindingsBySite(int siteId, [Service] IFindingRepository repository)
        => await repository.GetFindingsBySiteAsync(siteId);

    public async Task<IEnumerable<Finding>> GetFindingsByCompany(int companyId, [Service] IFindingRepository repository)
        => await repository.GetFindingsByCompanyAsync(companyId);

    public async Task<IEnumerable<Finding>> GetFindingsByAssignee(string assignedTo, [Service] IFindingRepository repository)
        => await repository.GetFindingsByAssigneeAsync(assignedTo);

    public async Task<IEnumerable<Finding>> GetOverdueFindings([Service] IFindingRepository repository)
        => await repository.GetOverdueFindingsAsync();

    public async Task<IEnumerable<Finding>> GetFindingsByDateRange(DateTime startDate, DateTime endDate, [Service] IFindingRepository repository)
        => await repository.GetFindingsByDateRangeAsync(startDate, endDate);

    // Finding Categories
    public async Task<IEnumerable<FindingCategory>> GetFindingCategories([Service] IFindingCategoryRepository repository)
        => await repository.GetAllAsync();

    public async Task<FindingCategory?> GetFindingCategory(int id, [Service] IFindingCategoryRepository repository)
        => await repository.GetByIdAsync(id);

    public async Task<FindingCategory?> GetFindingCategoryByCode(string code, [Service] IFindingCategoryRepository repository)
        => await repository.GetByCodeAsync(code);

    // Finding Statuses
    public async Task<IEnumerable<FindingStatus>> GetFindingStatuses([Service] IFindingStatusRepository repository)
        => await repository.GetAllAsync();

    public async Task<FindingStatus?> GetFindingStatus(int id, [Service] IFindingStatusRepository repository)
        => await repository.GetByIdAsync(id);

    public async Task<FindingStatus?> GetFindingStatusByCode(string code, [Service] IFindingStatusRepository repository)
        => await repository.GetByCodeAsync(code);

    public async Task<IEnumerable<FindingStatus>> GetActiveFindingStatuses([Service] IFindingStatusRepository repository)
        => await repository.GetActiveStatusesAsync();
}