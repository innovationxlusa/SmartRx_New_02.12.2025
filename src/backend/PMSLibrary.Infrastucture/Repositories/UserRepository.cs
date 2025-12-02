using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Services;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Infrastucture.Data;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PMSBackend.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PMSDbContext _context;
        private readonly IBaseRepository<SmartRxUserEntity> _userRepository;
        private readonly ICodeGenerationService _codeGenerationService;
        public UserRepository(PMSDbContext context, IBaseRepository<SmartRxUserEntity> userRepository, ICodeGenerationService codeGenerationService)
        {
            _context = context;
            _userRepository = userRepository;
            _codeGenerationService = codeGenerationService;
        }

        public async Task<SmartRxUserEntity> AddAsync(SmartRxUserEntity entity)
        {
            try
            {
                var result = await _userRepository.AddAsync(entity);
                await Task.CompletedTask;
                return result;
            }
            catch (Exception)
            {

                throw;
            }           
        }

        public async Task UpdateAsync(SmartRxUserEntity entity)
        {
            try
            {
                await _userRepository.UpdateAsync(entity);
                await Task.CompletedTask;
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
                var result = await _userRepository.GetAllAsync();
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
                return await _context.PMSUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(user => user.Id == userId) ?? new SmartRxUserEntity();
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
                var result = await _userRepository.GetAllAsync();
                if (result is not null)
                {
                    return result.DistinctBy(data => data.UserName == userName).Count() > 1 ? true : false;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public async Task<SmartRxUserEntity> SigninUserAsync(string userName, string password)
        {
            try
            {
                return await _context.PMSUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(data => data.UserName == userName && data.Password == password) ?? new SmartRxUserEntity();
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
                return await _context.PMSUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(data => data.UserCode == userCode) ?? new SmartRxUserEntity();
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
                return await _context.PMSUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(data => data.UserName.Trim().ToLower() == userName.Trim().ToLower()) ?? new SmartRxUserEntity();
            }
            catch (Exception)
            {

                throw;
            }
          
        }
        public async Task<string> GetNextProductCodeAsync()
        {
            return await _codeGenerationService.GenerateUserCodeAsync();
        }

      
    }
}