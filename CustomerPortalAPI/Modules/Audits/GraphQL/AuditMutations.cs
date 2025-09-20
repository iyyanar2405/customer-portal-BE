using CustomerPortalAPI.Modules.Audits.Entities;
using CustomerPortalAPI.Modules.Audits.Repositories;
using CustomerPortalAPI.Modules.Audits.GraphQL;
using HotChocolate;

namespace CustomerPortalAPI.Modules.Audits.GraphQL
{
    [ExtendObjectType("Mutation")]
    public class AuditMutations
    {
        /// <summary>
        /// Create a new audit
        /// </summary>
        public async Task<CreateAuditPayload> CreateAuditAsync(
            AuditInput input,
            [Service] IAuditRepository auditRepository)
        {
            try
            {
                var audit = new Audit
                {
                    AuditName = input.Sites ?? "New Audit", // Map Sites to AuditName
                    AuditNumber = input.AuditNumber,
                    CompanyId = input.CompanyId,
                    SiteId = 1, // Default site ID, would need proper mapping from Sites
                    AuditTypeId = input.AuditTypeId,
                    Status = input.Status,
                    PlannedStartDate = input.StartDate,
                    PlannedEndDate = input.EndDate,
                    LeadAuditorId = null, // Would need proper user ID mapping from LeadAuditor
                    Scope = input.Services, // Map Services to Scope
                    Summary = input.Description,
                    CreatedBy = input.CreatedBy,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedBy = input.CreatedBy,
                    ModifiedDate = DateTime.UtcNow,
                    IsActive = true
                };

                var createdAudit = await auditRepository.AddAsync(audit);
                await auditRepository.SaveChangesAsync();

                var auditType = MapToAuditType(createdAudit);
                return new CreateAuditPayload(auditType, null);
            }
            catch (Exception ex)
            {
                return new CreateAuditPayload(null, ex.Message);
            }
        }

        /// <summary>
        /// Update an existing audit
        /// </summary>
        public async Task<UpdateAuditPayload> UpdateAuditAsync(
            UpdateAuditInput input,
            [Service] IAuditRepository auditRepository)
        {
            try
            {
                var audit = await auditRepository.GetByIdAsync(input.AuditId);
                if (audit == null)
                {
                    return new UpdateAuditPayload(null, "Audit not found");
                }

                // Update fields only if provided - map GraphQL input to entity properties
                if (!string.IsNullOrEmpty(input.Sites)) audit.AuditName = input.Sites;
                if (!string.IsNullOrEmpty(input.Services)) audit.Scope = input.Services;
                if (input.CompanyId.HasValue) audit.CompanyId = input.CompanyId.Value;
                if (!string.IsNullOrEmpty(input.Status)) audit.Status = input.Status;
                if (input.StartDate.HasValue) audit.PlannedStartDate = input.StartDate.Value;
                if (input.EndDate.HasValue) audit.PlannedEndDate = input.EndDate;
                // LeadAuditor would need user lookup by name to get ID
                // Type would map to AuditType relationship
                if (!string.IsNullOrEmpty(input.AuditNumber)) audit.AuditNumber = input.AuditNumber;
                if (!string.IsNullOrEmpty(input.Description)) audit.Summary = input.Description;
                if (input.AuditTypeId.HasValue) audit.AuditTypeId = input.AuditTypeId;
                if (input.ModifiedBy.HasValue) audit.ModifiedBy = input.ModifiedBy;
                
                audit.ModifiedDate = DateTime.UtcNow;

                await auditRepository.UpdateAsync(audit);
                await auditRepository.SaveChangesAsync();

                var auditType = MapToAuditType(audit);
                return new UpdateAuditPayload(auditType, null);
            }
            catch (Exception ex)
            {
                return new UpdateAuditPayload(null, ex.Message);
            }
        }

        /// <summary>
        /// Schedule an audit
        /// </summary>
        public async Task<ScheduleAuditPayload> ScheduleAuditAsync(
            AuditScheduleInput input,
            [Service] IAuditRepository auditRepository)
        {
            try
            {
                var audit = await auditRepository.GetByIdAsync(input.AuditId);
                if (audit == null)
                {
                    return new ScheduleAuditPayload(null, "Audit not found");
                }

                audit.PlannedStartDate = input.StartDate;
                audit.PlannedEndDate = input.EndDate;
                audit.Status = "Scheduled";
                audit.ModifiedDate = DateTime.UtcNow;
                if (input.ModifiedBy.HasValue) audit.ModifiedBy = input.ModifiedBy;

                await auditRepository.UpdateAsync(audit);
                await auditRepository.SaveChangesAsync();

                var auditType = MapToAuditType(audit);
                return new ScheduleAuditPayload(auditType, null);
            }
            catch (Exception ex)
            {
                return new ScheduleAuditPayload(null, ex.Message);
            }
        }

        /// <summary>
        /// Complete an audit
        /// </summary>
        public async Task<CompleteAuditPayload> CompleteAuditAsync(
            int auditId,
            string? comments,
            [Service] IAuditRepository auditRepository)
        {
            try
            {
                var audit = await auditRepository.GetByIdAsync(auditId);
                if (audit == null)
                {
                    return new CompleteAuditPayload(null, "Audit not found");
                }

                audit.Status = "Completed";
                audit.ActualEndDate = DateTime.UtcNow;
                audit.ModifiedDate = DateTime.UtcNow;
                if (!string.IsNullOrEmpty(comments))
                {
                    audit.Summary = string.IsNullOrEmpty(audit.Summary)
                        ? comments
                        : $"{audit.Summary}\n\nCompletion Comments: {comments}";
                }

                await auditRepository.UpdateAsync(audit);
                await auditRepository.SaveChangesAsync();

                var auditType = MapToAuditType(audit);
                return new CompleteAuditPayload(auditType, null);
            }
            catch (Exception ex)
            {
                return new CompleteAuditPayload(null, ex.Message);
            }
        }

        /// <summary>
        /// Map Audit entity to AuditType - translating entity properties to GraphQL output
        /// </summary>
        private static AuditType MapToAuditType(Audit audit)
        {
            return new AuditType(
                AuditId: audit.Id, // Entity uses Id, GraphQL expects AuditId
                Sites: audit.AuditName, // Map AuditName to Sites for GraphQL
                Services: audit.Scope ?? "", // Map Scope to Services
                CompanyId: audit.CompanyId,
                CompanyName: null, // Would be populated with join to Company
                Status: audit.Status ?? "Draft",
                StartDate: audit.PlannedStartDate ?? DateTime.UtcNow, // Use planned start date
                EndDate: audit.PlannedEndDate,
                LeadAuditor: audit.LeadAuditor?.Username, // Get username from User entity
                Type: audit.AuditType?.AuditTypeName, // Get name from AuditType entity
                AuditNumber: audit.AuditNumber,
                Description: audit.Summary,
                AuditTypeId: audit.AuditTypeId,
                AuditTypeName: audit.AuditType?.AuditTypeName, // Would be populated with join
                IsActive: audit.IsActive,
                CreatedDate: audit.CreatedDate,
                ModifiedDate: audit.ModifiedDate,
                CreatedBy: audit.CreatedBy,
                ModifiedBy: audit.ModifiedBy,
                CreatedByName: null, // Would be populated with join to User
                ModifiedByName: null, // Would be populated with join to User
                TeamMembers: null, // Would be populated with join to AuditTeamMembers
                AuditSites: null, // Would be populated with join to AuditSiteAudits
                AuditServices: null // Would be populated with join to AuditServices
            );
        }
    }
}