using Microsoft.EntityFrameworkCore;
using CustomerPortalAPI.Data;
using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Audits.Entities;

namespace CustomerPortalAPI.Modules.Audits.Repositories
{
    public class AuditRepository : Repository<Audit>, IAuditRepository
    {
        public AuditRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Audit>> GetAuditsByCompanyAsync(int companyId)
        {
            return await _dbSet.Where(a => a.CompanyId == companyId).ToListAsync();
        }

        public async Task<IEnumerable<Audit>> GetAuditsBySiteAsync(int siteId)
        {
            return await _dbSet.Where(a => a.SiteId == siteId).ToListAsync();
        }

        public async Task<IEnumerable<Audit>> GetAuditsByStatusAsync(string status)
        {
            return await _dbSet.Where(a => a.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<Audit>> GetAuditsByTypeAsync(int auditTypeId)
        {
            return await _dbSet.Where(a => a.AuditTypeId == auditTypeId).ToListAsync();
        }

        public async Task<IEnumerable<Audit>> GetAuditsByLeadAuditorAsync(int leadAuditorId)
        {
            return await _dbSet.Where(a => a.LeadAuditorId == leadAuditorId).ToListAsync();
        }

        public async Task<IEnumerable<Audit>> GetActiveAuditsAsync()
        {
            return await _dbSet.Where(a => a.Status != "Completed" && a.Status != "Cancelled").ToListAsync();
        }

        public async Task<IEnumerable<Audit>> GetCompletedAuditsAsync()
        {
            return await _dbSet.Where(a => a.Status == "Completed").ToListAsync();
        }

        public async Task<IEnumerable<Audit>> GetUpcomingAuditsAsync()
        {
            var today = DateTime.Today;
            return await _dbSet.Where(a => a.PlannedStartDate > today && a.Status != "Cancelled").ToListAsync();
        }

        public async Task<IEnumerable<Audit>> GetOverdueAuditsAsync()
        {
            var today = DateTime.Today;
            return await _dbSet.Where(a => a.PlannedEndDate < today && a.Status != "Completed" && a.Status != "Cancelled").ToListAsync();
        }

        public async Task<Audit?> GetAuditWithDetailsAsync(int auditId)
        {
            return await _dbSet
                .Include(a => a.Company)
                .Include(a => a.Site)
                .Include(a => a.AuditType)
                .Include(a => a.LeadAuditor)
                .FirstOrDefaultAsync(a => a.Id == auditId);
        }

        public async Task<Audit?> GetByAuditNumberAsync(string auditNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.AuditNumber == auditNumber);
        }

        public async Task<IEnumerable<Audit>> GetAuditsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(a => a.PlannedStartDate >= startDate && a.PlannedStartDate <= endDate).ToListAsync();
        }

        public async Task<IEnumerable<Audit>> SearchAuditsAsync(string searchTerm)
        {
            return await _dbSet.Where(a => 
                a.AuditName.Contains(searchTerm) || 
                a.AuditNumber!.Contains(searchTerm) ||
                a.Summary!.Contains(searchTerm)).ToListAsync();
        }

        public async Task UpdateAuditStatusAsync(int auditId, string status, int modifiedBy)
        {
            var audit = await GetByIdAsync(auditId);
            if (audit != null)
            {
                audit.Status = status;
                audit.ModifiedBy = modifiedBy;
                audit.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(audit);
            }
        }

        public async Task AssignLeadAuditorAsync(int auditId, int leadAuditorId, int modifiedBy)
        {
            var audit = await GetByIdAsync(auditId);
            if (audit != null)
            {
                audit.LeadAuditorId = leadAuditorId;
                audit.ModifiedBy = modifiedBy;
                audit.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(audit);
            }
        }

        public async Task<int> GetAuditCountByStatusAsync(string status)
        {
            return await _dbSet.CountAsync(a => a.Status == status);
        }
    }

    public class AuditTypeRepository : Repository<AuditType>, IAuditTypeRepository
    {
        public AuditTypeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AuditType>> GetActiveAuditTypesAsync()
        {
            return await _dbSet.Where(at => at.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<AuditType>> GetAuditTypesByCategoryAsync(string category)
        {
            return await _dbSet.Where(at => at.Category == category).ToListAsync();
        }

        public async Task<IEnumerable<AuditType>> SearchAuditTypesAsync(string searchTerm)
        {
            return await _dbSet.Where(at => 
                at.AuditTypeName.Contains(searchTerm) || 
                at.Description!.Contains(searchTerm)).ToListAsync();
        }

        public async Task<AuditType?> GetAuditTypeWithAuditsAsync(int auditTypeId)
        {
            return await _dbSet
                .Include(at => at.Audits)
                .FirstOrDefaultAsync(at => at.Id == auditTypeId);
        }
    }

    public class AuditServiceRepository : Repository<AuditService>, IAuditServiceRepository
    {
        public AuditServiceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AuditService>> GetServicesByAuditAsync(int auditId)
        {
            return await _dbSet.Where(aus => aus.AuditId == auditId).ToListAsync();
        }

        public async Task<IEnumerable<AuditService>> GetAuditsByServiceAsync(int serviceId)
        {
            return await _dbSet.Where(aus => aus.ServiceId == serviceId).ToListAsync();
        }

        public async Task<IEnumerable<AuditService>> GetAuditServicesByStatusAsync(string status)
        {
            return await _dbSet.Where(aus => aus.Status == status).ToListAsync();
        }

        public async Task UpdateAuditServiceStatusAsync(int auditServiceId, string status, string? comments, int modifiedBy)
        {
            var auditService = await GetByIdAsync(auditServiceId);
            if (auditService != null)
            {
                auditService.Status = status;
                auditService.Comments = comments;
                await UpdateAsync(auditService);
            }
        }

        public async Task<AuditService?> GetAuditServiceAsync(int auditId, int serviceId)
        {
            return await _dbSet.FirstOrDefaultAsync(aus => aus.AuditId == auditId && aus.ServiceId == serviceId);
        }

        public async Task AddServiceToAuditAsync(int auditId, int serviceId, int createdBy)
        {
            var existing = await GetAuditServiceAsync(auditId, serviceId);
            if (existing == null)
            {
                var auditService = new AuditService
                {
                    AuditId = auditId,
                    ServiceId = serviceId,
                    Status = "Planned",
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.UtcNow
                };
                await AddAsync(auditService);
            }
        }

        public async Task RemoveServiceFromAuditAsync(int auditId, int serviceId)
        {
            var auditService = await GetAuditServiceAsync(auditId, serviceId);
            if (auditService != null)
            {
                await DeleteAsync(auditService);
            }
        }
    }

    public class AuditTeamMemberRepository : Repository<AuditTeamMember>, IAuditTeamMemberRepository
    {
        public AuditTeamMemberRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AuditTeamMember>> GetTeamMembersByAuditAsync(int auditId)
        {
            return await _dbSet.Where(atm => atm.AuditId == auditId).ToListAsync();
        }

        public async Task<IEnumerable<AuditTeamMember>> GetAuditsByTeamMemberAsync(int userId)
        {
            return await _dbSet.Where(atm => atm.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<AuditTeamMember>> GetActiveTeamMembersAsync(int auditId)
        {
            return await _dbSet.Where(atm => atm.AuditId == auditId && atm.IsActive).ToListAsync();
        }

        public async Task<AuditTeamMember?> GetAuditTeamMemberAsync(int auditId, int userId)
        {
            return await _dbSet.FirstOrDefaultAsync(atm => atm.AuditId == auditId && atm.UserId == userId);
        }

        public async Task AddTeamMemberToAuditAsync(int auditId, int userId, string role, string? responsibilities, int assignedBy)
        {
            var existing = await GetAuditTeamMemberAsync(auditId, userId);
            if (existing == null)
            {
                var teamMember = new AuditTeamMember
                {
                    AuditId = auditId,
                    UserId = userId,
                    Role = role,
                    Responsibilities = responsibilities,
                    AssignedBy = assignedBy,
                    AssignedDate = DateTime.UtcNow,
                    IsActive = true
                };
                await AddAsync(teamMember);
            }
        }

        public async Task RemoveTeamMemberFromAuditAsync(int auditId, int userId)
        {
            var teamMember = await GetAuditTeamMemberAsync(auditId, userId);
            if (teamMember != null)
            {
                await DeleteAsync(teamMember);
            }
        }

        public async Task UpdateTeamMemberRoleAsync(int auditTeamMemberId, string role, string? responsibilities, int modifiedBy)
        {
            var teamMember = await GetByIdAsync(auditTeamMemberId);
            if (teamMember != null)
            {
                teamMember.Role = role;
                teamMember.Responsibilities = responsibilities;
                await UpdateAsync(teamMember);
            }
        }
    }

    public class AuditSiteRepository : Repository<AuditSite>, IAuditSiteRepository
    {
        public AuditSiteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AuditSite>> GetAuditSitesByCompanyAsync(int companyId)
        {
            return await _dbSet.Where(aus => aus.CompanyId == companyId).ToListAsync();
        }

        public async Task<IEnumerable<AuditSite>> GetAuditSitesByCityAsync(int cityId)
        {
            return await _dbSet.Where(aus => aus.CityId == cityId).ToListAsync();
        }

        public async Task<IEnumerable<AuditSite>> GetActiveAuditSitesAsync()
        {
            return await _dbSet.Where(aus => aus.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<AuditSite>> SearchAuditSitesAsync(string searchTerm)
        {
            return await _dbSet.Where(aus => 
                aus.SiteName.Contains(searchTerm) ||
                aus.Address!.Contains(searchTerm)).ToListAsync();
        }

        public async Task<AuditSite?> GetAuditSiteWithDetailsAsync(int auditSiteId)
        {
            return await _dbSet
                .Include(aus => aus.Company)
                .Include(aus => aus.City)
                .FirstOrDefaultAsync(aus => aus.Id == auditSiteId);
        }
    }

    public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByTableAsync(string tableName)
        {
            return await _dbSet.Where(al => al.TableName == tableName).OrderByDescending(al => al.ActionDate).ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByUserAsync(int userId)
        {
            return await _dbSet.Where(al => al.UserId == userId).OrderByDescending(al => al.ActionDate).ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByActionAsync(string action)
        {
            return await _dbSet.Where(al => al.Action == action).OrderByDescending(al => al.ActionDate).ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(al => al.ActionDate >= startDate && al.ActionDate <= endDate)
                .OrderByDescending(al => al.ActionDate).ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsForRecordAsync(string tableName, int recordId)
        {
            return await _dbSet.Where(al => al.TableName == tableName && al.RecordId == recordId)
                .OrderByDescending(al => al.ActionDate).ToListAsync();
        }

        public async Task LogActionAsync(string tableName, string action, int? recordId, string? oldValues, string? newValues, int? userId, string? userName, string? ipAddress)
        {
            var auditLog = new AuditLog
            {
                TableName = tableName,
                Action = action,
                RecordId = recordId,
                OldValues = oldValues,
                NewValues = newValues,
                UserId = userId,
                UserName = userName,
                IPAddress = ipAddress,
                ActionDate = DateTime.UtcNow
            };
            await AddAsync(auditLog);
        }
    }
}