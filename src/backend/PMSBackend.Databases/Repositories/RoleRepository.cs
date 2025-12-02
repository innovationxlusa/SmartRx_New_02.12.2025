using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Databases.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly PMSDbContext _context;
        private readonly IBaseRepository<SmartRxRoleEntity> _roleRepository;
        public RoleRepository(PMSDbContext context, IBaseRepository<SmartRxRoleEntity> roleRepository)
        {
            _context = context;
            _roleRepository = roleRepository;
        }
        public async Task<SmartRxRoleEntity> AddAsync(SmartRxRoleEntity entity)
        {
            try
            {
                var result = await _roleRepository.AddAsync(entity);
                await Task.CompletedTask;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<SmartRxRoleEntity> UpdateAsync(SmartRxRoleEntity entity)
        {
            try
            {
                var result = await _roleRepository.UpdateAsync(entity);
                await Task.CompletedTask;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(long roleId)
        {
            try
            {
                await _roleRepository.DeleteAsync(roleId);
                await Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<IEnumerable<SmartRxRoleEntity>> GetAllAsync()
        {
            try
            {
                var result = await _roleRepository.GetAllAsync();
                await Task.CompletedTask;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SmartRxRoleEntity?> GetDetailsByIdAsync(long roleId)
        {
            try
            {
                var result = await _context.Security_Roles.FindAsync(roleId);
                await Task.CompletedTask;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<SmartRxRoleEntity?> GetDetailsByRoleNameAsync(string roleName)
        {
            try
            {
                var result = await _context.Security_Roles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(data => data.Name == roleName);
                await Task.CompletedTask;
                return result ?? new SmartRxRoleEntity();
            }
            catch (Exception)
            {
                throw;
            }           
        }

        public async Task<bool> IsExistsRole(string roleName)
        {
            try
            {
                var result = await _context.Security_Roles.AnyAsync(data => data.Name == roleName);
                await Task.CompletedTask;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
