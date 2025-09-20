using HotChocolate;
using CustomerPortalAPI.Modules.Shared;

namespace CustomerPortalAPI.Modules.Users.GraphQL
{
    // Input Types
    public record CreateUserInput(
        string Username, 
        string Email, 
        string? FirstName, 
        string? LastName, 
        string? Phone, 
        string? JobTitle, 
        string? Department, 
        int? CompanyId);

    public record UpdateUserInput(
        int Id, 
        string? Username, 
        string? Email, 
        string? FirstName, 
        string? LastName, 
        string? Phone, 
        string? JobTitle, 
        string? Department, 
        int? CompanyId, 
        bool? IsActive, 
        bool? IsEmailVerified);

    public record CreateUserRoleInput(int UserId, int RoleId, int AssignedBy);
    public record UpdateUserRoleInput(int Id, bool? IsActive);

    public record CreateUserCompanyAccessInput(int UserId, int CompanyId, string? AccessLevel, int GrantedBy);
    public record UpdateUserCompanyAccessInput(int Id, string? AccessLevel, bool? IsActive);

    public record CreateUserPreferenceInput(int UserId, string PreferenceKey, string? PreferenceValue);
    public record UpdateUserPreferenceInput(int Id, string? PreferenceValue);

    // Output Types
    public record UserOutput(
        int Id,
        string Username,
        string Email,
        string? FirstName,
        string? LastName,
        string? Phone,
        string? JobTitle,
        string? Department,
        int? CompanyId,
        bool IsActive,
        bool IsEmailVerified,
        DateTime? LastLoginDate,
        DateTime CreatedDate);

    public record UserRoleOutput(
        int Id,
        int UserId,
        int RoleId,
        DateTime AssignedDate,
        int? AssignedBy,
        bool IsActive);

    public record UserCompanyAccessOutput(
        int Id,
        int UserId,
        int CompanyId,
        string? AccessLevel,
        DateTime GrantedDate,
        int? GrantedBy,
        bool IsActive);

    public record UserSiteAccessOutput(
        int Id,
        int UserId,
        int SiteId,
        string? AccessLevel,
        DateTime GrantedDate,
        int? GrantedBy,
        bool IsActive);

    public record UserPreferenceOutput(
        int Id,
        int UserId,
        string PreferenceKey,
        string? PreferenceValue,
        DateTime CreatedDate,
        DateTime? ModifiedDate);

    public record UserTrainingOutput(
        int Id,
        int UserId,
        int TrainingId,
        DateTime? StartDate,
        DateTime? CompletionDate,
        string? Status,
        decimal? Score,
        string? Comments,
        DateTime EnrolledDate,
        int? EnrolledBy);

    // Payload Types
    public record CreateUserPayload(UserOutput? User, string? Error);
    public record UpdateUserPayload(UserOutput? User, string? Error);

    // Filter Input
    public record UserFilterInput(
        string? Username, 
        string? Email, 
        string? Name, 
        int? CompanyId, 
        bool? IsActive, 
        bool? IsEmailVerified);
}