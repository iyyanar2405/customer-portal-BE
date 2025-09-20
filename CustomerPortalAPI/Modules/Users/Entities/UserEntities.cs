using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CustomerPortalAPI.Modules.Master.Entities;

namespace CustomerPortalAPI.Modules.Users.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [StringLength(255)]
        public string? PasswordHash { get; set; }

        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        [StringLength(50)]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string? JobTitle { get; set; }

        [StringLength(255)]
        public string? Department { get; set; }

        public int? CompanyId { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsEmailVerified { get; set; } = false;

        public DateTime? LastLoginDate { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }
        
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<UserCompanyAccess> UserCompanyAccesses { get; set; } = new List<UserCompanyAccess>();
        public virtual ICollection<UserSiteAccess> UserSiteAccesses { get; set; } = new List<UserSiteAccess>();
        public virtual ICollection<UserServiceAccess> UserServiceAccesses { get; set; } = new List<UserServiceAccess>();
        public virtual ICollection<UserCityAccess> UserCityAccesses { get; set; } = new List<UserCityAccess>();
        public virtual ICollection<UserCountryAccess> UserCountryAccesses { get; set; } = new List<UserCountryAccess>();
        public virtual ICollection<UserNotificationAccess> UserNotificationAccesses { get; set; } = new List<UserNotificationAccess>();
        public virtual ICollection<UserPreference> UserPreferences { get; set; } = new List<UserPreference>();
        public virtual ICollection<UserTraining> UserTrainings { get; set; } = new List<UserTraining>();
    }

    [Table("UserRoles")]
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

        public int? AssignedBy { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } = null!;
    }

    [Table("UserCompanyAccess")]
    public class UserCompanyAccess
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [StringLength(50)]
        public string? AccessLevel { get; set; }

        [Required]
        public DateTime GrantedDate { get; set; } = DateTime.UtcNow;

        public int? GrantedBy { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; } = null!;
    }

    [Table("UserSiteAccess")]
    public class UserSiteAccess
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int SiteId { get; set; }

        [StringLength(50)]
        public string? AccessLevel { get; set; }

        [Required]
        public DateTime GrantedDate { get; set; } = DateTime.UtcNow;

        public int? GrantedBy { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; } = null!;
    }

    [Table("UserServiceAccess")]
    public class UserServiceAccess
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [StringLength(50)]
        public string? AccessLevel { get; set; }

        [Required]
        public DateTime GrantedDate { get; set; } = DateTime.UtcNow;

        public int? GrantedBy { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; } = null!;
    }

    [Table("UserCityAccess")]
    public class UserCityAccess
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CityId { get; set; }

        [StringLength(50)]
        public string? AccessLevel { get; set; }

        [Required]
        public DateTime GrantedDate { get; set; } = DateTime.UtcNow;

        public int? GrantedBy { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("CityId")]
        public virtual City City { get; set; } = null!;
    }

    [Table("UserCountryAccess")]
    public class UserCountryAccess
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CountryId { get; set; }

        [StringLength(50)]
        public string? AccessLevel { get; set; }

        [Required]
        public DateTime GrantedDate { get; set; } = DateTime.UtcNow;

        public int? GrantedBy { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; } = null!;
    }

    [Table("UserNotificationAccess")]
    public class UserNotificationAccess
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int NotificationCategoryId { get; set; }

        public bool CanReceive { get; set; } = true;

        public bool CanManage { get; set; } = false;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        // [ForeignKey("NotificationCategoryId")]
        // public virtual NotificationCategory NotificationCategory { get; set; } = null!;
    }

    [Table("UserPreferences")]
    public class UserPreference
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string PreferenceKey { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? PreferenceValue { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }

    [Table("UserTrainings")]
    public class UserTraining
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int TrainingId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        public decimal? Score { get; set; }

        [StringLength(500)]
        public string? Comments { get; set; }

        [Required]
        public DateTime EnrolledDate { get; set; } = DateTime.UtcNow;

        public int? EnrolledBy { get; set; }

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        // [ForeignKey("TrainingId")]
        // public virtual Training Training { get; set; } = null!;
    }
}