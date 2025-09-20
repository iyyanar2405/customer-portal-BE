using CustomerPortalAPI.Modules.Actions.Entities;
using CustomerPortalAPI.Modules.Actions.Repositories;
using CustomerPortalAPI.Modules.Actions.GraphQL;
using HotChocolate;

namespace CustomerPortalAPI.Modules.Actions.GraphQL
{
    [ExtendObjectType("Mutation")]
    public class ActionMutations
    {
        public async Task<CreateActionPayload> CreateActionAsync(
            ActionInput input,
            [Service] IActionRepository actionRepository)
        {
            try
            {
                var action = new Entities.Action
                {
                    ActionName = input.ActionName,
                    Description = input.Description,
                    ActionType = input.ActionType,
                    Priority = input.Priority,
                    Status = input.Status ?? "New",
                    AssignedToUserId = input.AssignedToUserId,
                    CompanyId = input.CompanyId,
                    SiteId = input.SiteId,
                    AuditId = input.AuditId,
                    FindingId = input.FindingId,
                    DueDate = input.DueDate,
                    Comments = input.Comments,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };

                var createdAction = await actionRepository.AddAsync(action);
                await actionRepository.SaveChangesAsync();

                var actionType = MapToActionType(createdAction);
                return new CreateActionPayload(actionType, null);
            }
            catch (Exception ex)
            {
                return new CreateActionPayload(null, ex.Message);
            }
        }

        public async Task<UpdateActionPayload> UpdateActionAsync(
            UpdateActionInput input,
            [Service] IActionRepository actionRepository)
        {
            try
            {
                var action = await actionRepository.GetByIdAsync(input.Id);
                if (action == null)
                {
                    return new UpdateActionPayload(null, "Action not found");
                }

                // Update fields only if provided
                if (input.ActionName != null) action.ActionName = input.ActionName;
                if (input.Description != null) action.Description = input.Description;
                if (input.ActionType != null) action.ActionType = input.ActionType;
                if (input.Priority != null) action.Priority = input.Priority;
                if (input.Status != null) action.Status = input.Status;
                if (input.AssignedToUserId.HasValue) action.AssignedToUserId = input.AssignedToUserId;
                if (input.CompanyId.HasValue) action.CompanyId = input.CompanyId;
                if (input.SiteId.HasValue) action.SiteId = input.SiteId;
                if (input.AuditId.HasValue) action.AuditId = input.AuditId;
                if (input.FindingId.HasValue) action.FindingId = input.FindingId;
                if (input.DueDate.HasValue) action.DueDate = input.DueDate;
                if (input.Comments != null) action.Comments = input.Comments;

                action.ModifiedDate = DateTime.UtcNow;

                await actionRepository.UpdateAsync(action);
                await actionRepository.SaveChangesAsync();

                var actionType = MapToActionType(action);
                return new UpdateActionPayload(actionType, null);
            }
            catch (Exception ex)
            {
                return new UpdateActionPayload(null, ex.Message);
            }
        }

        public async Task<CompleteActionPayload> CompleteActionAsync(
            CompleteActionInput input,
            [Service] IActionRepository actionRepository)
        {
            try
            {
                var action = await actionRepository.GetByIdAsync(input.Id);
                if (action == null)
                {
                    return new CompleteActionPayload(null, "Action not found");
                }

                action.Status = "Completed";
                action.CompletedDate = DateTime.UtcNow;
                action.ModifiedDate = DateTime.UtcNow;
                
                if (!string.IsNullOrEmpty(input.Comments))
                {
                    action.Comments = string.IsNullOrEmpty(action.Comments) 
                        ? input.Comments 
                        : $"{action.Comments}\n\nCompletion Comments: {input.Comments}";
                }

                await actionRepository.UpdateAsync(action);
                await actionRepository.SaveChangesAsync();

                var actionType = MapToActionType(action);
                return new CompleteActionPayload(actionType, null);
            }
            catch (Exception ex)
            {
                return new CompleteActionPayload(null, ex.Message);
            }
        }

        public async Task<AssignActionPayload> AssignActionAsync(
            AssignActionInput input,
            [Service] IActionRepository actionRepository)
        {
            try
            {
                var action = await actionRepository.GetByIdAsync(input.Id);
                if (action == null)
                {
                    return new AssignActionPayload(null, "Action not found");
                }

                action.AssignedToUserId = input.AssignedToUserId;
                action.ModifiedDate = DateTime.UtcNow;

                await actionRepository.UpdateAsync(action);
                await actionRepository.SaveChangesAsync();

                var actionType = MapToActionType(action);
                return new AssignActionPayload(actionType, null);
            }
            catch (Exception ex)
            {
                return new AssignActionPayload(null, ex.Message);
            }
        }

        public async Task<DeleteActionPayload> DeleteActionAsync(
            int id,
            [Service] IActionRepository actionRepository)
        {
            try
            {
                var action = await actionRepository.GetByIdAsync(id);
                if (action == null)
                {
                    return new DeleteActionPayload(false, "Action not found");
                }

                // Soft delete by setting IsActive to false
                action.IsActive = false;
                action.ModifiedDate = DateTime.UtcNow;

                await actionRepository.UpdateAsync(action);
                await actionRepository.SaveChangesAsync();

                return new DeleteActionPayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeleteActionPayload(false, ex.Message);
            }
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