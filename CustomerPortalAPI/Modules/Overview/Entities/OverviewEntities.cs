using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortalAPI.Modules.Overview.Entities
{
    [Table("OverviewDashboard")]
    public class OverviewDashboard
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string DashboardName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public int UserId { get; set; }

        public int? CompanyId { get; set; }

        [Required]
        [StringLength(50)]
        public string DashboardType { get; set; } = string.Empty; // Executive, Manager, Auditor, etc.

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active"; // Active, Inactive

        public bool IsDefault { get; set; } = false;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public bool IsActive { get; set; } = true;

        // Configuration JSON for dashboard layout
        [Column(TypeName = "nvarchar(max)")]
        public string? Configuration { get; set; }
    }

    [Table("OverviewMetrics")]
    public class OverviewMetric
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string MetricName { get; set; } = string.Empty;

        [StringLength(50)]
        public string MetricType { get; set; } = string.Empty; // Count, Percentage, Amount, etc.

        [Required]
        [StringLength(100)]
        public string MetricCategory { get; set; } = string.Empty; // Audits, Certificates, Findings, etc.

        public decimal? MetricValue { get; set; }

        [StringLength(20)]
        public string? Unit { get; set; } // %, $, count, etc.

        public int? CompanyId { get; set; }

        public int? SiteId { get; set; }

        [Required]
        public DateTime CalculatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime PeriodStart { get; set; }

        [Required]
        public DateTime PeriodEnd { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Current"; // Current, Historical

        [StringLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int CreatedBy { get; set; }
    }

    [Table("OverviewReports")]
    public class OverviewReport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string ReportName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string ReportType { get; set; } = string.Empty; // Summary, Detailed, Trend, etc.

        [Required]
        [StringLength(50)]
        public string ReportCategory { get; set; } = string.Empty; // Compliance, Financial, Operational

        public int? CompanyId { get; set; }

        public int? SiteId { get; set; }

        [Required]
        public DateTime GeneratedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime PeriodStart { get; set; }

        [Required]
        public DateTime PeriodEnd { get; set; }

        public int GeneratedBy { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Generated"; // Generated, Archived, Deleted

        // Report data stored as JSON
        [Column(TypeName = "nvarchar(max)")]
        public string? ReportData { get; set; }

        [StringLength(500)]
        public string? FilePath { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public bool IsScheduled { get; set; } = false;

        [StringLength(100)]
        public string? ScheduleFrequency { get; set; } // Daily, Weekly, Monthly, Quarterly

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }
    }
}