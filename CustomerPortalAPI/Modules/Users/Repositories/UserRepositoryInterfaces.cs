using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Users.Entities;

namespace CustomerPortalAPI.Modules.Users.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByCompanyAsync(int companyId);
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<User?> AuthenticateAsync(string username, string password);
        Task<bool> IsUsernameAvailableAsync(string username);
        Task<bool> IsEmailAvailableAsync(string email);
        Task UpdateLastLoginAsync(int userId);
        Task<IEnumerable<User>> SearchUsersAsync(string searchTerm);
    }

    public interface IUserRoleRepository : IRepository<UserRole>
    {
        Task<IEnumerable<UserRole>> GetUserRolesAsync(int userId);
        Task<IEnumerable<UserRole>> GetRoleUsersAsync(int roleId);
        Task<bool> HasRoleAsync(int userId, int roleId);
        Task AssignRoleAsync(int userId, int roleId, int assignedBy);
        Task RemoveRoleAsync(int userId, int roleId);
        Task<IEnumerable<UserRole>> GetActiveUserRolesAsync(int userId);
    }

    public interface IUserCompanyAccessRepository : IRepository<UserCompanyAccess>
    {
        Task<IEnumerable<UserCompanyAccess>> GetUserCompanyAccessAsync(int userId);
        Task<IEnumerable<UserCompanyAccess>> GetCompanyUsersAsync(int companyId);
        Task<bool> HasCompanyAccessAsync(int userId, int companyId);
        Task GrantCompanyAccessAsync(int userId, int companyId, string accessLevel, int grantedBy);
        Task RevokeCompanyAccessAsync(int userId, int companyId);
        Task<IEnumerable<UserCompanyAccess>> GetActiveAccessByUserAsync(int userId);
    }

    public interface IUserSiteAccessRepository : IRepository<UserSiteAccess>
    {
        Task<IEnumerable<UserSiteAccess>> GetUserSiteAccessAsync(int userId);
        Task<IEnumerable<UserSiteAccess>> GetSiteUsersAsync(int siteId);
        Task<bool> HasSiteAccessAsync(int userId, int siteId);
        Task GrantSiteAccessAsync(int userId, int siteId, string accessLevel, int grantedBy);
        Task RevokeSiteAccessAsync(int userId, int siteId);
        Task<IEnumerable<UserSiteAccess>> GetActiveAccessByUserAsync(int userId);
    }

    public interface IUserServiceAccessRepository : IRepository<UserServiceAccess>
    {
        Task<IEnumerable<UserServiceAccess>> GetUserServiceAccessAsync(int userId);
        Task<IEnumerable<UserServiceAccess>> GetServiceUsersAsync(int serviceId);
        Task<bool> HasServiceAccessAsync(int userId, int serviceId);
        Task GrantServiceAccessAsync(int userId, int serviceId, string accessLevel, int grantedBy);
        Task RevokeServiceAccessAsync(int userId, int serviceId);
    }

    public interface IUserCityAccessRepository : IRepository<UserCityAccess>
    {
        Task<IEnumerable<UserCityAccess>> GetUserCityAccessAsync(int userId);
        Task<IEnumerable<UserCityAccess>> GetCityUsersAsync(int cityId);
        Task<bool> HasCityAccessAsync(int userId, int cityId);
        Task GrantCityAccessAsync(int userId, int cityId, string accessLevel, int grantedBy);
        Task RevokeCityAccessAsync(int userId, int cityId);
    }

    public interface IUserCountryAccessRepository : IRepository<UserCountryAccess>
    {
        Task<IEnumerable<UserCountryAccess>> GetUserCountryAccessAsync(int userId);
        Task<IEnumerable<UserCountryAccess>> GetCountryUsersAsync(int countryId);
        Task<bool> HasCountryAccessAsync(int userId, int countryId);
        Task GrantCountryAccessAsync(int userId, int countryId, string accessLevel, int grantedBy);
        Task RevokeCountryAccessAsync(int userId, int countryId);
    }

    public interface IUserNotificationAccessRepository : IRepository<UserNotificationAccess>
    {
        Task<IEnumerable<UserNotificationAccess>> GetUserNotificationAccessAsync(int userId);
        Task<IEnumerable<UserNotificationAccess>> GetNotificationUsersAsync(int notificationCategoryId);
        Task<bool> CanReceiveNotificationAsync(int userId, int notificationCategoryId);
        Task<bool> CanManageNotificationAsync(int userId, int notificationCategoryId);
        Task UpdateNotificationAccessAsync(int userId, int notificationCategoryId, bool canReceive, bool canManage);
    }

    public interface IUserPreferenceRepository : IRepository<UserPreference>
    {
        Task<UserPreference?> GetUserPreferenceAsync(int userId, string preferenceKey);
        Task<IEnumerable<UserPreference>> GetUserPreferencesAsync(int userId);
        Task SetUserPreferenceAsync(int userId, string preferenceKey, string preferenceValue);
        Task RemoveUserPreferenceAsync(int userId, string preferenceKey);
        Task<Dictionary<string, string>> GetUserPreferencesDictionaryAsync(int userId);
    }

    public interface IUserTrainingRepository : IRepository<UserTraining>
    {
        Task<IEnumerable<UserTraining>> GetUserTrainingsAsync(int userId);
        Task<IEnumerable<UserTraining>> GetTrainingUsersAsync(int trainingId);
        Task<IEnumerable<UserTraining>> GetCompletedTrainingsAsync(int userId);
        Task<IEnumerable<UserTraining>> GetPendingTrainingsAsync(int userId);
        Task EnrollUserInTrainingAsync(int userId, int trainingId, int enrolledBy);
        Task CompleteTrainingAsync(int userId, int trainingId, decimal? score, string? comments);
        Task<UserTraining?> GetUserTrainingAsync(int userId, int trainingId);
    }
}