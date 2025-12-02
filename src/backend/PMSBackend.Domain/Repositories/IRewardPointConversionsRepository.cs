using PMSBackend.Domain.Entities;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Domain.Repositories
{
    public interface IRewardPointConversionsRepository
    {
        Task<SmartRx_RewardPointConversionsEntity> CreateAsync(SmartRx_RewardPointConversionsEntity entity);
        Task<SmartRx_RewardPointConversionsEntity?> GetByIdAsync(long id);
        Task<IEnumerable<SmartRx_RewardPointConversionsEntity>> GetAllAsync();
        Task<IEnumerable<SmartRx_RewardPointConversionsEntity>> GetByUserIdAsync(long userId);
        Task<SmartRx_RewardPointConversionsEntity> UpdateAsync(SmartRx_RewardPointConversionsEntity entity);
        Task<bool> DeleteAsync(long id);
        Task<bool> ExistsAsync(long id);
        Task<RewardPointConversionsSummaryContract> GetConversionSummaryByUserIdAsync(long userId);
    }
}
