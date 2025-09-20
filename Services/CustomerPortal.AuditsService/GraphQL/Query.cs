using CustomerPortal.AuditsService.Entities;
using CustomerPortal.AuditsService.GraphQL.Inputs;
using CustomerPortal.AuditsService.GraphQL.Outputs;
using CustomerPortal.AuditsService.Repositories;
using CustomerPortal.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.AuditsService.GraphQL.Resolvers
{
    public class Query
    {
        // Get all audits with details
        public async Task<IEnumerable<Audit>> GetAudits([Service] IAuditRepository auditRepository)
        {
            return await auditRepository.GetAuditsWithDetailsAsync();
        }

        // Get audit by ID
        public async Task<Audit?> GetAudit(int id, [Service] IAuditRepository auditRepository)
        {
            return await auditRepository.GetAuditWithDetailsAsync(id);
        }

        // Get audits by company
        public async Task<IEnumerable<Audit>> GetAuditsByCompany(int companyId, [Service] IAuditRepository auditRepository)
        {
            var audits = await auditRepository.FindAsync(a => a.CompanyId == companyId);
            return audits;
        }

        // Get audits by status
        public async Task<IEnumerable<Audit>> GetAuditsByStatus(AuditStatus status, [Service] IAuditRepository auditRepository)
        {
            var audits = await auditRepository.FindAsync(a => a.Status == status.ToString());
            return audits;
        }

        // Get audit schedule
        public async Task<IEnumerable<Audit>> GetAuditSchedule(
            DateTime startDate, 
            DateTime endDate, 
            [Service] IAuditRepository auditRepository)
        {
            return await auditRepository.GetAuditScheduleAsync(startDate, endDate);
        }

        // Get auditor availability
        public async Task<AuditorAvailabilityOutput> GetAuditorAvailability(
            int auditorId,
            DateTime startDate,
            DateTime endDate,
            [Service] IAuditRepository auditRepository)
        {
            var conflictingAudits = await auditRepository.GetAuditorScheduleAsync(auditorId, startDate, endDate);
            
            var conflictingAuditOutputs = conflictingAudits.Select(a => new ConflictingAuditOutput(
                a.AuditNumber,
                a.StartDate,
                a.EndDate
            )).ToList();

            // Generate available dates (simplified logic - exclude conflicting audit dates)
            var availableDates = new List<DateTime>();
            var currentDate = startDate;
            
            while (currentDate <= endDate)
            {
                bool hasConflict = conflictingAudits.Any(a => 
                    currentDate >= a.StartDate.Date && currentDate <= a.EndDate.Date);
                
                if (!hasConflict)
                {
                    availableDates.Add(currentDate);
                }
                
                currentDate = currentDate.AddDays(1);
            }

            return new AuditorAvailabilityOutput(
                auditorId,
                availableDates,
                conflictingAuditOutputs
            );
        }

        // Get audit team
        public async Task<IEnumerable<AuditTeamMember>> GetAuditTeam(
            int auditId, 
            [Service] IAuditTeamMemberRepository teamMemberRepository)
        {
            return await teamMemberRepository.GetAuditTeamAsync(auditId);
        }

        // Get audit sites
        public async Task<IEnumerable<AuditSite>> GetAuditSites(
            int auditId, 
            [Service] IAuditSiteRepository auditSiteRepository)
        {
            return await auditSiteRepository.GetAuditSitesAsync(auditId);
        }

        // Get audit summary
        public async Task<AuditSummaryOutput?> GetAuditSummary(
            int auditId, 
            [Service] IAuditRepository auditRepository,
            [Service] IAuditTeamMemberRepository teamMemberRepository,
            [Service] IAuditSiteRepository auditSiteRepository)
        {
            var audit = await auditRepository.GetAuditWithDetailsAsync(auditId);
            if (audit == null) return null;

            var teamMembers = await teamMemberRepository.GetAuditTeamAsync(auditId);
            var auditSites = await auditSiteRepository.GetAuditSitesAsync(auditId);

            // Calculate audit duration in days
            var auditDuration = (audit.EndDate - audit.StartDate).Days + 1;

            // Mock findings data (in real implementation, this would come from findings service)
            var totalFindings = 15;
            var criticalFindings = 2;
            var majorFindings = 5;
            var minorFindings = 8;
            var observationsCount = 3;
            var compliancePercentage = 85.5m;

            return new AuditSummaryOutput(
                auditId,
                totalFindings,
                criticalFindings,
                majorFindings,
                minorFindings,
                observationsCount,
                compliancePercentage,
                compliancePercentage >= 80 ? "RECOMMENDED" : "NOT_RECOMMENDED",
                auditDuration,
                auditSites.Count(),
                teamMembers.Count()
            );
        }

        // Get all companies
        public async Task<IEnumerable<Company>> GetCompanies([Service] IRepository<Company> companyRepository)
        {
            return await companyRepository.GetAllAsync();
        }

        // Get all users
        public async Task<IEnumerable<User>> GetUsers([Service] IRepository<User> userRepository)
        {
            return await userRepository.GetAllAsync();
        }

        // Get all sites
        public async Task<IEnumerable<Site>> GetSites([Service] IRepository<Site> siteRepository)
        {
            return await siteRepository.GetAllAsync();
        }

        // Get all services
        public async Task<IEnumerable<Service>> GetServices([Service] IRepository<Service> serviceRepository)
        {
            return await serviceRepository.GetAllAsync();
        }

        // Get all audit types
        public async Task<IEnumerable<Entities.AuditType>> GetAuditTypes([Service] IRepository<Entities.AuditType> auditTypeRepository)
        {
            return await auditTypeRepository.GetAllAsync();
        }

        // Get audits by lead auditor
        public async Task<IEnumerable<Audit>> GetAuditsByLeadAuditor(
            int leadAuditorId, 
            [Service] IAuditRepository auditRepository)
        {
            return await auditRepository.FindAsync(a => a.LeadAuditorId == leadAuditorId);
        }

        // Get upcoming audits
        public async Task<IEnumerable<Audit>> GetUpcomingAudits([Service] IAuditRepository auditRepository)
        {
            var today = DateTime.UtcNow.Date;
            return await auditRepository.FindAsync(a => a.StartDate >= today && a.Status == "PLANNED");
        }

        // Get audits in progress
        public async Task<IEnumerable<Audit>> GetAuditsInProgress([Service] IAuditRepository auditRepository)
        {
            return await auditRepository.FindAsync(a => a.Status == "IN_PROGRESS");
        }
    }
}