using PMSBackend.Domain.Entities;

namespace PMSBackend.Domain.Repositories
{
    public interface IUserRoleRepository
    {
        // User's Role section
        Task<bool> IsInRoleAsync(long userId, long roleId);
        Task<List<SmartRxUserRoleEntity>?> GetUserRolesAsync(long userId);
        Task<bool> AssignUserToRoleByUserId(long userId, IList<long> roles);
        Task<bool> UpdateUsersRole(long userId, IList<long> userRoles);
        Task<bool> UpdateExternalUsersRole(long userId);
        Task<bool> AssignUserToRoleByUserName(string userName, IList<long> roles);
        Task<bool> DeleteUserRoleAsync(long userRoleId);

    }
}
