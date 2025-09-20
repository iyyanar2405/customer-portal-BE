using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CustomerPortalAPI.Modules.Master.Entities;
using CustomerPortalAPI.Modules.Users.Entities;

namespace CustomerPortalAPI.Modules.Audits.Entities
{
    [Table("Audits")]
    public class Audit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string AuditName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? AuditNumber { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int SiteId { get; set; }

        public int? AuditTypeId { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        public DateTime? PlannedStartDate { get; set; }

        public DateTime? PlannedEndDate { get; set; }

        public DateTime? ActualStartDate { get; set; }

        public DateTime? ActualEndDate { get; set; }

        public int? LeadAuditorId { get; set; }

        [StringLength(2000)]
        public string? Scope { get; set; }

        [StringLength(2000)]
        public string? Objectives { get; set; }

        [StringLength(2000)]
        public string? Summary { get; set; }

        [StringLength(2000)]
        public string? Recommendations { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; } = null!;

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; } = null!;

        [ForeignKey("AuditTypeId")]
        public virtual AuditType? AuditType { get; set; }

        [ForeignKey("LeadAuditorId")]
        public virtual User? LeadAuditor { get; set; }

        public virtual ICollection<AuditService> AuditServices { get; set; } = new List<AuditService>();
        public virtual ICollection<AuditTeamMember> AuditTeamMembers { get; set; } = new List<AuditTeamMember>();
        public virtual ICollection<AuditSiteAudit> AuditSiteAudits { get; set; } = new List<AuditSiteAudit>();
        // public virtual ICollection<Finding> Findings { get; set; } = new List<Finding>();
    }

    [Table("AuditTypes")]
    public class AuditType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string AuditTypeName { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? Category { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        public virtual ICollection<Audit> Audits { get; set; } = new List<Audit>();
    }

    [Table("AuditServices")]
    public class AuditService
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AuditId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(1000)]
        public string? Comments { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        // Navigation Properties
        [ForeignKey("AuditId")]
        public virtual Audit Audit { get; set; } = null!;

        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; } = null!;
    }

    [Table("AuditTeamMembers")]
    public class AuditTeamMember
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AuditId { get; set; }

        [Required]
        public int UserId { get; set; }

        [StringLength(100)]
        public string? Role { get; set; }

        [StringLength(500)]
        public string? Responsibilities { get; set; }

        [Required]
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

        public int? AssignedBy { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        [ForeignKey("AuditId")]
        public virtual Audit Audit { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }

    [Table("AuditSites")]
    public class AuditSite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string SiteName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Address { get; set; }

        public int? CompanyId { get; set; }

        public int? CityId { get; set; }

        [StringLength(50)]
        public string? ContactPerson { get; set; }

        [StringLength(50)]
        public string? Phone { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }

        [ForeignKey("CityId")]
        public virtual City? City { get; set; }

        public virtual ICollection<AuditSiteAudit> AuditSiteAudits { get; set; } = new List<AuditSiteAudit>();
        public virtual ICollection<AuditSiteRepresentative> AuditSiteRepresentatives { get; set; } = new List<AuditSiteRepresentative>();
        public virtual ICollection<AuditSiteService> AuditSiteServices { get; set; } = new List<AuditSiteService>();
    }

    [Table("AuditSiteAudits")]
    public class AuditSiteAudit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AuditId { get; set; }

        [Required]
        public int AuditSiteId { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        public DateTime? ScheduledDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        [StringLength(1000)]
        public string? Comments { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        // Navigation Properties
        [ForeignKey("AuditId")]
        public virtual Audit Audit { get; set; } = null!;

        [ForeignKey("AuditSiteId")]
        public virtual AuditSite AuditSite { get; set; } = null!;
    }

    [Table("AuditSiteRepresentatives")]
    public class AuditSiteRepresentative
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AuditSiteId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Position { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(50)]
        public string? Phone { get; set; }

        [StringLength(50)]
        public string? Role { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        // Navigation Properties
        [ForeignKey("AuditSiteId")]
        public virtual AuditSite AuditSite { get; set; } = null!;
    }

    [Table("AuditSiteServices")]
    public class AuditSiteService
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AuditSiteId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(1000)]
        public string? Comments { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        // Navigation Properties
        [ForeignKey("AuditSiteId")]
        public virtual AuditSite AuditSite { get; set; } = null!;

        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; } = null!;
    }

    [Table("AuditLogs")]
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string? TableName { get; set; }

        [StringLength(50)]
        public string? Action { get; set; }

        public int? RecordId { get; set; }

        [StringLength(4000)]
        public string? OldValues { get; set; }

        [StringLength(4000)]
        public string? NewValues { get; set; }

        [Required]
        public DateTime ActionDate { get; set; } = DateTime.UtcNow;

        public int? UserId { get; set; }

        [StringLength(255)]
        public string? UserName { get; set; }

        [StringLength(100)]
        public string? IPAddress { get; set; }

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}