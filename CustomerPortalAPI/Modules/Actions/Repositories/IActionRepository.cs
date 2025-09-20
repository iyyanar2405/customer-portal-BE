using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Actions.Entities;

namespace CustomerPortalAPI.Modules.Actions.Repositories
{
    public interface IActionRepository : IRepository<Entities.Action>
    {
        Task<IEnumerable<Entities.Action>> GetActionsByUserAsync(int userId);
        Task<IEnumerable<Entities.Action>> GetActionsByCompanyAsync(int companyId);
        Task<IEnumerable<Entities.Action>> GetActionsBySiteAsync(int siteId);
        Task<IEnumerable<Entities.Action>> GetActionsByAuditAsync(int auditId);
        Task<IEnumerable<Entities.Action>> GetActionsByFindingAsync(int findingId);
        Task<IEnumerable<Entities.Action>> GetActionsByStatusAsync(string status);
        Task<IEnumerable<Entities.Action>> GetActionsByPriorityAsync(string priority);
        Task<IEnumerable<Entities.Action>> GetOverdueActionsAsync();
        Task<IEnumerable<Entities.Action>> GetActionsDueSoonAsync(int days = 7);
        Task<IEnumerable<Entities.Action>> GetActiveActionsAsync();
        Task<IEnumerable<Entities.Action>> GetCompletedActionsAsync();
        Task<IEnumerable<Entities.Action>> SearchActionsAsync(string searchTerm);
        Task<Entities.Action?> GetActionByIdWithDetailsAsync(int id);
        Task MarkActionAsCompletedAsync(int actionId, int completedBy, string? comments = null);
        Task AssignActionAsync(int actionId, int assignedToUserId, int assignedBy);
        Task UpdateActionStatusAsync(int actionId, string status, int modifiedBy);
        Task<IEnumerable<Entities.Action>> GetActionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> GetActionCountByStatusAsync(string status);
        Task<int> GetOverdueActionCountAsync();
    }
}