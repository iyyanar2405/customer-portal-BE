using CustomerPortal.AuditsService.Entities;
using CustomerPortal.AuditsService.GraphQL.Inputs;
using CustomerPortal.AuditsService.GraphQL.Outputs;
using CustomerPortal.AuditsService.Repositories;
using CustomerPortal.AuditsService.Data;
using CustomerPortal.Shared.Interfaces;

namespace CustomerPortal.AuditsService.GraphQL.Resolvers
{
    public class Mutation
    {
        // Create new audit
        public async Task<AuditResponse> CreateAudit(
            CreateAuditInput input,
            [Service] IAuditRepository auditRepository,
            [Service] IAuditTeamMemberRepository teamMemberRepository,
            [Service] IAuditSiteRepository auditSiteRepository,
            [Service] IRepository<AuditService> auditServiceRepository,
            [Service] AuditsDbContext context)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            
            try
            {
                // Generate audit number
                var auditNumber = await auditRepository.GenerateAuditNumberAsync();

                // Create audit
                var audit = new Audit
                {
                    AuditNumber = auditNumber,
                    AuditTitle = input.AuditTitle,
                    CompanyId = input.CompanyId,
                    AuditTypeId = input.AuditTypeId,
                    StartDate = input.StartDate,
                    EndDate = input.EndDate,
                    LeadAuditorId = input.LeadAuditorId,
                    Status = "PLANNED"
                };

                var createdAudit = await auditRepository.AddAsync(audit);

                // Add sites to audit
                if (input.SiteIds != null && input.SiteIds.Any())
                {
                    foreach (var siteId in input.SiteIds)
                    {
                        var auditSite = new AuditSite
                        {
                            AuditId = createdAudit.Id,
                            SiteId = siteId,
                            Status = "SCHEDULED"
                        };
                        await auditSiteRepository.AddAsync(auditSite);
                    }
                }

                // Add services to audit
                if (input.ServiceIds != null && input.ServiceIds.Any())
                {
                    foreach (var serviceId in input.ServiceIds)
                    {
                        var auditService = new AuditService
                        {
                            AuditId = createdAudit.Id,
                            ServiceId = serviceId,
                            Status = "ACTIVE"
                        };
                        await auditServiceRepository.AddAsync(auditService);
                    }
                }

                // Add team members
                if (input.TeamMembers != null && input.TeamMembers.Any())
                {
                    foreach (var teamMember in input.TeamMembers)
                    {
                        var auditTeamMember = new AuditTeamMember
                        {
                            AuditId = createdAudit.Id,
                            UserId = teamMember.UserId,
                            Role = teamMember.Role,
                            AssignedDate = DateTime.UtcNow
                        };
                        await teamMemberRepository.AddAsync(auditTeamMember);
                    }
                }

                await transaction.CommitAsync();

                return new AuditResponse(
                    createdAudit.Id,
                    createdAudit.AuditNumber,
                    createdAudit.AuditTitle,
                    createdAudit.Status,
                    createdAudit.CreatedDate,
                    createdAudit.ModifiedDate
                );
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Update audit status
        public async Task<AuditResponse?> UpdateAuditStatus(
            int auditId,
            AuditStatus status,
            [Service] IAuditRepository auditRepository)
        {
            var audit = await auditRepository.GetByIdAsync(auditId);
            if (audit == null) return null;

            audit.Status = status.ToString();
            audit.ModifiedDate = DateTime.UtcNow;

            var updatedAudit = await auditRepository.UpdateAsync(audit);

            return new AuditResponse(
                updatedAudit.Id,
                updatedAudit.AuditNumber,
                updatedAudit.AuditTitle,
                updatedAudit.Status,
                updatedAudit.CreatedDate,
                updatedAudit.ModifiedDate
            );
        }

        // Assign team member to audit
        public async Task<AuditTeamMember?> AssignTeamMember(
            AssignTeamMemberInput input,
            [Service] IAuditTeamMemberRepository teamMemberRepository)
        {
            // Check if team member is already assigned
            var existingTeamMembers = await teamMemberRepository.GetAuditTeamAsync(input.AuditId);
            var existingAssignment = existingTeamMembers.FirstOrDefault(tm => tm.UserId == input.UserId);

            if (existingAssignment != null)
            {
                // Update existing assignment
                existingAssignment.Role = input.Role;
                existingAssignment.Notes = input.Notes;
                existingAssignment.ModifiedDate = DateTime.UtcNow;
                return await teamMemberRepository.UpdateAsync(existingAssignment);
            }

            // Create new assignment
            var teamMember = new AuditTeamMember
            {
                AuditId = input.AuditId,
                UserId = input.UserId,
                Role = input.Role,
                Notes = input.Notes,
                AssignedDate = DateTime.UtcNow
            };

            return await teamMemberRepository.AddAsync(teamMember);
        }

        // Remove team member from audit
        public async Task<bool> RemoveTeamMember(
            int auditId,
            int userId,
            [Service] IAuditTeamMemberRepository teamMemberRepository)
        {
            var teamMembers = await teamMemberRepository.GetAuditTeamAsync(auditId);
            var teamMember = teamMembers.FirstOrDefault(tm => tm.UserId == userId);

            if (teamMember == null) return false;

            return await teamMemberRepository.DeleteAsync(teamMember.Id);
        }

        // Add site to audit
        public async Task<AuditSite?> AddSiteToAudit(
            AddSiteToAuditInput input,
            [Service] IAuditSiteRepository auditSiteRepository)
        {
            // Check if site is already added to audit
            var existingSites = await auditSiteRepository.GetAuditSitesAsync(input.AuditId);
            var existingSite = existingSites.FirstOrDefault(as_ => as_.SiteId == input.SiteId);

            if (existingSite != null)
            {
                // Update existing site assignment
                existingSite.ScheduledDate = input.ScheduledDate;
                existingSite.Notes = input.Notes;
                existingSite.ModifiedDate = DateTime.UtcNow;
                return await auditSiteRepository.UpdateAsync(existingSite);
            }

            // Create new site assignment
            var auditSite = new AuditSite
            {
                AuditId = input.AuditId,
                SiteId = input.SiteId,
                ScheduledDate = input.ScheduledDate,
                Notes = input.Notes,
                Status = "SCHEDULED"
            };

            return await auditSiteRepository.AddAsync(auditSite);
        }

        // Remove site from audit
        public async Task<bool> RemoveSiteFromAudit(
            int auditId,
            int siteId,
            [Service] IAuditSiteRepository auditSiteRepository)
        {
            var auditSites = await auditSiteRepository.GetAuditSitesAsync(auditId);
            var auditSite = auditSites.FirstOrDefault(as_ => as_.SiteId == siteId);

            if (auditSite == null) return false;

            return await auditSiteRepository.DeleteAsync(auditSite.Id);
        }

        // Update audit details
        public async Task<AuditResponse?> UpdateAudit(
            int auditId,
            string? auditTitle,
            DateTime? startDate,
            DateTime? endDate,
            int? leadAuditorId,
            [Service] IAuditRepository auditRepository)
        {
            var audit = await auditRepository.GetByIdAsync(auditId);
            if (audit == null) return null;

            if (!string.IsNullOrWhiteSpace(auditTitle))
                audit.AuditTitle = auditTitle;
            
            if (startDate.HasValue)
                audit.StartDate = startDate.Value;
            
            if (endDate.HasValue)
                audit.EndDate = endDate.Value;
            
            if (leadAuditorId.HasValue)
                audit.LeadAuditorId = leadAuditorId.Value;

            audit.ModifiedDate = DateTime.UtcNow;

            var updatedAudit = await auditRepository.UpdateAsync(audit);

            return new AuditResponse(
                updatedAudit.Id,
                updatedAudit.AuditNumber,
                updatedAudit.AuditTitle,
                updatedAudit.Status,
                updatedAudit.CreatedDate,
                updatedAudit.ModifiedDate
            );
        }

        // Delete audit
        public async Task<bool> DeleteAudit(
            int auditId,
            [Service] IAuditRepository auditRepository)
        {
            return await auditRepository.DeleteAsync(auditId);
        }

        // Update audit site status
        public async Task<AuditSite?> UpdateAuditSiteStatus(
            int auditId,
            int siteId,
            AuditSiteStatus status,
            [Service] IAuditSiteRepository auditSiteRepository)
        {
            var auditSites = await auditSiteRepository.GetAuditSitesAsync(auditId);
            var auditSite = auditSites.FirstOrDefault(as_ => as_.SiteId == siteId);

            if (auditSite == null) return null;

            auditSite.Status = status.ToString();
            auditSite.ModifiedDate = DateTime.UtcNow;

            return await auditSiteRepository.UpdateAsync(auditSite);
        }
    }
}