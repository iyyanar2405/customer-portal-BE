using CustomerPortal.ActionsService.Data;
using CustomerPortal.ActionsService.Entities;
using ActionEntity = CustomerPortal.ActionsService.Entities.Action;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.ActionsService.Repositories
{
    public class ActionRepository : IActionRepository
    {
        private readonly ActionsDbContext _context;

        public ActionRepository(ActionsDbContext context)
        {
            _context = context;
        }

        public async Task<ActionEntity?> GetByIdAsync(int id)
        {
            return await _context.Actions
                .Include(a => a.ActionType)
                .Include(a => a.AssignedTo)
                .Include(a => a.CreatedBy)
                .Include(a => a.Attachments)
                .Include(a => a.ActionComments)
                .Include(a => a.Dependencies)
                .Include(a => a.DependentActions)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<ActionEntity?> GetByActionNumberAsync(string actionNumber)
        {
            return await _context.Actions
                .Include(a => a.ActionType)
                .Include(a => a.AssignedTo)
                .Include(a => a.CreatedBy)
                .FirstOrDefaultAsync(a => a.ActionNumber == actionNumber);
        }

        public async Task<IEnumerable<ActionEntity>> GetAllAsync()
        {
            return await _context.Actions
                .Include(a => a.ActionType)
                .Include(a => a.AssignedTo)
                .Include(a => a.CreatedBy)
                .ToListAsync();
        }

        public async Task<ActionEntity> CreateAsync(ActionEntity action)
        {
            action.CreatedDate = DateTime.UtcNow;
            action.ModifiedDate = DateTime.UtcNow;
            
            _context.Actions.Add(action);
            await _context.SaveChangesAsync();
            return action;
        }

        public async Task<ActionEntity> UpdateAsync(ActionEntity action)
        {
            action.ModifiedDate = DateTime.UtcNow;
            
            _context.Entry(action).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return action;
        }

        public async Task DeleteAsync(ActionEntity action)
        {
            _context.Actions.Remove(action);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var action = await _context.Actions.FindAsync(id);
            if (action == null) return false;

            _context.Actions.Remove(action);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Actions.AnyAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<ActionEntity>> GetByAssigneeAsync(int userId)
        {
            return await _context.Actions
                .Include(a => a.ActionType)
                .Include(a => a.CreatedBy)
                .Where(a => a.AssignedToId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionEntity>> GetByCreatorAsync(int userId)
        {
            return await _context.Actions
                .Include(a => a.ActionType)
                .Include(a => a.AssignedTo)
                .Where(a => a.CreatedById == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionEntity>> GetByStatusAsync(string status)
        {
            return await _context.Actions
                .Include(a => a.ActionType)
                .Include(a => a.AssignedTo)
                .Include(a => a.CreatedBy)
                .Where(a => a.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionEntity>> GetByPriorityAsync(string priority)
        {
            return await _context.Actions
                .Include(a => a.ActionType)
                .Include(a => a.AssignedTo)
                .Include(a => a.CreatedBy)
                .Where(a => a.Priority == priority)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionEntity>> GetByActionTypeAsync(int actionTypeId)
        {
            return await _context.Actions
                .Include(a => a.ActionType)
                .Include(a => a.AssignedTo)
                .Include(a => a.CreatedBy)
                .Where(a => a.ActionTypeId == actionTypeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionEntity>> GetByTeamAsync(int teamId)
        {
            return await _context.Actions
                .Include(a => a.ActionType)
                .Include(a => a.AssignedTo)
                .Include(a => a.CreatedBy)
                .Where(a => a.AssignedTo != null && a.AssignedTo.TeamId == teamId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionEntity>> GetByDueDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Actions
                .Include(a => a.ActionType)
                .Include(a => a.AssignedTo)
                .Include(a => a.CreatedBy)
                .Where(a => a.DueDate >= startDate && a.DueDate <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionEntity>> GetOverdueAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.Actions
                .Include(a => a.ActionType)
                .Include(a => a.AssignedTo)
                .Include(a => a.CreatedBy)
                .Where(a => a.DueDate < today && a.Status != "COMPLETED" && a.Status != "CANCELLED")
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionEntity>> GetBySearchTermAsync(string searchTerm)
        {
            var term = searchTerm.ToLower();
            return await _context.Actions
                .Include(a => a.ActionType)
                .Include(a => a.AssignedTo)
                .Include(a => a.CreatedBy)
                .Where(a => a.Title.ToLower().Contains(term) ||
                           (a.Description != null && a.Description.ToLower().Contains(term)) ||
                           a.ActionNumber.ToLower().Contains(term))
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionEntity>> GetActionsWithDependenciesAsync(int actionId)
        {
            return await _context.Actions
                .Include(a => a.ActionType)
                .Include(a => a.AssignedTo)
                .Include(a => a.Dependencies)
                    .ThenInclude(d => d.DependsOn)
                .Where(a => a.Id == actionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionEntity>> GetDependentActionsAsync(int actionId)
        {
            return await _context.Actions
                .Include(a => a.ActionType)
                .Include(a => a.AssignedTo)
                .Where(a => a.Dependencies.Any(d => d.DependsOnActionId == actionId))
                .ToListAsync();
        }

        public Task<IEnumerable<ActionEntity>> GetActionsByWorkflowInstanceAsync(int workflowInstanceId)
        {
            // Note: WorkflowInstanceId property doesn't exist in Action entity
            // This method returns empty list for now
            return Task.FromResult<IEnumerable<ActionEntity>>(new List<ActionEntity>());
        }

        public Task<IEnumerable<ActionEntity>> GetActionsByTemplateAsync(int templateId)
        {
            // Note: TemplateId property doesn't exist in Action entity  
            // This method returns empty list for now
            return Task.FromResult<IEnumerable<ActionEntity>>(new List<ActionEntity>());
        }

        public async Task<(IEnumerable<ActionEntity> Items, int TotalCount)> GetPagedAsync(int skip, int take, string? status = null, string? priority = null, int? assigneeId = null)
        {
            var query = _context.Actions
                .Include(a => a.ActionType)
                .Include(a => a.AssignedTo)
                .Include(a => a.CreatedBy)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(a => a.Status == status);

            if (!string.IsNullOrEmpty(priority))
                query = query.Where(a => a.Priority == priority);

            if (assigneeId.HasValue)
                query = query.Where(a => a.AssignedToId == assigneeId.Value);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(a => a.CreatedDate)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Actions.CountAsync();
        }

        public async Task<int> GetCountByStatusAsync(string status)
        {
            return await _context.Actions.CountAsync(a => a.Status == status);
        }

        public async Task<Dictionary<int, int>> GetCountByAssigneeAsync()
        {
            return await _context.Actions
                .Where(a => a.AssignedToId.HasValue)
                .GroupBy(a => a.AssignedToId!.Value)
                .Select(g => new { AssigneeId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.AssigneeId, x => x.Count);
        }

        public async Task<Dictionary<string, int>> GetStatusDistributionAsync()
        {
            return await _context.Actions
                .GroupBy(a => a.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);
        }

        public async Task<Dictionary<string, int>> GetPriorityDistributionAsync()
        {
            return await _context.Actions
                .GroupBy(a => a.Priority)
                .Select(g => new { Priority = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Priority, x => x.Count);
        }
    }

    public class ActionTypeRepository : IActionTypeRepository
    {
        private readonly ActionsDbContext _context;

        public ActionTypeRepository(ActionsDbContext context)
        {
            _context = context;
        }

        public async Task<ActionType?> GetByIdAsync(int id)
        {
            return await _context.ActionTypes
                .Include(at => at.Actions)
                .Include(at => at.ActionTemplates)
                .FirstOrDefaultAsync(at => at.Id == id);
        }

        public async Task<IEnumerable<ActionType>> GetAllAsync()
        {
            return await _context.ActionTypes
                .Where(at => at.IsActive)
                .ToListAsync();
        }

        public async Task<ActionType> CreateAsync(ActionType actionType)
        {
            actionType.CreatedDate = DateTime.UtcNow;
            actionType.ModifiedDate = DateTime.UtcNow;
            
            _context.ActionTypes.Add(actionType);
            await _context.SaveChangesAsync();
            return actionType;
        }

        public async Task<ActionType> UpdateAsync(ActionType actionType)
        {
            actionType.ModifiedDate = DateTime.UtcNow;
            
            _context.Entry(actionType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return actionType;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var actionType = await _context.ActionTypes.FindAsync(id);
            if (actionType == null) return false;

            actionType.IsActive = false;
            actionType.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ActionTypes.AnyAsync(at => at.Id == id && at.IsActive);
        }

        public async Task<IEnumerable<ActionType>> GetByCategoryAsync(string category)
        {
            return await _context.ActionTypes
                .Where(at => at.Category == category && at.IsActive)
                .ToListAsync();
        }

        public async Task<ActionType?> GetByNameAsync(string typeName)
        {
            return await _context.ActionTypes
                .FirstOrDefaultAsync(at => at.TypeName == typeName && at.IsActive);
        }

        public async Task<bool> IsInUseAsync(int id)
        {
            return await _context.Actions.AnyAsync(a => a.ActionTypeId == id);
        }

        public async Task DeleteAsync(ActionType actionType)
        {
            _context.ActionTypes.Remove(actionType);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ActionType>> GetActivesAsync()
        {
            return await _context.ActionTypes
                .Where(at => at.IsActive)
                .ToListAsync();
        }

        public async Task<(IEnumerable<ActionType> Items, int TotalCount)> GetPagedAsync(int skip, int take, string? category = null)
        {
            var query = _context.ActionTypes.AsQueryable();
            
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(at => at.Category == category);
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return (items, totalCount);
        }
    }

    public class UserRepository : IUserRepository
    {
        private readonly ActionsDbContext _context;

        public UserRepository(ActionsDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Team)
                .Include(u => u.AssignedActions)
                .Include(u => u.CreatedActions)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Team)
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Team)
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            user.CreatedDate = DateTime.UtcNow;
            user.ModifiedDate = DateTime.UtcNow;
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            user.ModifiedDate = DateTime.UtcNow;
            
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.IsActive = false;
            user.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id && u.IsActive);
        }

        public async Task<IEnumerable<User>> GetByTeamAsync(int teamId)
        {
            return await _context.Users
                .Include(u => u.Team)
                .Where(u => u.TeamId == teamId && u.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(string role)
        {
            return await _context.Users
                .Include(u => u.Team)
                .Where(u => u.Role == role && u.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetByDepartmentAsync(string department)
        {
            return await _context.Users
                .Include(u => u.Team)
                .Where(u => u.Department == department && u.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Team)
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
        {
            var query = _context.Users.Where(u => u.Email == email && u.IsActive);
            
            if (excludeUserId.HasValue)
                query = query.Where(u => u.Id != excludeUserId.Value);
                
            return await query.AnyAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<User> Items, int TotalCount)> GetPagedAsync(int skip, int take, string? searchTerm = null, int? teamId = null)
        {
            var query = _context.Users
                .Include(u => u.Team)
                .Where(u => u.IsActive)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var term = searchTerm.ToLower();
                query = query.Where(u => u.FirstName.ToLower().Contains(term) ||
                                        u.LastName.ToLower().Contains(term) ||
                                        u.Email.ToLower().Contains(term));
            }

            if (teamId.HasValue)
            {
                query = query.Where(u => u.TeamId == teamId.Value);
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return (items, totalCount);
        }
    }

    public class TeamRepository : ITeamRepository
    {
        private readonly ActionsDbContext _context;

        public TeamRepository(ActionsDbContext context)
        {
            _context = context;
        }

        public async Task<Team?> GetByIdAsync(int id)
        {
            return await _context.Teams
                .Include(t => t.Members)
                .Include(t => t.Manager)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await _context.Teams
                .Include(t => t.Manager)
                .Where(t => t.IsActive)
                .ToListAsync();
        }

        public async Task<Team> CreateAsync(Team team)
        {
            team.CreatedDate = DateTime.UtcNow;
            team.ModifiedDate = DateTime.UtcNow;
            
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return team;
        }

        public async Task<Team> UpdateAsync(Team team)
        {
            team.ModifiedDate = DateTime.UtcNow;
            
            _context.Entry(team).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return team;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return false;

            team.IsActive = false;
            team.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Teams.AnyAsync(t => t.Id == id && t.IsActive);
        }

        public async Task<IEnumerable<Team>> GetByDepartmentAsync(string department)
        {
            return await _context.Teams
                .Include(t => t.Manager)
                .Where(t => t.Department == department && t.IsActive)
                .ToListAsync();
        }

        public async Task<Team?> GetByNameAsync(string teamName)
        {
            return await _context.Teams
                .Include(t => t.Manager)
                .FirstOrDefaultAsync(t => t.TeamName == teamName && t.IsActive);
        }

        public async Task<IEnumerable<Team>> GetByManagerAsync(int managerId)
        {
            return await _context.Teams
                .Include(t => t.Manager)
                .Where(t => t.ManagerId == managerId && t.IsActive)
                .ToListAsync();
        }

        public async Task<bool> HasMembersAsync(int teamId)
        {
            return await _context.Users.AnyAsync(u => u.TeamId == teamId && u.IsActive);
        }

        public async Task DeleteAsync(Team team)
        {
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Team>> GetActiveTeamsAsync()
        {
            return await _context.Teams
                .Include(t => t.Manager)
                .Where(t => t.IsActive)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Team> Items, int TotalCount)> GetPagedAsync(int skip, int take, string? searchTerm = null)
        {
            var query = _context.Teams
                .Include(t => t.Manager)
                .Where(t => t.IsActive)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var term = searchTerm.ToLower();
                query = query.Where(t => t.TeamName.ToLower().Contains(term) ||
                                        (t.Department != null && t.Department.ToLower().Contains(term)));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}


