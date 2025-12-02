using PMSBackend.Domain.Entities;

namespace PMSBackend.Domain.Repositories
{
    public interface IRoleRepository : IBaseRepository<SmartRxRoleEntity>
    {
        // Role Section
        //Task<bool> CreateRoleAsync(string roleName);
        //Task<bool> DeleteRoleAsync(string roleId);
        //Task<List<(string id, string roleName)>> GetRolesAsync();
        //Task<(string id, string roleName)> GetRoleByIdAsync(string id);
        //Task<bool> UpdateRole(string id, string roleName);
        Task<bool> IsExistsRole(string roleName);
        Task<SmartRxRoleEntity?> GetDetailsByRoleNameAsync(string roleName);
    }
}
