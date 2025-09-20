using CustomerPortalAPI.Modules.Users.Entities;
using CustomerPortalAPI.Modules.Users.Repositories;
using CustomerPortalAPI.Modules.Users.GraphQL;
using CustomerPortalAPI.Modules.Shared;
using HotChocolate;

namespace CustomerPortalAPI.Modules.Users.GraphQL
{
    public class UserMutations
    {
        // User Mutations
        public async Task<CreateUserPayload> CreateUser(
            CreateUserInput input,
            [Service] IUserRepository repository)
        {
            try
            {
                var user = new User
                {
                    Username = input.Username,
                    Email = input.Email,
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    Phone = input.Phone,
                    JobTitle = input.JobTitle,
                    Department = input.Department,
                    CompanyId = input.CompanyId,
                    IsActive = true,
                    IsEmailVerified = false,
                    CreatedDate = DateTime.UtcNow
                };

                var created = await repository.AddAsync(user);
                
                return new CreateUserPayload(
                    new UserOutput(
                        created.Id,
                        created.Username,
                        created.Email,
                        created.FirstName,
                        created.LastName,
                        created.Phone,
                        created.JobTitle,
                        created.Department,
                        created.CompanyId,
                        created.IsActive,
                        created.IsEmailVerified,
                        created.LastLoginDate,
                        created.CreatedDate
                    ),
                    null
                );
            }
            catch (Exception ex)
            {
                return new CreateUserPayload(null, ex.Message);
            }
        }

        public async Task<UpdateUserPayload> UpdateUser(
            UpdateUserInput input,
            [Service] IUserRepository repository)
        {
            try
            {
                var user = await repository.GetByIdAsync(input.Id);
                if (user == null)
                    return new UpdateUserPayload(null, "User not found");

                if (input.Username != null) user.Username = input.Username;
                if (input.Email != null) user.Email = input.Email;
                if (input.FirstName != null) user.FirstName = input.FirstName;
                if (input.LastName != null) user.LastName = input.LastName;
                if (input.Phone != null) user.Phone = input.Phone;
                if (input.JobTitle != null) user.JobTitle = input.JobTitle;
                if (input.Department != null) user.Department = input.Department;
                if (input.CompanyId.HasValue) user.CompanyId = input.CompanyId;
                if (input.IsActive.HasValue) user.IsActive = input.IsActive.Value;
                if (input.IsEmailVerified.HasValue) user.IsEmailVerified = input.IsEmailVerified.Value;
                user.ModifiedDate = DateTime.UtcNow;

                await repository.UpdateAsync(user);
                
                return new UpdateUserPayload(
                    new UserOutput(
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
                    ),
                    null
                );
            }
            catch (Exception ex)
            {
                return new UpdateUserPayload(null, ex.Message);
            }
        }

        public async Task<BaseDeletePayload> DeleteUser(
            int id,
            [Service] IUserRepository repository)
        {
            try
            {
                await repository.DeleteAsync(id);
                return new BaseDeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new BaseDeletePayload(false, ex.Message);
            }
        }

        // User Role Mutations
        public async Task<BaseDeletePayload> CreateUserRole(
            CreateUserRoleInput input,
            [Service] IUserRoleRepository repository)
        {
            try
            {
                var userRole = new UserRole
                {
                    UserId = input.UserId,
                    RoleId = input.RoleId,
                    AssignedDate = DateTime.UtcNow,
                    AssignedBy = input.AssignedBy,
                    IsActive = true
                };

                await repository.AddAsync(userRole);
                return new BaseDeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new BaseDeletePayload(false, ex.Message);
            }
        }

        public async Task<BaseDeletePayload> UpdateUserRole(
            UpdateUserRoleInput input,
            [Service] IUserRoleRepository repository)
        {
            try
            {
                var userRole = await repository.GetByIdAsync(input.Id);
                if (userRole == null)
                    return new BaseDeletePayload(false, "User role not found");

                if (input.IsActive.HasValue) userRole.IsActive = input.IsActive.Value;

                await repository.UpdateAsync(userRole);
                return new BaseDeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new BaseDeletePayload(false, ex.Message);
            }
        }

        public async Task<BaseDeletePayload> DeleteUserRole(
            int id,
            [Service] IUserRoleRepository repository)
        {
            try
            {
                await repository.DeleteAsync(id);
                return new BaseDeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new BaseDeletePayload(false, ex.Message);
            }
        }

        // User Company Access Mutations
        public async Task<BaseDeletePayload> CreateUserCompanyAccess(
            CreateUserCompanyAccessInput input,
            [Service] IUserCompanyAccessRepository repository)
        {
            try
            {
                var access = new UserCompanyAccess
                {
                    UserId = input.UserId,
                    CompanyId = input.CompanyId,
                    AccessLevel = input.AccessLevel,
                    GrantedDate = DateTime.UtcNow,
                    GrantedBy = input.GrantedBy,
                    IsActive = true
                };

                await repository.AddAsync(access);
                return new BaseDeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new BaseDeletePayload(false, ex.Message);
            }
        }

        public async Task<BaseDeletePayload> UpdateUserCompanyAccess(
            UpdateUserCompanyAccessInput input,
            [Service] IUserCompanyAccessRepository repository)
        {
            try
            {
                var access = await repository.GetByIdAsync(input.Id);
                if (access == null)
                    return new BaseDeletePayload(false, "User company access not found");

                if (input.AccessLevel != null) access.AccessLevel = input.AccessLevel;
                if (input.IsActive.HasValue) access.IsActive = input.IsActive.Value;

                await repository.UpdateAsync(access);
                return new BaseDeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new BaseDeletePayload(false, ex.Message);
            }
        }

        public async Task<BaseDeletePayload> DeleteUserCompanyAccess(
            int id,
            [Service] IUserCompanyAccessRepository repository)
        {
            try
            {
                await repository.DeleteAsync(id);
                return new BaseDeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new BaseDeletePayload(false, ex.Message);
            }
        }

        // User Preference Mutations
        public async Task<BaseDeletePayload> CreateUserPreference(
            CreateUserPreferenceInput input,
            [Service] IUserPreferenceRepository repository)
        {
            try
            {
                var preference = new UserPreference
                {
                    UserId = input.UserId,
                    PreferenceKey = input.PreferenceKey,
                    PreferenceValue = input.PreferenceValue,
                    CreatedDate = DateTime.UtcNow
                };

                await repository.AddAsync(preference);
                return new BaseDeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new BaseDeletePayload(false, ex.Message);
            }
        }

        public async Task<BaseDeletePayload> UpdateUserPreference(
            UpdateUserPreferenceInput input,
            [Service] IUserPreferenceRepository repository)
        {
            try
            {
                var preference = await repository.GetByIdAsync(input.Id);
                if (preference == null)
                    return new BaseDeletePayload(false, "User preference not found");

                if (input.PreferenceValue != null) preference.PreferenceValue = input.PreferenceValue;
                preference.ModifiedDate = DateTime.UtcNow;

                await repository.UpdateAsync(preference);
                return new BaseDeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new BaseDeletePayload(false, ex.Message);
            }
        }

        public async Task<BaseDeletePayload> DeleteUserPreference(
            int id,
            [Service] IUserPreferenceRepository repository)
        {
            try
            {
                await repository.DeleteAsync(id);
                return new BaseDeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new BaseDeletePayload(false, ex.Message);
            }
        }
    }
}

