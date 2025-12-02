using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Services;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Infrastucture.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly PMSDbContext _context;
        private readonly IBaseRepository<SmartRxRoleEntity> _roleRepository;
        private readonly ICodeGenerationService _codeGenerationService;
        public RoleRepository(PMSDbContext context, IBaseRepository<SmartRxRoleEntity> roleRepository, ICodeGenerationService codeGenerationService)
        {
            _context = context;
            _roleRepository = roleRepository;
            _codeGenerationService = codeGenerationService;
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
        public async Task UpdateAsync(SmartRxRoleEntity entity)
        {
            try
            {
                await _roleRepository.UpdateAsync(entity);
                await Task.CompletedTask;
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
                return await _context.Roles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == roleId);
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
                return await _context.Roles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(data=>data.Name==roleName) ?? new SmartRxRoleEntity();
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
                var result = await _context.Roles.AnyAsync(data=>data.Name==roleName);
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
