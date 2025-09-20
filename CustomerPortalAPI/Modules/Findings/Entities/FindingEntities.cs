using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CustomerPortalAPI.Modules.Master.Entities;
using CustomerPortalAPI.Modules.Users.Entities;
using CustomerPortalAPI.Modules.Audits.Entities;

namespace CustomerPortalAPI.Modules.Findings.Entities
{
    [Table("Findings")]
    public class Finding
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FindingNumber { get; set; } = string.Empty;

        [Required]
        public int AuditId { get; set; }

        public int? SiteId { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string FindingType { get; set; } = string.Empty; // 'NC', 'OFI', 'Observation'

        [StringLength(50)]
        public string? Severity { get; set; } // 'Critical', 'Major', 'Minor'

        [Required]
        public int FindingStatusId { get; set; }

        public int? FindingCategoryId { get; set; }

        [Required]
        public DateTime IdentifiedDate { get; set; } = DateTime.UtcNow;

        public DateTime? DueDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public int? IdentifiedBy { get; set; } // Auditor who identified the finding

        public int? AssignedTo { get; set; } // Person responsible for resolution

        public string? Evidence { get; set; }

        public string? RootCause { get; set; }

        public string? CorrectiveAction { get; set; }

        public string? PreventiveAction { get; set; }

        public string? VerificationMethod { get; set; }

        public DateTime? CompletionDate { get; set; }

        public DateTime? VerificationDate { get; set; }

        public int? VerifiedBy { get; set; }

        // Navigation Properties
        [ForeignKey("AuditId")]
        public virtual Audit Audit { get; set; } = null!;

        [ForeignKey("SiteId")]
        public virtual Site? Site { get; set; }

        [ForeignKey("FindingStatusId")]
        public virtual FindingStatus FindingStatus { get; set; } = null!;

        [ForeignKey("FindingCategoryId")]
        public virtual FindingCategory? FindingCategory { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User? CreatedByUser { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual User? ModifiedByUser { get; set; }

        [ForeignKey("IdentifiedBy")]
        public virtual User? IdentifiedByUser { get; set; }

        [ForeignKey("AssignedTo")]
        public virtual User? AssignedToUser { get; set; }

        [ForeignKey("VerifiedBy")]
        public virtual User? VerifiedByUser { get; set; }
    }

    [Table("FindingCategories")]
    public class FindingCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string CategoryCode { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public int? ParentCategoryId { get; set; } // For hierarchical categories

        [StringLength(7)]
        public string? Color { get; set; } // Hex color code for UI display

        public int DisplayOrder { get; set; } = 999;

        // Navigation Properties
        [ForeignKey("ParentCategoryId")]
        public virtual FindingCategory? ParentCategory { get; set; }

        public virtual ICollection<FindingCategory> SubCategories { get; set; } = new List<FindingCategory>();
        public virtual ICollection<Finding> Findings { get; set; } = new List<Finding>();

        [ForeignKey("CreatedBy")]
        public virtual User? CreatedByUser { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual User? ModifiedByUser { get; set; }
    }

    [Table("FindingStatuses")]
    public class FindingStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string StatusName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string StatusCode { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        [StringLength(7)]
        public string? Color { get; set; } // Hex color code for UI display

        public int DisplayOrder { get; set; } = 999;

        public bool IsFinal { get; set; } = false; // Indicates if this is a final status (closed/resolved)

        // Navigation Properties
        public virtual ICollection<Finding> Findings { get; set; } = new List<Finding>();

        [ForeignKey("CreatedBy")]
        public virtual User? CreatedByUser { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual User? ModifiedByUser { get; set; }
    }
}