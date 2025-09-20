using CustomerPortal.Shared.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.AuditsService.Entities
{
    public class AuditSiteAudit : BaseEntity
    {
        [Required]
        public int AuditId { get; set; }

        [Required]
        public int SiteId { get; set; }

        public DateTime? AuditDate { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "PENDING";

        public int? LeadAuditorId { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        public decimal? ComplianceScore { get; set; }

        public int? FindingsCount { get; set; }

        public int? CriticalFindingsCount { get; set; }

        public int? MajorFindingsCount { get; set; }

        public int? MinorFindingsCount { get; set; }

        public int? ObservationsCount { get; set; }

        // Navigation properties
        [ForeignKey(nameof(AuditId))]
        public virtual Audit? Audit { get; set; }

        [ForeignKey(nameof(SiteId))]
        public virtual Site? Site { get; set; }

        [ForeignKey(nameof(LeadAuditorId))]
        public virtual User? LeadAuditor { get; set; }
    }
}