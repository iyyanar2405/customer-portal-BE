using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CustomerPortalAPI.Modules.Master.Entities;

namespace CustomerPortalAPI.Modules.Financial.Entities
{
    [Table("Financials")]
    public class FinancialTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string TransactionName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? TransactionNumber { get; set; }

        [StringLength(100)]
        public string? TransactionType { get; set; }

        public int? CompanyId { get; set; }

        public int? SiteId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }

        [StringLength(10)]
        public string? Currency { get; set; }

        public DateTime? TransactionDate { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(255)]
        public string? Reference { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site? Site { get; set; }
    }

    [Table("Invoices")]
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required]
        public int CompanyId { get; set; }

        public int? SiteId { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? DueDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAmount { get; set; }

        [StringLength(10)]
        public string? Currency { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(255)]
        public string? BillingAddress { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(2000)]
        public string? Terms { get; set; }

        [StringLength(500)]
        public string? DocumentPath { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; } = null!;

        [ForeignKey("SiteId")]
        public virtual Site? Site { get; set; }

        public virtual ICollection<InvoiceAuditLog> InvoiceAuditLogs { get; set; } = new List<InvoiceAuditLog>();
    }

    [Table("InvoiceAuditLog")]
    public class InvoiceAuditLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        [StringLength(50)]
        public string? Action { get; set; }

        [StringLength(4000)]
        public string? OldValues { get; set; }

        [StringLength(4000)]
        public string? NewValues { get; set; }

        [Required]
        public DateTime ActionDate { get; set; } = DateTime.UtcNow;

        public int? UserId { get; set; }

        [StringLength(255)]
        public string? UserName { get; set; }

        [StringLength(500)]
        public string? Comments { get; set; }

        // Navigation Properties
        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; } = null!;
    }
}