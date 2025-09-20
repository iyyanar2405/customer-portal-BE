using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.Shared.Entities
{
    /// <summary>
    /// Base entity class for all domain entities
    /// </summary>
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// Interface for auditable entities
    /// </summary>
    public interface IAuditableEntity
    {
        DateTime CreatedDate { get; set; }
        DateTime ModifiedDate { get; set; }
        int? CreatedBy { get; set; }
        int? ModifiedBy { get; set; }
    }

    /// <summary>
    /// Interface for soft-deletable entities
    /// </summary>
    public interface ISoftDeletable
    {
        bool IsActive { get; set; }
    }
}