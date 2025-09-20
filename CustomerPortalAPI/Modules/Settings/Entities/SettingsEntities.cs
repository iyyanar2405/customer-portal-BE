using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CustomerPortalAPI.Modules.Users.Entities;

namespace CustomerPortalAPI.Modules.Settings.Entities
{
    [Table("Trainings")]
    public class Training
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string TrainingName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string TrainingCode { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? TrainingType { get; set; } // 'Online', 'Classroom', 'Hybrid', 'Self-Paced'

        [StringLength(100)]
        public string? Category { get; set; }

        public int? Duration { get; set; } // Duration in hours

        public DateTime? DueDate { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        [StringLength(1000)]
        public string? Prerequisites { get; set; }

        public string? LearningObjectives { get; set; }

        public string? Materials { get; set; } // JSON array of materials/resources

        public bool AssessmentRequired { get; set; } = false;

        public int? PassingScore { get; set; } // Percentage required to pass

        public bool CertificateIssued { get; set; } = false;

        public int? ValidityPeriod { get; set; } // Validity in months

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Cost { get; set; }

        [StringLength(3)]
        public string Currency { get; set; } = "USD";

        // Navigation Properties
        [ForeignKey("CreatedBy")]
        public virtual User? CreatedByUser { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual User? ModifiedByUser { get; set; }

        public virtual ICollection<UserTraining> UserTrainings { get; set; } = new List<UserTraining>();
    }

    [Table("ErrorLogs")]
    public class ErrorLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ErrorMessage { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ErrorType { get; set; } // 'Database', 'Application', 'Authentication', 'Validation', 'Network'

        [Required]
        [StringLength(50)]
        public string Severity { get; set; } = "Error"; // 'Info', 'Warning', 'Error', 'Critical'

        [StringLength(200)]
        public string? Source { get; set; } // Source component/module

        public string? StackTrace { get; set; }

        public int? UserId { get; set; }

        [StringLength(100)]
        public string? SessionId { get; set; }

        [StringLength(45)]
        public string? IPAddress { get; set; }

        [StringLength(500)]
        public string? UserAgent { get; set; }

        [StringLength(500)]
        public string? RequestUrl { get; set; }

        [StringLength(10)]
        public string? RequestMethod { get; set; } // GET, POST, PUT, DELETE

        public string? RequestBody { get; set; }

        [StringLength(50)]
        public string? ErrorCode { get; set; }

        public string? InnerException { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string? MachineName { get; set; }

        public int? ProcessId { get; set; }

        public int? ThreadId { get; set; }

        [StringLength(100)]
        public string ApplicationName { get; set; } = "CustomerPortal";

        [StringLength(50)]
        public string Environment { get; set; } = "Production"; // 'Development', 'Testing', 'Staging', 'Production'

        [StringLength(100)]
        public string? CorrelationId { get; set; } // For tracking related requests

        public string? AdditionalData { get; set; } // JSON for additional context

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}