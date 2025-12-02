using PMSBackend.Domain.Entities;

namespace PMSBackend.Domain.Repositories
{
    public interface IUserRepository : IBaseRepository<SmartRxUserEntity>
    {
        Task<string> GetConnectionString();
        Task<bool> IsUniqueUserName(string userName);
        Task<SmartRxUserEntity> SigninUserAsync(string userName, string password);
        Task<SmartRxUserEntity> GetUserDetailsByUserCodeAsync(string userCode);
        Task<SmartRxUserEntity> GetUserDetailsByUserNameAsync(string userName);
        Task<string> GetNextProductCodeAsync();
        Task<bool> VerifyPassword(SmartRxUserEntity user, string password);

    }
}
