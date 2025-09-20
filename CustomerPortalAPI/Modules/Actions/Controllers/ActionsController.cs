using Microsoft.AspNetCore.Mvc;
using CustomerPortalAPI.Modules.Actions.Repositories;
using CustomerPortalAPI.Modules.Actions.Entities;
using CustomerPortalAPI.Modules.Actions.GraphQL;

namespace CustomerPortalAPI.Modules.Actions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActionsController : ControllerBase
    {
        private readonly IActionRepository _actionRepository;

        public ActionsController(IActionRepository actionRepository)
        {
            _actionRepository = actionRepository;
        }

        /// <summary>
        /// Get all actions with optional filtering and pagination
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActionType>>> GetActions(
            [FromQuery] string? status = null,
            [FromQuery] string? priority = null,
            [FromQuery] int? assignedToUserId = null,
            [FromQuery] int? companyId = null,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50)
        {
            try
            {
                var actions = await _actionRepository.GetAllAsync();

                // Apply filters
                if (!string.IsNullOrEmpty(status))
                    actions = actions.Where(a => a.Status == status);

                if (!string.IsNullOrEmpty(priority))
                    actions = actions.Where(a => a.Priority == priority);

                if (assignedToUserId.HasValue)
                    actions = actions.Where(a => a.AssignedToUserId == assignedToUserId);

                if (companyId.HasValue)
                    actions = actions.Where(a => a.CompanyId == companyId);

                // Apply pagination
                var pagedActions = actions.Skip(skip).Take(take);

                var result = pagedActions.Select(MapToActionType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get action by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ActionType>> GetAction(int id)
        {
            try
            {
                var action = await _actionRepository.GetByIdAsync(id);
                if (action == null)
                {
                    return NotFound(new { message = "Action not found" });
                }

                return Ok(MapToActionType(action));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get actions assigned to a specific user
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ActionType>>> GetActionsByUser(int userId)
        {
            try
            {
                var actions = await _actionRepository.GetActionsByUserAsync(userId);
                var result = actions.Select(MapToActionType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get overdue actions
        /// </summary>
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<ActionType>>> GetOverdueActions()
        {
            try
            {
                var actions = await _actionRepository.GetOverdueActionsAsync();
                var result = actions.Select(MapToActionType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new action
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ActionType>> CreateAction([FromBody] ActionInput input)
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

                var createdAction = await _actionRepository.AddAsync(action);
                await _actionRepository.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAction), new { id = createdAction.Id }, MapToActionType(createdAction));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing action
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ActionType>> UpdateAction(int id, [FromBody] UpdateActionInput input)
        {
            try
            {
                if (id != input.Id)
                {
                    return BadRequest(new { message = "ID mismatch" });
                }

                var action = await _actionRepository.GetByIdAsync(id);
                if (action == null)
                {
                    return NotFound(new { message = "Action not found" });
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

                await _actionRepository.UpdateAsync(action);
                await _actionRepository.SaveChangesAsync();

                return Ok(MapToActionType(action));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Mark action as completed
        /// </summary>
        [HttpPatch("{id}/complete")]
        public async Task<ActionResult<ActionType>> CompleteAction(int id, [FromBody] CompleteActionInput input)
        {
            try
            {
                if (id != input.Id)
                {
                    return BadRequest(new { message = "ID mismatch" });
                }

                var action = await _actionRepository.GetByIdAsync(id);
                if (action == null)
                {
                    return NotFound(new { message = "Action not found" });
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

                await _actionRepository.UpdateAsync(action);
                await _actionRepository.SaveChangesAsync();

                return Ok(MapToActionType(action));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete action (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAction(int id)
        {
            try
            {
                var action = await _actionRepository.GetByIdAsync(id);
                if (action == null)
                {
                    return NotFound(new { message = "Action not found" });
                }

                // Soft delete by setting IsActive to false
                action.IsActive = false;
                action.ModifiedDate = DateTime.UtcNow;

                await _actionRepository.UpdateAsync(action);
                await _actionRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
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