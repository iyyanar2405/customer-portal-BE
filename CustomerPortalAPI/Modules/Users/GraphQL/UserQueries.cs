using CustomerPortalAPI.Modules.Users.Repositories;
using CustomerPortalAPI.Modules.Users.GraphQL;
using HotChocolate;

namespace CustomerPortalAPI.Modules.Users.GraphQL
{
    public class UserQueries
    {
        // User Queries
        public async Task<IEnumerable<UserOutput>> GetUsers(
            [Service] IUserRepository repository,
            UserFilterInput? filter = null)
        {
            var users = await repository.GetAllAsync();
            
            if (filter != null)
            {
                if (filter.Username != null)
                    users = users.Where(u => u.Username.Contains(filter.Username, StringComparison.OrdinalIgnoreCase));
                if (filter.Email != null)
                    users = users.Where(u => u.Email.Contains(filter.Email, StringComparison.OrdinalIgnoreCase));
                if (filter.Name != null)
                    users = users.Where(u => 
                        (u.FirstName != null && u.FirstName.Contains(filter.Name, StringComparison.OrdinalIgnoreCase)) ||
                        (u.LastName != null && u.LastName.Contains(filter.Name, StringComparison.OrdinalIgnoreCase)));
                if (filter.CompanyId.HasValue)
                    users = users.Where(u => u.CompanyId == filter.CompanyId.Value);
                if (filter.IsActive.HasValue)
                    users = users.Where(u => u.IsActive == filter.IsActive.Value);
                if (filter.IsEmailVerified.HasValue)
                    users = users.Where(u => u.IsEmailVerified == filter.IsEmailVerified.Value);
            }
            
            return users.Select(u => new UserOutput(
                u.Id,
                u.Username,
                u.Email,
                u.FirstName,
                u.LastName,
                u.Phone,
                u.JobTitle,
                u.Department,
                u.CompanyId,
                u.IsActive,
                u.IsEmailVerified,
                u.LastLoginDate,
                u.CreatedDate
            ));
        }

        public async Task<UserOutput?> GetUserById(
            int id,
            [Service] IUserRepository repository)
        {
            var user = await repository.GetByIdAsync(id);
            if (user == null) return null;

            return new UserOutput(
                user.Id,
                user.Username,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Phone,
                user.JobTitle,
                user.Department,
                user.CompanyId,
                user.IsActive,
                user.IsEmailVerified,
                user.LastLoginDate,
                user.CreatedDate
            );
        }

        public async Task<UserOutput?> GetUserByUsername(
            string username,
            [Service] IUserRepository repository)
        {
            var user = await repository.GetByUsernameAsync(username);
            if (user == null) return null;

            return new UserOutput(
                user.Id,
                user.Username,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Phone,
                user.JobTitle,
                user.Department,
                user.CompanyId,
                user.IsActive,
                user.IsEmailVerified,
                user.LastLoginDate,
                user.CreatedDate
            );
        }

        public async Task<UserOutput?> GetUserByEmail(
            string email,
            [Service] IUserRepository repository)
        {
            var user = await repository.GetByEmailAsync(email);
            if (user == null) return null;

            return new UserOutput(
                user.Id,
                user.Username,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Phone,
                user.JobTitle,
                user.Department,
                user.CompanyId,
                user.IsActive,
                user.IsEmailVerified,
                user.LastLoginDate,
                user.CreatedDate
            );
        }

        // User Role Queries
        public async Task<IEnumerable<UserRoleOutput>> GetUserRoles(
            int userId,
            [Service] IUserRoleRepository repository)
        {
            var userRoles = await repository.GetUserRolesAsync(userId);
            
            return userRoles.Select(ur => new UserRoleOutput(
                ur.Id,
                ur.UserId,
                ur.RoleId,
                ur.AssignedDate,
                ur.AssignedBy,
                ur.IsActive
            ));
        }

        // User Company Access Queries
        public async Task<IEnumerable<UserCompanyAccessOutput>> GetUserCompanyAccess(
            int userId,
            [Service] IUserCompanyAccessRepository repository)
        {
            var accesses = await repository.GetUserCompanyAccessAsync(userId);
            
            return accesses.Select(a => new UserCompanyAccessOutput(
                a.Id,
                a.UserId,
                a.CompanyId,
                a.AccessLevel,
                a.GrantedDate,
                a.GrantedBy,
                a.IsActive
            ));
        }

        // User Site Access Queries
        public async Task<IEnumerable<UserSiteAccessOutput>> GetUserSiteAccess(
            int userId,
            [Service] IUserSiteAccessRepository repository)
        {
            var accesses = await repository.GetUserSiteAccessAsync(userId);
            
            return accesses.Select(a => new UserSiteAccessOutput(
                a.Id,
                a.UserId,
                a.SiteId,
                a.AccessLevel,
                a.GrantedDate,
                a.GrantedBy,
                a.IsActive
            ));
        }

        // User Preference Queries
        public async Task<IEnumerable<UserPreferenceOutput>> GetUserPreferences(
            int userId,
            [Service] IUserPreferenceRepository repository)
        {
            var preferences = await repository.GetUserPreferencesAsync(userId);
            
            return preferences.Select(p => new UserPreferenceOutput(
                p.Id,
                p.UserId,
                p.PreferenceKey,
                p.PreferenceValue,
                p.CreatedDate,
                p.ModifiedDate
            ));
        }

        // User Training Queries
        public async Task<IEnumerable<UserTrainingOutput>> GetUserTrainings(
            int userId,
            [Service] IUserTrainingRepository repository)
        {
            var trainings = await repository.GetUserTrainingsAsync(userId);
            
            return trainings.Select(t => new UserTrainingOutput(
                t.Id,
                t.UserId,
                t.TrainingId,
                t.StartDate,
                t.CompletionDate,
                t.Status,
                t.Score,
                t.Comments,
                t.EnrolledDate,
                t.EnrolledBy
            ));
        }
    }
}