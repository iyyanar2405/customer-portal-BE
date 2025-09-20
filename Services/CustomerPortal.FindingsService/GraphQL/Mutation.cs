using CustomerPortal.FindingsService.Entities;
using CustomerPortal.FindingsService.GraphQL.Types;
using CustomerPortal.FindingsService.Repositories;

namespace CustomerPortal.FindingsService.GraphQL;

public class Mutation
{
    public async Task<Finding> CreateFinding(CreateFindingInput input, [Service] IFindingRepository repository)
    {
        var finding = new Finding
        {
            Title = input.Title,
            Description = input.Description,
            ReferenceNumber = input.ReferenceNumber,
            IdentifiedDate = input.IdentifiedDate,
            RequiredCompletionDate = input.RequiredCompletionDate,
            RootCause = input.RootCause,
            CorrectiveAction = input.CorrectiveAction,
            PreventiveAction = input.PreventiveAction,
            Evidence = input.Evidence,
            AuditId = input.AuditId,
            SiteId = input.SiteId,
            CompanyId = input.CompanyId,
            IdentifiedBy = input.IdentifiedBy,
            AssignedTo = input.AssignedTo,
            Severity = input.Severity,
            Priority = input.Priority,
            CategoryId = input.CategoryId,
            StatusId = input.StatusId,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        var createdFinding = await repository.AddAsync(finding);
        return await repository.GetByIdAsync(createdFinding.Id) ?? createdFinding;
    }

    public async Task<Finding?> UpdateFinding(UpdateFindingInput input, [Service] IFindingRepository repository)
    {
        var existingFinding = await repository.GetByIdAsync(input.Id);
        if (existingFinding == null)
            return null;

        // Update only provided fields
        if (!string.IsNullOrEmpty(input.Title))
            existingFinding.Title = input.Title;
        
        if (!string.IsNullOrEmpty(input.Description))
            existingFinding.Description = input.Description;
        
        if (input.ReferenceNumber != null)
            existingFinding.ReferenceNumber = input.ReferenceNumber;
        
        if (input.IdentifiedDate.HasValue)
            existingFinding.IdentifiedDate = input.IdentifiedDate;
        
        if (input.RequiredCompletionDate.HasValue)
            existingFinding.RequiredCompletionDate = input.RequiredCompletionDate;
        
        if (input.ActualCompletionDate.HasValue)
            existingFinding.ActualCompletionDate = input.ActualCompletionDate;
        
        if (input.RootCause != null)
            existingFinding.RootCause = input.RootCause;
        
        if (input.CorrectiveAction != null)
            existingFinding.CorrectiveAction = input.CorrectiveAction;
        
        if (input.PreventiveAction != null)
            existingFinding.PreventiveAction = input.PreventiveAction;
        
        if (input.Evidence != null)
            existingFinding.Evidence = input.Evidence;
        
        if (input.AuditId.HasValue)
            existingFinding.AuditId = input.AuditId;
        
        if (input.SiteId.HasValue)
            existingFinding.SiteId = input.SiteId;
        
        if (input.CompanyId.HasValue)
            existingFinding.CompanyId = input.CompanyId;
        
        if (input.IdentifiedBy != null)
            existingFinding.IdentifiedBy = input.IdentifiedBy;
        
        if (input.AssignedTo != null)
            existingFinding.AssignedTo = input.AssignedTo;
        
        if (input.ReviewedBy != null)
            existingFinding.ReviewedBy = input.ReviewedBy;
        
        if (input.ReviewedDate.HasValue)
            existingFinding.ReviewedDate = input.ReviewedDate;
        
        if (input.Severity.HasValue)
            existingFinding.Severity = input.Severity.Value;
        
        if (input.Priority.HasValue)
            existingFinding.Priority = input.Priority.Value;
        
        if (input.CategoryId.HasValue)
            existingFinding.CategoryId = input.CategoryId.Value;
        
        if (input.StatusId.HasValue)
            existingFinding.StatusId = input.StatusId.Value;

        existingFinding.ModifiedDate = DateTime.UtcNow;

        await repository.UpdateAsync(existingFinding);
        return await repository.GetByIdAsync(existingFinding.Id);
    }

    public async Task<Finding?> UpdateFindingStatus(UpdateFindingStatusInput input, [Service] IFindingRepository repository)
    {
        var existingFinding = await repository.GetByIdAsync(input.Id);
        if (existingFinding == null)
            return null;

        existingFinding.StatusId = input.StatusId;
        
        if (!string.IsNullOrEmpty(input.ReviewedBy))
            existingFinding.ReviewedBy = input.ReviewedBy;
        
        if (input.ReviewedDate.HasValue)
            existingFinding.ReviewedDate = input.ReviewedDate;
        
        if (input.ActualCompletionDate.HasValue)
            existingFinding.ActualCompletionDate = input.ActualCompletionDate;

        existingFinding.ModifiedDate = DateTime.UtcNow;

        await repository.UpdateAsync(existingFinding);
        return await repository.GetByIdAsync(existingFinding.Id);
    }

    public async Task<bool> DeleteFinding(int id, [Service] IFindingRepository repository)
    {
        var finding = await repository.GetByIdAsync(id);
        if (finding == null)
            return false;

        await repository.DeleteAsync(finding.Id);
        return true;
    }
}