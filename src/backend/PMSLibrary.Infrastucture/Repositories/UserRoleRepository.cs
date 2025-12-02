using Microsoft.EntityFrameworkCore;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Infrastucture.Data;
using System.Data;
using PMSBackend.Application.CommonServices.Exceptions;

namespace PMSBackend.Infrastructure.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly PMSDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IBaseRepository<UserRoleEntity> _userRoleRepository;


        public UserRoleRepository(PMSDbContext context, IUserRepository userRepository, IRoleRepository roleRepository, IBaseRepository<UserRoleEntity> userRoleRepository)
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
                List<UserRoleEntity> userRoles = new List<UserRoleEntity>();
                ParallelOptions parallelOptions = new()
                {
                    MaxDegreeOfParallelism = Int32.MaxValue
                };
                await Parallel.ForEachAsync(roles, parallelOptions, async (role, cancellationToken) =>
                {
                    userRoles.Add(new UserRoleEntity { UserId = userId, RoleId = role });
                });
                await _context.UserRoles.AddRangeAsync(userRoles);
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
                var user = await _context.PMSUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserName == userName);
                if (user is not null)
                {
                    List<UserRoleEntity> userRoles = new List<UserRoleEntity>();
                    ParallelOptions parallelOptions = new()
                    {
                        MaxDegreeOfParallelism = Environment.ProcessorCount
                    };
                    await Parallel.ForEachAsync(roles, parallelOptions, async (role, cancellationToken) =>
                    {
                        await Task.Delay(10, cancellationToken);
                        lock (roles)
                        {
                            userRoles.Add(new UserRoleEntity { UserId = user.Id, RoleId = role,CreatedBy=user.Id, CreatedDate=DateTime.Now });
                        }
                    });
                    await _context.UserRoles.AddRangeAsync(userRoles);
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

        public async Task<List<UserRoleEntity>?> GetUserRolesAsync(long userId)
        {
            try
            {
                //var userRole=new UserRole();
                var roles = new List<UserRoleEntity>();
                roles = await _context.UserRoles
                    .AsNoTracking()
                    .Where(data => data.UserId == userId)
                    .ToListAsync();
                await Task.CompletedTask;
                return roles;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<UserRoleEntity>> GetUserRolesAsync(string userName)
        {
            try
            {
                var roles = new List<UserRoleEntity>();
                var isUser = await _userRepository.IsUniqueUserName(userName);
                if (isUser)
                {
                    var user = await _userRepository.GetUserDetailsByUserNameAsync(userName);
                    roles = await _context.UserRoles
                        .AsNoTracking()
                        .Where(data => data.UserId == user.Id)
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
                var roles = await _context.UserRoles
                    .AsNoTracking()
                    .Where(data => data.UserId == userId && data.RoleId == roleId)
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
                    _context.UserRoles.RemoveRange(existingRoles);
                }
                List<UserRoleEntity> userWiseRoles = new List<UserRoleEntity>();
                ParallelOptions parallelOptions = new()
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                };
                await Parallel.ForEachAsync(userRoles, parallelOptions, async (roleId, cancellationToken) =>
                {
                    await Task.Delay(10, cancellationToken);
                    lock (userRoles)
                    {
                        userWiseRoles.Add(new UserRoleEntity
                        {
                            UserId = userId,
                            RoleId = roleId,
                            CreatedDate = DateTime.Now,
                            CreatedBy = 6
                        });
                    }
                });
                await _context.UserRoles.AddRangeAsync(userWiseRoles);
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
                    _context.UserRoles.RemoveRange(existingRoles);
                }
                UserRoleEntity userWiseRole = new UserRoleEntity
                {
                    UserId = userId,
                    RoleId = (int)Roles.externaluser,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 6
                };

                await _context.UserRoles.AddAsync(userWiseRole);
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
