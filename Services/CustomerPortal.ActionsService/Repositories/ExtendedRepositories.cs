using CustomerPortal.ActionsService.Data;
using CustomerPortal.ActionsService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.ActionsService.Repositories
{
    public class ActionDependencyRepository : IActionDependencyRepository
    {
        private readonly ActionsDbContext _context;

        public ActionDependencyRepository(ActionsDbContext context)
        {
            _context = context;
        }

        public async Task<ActionDependency?> GetByIdAsync(int id)
        {
            return await _context.ActionDependencies
                .Include(ad => ad.DependentAction)
                .Include(ad => ad.DependsOn)
                .FirstOrDefaultAsync(ad => ad.Id == id);
        }

        public async Task<IEnumerable<ActionDependency>> GetAllAsync()
        {
            return await _context.ActionDependencies
                .Include(ad => ad.DependentAction)
                .Include(ad => ad.DependsOn)
                .ToListAsync();
        }

        public async Task<ActionDependency> CreateAsync(ActionDependency dependency)
        {
            dependency.CreatedDate = DateTime.UtcNow;
            dependency.ModifiedDate = DateTime.UtcNow;
            
            _context.ActionDependencies.Add(dependency);
            await _context.SaveChangesAsync();
            return dependency;
        }

        public async Task<ActionDependency> UpdateAsync(ActionDependency dependency)
        {
            dependency.ModifiedDate = DateTime.UtcNow;
            
            _context.Entry(dependency).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return dependency;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var dependency = await _context.ActionDependencies.FindAsync(id);
            if (dependency == null) return false;

            _context.ActionDependencies.Remove(dependency);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ActionDependencies.AnyAsync(ad => ad.Id == id);
        }

        public async Task<IEnumerable<ActionDependency>> GetByActionAsync(int actionId)
        {
            return await _context.ActionDependencies
                .Include(ad => ad.DependsOn)
                .Where(ad => ad.ActionId == actionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionDependency>> GetDependenciesOfActionAsync(int actionId)
        {
            return await _context.ActionDependencies
                .Include(ad => ad.DependentAction)
                .Where(ad => ad.DependsOnActionId == actionId)
                .ToListAsync();
        }

        public async Task<bool> WouldCreateCircularDependencyAsync(int actionId, int dependsOnActionId)
        {
            // Check if dependsOnActionId depends (directly or indirectly) on actionId
            var visited = new HashSet<int>();
            var stack = new Stack<int>();
            stack.Push(dependsOnActionId);

            while (stack.Count > 0)
            {
                var currentActionId = stack.Pop();
                
                if (currentActionId == actionId)
                    return true;

                if (visited.Contains(currentActionId))
                    continue;

                visited.Add(currentActionId);

                var dependencies = await _context.ActionDependencies
                    .Where(ad => ad.ActionId == currentActionId)
                    .Select(ad => ad.DependsOnActionId)
                    .ToListAsync();

                foreach (var depId in dependencies)
                {
                    if (!visited.Contains(depId))
                        stack.Push(depId);
                }
            }

            return false;
        }

        public async Task<bool> DependencyExistsAsync(int actionId, int dependsOnActionId)
        {
            return await _context.ActionDependencies
                .AnyAsync(ad => ad.ActionId == actionId && ad.DependsOnActionId == dependsOnActionId);
        }

        public async Task DeleteAsync(ActionDependency dependency)
        {
            _context.ActionDependencies.Remove(dependency);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ActionDependency>> GetByActionIdAsync(int actionId)
        {
            return await _context.ActionDependencies
                .Include(ad => ad.DependentAction)
                .Include(ad => ad.DependsOn)
                .Where(ad => ad.ActionId == actionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionDependency>> GetDependenciesForActionAsync(int actionId)
        {
            return await _context.ActionDependencies
                .Include(ad => ad.DependentAction)
                .Include(ad => ad.DependsOn)
                .Where(ad => ad.ActionId == actionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionDependency>> GetDependentActionsAsync(int actionId)
        {
            return await _context.ActionDependencies
                .Include(ad => ad.DependentAction)
                .Include(ad => ad.DependsOn)
                .Where(ad => ad.DependsOnActionId == actionId)
                .ToListAsync();
        }
    }

    public class ActionAttachmentRepository : IActionAttachmentRepository
    {
        private readonly ActionsDbContext _context;

        public ActionAttachmentRepository(ActionsDbContext context)
        {
            _context = context;
        }

        public async Task<ActionAttachment?> GetByIdAsync(int id)
        {
            return await _context.ActionAttachments
                .Include(aa => aa.Action)
                .Include(aa => aa.UploadedBy)
                .FirstOrDefaultAsync(aa => aa.Id == id);
        }

        public async Task<IEnumerable<ActionAttachment>> GetAllAsync()
        {
            return await _context.ActionAttachments
                .Include(aa => aa.Action)
                .Include(aa => aa.UploadedBy)
                .ToListAsync();
        }

        public async Task<ActionAttachment> CreateAsync(ActionAttachment attachment)
        {
            _context.ActionAttachments.Add(attachment);
            await _context.SaveChangesAsync();
            return attachment;
        }

        public async Task<ActionAttachment> UpdateAsync(ActionAttachment attachment)
        {
            _context.Entry(attachment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return attachment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var attachment = await _context.ActionAttachments.FindAsync(id);
            if (attachment == null) return false;

            _context.ActionAttachments.Remove(attachment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ActionAttachments.AnyAsync(aa => aa.Id == id);
        }

        public async Task<IEnumerable<ActionAttachment>> GetByActionAsync(int actionId)
        {
            return await _context.ActionAttachments
                .Include(aa => aa.UploadedBy)
                .Where(aa => aa.ActionId == actionId)
                .OrderByDescending(aa => aa.UploadDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionAttachment>> GetByUserAsync(int userId)
        {
            return await _context.ActionAttachments
                .Include(aa => aa.Action)
                .Where(aa => aa.UploadedById == userId)
                .OrderByDescending(aa => aa.UploadDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionAttachment>> GetByFileTypeAsync(string fileType)
        {
            return await _context.ActionAttachments
                .Include(aa => aa.Action)
                .Include(aa => aa.UploadedBy)
                .Where(aa => aa.FileType == fileType)
                .ToListAsync();
        }

        public async Task<long> GetTotalFileSizeByActionAsync(int actionId)
        {
            return await _context.ActionAttachments
                .Where(aa => aa.ActionId == actionId)
                .SumAsync(aa => aa.FileSize);
        }

        public async Task DeleteAsync(ActionAttachment attachment)
        {
            _context.ActionAttachments.Remove(attachment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ActionAttachment>> GetByActionIdAsync(int actionId)
        {
            return await _context.ActionAttachments
                .Where(aa => aa.ActionId == actionId)
                .ToListAsync();
        }
    }

    public class ActionCommentRepository : IActionCommentRepository
    {
        private readonly ActionsDbContext _context;

        public ActionCommentRepository(ActionsDbContext context)
        {
            _context = context;
        }

        public async Task<ActionComment?> GetByIdAsync(int id)
        {
            return await _context.ActionComments
                .Include(ac => ac.Action)
                .Include(ac => ac.User)
                .Include(ac => ac.ParentComment)
                .Include(ac => ac.Replies)
                .FirstOrDefaultAsync(ac => ac.Id == id);
        }

        public async Task<IEnumerable<ActionComment>> GetAllAsync()
        {
            return await _context.ActionComments
                .Include(ac => ac.Action)
                .Include(ac => ac.User)
                .ToListAsync();
        }

        public async Task<ActionComment> CreateAsync(ActionComment comment)
        {
            _context.ActionComments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<ActionComment> UpdateAsync(ActionComment comment)
        {
            _context.Entry(comment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment = await _context.ActionComments.FindAsync(id);
            if (comment == null) return false;

            _context.ActionComments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ActionComments.AnyAsync(ac => ac.Id == id);
        }

        public async Task<IEnumerable<ActionComment>> GetByActionAsync(int actionId)
        {
            return await _context.ActionComments
                .Include(ac => ac.User)
                .Include(ac => ac.Replies)
                    .ThenInclude(r => r.User)
                .Where(ac => ac.ActionId == actionId && ac.ParentCommentId == null)
                .OrderByDescending(ac => ac.CommentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionComment>> GetByUserAsync(int userId)
        {
            return await _context.ActionComments
                .Include(ac => ac.Action)
                .Where(ac => ac.UserId == userId)
                .OrderByDescending(ac => ac.CommentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionComment>> GetRepliesAsync(int parentCommentId)
        {
            return await _context.ActionComments
                .Include(ac => ac.User)
                .Where(ac => ac.ParentCommentId == parentCommentId)
                .OrderBy(ac => ac.CommentDate)
                .ToListAsync();
        }

        public async Task<int> GetCommentCountByActionAsync(int actionId)
        {
            return await _context.ActionComments
                .CountAsync(ac => ac.ActionId == actionId);
        }

        public async Task DeleteAsync(ActionComment comment)
        {
            _context.ActionComments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ActionComment>> GetByActionIdAsync(int actionId)
        {
            return await _context.ActionComments
                .Include(ac => ac.CreatedBy)
                .Where(ac => ac.ActionId == actionId)
                .OrderBy(ac => ac.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionComment>> GetByUserIdAsync(int userId)
        {
            return await _context.ActionComments
                .Include(ac => ac.Action)
                .Where(ac => ac.CreatedBy == userId)
                .OrderByDescending(ac => ac.CreatedDate)
                .ToListAsync();
        }
    }

    public class ActionHistoryRepository : IActionHistoryRepository
    {
        private readonly ActionsDbContext _context;

        public ActionHistoryRepository(ActionsDbContext context)
        {
            _context = context;
        }

        public async Task<ActionHistory?> GetByIdAsync(int id)
        {
            return await _context.ActionHistory
                .Include(ah => ah.Action)
                .Include(ah => ah.ChangedBy)
                .FirstOrDefaultAsync(ah => ah.Id == id);
        }

        public async Task<IEnumerable<ActionHistory>> GetAllAsync()
        {
            return await _context.ActionHistory
                .Include(ah => ah.Action)
                .Include(ah => ah.ChangedBy)
                .ToListAsync();
        }

        public async Task<ActionHistory> CreateAsync(ActionHistory history)
        {
            _context.ActionHistory.Add(history);
            await _context.SaveChangesAsync();
            return history;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var history = await _context.ActionHistory.FindAsync(id);
            if (history == null) return false;

            _context.ActionHistory.Remove(history);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ActionHistory.AnyAsync(ah => ah.Id == id);
        }

        public async Task<IEnumerable<ActionHistory>> GetByActionAsync(int actionId)
        {
            return await _context.ActionHistory
                .Include(ah => ah.ChangedBy)
                .Where(ah => ah.ActionId == actionId)
                .OrderByDescending(ah => ah.ChangeDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionHistory>> GetByUserAsync(int userId)
        {
            return await _context.ActionHistory
                .Include(ah => ah.Action)
                .Where(ah => ah.ChangedById == userId)
                .OrderByDescending(ah => ah.ChangeDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionHistory>> GetByChangeTypeAsync(string changeType)
        {
            return await _context.ActionHistory
                .Include(ah => ah.Action)
                .Include(ah => ah.ChangedBy)
                .Where(ah => ah.ChangeType == changeType)
                .OrderByDescending(ah => ah.ChangeDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionHistory>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ActionHistory
                .Include(ah => ah.Action)
                .Include(ah => ah.ChangedBy)
                .Where(ah => ah.ChangeDate >= startDate && ah.ChangeDate <= endDate)
                .OrderByDescending(ah => ah.ChangeDate)
                .ToListAsync();
        }

        public async Task<ActionHistory?> GetLatestChangeAsync(int actionId)
        {
            return await _context.ActionHistory
                .Include(ah => ah.ChangedBy)
                .Where(ah => ah.ActionId == actionId)
                .OrderByDescending(ah => ah.ChangeDate)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(ActionHistory history)
        {
            _context.ActionHistory.Remove(history);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ActionHistory>> GetByActionIdAsync(int actionId)
        {
            return await _context.ActionHistory
                .Include(ah => ah.ChangedBy)
                .Where(ah => ah.ActionId == actionId)
                .OrderByDescending(ah => ah.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionHistory>> GetByUserIdAsync(int userId)
        {
            return await _context.ActionHistory
                .Include(ah => ah.Action)
                .Where(ah => ah.ChangedById == userId)
                .OrderByDescending(ah => ah.CreatedDate)
                .ToListAsync();
        }
    }

    public class ActionTemplateRepository : IActionTemplateRepository
    {
        private readonly ActionsDbContext _context;

        public ActionTemplateRepository(ActionsDbContext context)
        {
            _context = context;
        }

        public async Task<ActionTemplate?> GetByIdAsync(int id)
        {
            return await _context.ActionTemplates
                .Include(at => at.ActionType)
                .FirstOrDefaultAsync(at => at.Id == id);
        }

        public async Task<IEnumerable<ActionTemplate>> GetAllAsync()
        {
            return await _context.ActionTemplates
                .Include(at => at.ActionType)
                .Where(at => at.IsActive)
                .ToListAsync();
        }

        public async Task<ActionTemplate> CreateAsync(ActionTemplate template)
        {
            template.CreatedDate = DateTime.UtcNow;
            template.ModifiedDate = DateTime.UtcNow;
            
            _context.ActionTemplates.Add(template);
            await _context.SaveChangesAsync();
            return template;
        }

        public async Task<ActionTemplate> UpdateAsync(ActionTemplate template)
        {
            template.ModifiedDate = DateTime.UtcNow;
            
            _context.Entry(template).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return template;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var template = await _context.ActionTemplates.FindAsync(id);
            if (template == null) return false;

            template.IsActive = false;
            template.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ActionTemplates.AnyAsync(at => at.Id == id && at.IsActive);
        }

        public async Task<IEnumerable<ActionTemplate>> GetByCategoryAsync(string category)
        {
            return await _context.ActionTemplates
                .Include(at => at.ActionType)
                .Where(at => at.Category == category && at.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionTemplate>> GetByActionTypeAsync(int actionTypeId)
        {
            return await _context.ActionTemplates
                .Include(at => at.ActionType)
                .Where(at => at.ActionTypeId == actionTypeId && at.IsActive)
                .ToListAsync();
        }

        public async Task<ActionTemplate?> GetByNameAsync(string templateName)
        {
            return await _context.ActionTemplates
                .Include(at => at.ActionType)
                .FirstOrDefaultAsync(at => at.TemplateName == templateName && at.IsActive);
        }

        public async Task<IEnumerable<ActionTemplate>> GetActiveTemplatesAsync()
        {
            return await _context.ActionTemplates
                .Include(at => at.ActionType)
                .Where(at => at.IsActive)
                .OrderBy(at => at.TemplateName)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionTemplate>> GetMostUsedTemplatesAsync(int count = 10)
        {
            return await _context.ActionTemplates
                .Include(at => at.ActionType)
                .Where(at => at.IsActive)
                .OrderByDescending(at => at.UsageCount)
                .Take(count)
                .ToListAsync();
        }

        public async Task IncrementUsageCountAsync(int templateId)
        {
            var template = await _context.ActionTemplates.FindAsync(templateId);
            if (template != null)
            {
                template.UsageCount++;
                template.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(ActionTemplate template)
        {
            _context.ActionTemplates.Remove(template);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<ActionTemplate> Items, int TotalCount)> GetPagedAsync(int skip, int take, string? category = null)
        {
            var query = _context.ActionTemplates.AsQueryable();
            
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

    public class WorkflowRepository : IWorkflowRepository
    {
        private readonly ActionsDbContext _context;

        public WorkflowRepository(ActionsDbContext context)
        {
            _context = context;
        }

        public async Task<Workflow?> GetByIdAsync(int id)
        {
            return await _context.Workflows
                .Include(w => w.Steps.OrderBy(s => s.StepNumber))
                .Include(w => w.Instances)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<Workflow>> GetAllAsync()
        {
            return await _context.Workflows
                .Where(w => w.IsActive)
                .ToListAsync();
        }

        public async Task<Workflow> CreateAsync(Workflow workflow)
        {
            workflow.CreatedDate = DateTime.UtcNow;
            workflow.ModifiedDate = DateTime.UtcNow;
            
            _context.Workflows.Add(workflow);
            await _context.SaveChangesAsync();
            return workflow;
        }

        public async Task<Workflow> UpdateAsync(Workflow workflow)
        {
            workflow.ModifiedDate = DateTime.UtcNow;
            
            _context.Entry(workflow).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return workflow;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var workflow = await _context.Workflows.FindAsync(id);
            if (workflow == null) return false;

            workflow.IsActive = false;
            workflow.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Workflows.AnyAsync(w => w.Id == id && w.IsActive);
        }

        public async Task<Workflow?> GetByNameAsync(string workflowName)
        {
            return await _context.Workflows
                .Include(w => w.Steps.OrderBy(s => s.StepNumber))
                .FirstOrDefaultAsync(w => w.WorkflowName == workflowName && w.IsActive);
        }

        public async Task<IEnumerable<Workflow>> GetActiveWorkflowsAsync()
        {
            return await _context.Workflows
                .Where(w => w.IsActive)
                .OrderBy(w => w.WorkflowName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Workflow>> GetByTriggerTypeAsync(string triggerType)
        {
            return await _context.Workflows
                .Where(w => w.TriggerType == triggerType && w.IsActive)
                .ToListAsync();
        }

        public async Task<bool> HasInstancesAsync(int workflowId)
        {
            return await _context.WorkflowInstances.AnyAsync(wi => wi.WorkflowId == workflowId);
        }

        public async Task DeleteAsync(Workflow workflow)
        {
            _context.Workflows.Remove(workflow);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Workflow> Items, int TotalCount)> GetPagedAsync(int skip, int take, string? searchTerm = null)
        {
            var query = _context.Workflows.AsQueryable();
            
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var term = searchTerm.ToLower();
                query = query.Where(w => w.WorkflowName.ToLower().Contains(term) ||
                                        (w.Description != null && w.Description.ToLower().Contains(term)));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return (items, totalCount);
        }
    }

    public class WorkflowStepRepository : IWorkflowStepRepository
    {
        private readonly ActionsDbContext _context;

        public WorkflowStepRepository(ActionsDbContext context)
        {
            _context = context;
        }

        public async Task<WorkflowStep?> GetByIdAsync(int id)
        {
            return await _context.WorkflowSteps
                .Include(ws => ws.Workflow)
                .Include(ws => ws.ActionType)
                .FirstOrDefaultAsync(ws => ws.Id == id);
        }

        public async Task<IEnumerable<WorkflowStep>> GetAllAsync()
        {
            return await _context.WorkflowSteps
                .Include(ws => ws.Workflow)
                .Include(ws => ws.ActionType)
                .ToListAsync();
        }

        public async Task<WorkflowStep> CreateAsync(WorkflowStep step)
        {
            step.CreatedDate = DateTime.UtcNow;
            step.ModifiedDate = DateTime.UtcNow;
            
            _context.WorkflowSteps.Add(step);
            await _context.SaveChangesAsync();
            return step;
        }

        public async Task<WorkflowStep> UpdateAsync(WorkflowStep step)
        {
            step.ModifiedDate = DateTime.UtcNow;
            
            _context.Entry(step).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return step;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var step = await _context.WorkflowSteps.FindAsync(id);
            if (step == null) return false;

            _context.WorkflowSteps.Remove(step);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.WorkflowSteps.AnyAsync(ws => ws.Id == id);
        }

        public async Task<IEnumerable<WorkflowStep>> GetByWorkflowAsync(int workflowId)
        {
            return await _context.WorkflowSteps
                .Include(ws => ws.ActionType)
                .Where(ws => ws.WorkflowId == workflowId)
                .OrderBy(ws => ws.StepNumber)
                .ToListAsync();
        }

        public async Task<WorkflowStep?> GetByWorkflowAndStepNumberAsync(int workflowId, int stepNumber)
        {
            return await _context.WorkflowSteps
                .Include(ws => ws.ActionType)
                .FirstOrDefaultAsync(ws => ws.WorkflowId == workflowId && ws.StepNumber == stepNumber);
        }

        public async Task<WorkflowStep?> GetNextStepAsync(int workflowId, int currentStepNumber)
        {
            return await _context.WorkflowSteps
                .Include(ws => ws.ActionType)
                .Where(ws => ws.WorkflowId == workflowId && ws.StepNumber > currentStepNumber)
                .OrderBy(ws => ws.StepNumber)
                .FirstOrDefaultAsync();
        }

        public async Task<WorkflowStep?> GetFirstStepAsync(int workflowId)
        {
            return await _context.WorkflowSteps
                .Include(ws => ws.ActionType)
                .Where(ws => ws.WorkflowId == workflowId)
                .OrderBy(ws => ws.StepNumber)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> StepNumberExistsAsync(int workflowId, int stepNumber, int? excludeStepId = null)
        {
            var query = _context.WorkflowSteps
                .Where(ws => ws.WorkflowId == workflowId && ws.StepNumber == stepNumber);
                
            if (excludeStepId.HasValue)
                query = query.Where(ws => ws.Id != excludeStepId.Value);
                
            return await query.AnyAsync();
        }

        public async Task DeleteAsync(WorkflowStep step)
        {
            _context.WorkflowSteps.Remove(step);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<WorkflowStep>> GetByWorkflowIdAsync(int workflowId)
        {
            return await _context.WorkflowSteps
                .Where(ws => ws.WorkflowId == workflowId)
                .OrderBy(ws => ws.StepNumber)
                .ToListAsync();
        }
    }

    public class WorkflowInstanceRepository : IWorkflowInstanceRepository
    {
        private readonly ActionsDbContext _context;

        public WorkflowInstanceRepository(ActionsDbContext context)
        {
            _context = context;
        }

        public async Task<WorkflowInstance?> GetByIdAsync(int id)
        {
            return await _context.WorkflowInstances
                .Include(wi => wi.Workflow)
                    .ThenInclude(w => w.Steps.OrderBy(s => s.StepNumber))
                .Include(wi => wi.StartedBy)
                .FirstOrDefaultAsync(wi => wi.Id == id);
        }

        public async Task<WorkflowInstance?> GetByInstanceNumberAsync(string instanceNumber)
        {
            return await _context.WorkflowInstances
                .Include(wi => wi.Workflow)
                .Include(wi => wi.StartedBy)
                .FirstOrDefaultAsync(wi => wi.InstanceNumber == instanceNumber);
        }

        public async Task<IEnumerable<WorkflowInstance>> GetAllAsync()
        {
            return await _context.WorkflowInstances
                .Include(wi => wi.Workflow)
                .Include(wi => wi.StartedBy)
                .ToListAsync();
        }

        public async Task<WorkflowInstance> CreateAsync(WorkflowInstance instance)
        {
            _context.WorkflowInstances.Add(instance);
            await _context.SaveChangesAsync();
            return instance;
        }

        public async Task<WorkflowInstance> UpdateAsync(WorkflowInstance instance)
        {
            _context.Entry(instance).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return instance;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var instance = await _context.WorkflowInstances.FindAsync(id);
            if (instance == null) return false;

            _context.WorkflowInstances.Remove(instance);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.WorkflowInstances.AnyAsync(wi => wi.Id == id);
        }

        public async Task<IEnumerable<WorkflowInstance>> GetByWorkflowAsync(int workflowId)
        {
            return await _context.WorkflowInstances
                .Include(wi => wi.StartedBy)
                .Where(wi => wi.WorkflowId == workflowId)
                .OrderByDescending(wi => wi.StartedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkflowInstance>> GetByStatusAsync(string status)
        {
            return await _context.WorkflowInstances
                .Include(wi => wi.Workflow)
                .Include(wi => wi.StartedBy)
                .Where(wi => wi.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkflowInstance>> GetByStarterAsync(int userId)
        {
            return await _context.WorkflowInstances
                .Include(wi => wi.Workflow)
                .Where(wi => wi.StartedById == userId)
                .OrderByDescending(wi => wi.StartedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkflowInstance>> GetActiveInstancesAsync()
        {
            return await _context.WorkflowInstances
                .Include(wi => wi.Workflow)
                .Include(wi => wi.StartedBy)
                .Where(wi => wi.Status == "ACTIVE" || wi.Status == "IN_PROGRESS")
                .ToListAsync();
        }

        public async Task<bool> InstanceNumberExistsAsync(string instanceNumber)
        {
            return await _context.WorkflowInstances.AnyAsync(wi => wi.InstanceNumber == instanceNumber);
        }

        public async Task DeleteAsync(WorkflowInstance instance)
        {
            _context.WorkflowInstances.Remove(instance);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<WorkflowInstance>> GetByWorkflowIdAsync(int workflowId)
        {
            return await _context.WorkflowInstances
                .Include(wi => wi.Workflow)
                .Where(wi => wi.WorkflowId == workflowId)
                .OrderByDescending(wi => wi.StartedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkflowInstance>> GetByStartedByAsync(int userId)
        {
            return await _context.WorkflowInstances
                .Include(wi => wi.Workflow)
                .Include(wi => wi.StartedBy)
                .Where(wi => wi.StartedById == userId)
                .OrderByDescending(wi => wi.StartedDate)
                .ToListAsync();
        }

        public async Task<(IEnumerable<WorkflowInstance> Items, int TotalCount)> GetPagedAsync(int skip, int take, string? status = null)
        {
            var query = _context.WorkflowInstances
                .Include(wi => wi.Workflow)
                .Include(wi => wi.StartedBy)
                .AsQueryable();
            
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(wi => wi.Status == status);
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(wi => wi.StartedDate)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
