using CustomerPortal.ActionsService.Repositories;
using CustomerPortal.ActionsService.GraphQL.Types;
using CustomerPortal.ActionsService.Entities;
using ActionEntity = CustomerPortal.ActionsService.Entities.Action;
using HotChocolate;
using AutoMapper;

namespace CustomerPortal.ActionsService.GraphQL
{
    public class Mutation
    {
        private readonly IMapper _mapper;

        public Mutation(IMapper mapper)
        {
            _mapper = mapper;
        }

        // Action Mutations
        [GraphQLDescription("Create a new action")]
        public async Task<ActionGraphQLType> CreateActionAsync(
            [Service] IActionRepository actionRepository,
            [Service] IActionHistoryRepository historyRepository,
            CreateActionInput input,
            int currentUserId = 1)
        {
            var action = _mapper.Map<ActionEntity>(input);
            action.CreatedById = currentUserId;
            action.Status = "NOT_STARTED";
            action.Progress = 0;

            var createdAction = await actionRepository.CreateAsync(action);

            // Log creation in history
            await historyRepository.CreateAsync(new ActionHistory
            {
                ActionId = createdAction.Id,
                ChangeType = "CREATED",
                NewValue = "Action created",
                ChangedById = currentUserId,
                ChangeDate = DateTime.UtcNow
            });

            return _mapper.Map<ActionGraphQLType>(createdAction);
        }

        [GraphQLDescription("Update an existing action")]
        public async Task<ActionGraphQLType?> UpdateActionAsync(
            [Service] IActionRepository actionRepository,
            [Service] IActionHistoryRepository historyRepository,
            UpdateActionInput input,
            int currentUserId = 1)
        {
            var existingAction = await actionRepository.GetByIdAsync(input.Id);
            if (existingAction == null)
                throw new GraphQLException("Action not found");

            // Track changes for history
            var changes = new List<(string Field, string OldValue, string NewValue)>();

            if (input.Title != null && input.Title != existingAction.Title)
            {
                changes.Add(("Title", existingAction.Title, input.Title));
                existingAction.Title = input.Title;
            }

            if (input.Description != null && input.Description != existingAction.Description)
            {
                changes.Add(("Description", existingAction.Description ?? "", input.Description));
                existingAction.Description = input.Description;
            }

            if (input.Status != null && input.Status != existingAction.Status)
            {
                changes.Add(("Status", existingAction.Status, input.Status));
                existingAction.Status = input.Status;
                
                if (input.Status == "COMPLETED" && existingAction.CompletedDate == null)
                {
                    existingAction.CompletedDate = DateTime.UtcNow;
                    existingAction.Progress = 100;
                }
            }

            if (input.Priority != null && input.Priority != existingAction.Priority)
            {
                changes.Add(("Priority", existingAction.Priority, input.Priority));
                existingAction.Priority = input.Priority;
            }

            if (input.AssignedToId.HasValue && input.AssignedToId != existingAction.AssignedToId)
            {
                changes.Add(("AssignedTo", existingAction.AssignedToId?.ToString() ?? "", input.AssignedToId.ToString()));
                existingAction.AssignedToId = input.AssignedToId;
            }

            if (input.Progress.HasValue && input.Progress != existingAction.Progress)
            {
                changes.Add(("Progress", existingAction.Progress.ToString(), input.Progress.ToString()));
                existingAction.Progress = input.Progress.Value;
            }

            if (input.DueDate.HasValue && input.DueDate != existingAction.DueDate)
            {
                changes.Add(("DueDate", existingAction.DueDate?.ToString() ?? "", input.DueDate.ToString()));
                existingAction.DueDate = input.DueDate;
            }

            if (input.ActualHours.HasValue && input.ActualHours != existingAction.ActualHours)
            {
                changes.Add(("ActualHours", existingAction.ActualHours?.ToString() ?? "", input.ActualHours.ToString()));
                existingAction.ActualHours = input.ActualHours;
            }

            var updatedAction = await actionRepository.UpdateAsync(existingAction);

            // Log changes in history
            foreach (var (field, oldValue, newValue) in changes)
            {
                await historyRepository.CreateAsync(new ActionHistory
                {
                    ActionId = updatedAction.Id,
                    ChangeType = "UPDATED",
                    FieldName = field,
                    OldValue = oldValue,
                    NewValue = newValue,
                    ChangedById = currentUserId,
                    ChangeDate = DateTime.UtcNow
                });
            }

            return _mapper.Map<ActionGraphQLType>(updatedAction);
        }

        [GraphQLDescription("Delete an action")]
        public async Task<bool> DeleteActionAsync(
            [Service] IActionRepository actionRepository,
            [Service] IActionHistoryRepository historyRepository,
            int id,
            int currentUserId = 1)
        {
            var action = await actionRepository.GetByIdAsync(id);
            if (action == null)
                return false;

            // Log deletion in history before deleting
            await historyRepository.CreateAsync(new ActionHistory
            {
                ActionId = id,
                ChangeType = "DELETED",
                NewValue = "Action deleted",
                ChangedById = currentUserId,
                ChangeDate = DateTime.UtcNow
            });

            return await actionRepository.DeleteAsync(id);
        }

        // Action Type Mutations
        [GraphQLDescription("Create a new action type")]
        public async Task<ActionTypeGraphQLType> CreateActionTypeAsync(
            [Service] IActionTypeRepository actionTypeRepository,
            CreateActionTypeInput input)
        {
            var actionType = _mapper.Map<Entities.ActionType>(input);
            var createdActionType = await actionTypeRepository.CreateAsync(actionType);
            return _mapper.Map<ActionTypeGraphQLType>(createdActionType);
        }

        [GraphQLDescription("Update an existing action type")]
        public async Task<ActionTypeGraphQLType?> UpdateActionTypeAsync(
            [Service] IActionTypeRepository actionTypeRepository,
            UpdateActionTypeInput input)
        {
            var existingActionType = await actionTypeRepository.GetByIdAsync(input.Id);
            if (existingActionType == null)
                throw new GraphQLException("Action type not found");

            _mapper.Map(input, existingActionType);
            var updatedActionType = await actionTypeRepository.UpdateAsync(existingActionType);
            return _mapper.Map<ActionTypeGraphQLType>(updatedActionType);
        }

        [GraphQLDescription("Delete an action type")]
        public async Task<bool> DeleteActionTypeAsync(
            [Service] IActionTypeRepository actionTypeRepository,
            int id)
        {
            return await actionTypeRepository.DeleteAsync(id);
        }

        // User Mutations
        [GraphQLDescription("Create a new user")]
        public async Task<UserType> CreateUserAsync(
            [Service] IUserRepository userRepository,
            CreateUserInput input)
        {
            var user = _mapper.Map<User>(input);
            var createdUser = await userRepository.CreateAsync(user);
            return _mapper.Map<UserType>(createdUser);
        }

        [GraphQLDescription("Update an existing user")]
        public async Task<UserType?> UpdateUserAsync(
            [Service] IUserRepository userRepository,
            UpdateUserInput input)
        {
            var existingUser = await userRepository.GetByIdAsync(input.Id);
            if (existingUser == null)
                throw new GraphQLException("User not found");

            _mapper.Map(input, existingUser);
            var updatedUser = await userRepository.UpdateAsync(existingUser);
            return _mapper.Map<UserType>(updatedUser);
        }

        [GraphQLDescription("Delete a user")]
        public async Task<bool> DeleteUserAsync(
            [Service] IUserRepository userRepository,
            int id)
        {
            return await userRepository.DeleteAsync(id);
        }

        // Team Mutations
        [GraphQLDescription("Create a new team")]
        public async Task<TeamType> CreateTeamAsync(
            [Service] ITeamRepository teamRepository,
            CreateTeamInput input)
        {
            var team = _mapper.Map<Team>(input);
            var createdTeam = await teamRepository.CreateAsync(team);
            return _mapper.Map<TeamType>(createdTeam);
        }

        [GraphQLDescription("Update an existing team")]
        public async Task<TeamType?> UpdateTeamAsync(
            [Service] ITeamRepository teamRepository,
            UpdateTeamInput input)
        {
            var existingTeam = await teamRepository.GetByIdAsync(input.Id);
            if (existingTeam == null)
                throw new GraphQLException("Team not found");

            _mapper.Map(input, existingTeam);
            var updatedTeam = await teamRepository.UpdateAsync(existingTeam);
            return _mapper.Map<TeamType>(updatedTeam);
        }

        [GraphQLDescription("Delete a team")]
        public async Task<bool> DeleteTeamAsync(
            [Service] ITeamRepository teamRepository,
            int id)
        {
            return await teamRepository.DeleteAsync(id);
        }

        // Action Dependency Mutations
        [GraphQLDescription("Create an action dependency")]
        public async Task<ActionDependencyType> CreateActionDependencyAsync(
            [Service] IActionDependencyRepository dependencyRepository,
            CreateActionDependencyInput input)
        {
            // Check for circular dependency
            var wouldCreateCircular = await dependencyRepository.WouldCreateCircularDependencyAsync(
                input.ActionId, input.DependsOnActionId);
            
            if (wouldCreateCircular)
                throw new GraphQLException("Creating this dependency would result in a circular dependency");

            var dependency = _mapper.Map<ActionDependency>(input);
            var createdDependency = await dependencyRepository.CreateAsync(dependency);
            return _mapper.Map<ActionDependencyType>(createdDependency);
        }

        [GraphQLDescription("Update an action dependency")]
        public async Task<ActionDependencyType?> UpdateActionDependencyAsync(
            [Service] IActionDependencyRepository dependencyRepository,
            UpdateActionDependencyInput input)
        {
            var existingDependency = await dependencyRepository.GetByIdAsync(input.Id);
            if (existingDependency == null)
                throw new GraphQLException("Action dependency not found");

            _mapper.Map(input, existingDependency);
            var updatedDependency = await dependencyRepository.UpdateAsync(existingDependency);
            return _mapper.Map<ActionDependencyType>(updatedDependency);
        }

        [GraphQLDescription("Delete an action dependency")]
        public async Task<bool> DeleteActionDependencyAsync(
            [Service] IActionDependencyRepository dependencyRepository,
            int id)
        {
            return await dependencyRepository.DeleteAsync(id);
        }

        // Action Comment Mutations
        [GraphQLDescription("Create an action comment")]
        public async Task<ActionCommentType> CreateActionCommentAsync(
            [Service] IActionCommentRepository commentRepository,
            CreateActionCommentInput input,
            int currentUserId = 1)
        {
            var comment = _mapper.Map<ActionComment>(input);
            comment.UserId = currentUserId;
            comment.CommentDate = DateTime.UtcNow;

            var createdComment = await commentRepository.CreateAsync(comment);
            return _mapper.Map<ActionCommentType>(createdComment);
        }

        [GraphQLDescription("Update an action comment")]
        public async Task<ActionCommentType?> UpdateActionCommentAsync(
            [Service] IActionCommentRepository commentRepository,
            UpdateActionCommentInput input,
            int currentUserId = 1)
        {
            var existingComment = await commentRepository.GetByIdAsync(input.Id);
            if (existingComment == null)
                throw new GraphQLException("Comment not found");

            // Only allow the comment author to edit
            if (existingComment.UserId != currentUserId)
                throw new GraphQLException("You can only edit your own comments");

            existingComment.Comment = input.Comment;
            var updatedComment = await commentRepository.UpdateAsync(existingComment);
            return _mapper.Map<ActionCommentType>(updatedComment);
        }

        [GraphQLDescription("Delete an action comment")]
        public async Task<bool> DeleteActionCommentAsync(
            [Service] IActionCommentRepository commentRepository,
            int id,
            int currentUserId = 1)
        {
            var comment = await commentRepository.GetByIdAsync(id);
            if (comment == null)
                return false;

            // Only allow the comment author to delete
            if (comment.UserId != currentUserId)
                throw new GraphQLException("You can only delete your own comments");

            return await commentRepository.DeleteAsync(id);
        }

        // Action Template Mutations
        [GraphQLDescription("Create an action template")]
        public async Task<ActionTemplateType> CreateActionTemplateAsync(
            [Service] IActionTemplateRepository templateRepository,
            CreateActionTemplateInput input)
        {
            var template = _mapper.Map<ActionTemplate>(input);
            var createdTemplate = await templateRepository.CreateAsync(template);
            return _mapper.Map<ActionTemplateType>(createdTemplate);
        }

        [GraphQLDescription("Update an action template")]
        public async Task<ActionTemplateType?> UpdateActionTemplateAsync(
            [Service] IActionTemplateRepository templateRepository,
            UpdateActionTemplateInput input)
        {
            var existingTemplate = await templateRepository.GetByIdAsync(input.Id);
            if (existingTemplate == null)
                throw new GraphQLException("Action template not found");

            _mapper.Map(input, existingTemplate);
            var updatedTemplate = await templateRepository.UpdateAsync(existingTemplate);
            return _mapper.Map<ActionTemplateType>(updatedTemplate);
        }

        [GraphQLDescription("Delete an action template")]
        public async Task<bool> DeleteActionTemplateAsync(
            [Service] IActionTemplateRepository templateRepository,
            int id)
        {
            return await templateRepository.DeleteAsync(id);
        }

        [GraphQLDescription("Create action from template")]
        public async Task<ActionGraphQLType> CreateActionFromTemplateAsync(
            [Service] IActionRepository actionRepository,
            [Service] IActionTemplateRepository templateRepository,
            [Service] IActionHistoryRepository historyRepository,
            int templateId,
            int? assignedToId = null,
            DateTime? dueDate = null,
            int currentUserId = 1)
        {
            var template = await templateRepository.GetByIdAsync(templateId);
            if (template == null)
                throw new GraphQLException("Template not found");

            // Generate action number (simplified)
            var actionNumber = $"ACT-{DateTime.UtcNow:yyyy}-{DateTime.UtcNow.Ticks % 10000:D4}";

            var action = new ActionEntity
            {
                ActionNumber = actionNumber,
                Title = template.DefaultTitle,
                Description = template.DefaultDescription,
                ActionTypeId = template.ActionTypeId,
                AssignedToId = assignedToId,
                CreatedById = currentUserId,
                Priority = template.DefaultPriority,
                Status = "NOT_STARTED",
                EstimatedHours = template.DefaultEstimatedHours,
                DueDate = dueDate,
                Progress = 0
            };

            var createdAction = await actionRepository.CreateAsync(action);

            // Increment template usage count
            await templateRepository.IncrementUsageCountAsync(templateId);

            // Log creation in history
            await historyRepository.CreateAsync(new ActionHistory
            {
                ActionId = createdAction.Id,
                ChangeType = "CREATED_FROM_TEMPLATE",
                NewValue = $"Action created from template: {template.TemplateName}",
                ChangedById = currentUserId,
                ChangeDate = DateTime.UtcNow
            });

            return _mapper.Map<ActionGraphQLType>(createdAction);
        }

        // Workflow Mutations
        [GraphQLDescription("Create a workflow")]
        public async Task<WorkflowType> CreateWorkflowAsync(
            [Service] IWorkflowRepository workflowRepository,
            [Service] IWorkflowStepRepository stepRepository,
            CreateWorkflowInput input)
        {
            var workflow = _mapper.Map<Workflow>(input);
            var createdWorkflow = await workflowRepository.CreateAsync(workflow);

            // Create workflow steps
            foreach (var stepInput in input.Steps)
            {
                var step = _mapper.Map<WorkflowStep>(stepInput);
                step.WorkflowId = createdWorkflow.Id;
                await stepRepository.CreateAsync(step);
            }

            return _mapper.Map<WorkflowType>(createdWorkflow);
        }

        [GraphQLDescription("Update a workflow")]
        public async Task<WorkflowType?> UpdateWorkflowAsync(
            [Service] IWorkflowRepository workflowRepository,
            UpdateWorkflowInput input)
        {
            var existingWorkflow = await workflowRepository.GetByIdAsync(input.Id);
            if (existingWorkflow == null)
                throw new GraphQLException("Workflow not found");

            _mapper.Map(input, existingWorkflow);
            var updatedWorkflow = await workflowRepository.UpdateAsync(existingWorkflow);
            return _mapper.Map<WorkflowType>(updatedWorkflow);
        }

        [GraphQLDescription("Delete a workflow")]
        public async Task<bool> DeleteWorkflowAsync(
            [Service] IWorkflowRepository workflowRepository,
            int id)
        {
            return await workflowRepository.DeleteAsync(id);
        }

        // Workflow Instance Mutations
        [GraphQLDescription("Start a workflow instance")]
        public async Task<WorkflowInstanceType> StartWorkflowInstanceAsync(
            [Service] IWorkflowInstanceRepository instanceRepository,
            [Service] IWorkflowRepository workflowRepository,
            CreateWorkflowInstanceInput input,
            int currentUserId = 1)
        {
            var workflow = await workflowRepository.GetByIdAsync(input.WorkflowId);
            if (workflow == null)
                throw new GraphQLException("Workflow not found");

            var instance = new WorkflowInstance
            {
                WorkflowId = input.WorkflowId,
                InstanceNumber = input.InstanceNumber,
                Status = "ACTIVE",
                StartedDate = DateTime.UtcNow,
                StartedById = currentUserId,
                Context = input.Context
            };

            var createdInstance = await instanceRepository.CreateAsync(instance);
            return _mapper.Map<WorkflowInstanceType>(createdInstance);
        }

        [GraphQLDescription("Update a workflow instance")]
        public async Task<WorkflowInstanceType?> UpdateWorkflowInstanceAsync(
            [Service] IWorkflowInstanceRepository instanceRepository,
            UpdateWorkflowInstanceInput input)
        {
            var existingInstance = await instanceRepository.GetByIdAsync(input.Id);
            if (existingInstance == null)
                throw new GraphQLException("Workflow instance not found");

            if (input.Status != null)
                existingInstance.Status = input.Status;

            if (input.CompletedDate.HasValue)
                existingInstance.CompletedDate = input.CompletedDate;

            if (input.Context != null)
                existingInstance.Context = input.Context;

            var updatedInstance = await instanceRepository.UpdateAsync(existingInstance);
            return _mapper.Map<WorkflowInstanceType>(updatedInstance);
        }

        // Bulk Operations
        [GraphQLDescription("Bulk update action status")]
        public async Task<BulkOperationResult> BulkUpdateActionStatusAsync(
            [Service] IActionRepository actionRepository,
            [Service] IActionHistoryRepository historyRepository,
            List<int> actionIds,
            string newStatus,
            int currentUserId = 1)
        {
            var result = new BulkOperationResult
            {
                ProcessedCount = actionIds.Count
            };

            foreach (var actionId in actionIds)
            {
                try
                {
                    var action = await actionRepository.GetByIdAsync(actionId);
                    if (action != null)
                    {
                        var oldStatus = action.Status;
                        action.Status = newStatus;
                        
                        if (newStatus == "COMPLETED")
                        {
                            action.CompletedDate = DateTime.UtcNow;
                            action.Progress = 100;
                        }

                        await actionRepository.UpdateAsync(action);

                        // Log change in history
                        await historyRepository.CreateAsync(new ActionHistory
                        {
                            ActionId = actionId,
                            ChangeType = "BULK_STATUS_UPDATE",
                            FieldName = "Status",
                            OldValue = oldStatus,
                            NewValue = newStatus,
                            ChangedById = currentUserId,
                            ChangeDate = DateTime.UtcNow
                        });

                        result.SuccessCount++;
                    }
                    else
                    {
                        result.ErrorCount++;
                        result.Errors.Add($"Action {actionId} not found");
                    }
                }
                catch (Exception ex)
                {
                    result.ErrorCount++;
                    result.Errors.Add($"Action {actionId}: {ex.Message}");
                }
            }

            return result;
        }

        [GraphQLDescription("Bulk assign actions")]
        public async Task<BulkOperationResult> BulkAssignActionsAsync(
            [Service] IActionRepository actionRepository,
            [Service] IActionHistoryRepository historyRepository,
            List<int> actionIds,
            int assigneeId,
            int currentUserId = 1)
        {
            var result = new BulkOperationResult
            {
                ProcessedCount = actionIds.Count
            };

            foreach (var actionId in actionIds)
            {
                try
                {
                    var action = await actionRepository.GetByIdAsync(actionId);
                    if (action != null)
                    {
                        var oldAssigneeId = action.AssignedToId;
                        action.AssignedToId = assigneeId;
                        await actionRepository.UpdateAsync(action);

                        // Log change in history
                        await historyRepository.CreateAsync(new ActionHistory
                        {
                            ActionId = actionId,
                            ChangeType = "BULK_ASSIGNMENT",
                            FieldName = "AssignedTo",
                            OldValue = oldAssigneeId?.ToString() ?? "",
                            NewValue = assigneeId.ToString(),
                            ChangedById = currentUserId,
                            ChangeDate = DateTime.UtcNow
                        });

                        result.SuccessCount++;
                    }
                    else
                    {
                        result.ErrorCount++;
                        result.Errors.Add($"Action {actionId} not found");
                    }
                }
                catch (Exception ex)
                {
                    result.ErrorCount++;
                    result.Errors.Add($"Action {actionId}: {ex.Message}");
                }
            }

            return result;
        }
    }
}
