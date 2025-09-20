using CustomerPortalAPI.Data;
using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortalAPI.Modules.Users.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetUsersByCompanyAsync(int companyId)
        {
            return await _dbSet.Where(u => u.CompanyId == companyId).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _dbSet.Where(u => u.IsActive).ToListAsync();
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            // Note: In a real implementation, you'd hash the password and compare
            // This is a simplified version
            return await _dbSet.FirstOrDefaultAsync(u => 
                u.Username == username && u.IsActive);
        }

        public async Task<bool> IsUsernameAvailableAsync(string username)
        {
            return !await _dbSet.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            return !await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.LastLoginDate = DateTime.UtcNow;
                await UpdateAsync(user);
            }
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm)
        {
            return await _dbSet.Where(u => 
                u.Username.Contains(searchTerm) ||
                u.Email.Contains(searchTerm) ||
                (u.FirstName != null && u.FirstName.Contains(searchTerm)) ||
                (u.LastName != null && u.LastName.Contains(searchTerm)))
                .ToListAsync();
        }
    }

    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserRole>> GetUserRolesAsync(int userId)
        {
            return await _dbSet.Where(ur => ur.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<UserRole>> GetRoleUsersAsync(int roleId)
        {
            return await _dbSet.Where(ur => ur.RoleId == roleId).ToListAsync();
        }

        public async Task<bool> HasRoleAsync(int userId, int roleId)
        {
            return await _dbSet.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        }

        public async Task AssignRoleAsync(int userId, int roleId, int assignedBy)
        {
            if (!await HasRoleAsync(userId, roleId))
            {
                var userRole = new UserRole
                {
                    UserId = userId,
                    RoleId = roleId,
                    AssignedBy = assignedBy,
                    AssignedDate = DateTime.UtcNow
                };
                await AddAsync(userRole);
            }
        }

        public async Task RemoveRoleAsync(int userId, int roleId)
        {
            var userRole = await _dbSet.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
            if (userRole != null)
            {
                await DeleteAsync(userRole);
            }
        }

        public async Task<IEnumerable<UserRole>> GetActiveUserRolesAsync(int userId)
        {
            return await _dbSet.Where(ur => ur.UserId == userId && ur.IsActive).ToListAsync();
        }
    }

    public class UserCompanyAccessRepository : Repository<UserCompanyAccess>, IUserCompanyAccessRepository
    {
        public UserCompanyAccessRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserCompanyAccess>> GetUserCompanyAccessAsync(int userId)
        {
            return await _dbSet.Where(uca => uca.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<UserCompanyAccess>> GetCompanyUsersAsync(int companyId)
        {
            return await _dbSet.Where(uca => uca.CompanyId == companyId).ToListAsync();
        }

        public async Task<bool> HasCompanyAccessAsync(int userId, int companyId)
        {
            return await _dbSet.AnyAsync(uca => uca.UserId == userId && uca.CompanyId == companyId);
        }

        public async Task GrantCompanyAccessAsync(int userId, int companyId, string accessLevel, int grantedBy)
        {
            if (!await HasCompanyAccessAsync(userId, companyId))
            {
                var access = new UserCompanyAccess
                {
                    UserId = userId,
                    CompanyId = companyId,
                    AccessLevel = accessLevel,
                    GrantedBy = grantedBy,
                    GrantedDate = DateTime.UtcNow
                };
                await AddAsync(access);
            }
        }

        public async Task RevokeCompanyAccessAsync(int userId, int companyId)
        {
            var access = await _dbSet.FirstOrDefaultAsync(uca => uca.UserId == userId && uca.CompanyId == companyId);
            if (access != null)
            {
                await DeleteAsync(access);
            }
        }

        public async Task<IEnumerable<UserCompanyAccess>> GetActiveAccessByUserAsync(int userId)
        {
            return await _dbSet.Where(uca => uca.UserId == userId && uca.IsActive).ToListAsync();
        }
    }

    public class UserSiteAccessRepository : Repository<UserSiteAccess>, IUserSiteAccessRepository
    {
        public UserSiteAccessRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserSiteAccess>> GetUserSiteAccessAsync(int userId)
        {
            return await _dbSet.Where(usa => usa.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<UserSiteAccess>> GetSiteUsersAsync(int siteId)
        {
            return await _dbSet.Where(usa => usa.SiteId == siteId).ToListAsync();
        }

        public async Task<bool> HasSiteAccessAsync(int userId, int siteId)
        {
            return await _dbSet.AnyAsync(usa => usa.UserId == userId && usa.SiteId == siteId);
        }

        public async Task GrantSiteAccessAsync(int userId, int siteId, string accessLevel, int grantedBy)
        {
            if (!await HasSiteAccessAsync(userId, siteId))
            {
                var access = new UserSiteAccess
                {
                    UserId = userId,
                    SiteId = siteId,
                    AccessLevel = accessLevel,
                    GrantedBy = grantedBy,
                    GrantedDate = DateTime.UtcNow
                };
                await AddAsync(access);
            }
        }

        public async Task RevokeSiteAccessAsync(int userId, int siteId)
        {
            var access = await _dbSet.FirstOrDefaultAsync(usa => usa.UserId == userId && usa.SiteId == siteId);
            if (access != null)
            {
                await DeleteAsync(access);
            }
        }

        public async Task<IEnumerable<UserSiteAccess>> GetActiveAccessByUserAsync(int userId)
        {
            return await _dbSet.Where(usa => usa.UserId == userId && usa.IsActive).ToListAsync();
        }
    }

    public class UserServiceAccessRepository : Repository<UserServiceAccess>, IUserServiceAccessRepository
    {
        public UserServiceAccessRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserServiceAccess>> GetUserServiceAccessAsync(int userId)
        {
            return await _dbSet.Where(usa => usa.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<UserServiceAccess>> GetServiceUsersAsync(int serviceId)
        {
            return await _dbSet.Where(usa => usa.ServiceId == serviceId).ToListAsync();
        }

        public async Task<bool> HasServiceAccessAsync(int userId, int serviceId)
        {
            return await _dbSet.AnyAsync(usa => usa.UserId == userId && usa.ServiceId == serviceId);
        }

        public async Task GrantServiceAccessAsync(int userId, int serviceId, string accessLevel, int grantedBy)
        {
            if (!await HasServiceAccessAsync(userId, serviceId))
            {
                var access = new UserServiceAccess
                {
                    UserId = userId,
                    ServiceId = serviceId,
                    AccessLevel = accessLevel,
                    GrantedBy = grantedBy,
                    GrantedDate = DateTime.UtcNow
                };
                await AddAsync(access);
            }
        }

        public async Task RevokeServiceAccessAsync(int userId, int serviceId)
        {
            var access = await _dbSet.FirstOrDefaultAsync(usa => usa.UserId == userId && usa.ServiceId == serviceId);
            if (access != null)
            {
                await DeleteAsync(access);
            }
        }
    }

    public class UserCityAccessRepository : Repository<UserCityAccess>, IUserCityAccessRepository
    {
        public UserCityAccessRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserCityAccess>> GetUserCityAccessAsync(int userId)
        {
            return await _dbSet.Where(uca => uca.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<UserCityAccess>> GetCityUsersAsync(int cityId)
        {
            return await _dbSet.Where(uca => uca.CityId == cityId).ToListAsync();
        }

        public async Task<bool> HasCityAccessAsync(int userId, int cityId)
        {
            return await _dbSet.AnyAsync(uca => uca.UserId == userId && uca.CityId == cityId);
        }

        public async Task GrantCityAccessAsync(int userId, int cityId, string accessLevel, int grantedBy)
        {
            if (!await HasCityAccessAsync(userId, cityId))
            {
                var access = new UserCityAccess
                {
                    UserId = userId,
                    CityId = cityId,
                    AccessLevel = accessLevel,
                    GrantedBy = grantedBy,
                    GrantedDate = DateTime.UtcNow
                };
                await AddAsync(access);
            }
        }

        public async Task RevokeCityAccessAsync(int userId, int cityId)
        {
            var access = await _dbSet.FirstOrDefaultAsync(uca => uca.UserId == userId && uca.CityId == cityId);
            if (access != null)
            {
                await DeleteAsync(access);
            }
        }
    }

    public class UserCountryAccessRepository : Repository<UserCountryAccess>, IUserCountryAccessRepository
    {
        public UserCountryAccessRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserCountryAccess>> GetUserCountryAccessAsync(int userId)
        {
            return await _dbSet.Where(uca => uca.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<UserCountryAccess>> GetCountryUsersAsync(int countryId)
        {
            return await _dbSet.Where(uca => uca.CountryId == countryId).ToListAsync();
        }

        public async Task<bool> HasCountryAccessAsync(int userId, int countryId)
        {
            return await _dbSet.AnyAsync(uca => uca.UserId == userId && uca.CountryId == countryId);
        }

        public async Task GrantCountryAccessAsync(int userId, int countryId, string accessLevel, int grantedBy)
        {
            if (!await HasCountryAccessAsync(userId, countryId))
            {
                var access = new UserCountryAccess
                {
                    UserId = userId,
                    CountryId = countryId,
                    AccessLevel = accessLevel,
                    GrantedBy = grantedBy,
                    GrantedDate = DateTime.UtcNow
                };
                await AddAsync(access);
            }
        }

        public async Task RevokeCountryAccessAsync(int userId, int countryId)
        {
            var access = await _dbSet.FirstOrDefaultAsync(uca => uca.UserId == userId && uca.CountryId == countryId);
            if (access != null)
            {
                await DeleteAsync(access);
            }
        }
    }

    public class UserNotificationAccessRepository : Repository<UserNotificationAccess>, IUserNotificationAccessRepository
    {
        public UserNotificationAccessRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserNotificationAccess>> GetUserNotificationAccessAsync(int userId)
        {
            return await _dbSet.Where(una => una.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<UserNotificationAccess>> GetNotificationUsersAsync(int notificationCategoryId)
        {
            return await _dbSet.Where(una => una.NotificationCategoryId == notificationCategoryId).ToListAsync();
        }

        public async Task<bool> CanReceiveNotificationAsync(int userId, int notificationCategoryId)
        {
            var access = await _dbSet.FirstOrDefaultAsync(una => una.UserId == userId && una.NotificationCategoryId == notificationCategoryId);
            return access?.CanReceive ?? false;
        }

        public async Task<bool> CanManageNotificationAsync(int userId, int notificationCategoryId)
        {
            var access = await _dbSet.FirstOrDefaultAsync(una => una.UserId == userId && una.NotificationCategoryId == notificationCategoryId);
            return access?.CanManage ?? false;
        }

        public async Task UpdateNotificationAccessAsync(int userId, int notificationCategoryId, bool canReceive, bool canManage)
        {
            var access = await _dbSet.FirstOrDefaultAsync(una => una.UserId == userId && una.NotificationCategoryId == notificationCategoryId);
            if (access != null)
            {
                access.CanReceive = canReceive;
                access.CanManage = canManage;
                await UpdateAsync(access);
            }
        }
    }

    public class UserPreferenceRepository : Repository<UserPreference>, IUserPreferenceRepository
    {
        public UserPreferenceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<UserPreference?> GetUserPreferenceAsync(int userId, string preferenceKey)
        {
            return await _dbSet.FirstOrDefaultAsync(up => up.UserId == userId && up.PreferenceKey == preferenceKey);
        }

        public async Task<IEnumerable<UserPreference>> GetUserPreferencesAsync(int userId)
        {
            return await _dbSet.Where(up => up.UserId == userId).ToListAsync();
        }

        public async Task SetUserPreferenceAsync(int userId, string preferenceKey, string preferenceValue)
        {
            var preference = await GetUserPreferenceAsync(userId, preferenceKey);
            if (preference != null)
            {
                preference.PreferenceValue = preferenceValue;
                await UpdateAsync(preference);
            }
            else
            {
                var newPreference = new UserPreference
                {
                    UserId = userId,
                    PreferenceKey = preferenceKey,
                    PreferenceValue = preferenceValue
                };
                await AddAsync(newPreference);
            }
        }

        public async Task RemoveUserPreferenceAsync(int userId, string preferenceKey)
        {
            var preference = await GetUserPreferenceAsync(userId, preferenceKey);
            if (preference != null)
            {
                await DeleteAsync(preference);
            }
        }

        public async Task<Dictionary<string, string>> GetUserPreferencesDictionaryAsync(int userId)
        {
            var preferences = await GetUserPreferencesAsync(userId);
            return preferences.ToDictionary(p => p.PreferenceKey, p => p.PreferenceValue ?? "");
        }
    }

    public class UserTrainingRepository : Repository<UserTraining>, IUserTrainingRepository
    {
        public UserTrainingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserTraining>> GetUserTrainingsAsync(int userId)
        {
            return await _dbSet.Where(ut => ut.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<UserTraining>> GetTrainingUsersAsync(int trainingId)
        {
            return await _dbSet.Where(ut => ut.TrainingId == trainingId).ToListAsync();
        }

        public async Task<UserTraining?> GetUserTrainingAsync(int userId, int trainingId)
        {
            return await _dbSet.FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TrainingId == trainingId);
        }

        public async Task EnrollUserInTrainingAsync(int userId, int trainingId, int enrolledBy)
        {
            var existing = await GetUserTrainingAsync(userId, trainingId);
            if (existing == null)
            {
                var enrollment = new UserTraining
                {
                    UserId = userId,
                    TrainingId = trainingId,
                    EnrolledBy = enrolledBy,
                    EnrolledDate = DateTime.UtcNow,
                    Status = "Enrolled"
                };
                await AddAsync(enrollment);
            }
        }

        public async Task CompleteTrainingAsync(int userId, int trainingId, decimal? score, string? comments)
        {
            var training = await GetUserTrainingAsync(userId, trainingId);
            if (training != null)
            {
                training.Status = "Completed";
                training.CompletionDate = DateTime.UtcNow;
                training.Score = score;
                training.Comments = comments;
                await UpdateAsync(training);
            }
        }

        public async Task CompleteUserTrainingAsync(int userId, int trainingId, DateTime? completionDate = null)
        {
            var training = await GetUserTrainingAsync(userId, trainingId);
            if (training != null)
            {
                training.Status = "Completed";
                training.CompletionDate = completionDate ?? DateTime.UtcNow;
                await UpdateAsync(training);
            }
        }

        public async Task<IEnumerable<UserTraining>> GetCompletedTrainingsAsync(int userId)
        {
            return await _dbSet.Where(ut => ut.UserId == userId && ut.Status == "Completed").ToListAsync();
        }

        public async Task<IEnumerable<UserTraining>> GetPendingTrainingsAsync(int userId)
        {
            return await _dbSet.Where(ut => ut.UserId == userId && ut.Status != "Completed").ToListAsync();
        }
    }
}