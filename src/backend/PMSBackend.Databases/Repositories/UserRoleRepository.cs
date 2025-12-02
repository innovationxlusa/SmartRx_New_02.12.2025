using Microsoft.EntityFrameworkCore;
using PMSBackend.Application.CommonServices.Exceptions;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Databases.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly PMSDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IBaseRepository<SmartRxUserRoleEntity> _userRoleRepository;


        public UserRoleRepository(PMSDbContext context, IUserRepository userRepository, IRoleRepository roleRepository, IBaseRepository<SmartRxUserRoleEntity> userRoleRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }
        public async Task<bool> AssignUserToRoleByUserId(long userId, IList<long> roles)
        {
            try
            {
                List<SmartRxUserRoleEntity> userRoles = new List<SmartRxUserRoleEntity>();
                ParallelOptions parallelOptions = new()
                {
                    MaxDegreeOfParallelism = Int32.MaxValue
                };
                await Parallel.ForEachAsync(roles, parallelOptions, async (role, cancellationToken) =>
                {
                    userRoles.Add(new SmartRxUserRoleEntity { UserId = userId, RoleId = role, CreatedById = userId, CreatedDate = DateTime.Now });
                });
                await _context.Security_UserRoles.AddRangeAsync(userRoles);
                await _context.SaveChangesAsync();
                await Task.CompletedTask;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AssignUserToRoleByUserName(string userName, IList<long> roles)
        {
            try
            {
                var user = await _context.Security_PMSUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserName == userName && !x.IsDeleted);
                if (user is not null)
                {
                    List<SmartRxUserRoleEntity> userRoles = new List<SmartRxUserRoleEntity>();
                    ParallelOptions parallelOptions = new()
                    {
                        MaxDegreeOfParallelism = Environment.ProcessorCount
                    };
                    await Parallel.ForEachAsync(roles, parallelOptions, async (role, cancellationToken) =>
                    {
                        await Task.Delay(10, cancellationToken);
                        lock (roles)
                        {
                            userRoles.Add(new SmartRxUserRoleEntity { UserId = user.Id, RoleId = role, CreatedById = user.Id, CreatedDate = DateTime.Now });
                        }
                    });
                    await _context.Security_UserRoles.AddRangeAsync(userRoles);
                    await _context.SaveChangesAsync();
                    await Task.CompletedTask;
                    return true;
                }
                throw new NotFoundException("User not found", 404);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SmartRxUserRoleEntity>?> GetUserRolesAsync(long userId)
        {
            try
            {
                //var userRole=new UserRole();
                var roles = new List<SmartRxUserRoleEntity>();
                roles = await _context.Security_UserRoles
                    .AsNoTracking()
                    .Include(ur => ur.User)
                    .Where(data => data.UserId == userId && data.User != null && !data.User.IsDeleted)
                    .ToListAsync();
                await Task.CompletedTask;
                return roles;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<SmartRxUserRoleEntity>> GetUserRolesAsync(string userName)
        {
            try
            {
                var roles = new List<SmartRxUserRoleEntity>();
                var isUser = await _userRepository.IsUniqueUserName(userName);
                if (isUser)
                {
                    var user = await _userRepository.GetUserDetailsByUserNameAsync(userName);
                    roles = await _context.Security_UserRoles
                        .AsNoTracking()
                        .Include(ur => ur.User)
                        .Where(data => data.UserId == user.Id && data.User != null && !data.User.IsDeleted)
                        .ToListAsync();
                }
                await Task.CompletedTask;
                return roles;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> IsInRoleAsync(long userId, long roleId)
        {
            try
            {
                var roles = await _context.Security_UserRoles
                    .AsNoTracking()
                    .Include(ur => ur.User)
                    .Where(data => data.UserId == userId && data.RoleId == roleId && data.User != null && !data.User.IsDeleted)
                    .ToListAsync();
                await Task.CompletedTask;
                return roles.Count > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateUsersRole(long userId, IList<long> userRoles)
        {
            try
            {
                var existingRoles = await GetUserRolesAsync(userId);
                if (existingRoles != null && existingRoles.Count > 0)
                {
                    _context.Security_UserRoles.RemoveRange(existingRoles);
                }
                List<SmartRxUserRoleEntity> userWiseRoles = new List<SmartRxUserRoleEntity>();
                ParallelOptions parallelOptions = new()
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                };
                await Parallel.ForEachAsync(userRoles, parallelOptions, async (roleId, cancellationToken) =>
                {
                    await Task.Delay(10, cancellationToken);
                    lock (userRoles)
                    {
                        userWiseRoles.Add(new SmartRxUserRoleEntity
                        {
                            UserId = userId,
                            RoleId = roleId,
                            CreatedDate = DateTime.Now,
                            CreatedById = 6
                        });
                    }
                });
                await _context.Security_UserRoles.AddRangeAsync(userWiseRoles);
                await _context.SaveChangesAsync();
                await Task.CompletedTask;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> UpdateExternalUsersRole(long userId)
        {
            try
            {
                var existingRoles = await GetUserRolesAsync(userId);
                if (existingRoles != null && existingRoles.Count > 0)
                {
                    _context.Security_UserRoles.RemoveRange(existingRoles);
                }
                SmartRxUserRoleEntity userWiseRole = new SmartRxUserRoleEntity
                {
                    UserId = userId,
                    RoleId = (int)Roles.externaluser,
                    CreatedDate = DateTime.Now,
                    CreatedById = 6
                };

                await _context.Security_UserRoles.AddAsync(userWiseRole);
                await _context.SaveChangesAsync();
                await Task.CompletedTask;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteUserRoleAsync(long userRoleId)
        {
            try
            {
                await _userRoleRepository.DeleteAsync(userRoleId);
                await _context.SaveChangesAsync();
                await Task.CompletedTask;
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
