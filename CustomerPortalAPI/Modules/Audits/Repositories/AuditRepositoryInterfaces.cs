using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Audits.Entities;

namespace CustomerPortalAPI.Modules.Audits.Repositories
{
    public interface IAuditRepository : IRepository<Audit>
    {
        Task<IEnumerable<Audit>> GetAuditsByCompanyAsync(int companyId);
        Task<IEnumerable<Audit>> GetAuditsBySiteAsync(int siteId);
        Task<IEnumerable<Audit>> GetAuditsByStatusAsync(string status);
        Task<IEnumerable<Audit>> GetAuditsByTypeAsync(int auditTypeId);
        Task<IEnumerable<Audit>> GetAuditsByLeadAuditorAsync(int leadAuditorId);
        Task<IEnumerable<Audit>> GetActiveAuditsAsync();
        Task<IEnumerable<Audit>> GetCompletedAuditsAsync();
        Task<IEnumerable<Audit>> GetUpcomingAuditsAsync();
        Task<IEnumerable<Audit>> GetOverdueAuditsAsync();
        Task<Audit?> GetAuditWithDetailsAsync(int auditId);
        Task<Audit?> GetByAuditNumberAsync(string auditNumber);
        Task<IEnumerable<Audit>> GetAuditsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Audit>> SearchAuditsAsync(string searchTerm);
        Task UpdateAuditStatusAsync(int auditId, string status, int modifiedBy);
        Task AssignLeadAuditorAsync(int auditId, int leadAuditorId, int modifiedBy);
        Task<int> GetAuditCountByStatusAsync(string status);
    }

    public interface IAuditTypeRepository : IRepository<AuditType>
    {
        Task<IEnumerable<AuditType>> GetActiveAuditTypesAsync();
        Task<IEnumerable<AuditType>> GetAuditTypesByCategoryAsync(string category);
        Task<IEnumerable<AuditType>> SearchAuditTypesAsync(string searchTerm);
        Task<AuditType?> GetAuditTypeWithAuditsAsync(int auditTypeId);
    }

    public interface IAuditServiceRepository : IRepository<AuditService>
    {
        Task<IEnumerable<AuditService>> GetServicesByAuditAsync(int auditId);
        Task<IEnumerable<AuditService>> GetAuditsByServiceAsync(int serviceId);
        Task<IEnumerable<AuditService>> GetAuditServicesByStatusAsync(string status);
        Task UpdateAuditServiceStatusAsync(int auditServiceId, string status, string? comments, int modifiedBy);
        Task<AuditService?> GetAuditServiceAsync(int auditId, int serviceId);
        Task AddServiceToAuditAsync(int auditId, int serviceId, int createdBy);
        Task RemoveServiceFromAuditAsync(int auditId, int serviceId);
    }

    public interface IAuditTeamMemberRepository : IRepository<AuditTeamMember>
    {
        Task<IEnumerable<AuditTeamMember>> GetTeamMembersByAuditAsync(int auditId);
        Task<IEnumerable<AuditTeamMember>> GetAuditsByTeamMemberAsync(int userId);
        Task<IEnumerable<AuditTeamMember>> GetActiveTeamMembersAsync(int auditId);
        Task<AuditTeamMember?> GetAuditTeamMemberAsync(int auditId, int userId);
        Task AddTeamMemberToAuditAsync(int auditId, int userId, string role, string? responsibilities, int assignedBy);
        Task RemoveTeamMemberFromAuditAsync(int auditId, int userId);
        Task UpdateTeamMemberRoleAsync(int auditTeamMemberId, string role, string? responsibilities, int modifiedBy);
    }

    public interface IAuditSiteRepository : IRepository<AuditSite>
    {
        Task<IEnumerable<AuditSite>> GetAuditSitesByCompanyAsync(int companyId);
        Task<IEnumerable<AuditSite>> GetAuditSitesByCityAsync(int cityId);
        Task<IEnumerable<AuditSite>> GetActiveAuditSitesAsync();
        Task<IEnumerable<AuditSite>> SearchAuditSitesAsync(string searchTerm);
        Task<AuditSite?> GetAuditSiteWithDetailsAsync(int auditSiteId);
    }

    public interface IAuditLogRepository : IRepository<AuditLog>
    {
        Task<IEnumerable<AuditLog>> GetAuditLogsByTableAsync(string tableName);
        Task<IEnumerable<AuditLog>> GetAuditLogsByUserAsync(int userId);
        Task<IEnumerable<AuditLog>> GetAuditLogsByActionAsync(string action);
        Task<IEnumerable<AuditLog>> GetAuditLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<AuditLog>> GetAuditLogsForRecordAsync(string tableName, int recordId);
        Task LogActionAsync(string tableName, string action, int? recordId, string? oldValues, string? newValues, int? userId, string? userName, string? ipAddress);
    }
}