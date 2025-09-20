using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortalAPI.Modules.Widgets.Entities
{
    [Table("Widgets")]
    public class Widget
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string WidgetName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string WidgetType { get; set; } = string.Empty; // Chart, Gauge, List, Card, Table, etc.

        [Required]
        [StringLength(50)]
        public string DataSource { get; set; } = string.Empty; // Audits, Certificates, Findings, etc.

        [StringLength(100)]
        public string? DataQuery { get; set; } // SQL query or API endpoint

        // Widget configuration stored as JSON
        [Column(TypeName = "nvarchar(max)")]
        public string? Configuration { get; set; }

        // Widget layout properties
        public int? PositionX { get; set; } = 0;
        public int? PositionY { get; set; } = 0;
        public int? Width { get; set; } = 4;
        public int? Height { get; set; } = 3;

        [StringLength(50)]
        public string? Color { get; set; }

        [StringLength(50)]
        public string? Icon { get; set; }

        public bool IsVisible { get; set; } = true;

        public bool IsResizable { get; set; } = true;

        public bool IsMovable { get; set; } = true;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active"; // Active, Inactive

        public int? RefreshInterval { get; set; } // in minutes

        public DateTime? LastRefreshed { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public bool IsActive { get; set; } = true;
    }

    [Table("WidgetCategories")]
    public class WidgetCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? Color { get; set; }

        [StringLength(50)]
        public string? Icon { get; set; }

        public int? SortOrder { get; set; } = 0;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active"; // Active, Inactive

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property
        public virtual ICollection<WidgetUserAccess> WidgetUserAccesses { get; set; } = new List<WidgetUserAccess>();
    }

    [Table("WidgetUserAccess")]
    public class WidgetUserAccess
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int WidgetId { get; set; }

        [Required]
        public int UserId { get; set; }

        public int? DashboardId { get; set; }

        public int? WidgetCategoryId { get; set; }

        public bool CanView { get; set; } = true;

        public bool CanEdit { get; set; } = false;

        public bool CanDelete { get; set; } = false;

        public bool CanConfigure { get; set; } = false;

        public bool IsOwner { get; set; } = false;

        public bool IsFavorite { get; set; } = false;

        // Personal widget configuration
        [Column(TypeName = "nvarchar(max)")]
        public string? PersonalConfiguration { get; set; }

        // Personal layout settings
        public int? PersonalPositionX { get; set; }
        public int? PersonalPositionY { get; set; }
        public int? PersonalWidth { get; set; }
        public int? PersonalHeight { get; set; }

        public bool IsVisible { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Widget Widget { get; set; } = null!;
        public virtual WidgetCategory? WidgetCategory { get; set; }
    }

    [Table("WidgetData")]
    public class WidgetData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int WidgetId { get; set; }

        // Data stored as JSON
        [Column(TypeName = "nvarchar(max)")]
        public string? Data { get; set; }

        [Required]
        public DateTime DataDate { get; set; } = DateTime.UtcNow;

        public DateTime? ValidUntil { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Current"; // Current, Expired, Invalid

        public int? CompanyId { get; set; }

        public int? SiteId { get; set; }

        [StringLength(100)]
        public string? DataVersion { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int CreatedBy { get; set; }

        // Navigation property
        public virtual Widget Widget { get; set; } = null!;
    }
}