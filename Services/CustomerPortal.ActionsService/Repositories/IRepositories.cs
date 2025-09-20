using CustomerPortal.ActionsService.Entities;
using ActionEntity = CustomerPortal.ActionsService.Entities.Action;

namespace CustomerPortal.ActionsService.Repositories
{
    public interface IActionRepository
    {
        // Basic CRUD operations
        Task<ActionEntity?> GetByIdAsync(int id);
        Task<ActionEntity?> GetByActionNumberAsync(string actionNumber);
        Task<IEnumerable<ActionEntity>> GetAllAsync();
        Task<ActionEntity> CreateAsync(ActionEntity action);
        Task<ActionEntity> UpdateAsync(ActionEntity action);
        Task DeleteAsync(ActionEntity action);
        Task<bool> DeleteAsync(int id);

        // Query operations
        Task<IEnumerable<ActionEntity>> GetByAssigneeAsync(int userId);
        Task<IEnumerable<ActionEntity>> GetByCreatorAsync(int userId);
        Task<IEnumerable<ActionEntity>> GetByStatusAsync(string status);
        Task<IEnumerable<ActionEntity>> GetByPriorityAsync(string priority);
        Task<IEnumerable<ActionEntity>> GetByActionTypeAsync(int actionTypeId);
        Task<IEnumerable<ActionEntity>> GetByTeamAsync(int teamId);
        Task<IEnumerable<ActionEntity>> GetByDueDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ActionEntity>> GetOverdueAsync();
        Task<IEnumerable<ActionEntity>> GetBySearchTermAsync(string searchTerm);

        // Dependency operations
        Task<IEnumerable<ActionEntity>> GetActionsWithDependenciesAsync(int actionId);
        Task<IEnumerable<ActionEntity>> GetDependentActionsAsync(int actionId);
        Task<IEnumerable<ActionEntity>> GetActionsByWorkflowInstanceAsync(int workflowInstanceId);
        Task<IEnumerable<ActionEntity>> GetActionsByTemplateAsync(int templateId);

        // Paging operations
        Task<(IEnumerable<ActionEntity> Items, int TotalCount)> GetPagedAsync(int skip, int take, string? status = null, string? priority = null, int? assigneeId = null);
        
        // Analytics operations
        Task<int> GetTotalCountAsync();
        Task<Dictionary<string, int>> GetStatusDistributionAsync();
        Task<Dictionary<string, int>> GetPriorityDistributionAsync();
        Task<Dictionary<int, int>> GetCountByAssigneeAsync();
    }

    public interface IActionTypeRepository
    {
        Task<ActionType?> GetByIdAsync(int id);
        Task<ActionType?> GetByNameAsync(string typeName);
        Task<IEnumerable<ActionType>> GetAllAsync();
        Task<ActionType> CreateAsync(ActionType actionType);
        Task<ActionType> UpdateAsync(ActionType actionType);
        Task DeleteAsync(ActionType actionType);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<ActionType>> GetActivesAsync();
        Task<IEnumerable<ActionType>> GetByCategoryAsync(string category);
        Task<(IEnumerable<ActionType> Items, int TotalCount)> GetPagedAsync(int skip, int take, string? category = null);
    }

    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<IEnumerable<User>> GetByTeamAsync(int teamId);
        Task<IEnumerable<User>> GetByRoleAsync(string role);
        Task<IEnumerable<User>> GetByDepartmentAsync(string department);
        Task<(IEnumerable<User> Items, int TotalCount)> GetPagedAsync(int skip, int take, string? searchTerm = null, int? teamId = null);
    }

    public interface ITeamRepository
    {
        Task<Team?> GetByIdAsync(int id);
        Task<Team?> GetByNameAsync(string teamName);
        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team> CreateAsync(Team team);
        Task<Team> UpdateAsync(Team team);
        Task DeleteAsync(Team team);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<Team>> GetActiveTeamsAsync();
        Task<IEnumerable<Team>> GetByDepartmentAsync(string department);
        Task<IEnumerable<Team>> GetByManagerAsync(int managerId);
        Task<(IEnumerable<Team> Items, int TotalCount)> GetPagedAsync(int skip, int take, string? searchTerm = null);
    }

    public interface IActionDependencyRepository
    {
        Task<ActionDependency?> GetByIdAsync(int id);
        Task<IEnumerable<ActionDependency>> GetAllAsync();
        Task<ActionDependency> CreateAsync(ActionDependency dependency);
        Task<ActionDependency> UpdateAsync(ActionDependency dependency);
        Task DeleteAsync(ActionDependency dependency);
        Task<bool> DeleteAsync(int id);
        
        Task<IEnumerable<ActionDependency>> GetByActionIdAsync(int actionId);
        Task<IEnumerable<ActionDependency>> GetDependenciesForActionAsync(int actionId);
        Task<IEnumerable<ActionDependency>> GetDependentActionsAsync(int actionId);
        
        // Validation operations
        Task<bool> WouldCreateCircularDependencyAsync(int actionId, int dependsOnActionId);
    }

    public interface IActionAttachmentRepository
    {
        Task<ActionAttachment?> GetByIdAsync(int id);
        Task<IEnumerable<ActionAttachment>> GetAllAsync();
        Task<ActionAttachment> CreateAsync(ActionAttachment attachment);
        Task<ActionAttachment> UpdateAsync(ActionAttachment attachment);
        Task DeleteAsync(ActionAttachment attachment);
        
        Task<IEnumerable<ActionAttachment>> GetByActionIdAsync(int actionId);
        Task<IEnumerable<ActionAttachment>> GetByFileTypeAsync(string fileType);
    }

    public interface IActionCommentRepository
    {
        Task<ActionComment?> GetByIdAsync(int id);
        Task<IEnumerable<ActionComment>> GetAllAsync();
        Task<ActionComment> CreateAsync(ActionComment comment);
        Task<ActionComment> UpdateAsync(ActionComment comment);
        Task DeleteAsync(ActionComment comment);
        Task<bool> DeleteAsync(int id);
        
        Task<IEnumerable<ActionComment>> GetByActionIdAsync(int actionId);
        Task<IEnumerable<ActionComment>> GetByUserIdAsync(int userId);
        Task<IEnumerable<ActionComment>> GetRepliesAsync(int parentCommentId);
    }

    public interface IActionHistoryRepository
    {
        Task<ActionHistory?> GetByIdAsync(int id);
        Task<IEnumerable<ActionHistory>> GetAllAsync();
        Task<ActionHistory> CreateAsync(ActionHistory history);
        Task DeleteAsync(ActionHistory history);
        
        Task<IEnumerable<ActionHistory>> GetByActionIdAsync(int actionId);
        Task<IEnumerable<ActionHistory>> GetByUserIdAsync(int userId);
        Task<IEnumerable<ActionHistory>> GetByChangeTypeAsync(string changeType);
    }

    public interface IActionTemplateRepository
    {
        Task<ActionTemplate?> GetByIdAsync(int id);
        Task<ActionTemplate?> GetByNameAsync(string templateName);
        Task<IEnumerable<ActionTemplate>> GetAllAsync();
        Task<ActionTemplate> CreateAsync(ActionTemplate template);
        Task<ActionTemplate> UpdateAsync(ActionTemplate template);
        Task DeleteAsync(ActionTemplate template);
        Task<bool> DeleteAsync(int id);
        
        Task<IEnumerable<ActionTemplate>> GetByCategoryAsync(string category);
        Task<IEnumerable<ActionTemplate>> GetByActionTypeAsync(int actionTypeId);
        Task<IEnumerable<ActionTemplate>> GetActiveTemplatesAsync();
        Task<(IEnumerable<ActionTemplate> Items, int TotalCount)> GetPagedAsync(int skip, int take, string? category = null);
        
        // Usage tracking
        Task<IEnumerable<ActionTemplate>> GetMostUsedTemplatesAsync(int count = 10);
        Task IncrementUsageCountAsync(int templateId);
    }

    public interface IWorkflowRepository
    {
        Task<Workflow?> GetByIdAsync(int id);
        Task<Workflow?> GetByNameAsync(string workflowName);
        Task<IEnumerable<Workflow>> GetAllAsync();
        Task<Workflow> CreateAsync(Workflow workflow);
        Task<Workflow> UpdateAsync(Workflow workflow);
        Task DeleteAsync(Workflow workflow);
        Task<bool> DeleteAsync(int id);
        
        Task<IEnumerable<Workflow>> GetActiveWorkflowsAsync();
        Task<IEnumerable<Workflow>> GetByTriggerTypeAsync(string triggerType);
        Task<(IEnumerable<Workflow> Items, int TotalCount)> GetPagedAsync(int skip, int take, string? searchTerm = null);
    }

    public interface IWorkflowStepRepository
    {
        Task<WorkflowStep?> GetByIdAsync(int id);
        Task<IEnumerable<WorkflowStep>> GetAllAsync();
        Task<WorkflowStep> CreateAsync(WorkflowStep step);
        Task<WorkflowStep> UpdateAsync(WorkflowStep step);
        Task DeleteAsync(WorkflowStep step);
        
        Task<IEnumerable<WorkflowStep>> GetByWorkflowIdAsync(int workflowId);
        Task<WorkflowStep?> GetByWorkflowAndStepNumberAsync(int workflowId, int stepNumber);
        
        // Workflow operations
        Task<IEnumerable<WorkflowStep>> GetByWorkflowAsync(int workflowId);
    }

    public interface IWorkflowInstanceRepository
    {
        Task<WorkflowInstance?> GetByIdAsync(int id);
        Task<WorkflowInstance?> GetByInstanceNumberAsync(string instanceNumber);
        Task<IEnumerable<WorkflowInstance>> GetAllAsync();
        Task<WorkflowInstance> CreateAsync(WorkflowInstance instance);
        Task<WorkflowInstance> UpdateAsync(WorkflowInstance instance);
        Task DeleteAsync(WorkflowInstance instance);
        
        Task<IEnumerable<WorkflowInstance>> GetByWorkflowIdAsync(int workflowId);
        Task<IEnumerable<WorkflowInstance>> GetByStatusAsync(string status);
        Task<IEnumerable<WorkflowInstance>> GetByStartedByAsync(int userId);
        Task<(IEnumerable<WorkflowInstance> Items, int TotalCount)> GetPagedAsync(int skip, int take, string? status = null);
        
        // Workflow operations
        Task<IEnumerable<WorkflowInstance>> GetByWorkflowAsync(int workflowId);
        Task<IEnumerable<WorkflowInstance>> GetActiveInstancesAsync();
    }
}