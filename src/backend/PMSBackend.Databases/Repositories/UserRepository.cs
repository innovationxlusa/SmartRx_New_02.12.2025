using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Databases.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PMSDbContext _context;
        private readonly IBaseRepository<SmartRxUserEntity> _userRepository;
        private readonly PasswordHasher<SmartRxUserEntity> _passwordHasher = new();
        public UserRepository(PMSDbContext context, IBaseRepository<SmartRxUserEntity> userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }
        public async Task<string> GetConnectionString()
        {
            return _context.Database.GetDbConnection().ConnectionString;
        }
        public async Task<SmartRxUserEntity> AddAsync(SmartRxUserEntity entity)
        {
            try
            {
                string PasswordHash = _passwordHasher.HashPassword(entity, entity.Password);
                entity.Password = PasswordHash;
                var result = await _userRepository.AddAsync(entity);
                await Task.CompletedTask;
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SmartRxUserEntity> UpdateAsync(SmartRxUserEntity entity)
        {
            try
            {
                var result = await _userRepository.UpdateAsync(entity);
                await Task.CompletedTask;
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteAsync(long userId)
        {
            try
            {
                await _userRepository.DeleteAsync(userId);
                await Task.CompletedTask;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IEnumerable<SmartRxUserEntity>> GetAllAsync()
        {
            try
            {
                var result = await _context.Security_PMSUsers
                    .AsNoTracking()
                    .Where(u => !u.IsDeleted)
                    .ToListAsync();
                await Task.CompletedTask;
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SmartRxUserEntity> GetDetailsByIdAsync(long userId)
        {
            try
            {
                var result = await _context.Security_PMSUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
                await Task.CompletedTask;
                return result!;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<bool> IsUniqueUserName(string userName)
        {
            try
            {
                var result = await _context.Security_PMSUsers
                    .AsNoTracking()
                    .Where(u => !u.IsDeleted)
                    .ToListAsync();
                if (result is not null)
                {
                    return result.DistinctBy(data => data.UserName == userName).Count() > 1 ? false : true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<SmartRxUserEntity> SigninUserAsync(string userName, string? password)
        {
            try
            {
                var result = await GetUserDetailsByUserNameAsync(userName);
                await Task.CompletedTask;
                return result!;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<SmartRxUserEntity> GetUserDetailsByUserCodeAsync(string userCode)
        {
            try
            {
                var result = await _context.Security_PMSUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(data => data.UserCode == userCode && !data.IsDeleted);
                await Task.CompletedTask;
                return result!;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<SmartRxUserEntity> GetUserDetailsByUserNameAsync(string userName)
        {
            try
            {
                //var keyword = GetStringWithoutSpaceWithLowerCase(userName);
                var result = await _context.Security_PMSUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(data => data.UserName.Trim().ToLower() == userName.Trim().ToLower() && !data.IsDeleted);
                await Task.CompletedTask;
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<string> GetNextProductCodeAsync()
        {
            try
            {
                var lastCode = await _context.Security_PMSUsers
                .AsNoTracking()
                .OrderByDescending(p => p.UserCode)
                .Select(p => p.UserCode)
                .FirstOrDefaultAsync();

                // If no code found, start at 0
                int nextCode = 0;

                if (!string.IsNullOrEmpty(lastCode) && int.TryParse(lastCode, out int lastCodeNumber))
                {
                    nextCode = lastCodeNumber + 1;
                }

                return nextCode.ToString().PadLeft(10, '0');
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<bool> VerifyPassword(SmartRxUserEntity user, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password!, password);

            if (result == PasswordVerificationResult.Success)
            {
                return true;
            }
            await Task.CompletedTask;
            return false;
        }
    }
}