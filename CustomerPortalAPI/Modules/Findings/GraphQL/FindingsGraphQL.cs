using CustomerPortalAPI.Modules.Findings.Entities;
using CustomerPortalAPI.Modules.Findings.Repositories;
using HotChocolate;
using CustomerPortalAPI.Modules.Shared;

namespace CustomerPortalAPI.Modules.Findings.GraphQL
{
    public record FindingOutput(int Id, string FindingNumber, string Title, string Description, string FindingType, string? Severity, bool IsActive, DateTime CreatedDate);
    public record CreateFindingInput(string FindingNumber, int AuditId, string Title, string Description, string FindingType, string? Severity);
    public record UpdateFindingInput(int Id, string? Title, string? Description, string? FindingType, string? Severity, bool? IsActive);
    public record CreateFindingPayload(FindingOutput? Finding, string? Error);
    public record UpdateFindingPayload(FindingOutput? Finding, string? Error);
    [ExtendObjectType("Query")]
    public class FindingsQueries
    {
        public async Task<IEnumerable<FindingOutput>> GetFindings([Service] IFindingRepository repository)
        {
            var findings = await repository.GetAllAsync();
            return findings.Select(f => new FindingOutput(f.Id, f.FindingNumber, f.Title, f.Description, f.FindingType, f.Severity, f.IsActive, f.CreatedDate));
        }

        public async Task<FindingOutput?> GetFindingById(int id, [Service] IFindingRepository repository)
        {
            var finding = await repository.GetByIdAsync(id);
            return finding == null ? null : new FindingOutput(finding.Id, finding.FindingNumber, finding.Title, finding.Description, finding.FindingType, finding.Severity, finding.IsActive, finding.CreatedDate);
        }
    }

    [ExtendObjectType("Mutation")]
    public class FindingsMutations
    {
        public async Task<CreateFindingPayload> CreateFinding(CreateFindingInput input, [Service] IFindingRepository repository)
        {
            try
            {
                var finding = new Finding
                {
                    FindingNumber = input.FindingNumber,
                    AuditId = input.AuditId,
                    Title = input.Title,
                    Description = input.Description,
                    FindingType = input.FindingType,
                    Severity = input.Severity,
                    FindingStatusId = 1, // Default status
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                var created = await repository.AddAsync(finding);
                return new CreateFindingPayload(new FindingOutput(created.Id, created.FindingNumber, created.Title, created.Description, created.FindingType, created.Severity, created.IsActive, created.CreatedDate), null);
            }
            catch (Exception ex)
            {
                return new CreateFindingPayload(null, ex.Message);
            }
        }

        public async Task<UpdateFindingPayload> UpdateFinding(UpdateFindingInput input, [Service] IFindingRepository repository)
        {
            try
            {
                var finding = await repository.GetByIdAsync(input.Id);
                if (finding == null) return new UpdateFindingPayload(null, "Finding not found");

                if (input.Title != null) finding.Title = input.Title;
                if (input.Description != null) finding.Description = input.Description;
                if (input.FindingType != null) finding.FindingType = input.FindingType;
                if (input.Severity != null) finding.Severity = input.Severity;
                if (input.IsActive.HasValue) finding.IsActive = input.IsActive.Value;
                finding.ModifiedDate = DateTime.UtcNow;

                await repository.UpdateAsync(finding);
                return new UpdateFindingPayload(new FindingOutput(finding.Id, finding.FindingNumber, finding.Title, finding.Description, finding.FindingType, finding.Severity, finding.IsActive, finding.CreatedDate), null);
            }
            catch (Exception ex)
            {
                return new UpdateFindingPayload(null, ex.Message);
            }
        }

        public async Task<BaseDeletePayload> DeleteFinding(int id, [Service] IFindingRepository repository)
        {
            try
            {
                await repository.DeleteAsync(id);
                return new BaseDeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new BaseDeletePayload(false, ex.Message);
            }
        }
    }
}

