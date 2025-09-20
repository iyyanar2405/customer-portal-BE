using CustomerPortal.ActionsService.Repositories;
using CustomerPortal.ActionsService.GraphQL.Types;
using HotChocolate;
using AutoMapper;

namespace CustomerPortal.ActionsService.GraphQL
{
    public class Query
    {
        private readonly IMapper _mapper;

        public Query(IMapper mapper)
        {
            _mapper = mapper;
        }

        // Action Queries
        [GraphQLDescription("Get an action by ID")]
        public async Task<ActionGraphQLType?> GetActionAsync(
            [Service] IActionRepository actionRepository,
            int id)
        {
            var action = await actionRepository.GetByIdAsync(id);
            return action != null ? _mapper.Map<ActionGraphQLType>(action) : null;
        }

        [GraphQLDescription("Get an action by action number")]
        public async Task<ActionGraphQLType?> GetActionByNumberAsync(
            [Service] IActionRepository actionRepository,
            string actionNumber)
        {
            var action = await actionRepository.GetByActionNumberAsync(actionNumber);
            return action != null ? _mapper.Map<ActionGraphQLType>(action) : null;
        }

        [GraphQLDescription("Get all actions")]
        public async Task<List<ActionGraphQLType>> GetActionsAsync(
            [Service] IActionRepository actionRepository)
        {
            var actions = await actionRepository.GetAllAsync();
            return _mapper.Map<List<ActionGraphQLType>>(actions);
        }

        [GraphQLDescription("Search actions with filters and pagination")]
        public async Task<ActionPagedResult> SearchActionsAsync(
            [Service] IActionRepository actionRepository,
            ActionSearchInput input)
        {
            if (!string.IsNullOrEmpty(input.SearchTerm))
            {
                var searchResults = await actionRepository.GetBySearchTermAsync(input.SearchTerm);
                var searchItems = _mapper.Map<List<ActionGraphQLType>>(searchResults);
                return new ActionPagedResult
                {
                    Items = searchItems.Skip(input.Skip).Take(input.Take).ToList(),
                    TotalCount = searchItems.Count,
                    PageSize = input.Take,
                    CurrentPage = (input.Skip / input.Take) + 1,
                    TotalPages = (int)Math.Ceiling((double)searchItems.Count / input.Take)
                };
            }

            var (items, totalCount) = await actionRepository.GetPagedAsync(
                input.Skip, input.Take, input.Status, input.Priority, input.AssignedToId);

            return new ActionPagedResult
            {
                Items = _mapper.Map<List<ActionGraphQLType>>(items),
                TotalCount = totalCount,
                PageSize = input.Take,
                CurrentPage = (input.Skip / input.Take) + 1,
                TotalPages = (int)Math.Ceiling((double)totalCount / input.Take)
            };
        }

        [GraphQLDescription("Get actions assigned to a user")]
        public async Task<List<ActionGraphQLType>> GetActionsByAssigneeAsync(
            [Service] IActionRepository actionRepository,
            int userId)
        {
            var actions = await actionRepository.GetByAssigneeAsync(userId);
            return _mapper.Map<List<ActionGraphQLType>>(actions);
        }

        [GraphQLDescription("Get actions created by a user")]
        public async Task<List<ActionGraphQLType>> GetActionsByCreatorAsync(
            [Service] IActionRepository actionRepository,
            int userId)
        {
            var actions = await actionRepository.GetByCreatorAsync(userId);
            return _mapper.Map<List<ActionGraphQLType>>(actions);
        }

        [GraphQLDescription("Get actions by status")]
        public async Task<List<ActionGraphQLType>> GetActionsByStatusAsync(
            [Service] IActionRepository actionRepository,
            string status)
        {
            var actions = await actionRepository.GetByStatusAsync(status);
            return _mapper.Map<List<ActionGraphQLType>>(actions);
        }

        [GraphQLDescription("Get actions by priority")]
        public async Task<List<ActionGraphQLType>> GetActionsByPriorityAsync(
            [Service] IActionRepository actionRepository,
            string priority)
        {
            var actions = await actionRepository.GetByPriorityAsync(priority);
            return _mapper.Map<List<ActionGraphQLType>>(actions);
        }

        [GraphQLDescription("Get overdue actions")]
        public async Task<List<ActionGraphQLType>> GetOverdueActionsAsync(
            [Service] IActionRepository actionRepository)
        {
            var actions = await actionRepository.GetOverdueAsync();
            return _mapper.Map<List<ActionGraphQLType>>(actions);
        }

        [GraphQLDescription("Get actions by team")]
        public async Task<List<ActionGraphQLType>> GetActionsByTeamAsync(
            [Service] IActionRepository actionRepository,
            int teamId)
        {
            var actions = await actionRepository.GetByTeamAsync(teamId);
            return _mapper.Map<List<ActionGraphQLType>>(actions);
        }

        [GraphQLDescription("Get actions by date range")]
        public async Task<List<ActionGraphQLType>> GetActionsByDateRangeAsync(
            [Service] IActionRepository actionRepository,
            DateTime startDate,
            DateTime endDate)
        {
            var actions = await actionRepository.GetByDueDateRangeAsync(startDate, endDate);
            return _mapper.Map<List<ActionGraphQLType>>(actions);
        }

        // Action Type Queries
        [GraphQLDescription("Get an action type by ID")]
        public async Task<ActionTypeGraphQLType?> GetActionTypeAsync(
            [Service] IActionTypeRepository actionTypeRepository,
            int id)
        {
            var actionType = await actionTypeRepository.GetByIdAsync(id);
            return actionType != null ? _mapper.Map<ActionTypeGraphQLType>(actionType) : null;
        }

        [GraphQLDescription("Get all action types")]
        public async Task<List<ActionTypeGraphQLType>> GetActionTypesAsync(
            [Service] IActionTypeRepository actionTypeRepository)
        {
            var actionTypes = await actionTypeRepository.GetAllAsync();
            return _mapper.Map<List<ActionTypeGraphQLType>>(actionTypes);
        }

        [GraphQLDescription("Get action types by category")]
        public async Task<List<ActionTypeGraphQLType>> GetActionTypesByCategoryAsync(
            [Service] IActionTypeRepository actionTypeRepository,
            string category)
        {
            var actionTypes = await actionTypeRepository.GetByCategoryAsync(category);
            return _mapper.Map<List<ActionTypeGraphQLType>>(actionTypes);
        }

        // User Queries
        [GraphQLDescription("Get a user by ID")]
        public async Task<UserType?> GetUserAsync(
            [Service] IUserRepository userRepository,
            int id)
        {
            var user = await userRepository.GetByIdAsync(id);
            return user != null ? _mapper.Map<UserType>(user) : null;
        }

        [GraphQLDescription("Get a user by email")]
        public async Task<UserType?> GetUserByEmailAsync(
            [Service] IUserRepository userRepository,
            string email)
        {
            var user = await userRepository.GetByEmailAsync(email);
            return user != null ? _mapper.Map<UserType>(user) : null;
        }

        [GraphQLDescription("Get all users")]
        public async Task<List<UserType>> GetUsersAsync(
            [Service] IUserRepository userRepository)
        {
            var users = await userRepository.GetAllAsync();
            return _mapper.Map<List<UserType>>(users);
        }

        [GraphQLDescription("Get users by team")]
        public async Task<List<UserType>> GetUsersByTeamAsync(
            [Service] IUserRepository userRepository,
            int teamId)
        {
            var users = await userRepository.GetByTeamAsync(teamId);
            return _mapper.Map<List<UserType>>(users);
        }

        [GraphQLDescription("Get users by role")]
        public async Task<List<UserType>> GetUsersByRoleAsync(
            [Service] IUserRepository userRepository,
            string role)
        {
            var users = await userRepository.GetByRoleAsync(role);
            return _mapper.Map<List<UserType>>(users);
        }

        // Team Queries
        [GraphQLDescription("Get a team by ID")]
        public async Task<TeamType?> GetTeamAsync(
            [Service] ITeamRepository teamRepository,
            int id)
        {
            var team = await teamRepository.GetByIdAsync(id);
            return team != null ? _mapper.Map<TeamType>(team) : null;
        }

        [GraphQLDescription("Get all teams")]
        public async Task<List<TeamType>> GetTeamsAsync(
            [Service] ITeamRepository teamRepository)
        {
            var teams = await teamRepository.GetAllAsync();
            return _mapper.Map<List<TeamType>>(teams);
        }

        [GraphQLDescription("Get teams by department")]
        public async Task<List<TeamType>> GetTeamsByDepartmentAsync(
            [Service] ITeamRepository teamRepository,
            string department)
        {
            var teams = await teamRepository.GetByDepartmentAsync(department);
            return _mapper.Map<List<TeamType>>(teams);
        }

        // Action Dependency Queries
        [GraphQLDescription("Get action dependencies for an action")]
        public async Task<List<ActionDependencyType>> GetActionDependenciesAsync(
            [Service] IActionDependencyRepository dependencyRepository,
            int actionId)
        {
            var dependencies = await dependencyRepository.GetByActionIdAsync(actionId);
            return _mapper.Map<List<ActionDependencyType>>(dependencies);
        }

        [GraphQLDescription("Get actions that depend on a specific action")]
        public async Task<List<ActionDependencyType>> GetDependentActionsAsync(
            [Service] IActionDependencyRepository dependencyRepository,
            int actionId)
        {
            var dependencies = await dependencyRepository.GetDependenciesForActionAsync(actionId);
            return _mapper.Map<List<ActionDependencyType>>(dependencies);
        }

        // Action Attachment Queries
        [GraphQLDescription("Get attachments for an action")]
        public async Task<List<ActionAttachmentType>> GetActionAttachmentsAsync(
            [Service] IActionAttachmentRepository attachmentRepository,
            int actionId)
        {
            var attachments = await attachmentRepository.GetByActionIdAsync(actionId);
            return _mapper.Map<List<ActionAttachmentType>>(attachments);
        }

        // Action Comment Queries
        [GraphQLDescription("Get comments for an action")]
        public async Task<List<ActionCommentType>> GetActionCommentsAsync(
            [Service] IActionCommentRepository commentRepository,
            int actionId)
        {
            var comments = await commentRepository.GetByActionIdAsync(actionId);
            return _mapper.Map<List<ActionCommentType>>(comments);
        }

        // Action History Queries
        [GraphQLDescription("Get history for an action")]
        public async Task<List<ActionHistoryType>> GetActionHistoryAsync(
            [Service] IActionHistoryRepository historyRepository,
            int actionId)
        {
            var history = await historyRepository.GetByActionIdAsync(actionId);
            return _mapper.Map<List<ActionHistoryType>>(history);
        }

        // Action Template Queries
        [GraphQLDescription("Get an action template by ID")]
        public async Task<ActionTemplateType?> GetActionTemplateAsync(
            [Service] IActionTemplateRepository templateRepository,
            int id)
        {
            var template = await templateRepository.GetByIdAsync(id);
            return template != null ? _mapper.Map<ActionTemplateType>(template) : null;
        }

        [GraphQLDescription("Get all action templates")]
        public async Task<List<ActionTemplateType>> GetActionTemplatesAsync(
            [Service] IActionTemplateRepository templateRepository)
        {
            var templates = await templateRepository.GetAllAsync();
            return _mapper.Map<List<ActionTemplateType>>(templates);
        }

        [GraphQLDescription("Get action templates by category")]
        public async Task<List<ActionTemplateType>> GetActionTemplatesByCategoryAsync(
            [Service] IActionTemplateRepository templateRepository,
            string category)
        {
            var templates = await templateRepository.GetByCategoryAsync(category);
            return _mapper.Map<List<ActionTemplateType>>(templates);
        }

        [GraphQLDescription("Get most used action templates")]
        public async Task<List<ActionTemplateType>> GetMostUsedActionTemplatesAsync(
            [Service] IActionTemplateRepository templateRepository,
            int count = 10)
        {
            var templates = await templateRepository.GetMostUsedTemplatesAsync(count);
            return _mapper.Map<List<ActionTemplateType>>(templates);
        }

        // Workflow Queries
        [GraphQLDescription("Get a workflow by ID")]
        public async Task<WorkflowType?> GetWorkflowAsync(
            [Service] IWorkflowRepository workflowRepository,
            int id)
        {
            var workflow = await workflowRepository.GetByIdAsync(id);
            return workflow != null ? _mapper.Map<WorkflowType>(workflow) : null;
        }

        [GraphQLDescription("Get all workflows")]
        public async Task<List<WorkflowType>> GetWorkflowsAsync(
            [Service] IWorkflowRepository workflowRepository)
        {
            var workflows = await workflowRepository.GetAllAsync();
            return _mapper.Map<List<WorkflowType>>(workflows);
        }

        [GraphQLDescription("Get active workflows")]
        public async Task<List<WorkflowType>> GetActiveWorkflowsAsync(
            [Service] IWorkflowRepository workflowRepository)
        {
            var workflows = await workflowRepository.GetActiveWorkflowsAsync();
            return _mapper.Map<List<WorkflowType>>(workflows);
        }

        // Workflow Step Queries
        [GraphQLDescription("Get workflow steps for a workflow")]
        public async Task<List<WorkflowStepType>> GetWorkflowStepsAsync(
            [Service] IWorkflowStepRepository stepRepository,
            int workflowId)
        {
            var steps = await stepRepository.GetByWorkflowAsync(workflowId);
            return _mapper.Map<List<WorkflowStepType>>(steps);
        }

        // Workflow Instance Queries
        [GraphQLDescription("Get a workflow instance by ID")]
        public async Task<WorkflowInstanceType?> GetWorkflowInstanceAsync(
            [Service] IWorkflowInstanceRepository instanceRepository,
            int id)
        {
            var instance = await instanceRepository.GetByIdAsync(id);
            return instance != null ? _mapper.Map<WorkflowInstanceType>(instance) : null;
        }

        [GraphQLDescription("Get workflow instances for a workflow")]
        public async Task<List<WorkflowInstanceType>> GetWorkflowInstancesAsync(
            [Service] IWorkflowInstanceRepository instanceRepository,
            int workflowId)
        {
            var instances = await instanceRepository.GetByWorkflowAsync(workflowId);
            return _mapper.Map<List<WorkflowInstanceType>>(instances);
        }

        [GraphQLDescription("Get active workflow instances")]
        public async Task<List<WorkflowInstanceType>> GetActiveWorkflowInstancesAsync(
            [Service] IWorkflowInstanceRepository instanceRepository)
        {
            var instances = await instanceRepository.GetActiveInstancesAsync();
            return _mapper.Map<List<WorkflowInstanceType>>(instances);
        }

        // Statistics Queries
        [GraphQLDescription("Get action statistics")]
        public async Task<ActionStatistics> GetActionStatisticsAsync(
            [Service] IActionRepository actionRepository,
            ActionStatisticsInput? input = null)
        {
            var totalCount = await actionRepository.GetTotalCountAsync();
            var statusDistribution = await actionRepository.GetStatusDistributionAsync();
            var priorityDistribution = await actionRepository.GetPriorityDistributionAsync();
            var overdueActions = await actionRepository.GetOverdueAsync();
            var countByAssignee = await actionRepository.GetCountByAssigneeAsync();

            var stats = new ActionStatistics
            {
                TotalActions = totalCount,
                StatusDistribution = statusDistribution,
                PriorityDistribution = priorityDistribution,
                OverdueActions = overdueActions.Count(),
                CompletedThisMonth = statusDistribution.GetValueOrDefault("COMPLETED", 0),
                AssignedToUser = input?.UserId != null ? countByAssignee.GetValueOrDefault(input.UserId.Value, 0) : 0,
                AverageCompletionDays = 7.5 // This would need actual calculation based on completed actions
            };

            return stats;
        }
    }
}
