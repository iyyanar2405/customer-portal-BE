using CustomerPortalAPI.Data;
using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Actions.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortalAPI.Modules.Actions.Repositories
{
    public class ActionRepository : Repository<Entities.Action>, IActionRepository
    {
        public ActionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Entities.Action>> GetActionsByUserAsync(int userId)
        {
            return await _dbSet.Where(a => a.AssignedToUserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Entities.Action>> GetActionsByCompanyAsync(int companyId)
        {
            return await _dbSet.Where(a => a.CompanyId == companyId).ToListAsync();
        }

        public async Task<IEnumerable<Entities.Action>> GetActionsBySiteAsync(int siteId)
        {
            return await _dbSet.Where(a => a.SiteId == siteId).ToListAsync();
        }

        public async Task<IEnumerable<Entities.Action>> GetActionsByAuditAsync(int auditId)
        {
            return await _dbSet.Where(a => a.AuditId == auditId).ToListAsync();
        }

        public async Task<IEnumerable<Entities.Action>> GetActionsByFindingAsync(int findingId)
        {
            return await _dbSet.Where(a => a.FindingId == findingId).ToListAsync();
        }

        public async Task<IEnumerable<Entities.Action>> GetActionsByStatusAsync(string status)
        {
            return await _dbSet.Where(a => a.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<Entities.Action>> GetActionsByPriorityAsync(string priority)
        {
            return await _dbSet.Where(a => a.Priority == priority).ToListAsync();
        }

        public async Task<IEnumerable<Entities.Action>> GetOverdueActionsAsync()
        {
            var currentDate = DateTime.UtcNow;
            return await _dbSet.Where(a => a.DueDate < currentDate && a.Status != "Completed").ToListAsync();
        }

        public async Task<IEnumerable<Entities.Action>> GetActionsDueSoonAsync(int days = 7)
        {
            var dueDate = DateTime.UtcNow.AddDays(days);
            return await _dbSet.Where(a => a.DueDate <= dueDate && a.Status != "Completed").ToListAsync();
        }

        public async Task<IEnumerable<Entities.Action>> GetActiveActionsAsync()
        {
            return await _dbSet.Where(a => a.IsActive && a.Status != "Completed").ToListAsync();
        }

        public async Task<IEnumerable<Entities.Action>> GetCompletedActionsAsync()
        {
            return await _dbSet.Where(a => a.Status == "Completed").ToListAsync();
        }

        public async Task<IEnumerable<Entities.Action>> SearchActionsAsync(string searchTerm)
        {
            return await _dbSet.Where(a => 
                a.ActionName.Contains(searchTerm) || 
                (a.Description != null && a.Description.Contains(searchTerm)) ||
                (a.Comments != null && a.Comments.Contains(searchTerm))).ToListAsync();
        }

        public async Task<Entities.Action?> GetActionByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task MarkActionAsCompletedAsync(int actionId, int completedBy, string? comments = null)
        {
            var action = await GetByIdAsync(actionId);
            if (action != null)
            {
                action.Status = "Completed";
                action.CompletedDate = DateTime.UtcNow;
                action.ModifiedBy = completedBy;
                action.ModifiedDate = DateTime.UtcNow;
                if (!string.IsNullOrEmpty(comments))
                {
                    action.Comments = comments;
                }
                await UpdateAsync(action);
            }
        }

        public async Task AssignActionAsync(int actionId, int assignedToUserId, int assignedBy)
        {
            var action = await GetByIdAsync(actionId);
            if (action != null)
            {
                action.AssignedToUserId = assignedToUserId;
                action.ModifiedBy = assignedBy;
                action.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(action);
            }
        }

        public async Task UpdateActionStatusAsync(int actionId, string status, int modifiedBy)
        {
            var action = await GetByIdAsync(actionId);
            if (action != null)
            {
                action.Status = status;
                action.ModifiedBy = modifiedBy;
                action.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(action);
            }
        }

        public async Task<IEnumerable<Entities.Action>> GetActionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate).ToListAsync();
        }

        public async Task<int> GetActionCountByStatusAsync(string status)
        {
            return await _dbSet.CountAsync(a => a.Status == status);
        }

        public async Task<int> GetOverdueActionCountAsync()
        {
            var currentDate = DateTime.UtcNow;
            return await _dbSet.CountAsync(a => a.DueDate < currentDate && a.Status != "Completed");
        }
    }
}