using CustomerPortal.Shared.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.AuditsService.Entities
{
    public class Audit : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string AuditNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string AuditTitle { get; set; } = string.Empty;

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int AuditTypeId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "PLANNED";

        public int? LeadAuditorId { get; set; }

        // Navigation properties
        public virtual Company? Company { get; set; }
        public virtual AuditType? AuditType { get; set; }
        public virtual User? LeadAuditor { get; set; }
        public virtual ICollection<AuditSite> AuditSites { get; set; } = new List<AuditSite>();
        public virtual ICollection<AuditService> AuditServices { get; set; } = new List<AuditService>();
        public virtual ICollection<AuditTeamMember> AuditTeamMembers { get; set; } = new List<AuditTeamMember>();
        public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
        public virtual ICollection<AuditSiteAudit> AuditSiteAudits { get; set; } = new List<AuditSiteAudit>();
    }

    public class Company : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string CompanyCode { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? ContactPerson { get; set; }

        [StringLength(50)]
        public string? ContactEmail { get; set; }

        [StringLength(20)]
        public string? ContactPhone { get; set; }

        // Navigation properties
        public virtual ICollection<Audit> Audits { get; set; } = new List<Audit>();
    }

    public class User : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(500)]
        public string? Qualifications { get; set; }

        [StringLength(20)]
        public string Role { get; set; } = "USER";

        // Navigation properties
        public virtual ICollection<Audit> LeadAudits { get; set; } = new List<Audit>();
        public virtual ICollection<AuditTeamMember> AuditTeamMembers { get; set; } = new List<AuditTeamMember>();
    }

    public class Site : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string SiteName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string SiteCode { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? ContactPerson { get; set; }

        [StringLength(50)]
        public string? ContactEmail { get; set; }

        [StringLength(20)]
        public string? ContactPhone { get; set; }

        public int CompanyId { get; set; }

        // Navigation properties
        public virtual Company? Company { get; set; }
        public virtual ICollection<AuditSite> AuditSites { get; set; } = new List<AuditSite>();
        public virtual ICollection<AuditSiteAudit> AuditSiteAudits { get; set; } = new List<AuditSiteAudit>();
    }

    public class Service : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string ServiceName { get; set; } = string.Empty;

        [StringLength(20)]
        public string? ServiceCode { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<AuditService> AuditServices { get; set; } = new List<AuditService>();
    }
}