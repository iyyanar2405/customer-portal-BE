using CustomerPortalAPI.Modules.Actions.Entities;
using CustomerPortalAPI.Modules.Actions.Repositories;
using CustomerPortalAPI.Modules.Actions.GraphQL;
using HotChocolate;

namespace CustomerPortalAPI.Modules.Actions.GraphQL
{
    [ExtendObjectType("Query")]
    public class ActionQueries
    {
        public async Task<IEnumerable<ActionType>> GetActionsAsync(
            [Service] IActionRepository actionRepository,
            ActionFilterInput? filter = null,
            ActionSortInput? sort = null,
            int skip = 0,
            int take = 50)
        {
            var actions = await actionRepository.GetAllAsync();
            
            // Apply filters
            if (filter != null)
            {
                actions = ApplyFilters(actions, filter);
            }

            // Apply sorting
            if (sort != null)
            {
                actions = ApplySorting(actions, sort);
            }

            // Apply pagination
            actions = actions.Skip(skip).Take(take);

            return actions.Select(MapToActionType);
        }

        public async Task<ActionType?> GetActionByIdAsync(
            int id,
            [Service] IActionRepository actionRepository)
        {
            var action = await actionRepository.GetActionByIdWithDetailsAsync(id);
            return action != null ? MapToActionType(action) : null;
        }

        public async Task<IEnumerable<ActionType>> GetActionsByUserAsync(
            int userId,
            [Service] IActionRepository actionRepository)
        {
            var actions = await actionRepository.GetActionsByUserAsync(userId);
            return actions.Select(MapToActionType);
        }

        public async Task<IEnumerable<ActionType>> GetActionsByCompanyAsync(
            int companyId,
            [Service] IActionRepository actionRepository)
        {
            var actions = await actionRepository.GetActionsByCompanyAsync(companyId);
            return actions.Select(MapToActionType);
        }

        public async Task<IEnumerable<ActionType>> GetOverdueActionsAsync(
            [Service] IActionRepository actionRepository)
        {
            var actions = await actionRepository.GetOverdueActionsAsync();
            return actions.Select(MapToActionType);
        }

        public async Task<ActionStatisticsType> GetActionStatisticsAsync(
            [Service] IActionRepository actionRepository)
        {
            var totalActions = await actionRepository.CountAsync();
            var completedActions = await actionRepository.GetActionCountByStatusAsync("Completed");
            var pendingActions = await actionRepository.GetActionCountByStatusAsync("In Progress");
            var overdueActions = await actionRepository.GetOverdueActionCountAsync();

            return new ActionStatisticsType(
                TotalActions: totalActions,
                CompletedActions: completedActions,
                PendingActions: pendingActions,
                OverdueActions: overdueActions,
                HighPriorityActions: 0, // You can implement these counts
                MediumPriorityActions: 0,
                LowPriorityActions: 0
            );
        }

        private static IEnumerable<Entities.Action> ApplyFilters(IEnumerable<Entities.Action> actions, ActionFilterInput filter)
        {
            var query = actions.AsQueryable();

            if (!string.IsNullOrEmpty(filter.ActionName))
                query = query.Where(a => a.ActionName.Contains(filter.ActionName));

            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(a => a.Status == filter.Status);

            if (!string.IsNullOrEmpty(filter.Priority))
                query = query.Where(a => a.Priority == filter.Priority);

            if (filter.AssignedToUserId.HasValue)
                query = query.Where(a => a.AssignedToUserId == filter.AssignedToUserId);

            if (filter.CompanyId.HasValue)
                query = query.Where(a => a.CompanyId == filter.CompanyId);

            if (filter.SiteId.HasValue)
                query = query.Where(a => a.SiteId == filter.SiteId);

            if (filter.DueDateFrom.HasValue)
                query = query.Where(a => a.DueDate >= filter.DueDateFrom);

            if (filter.DueDateTo.HasValue)
                query = query.Where(a => a.DueDate <= filter.DueDateTo);

            if (filter.IsOverdue == true)
                query = query.Where(a => a.DueDate < DateTime.UtcNow && a.CompletedDate == null);

            if (filter.IsCompleted == true)
                query = query.Where(a => a.CompletedDate != null);
            else if (filter.IsCompleted == false)
                query = query.Where(a => a.CompletedDate == null);

            return query;
        }

        private static IEnumerable<Entities.Action> ApplySorting(IEnumerable<Entities.Action> actions, ActionSortInput sort)
        {
            return sort.Field switch
            {
                ActionSortField.ActionName => sort.Direction == SortDirection.Asc 
                    ? actions.OrderBy(a => a.ActionName) 
                    : actions.OrderByDescending(a => a.ActionName),
                ActionSortField.Priority => sort.Direction == SortDirection.Asc 
                    ? actions.OrderBy(a => a.Priority) 
                    : actions.OrderByDescending(a => a.Priority),
                ActionSortField.Status => sort.Direction == SortDirection.Asc 
                    ? actions.OrderBy(a => a.Status) 
                    : actions.OrderByDescending(a => a.Status),
                ActionSortField.DueDate => sort.Direction == SortDirection.Asc 
                    ? actions.OrderBy(a => a.DueDate) 
                    : actions.OrderByDescending(a => a.DueDate),
                ActionSortField.CreatedDate => sort.Direction == SortDirection.Asc 
                    ? actions.OrderBy(a => a.CreatedDate) 
                    : actions.OrderByDescending(a => a.CreatedDate),
                ActionSortField.ModifiedDate => sort.Direction == SortDirection.Asc 
                    ? actions.OrderBy(a => a.ModifiedDate) 
                    : actions.OrderByDescending(a => a.ModifiedDate),
                _ => actions.OrderBy(a => a.ActionName)
            };
        }

        private static ActionType MapToActionType(Entities.Action action)
        {
            return new ActionType(
                Id: action.Id,
                ActionName: action.ActionName,
                Description: action.Description,
                ActionTypeValue: action.ActionType,
                Priority: action.Priority,
                Status: action.Status,
                AssignedToUserId: action.AssignedToUserId,
                CompanyId: action.CompanyId,
                SiteId: action.SiteId,
                AuditId: action.AuditId,
                FindingId: action.FindingId,
                DueDate: action.DueDate,
                CompletedDate: action.CompletedDate,
                CreatedDate: action.CreatedDate,
                CreatedBy: action.CreatedBy,
                ModifiedDate: action.ModifiedDate,
                ModifiedBy: action.ModifiedBy,
                Comments: action.Comments,
                IsActive: action.IsActive
            );
        }
    }
}