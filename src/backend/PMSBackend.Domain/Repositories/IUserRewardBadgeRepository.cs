using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.SharedContract;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Domain.Repositories
{
    public interface IUserRewardBadgeRepository
    {
        Task<SmartRx_UserRewardBadgeEntity> CreateUserRewardBadgeAsync(
            SmartRx_UserRewardBadgeEntity userRewardBadge,
            CancellationToken cancellationToken);

        Task<SmartRx_UserRewardBadgeEntity> UpdateUserRewardBadgeAsync(
            SmartRx_UserRewardBadgeEntity userRewardBadge,
            CancellationToken cancellationToken);

        Task<bool> DeleteUserRewardBadgeAsync(long id, CancellationToken cancellationToken);

        Task<SmartRx_UserRewardBadgeEntity?> GetUserRewardBadgeByIdAsync(long id, CancellationToken cancellationToken);

        Task<PaginatedResult<SmartRx_UserRewardBadgeEntity>> GetAllUserRewardBadgesAsync(
            PagingSortingParams pagingSorting,
            CancellationToken cancellationToken);

        Task<bool> IsBadgeAlreadyAssignedAsync(
            long userId,
            long badgeId,
            long? excludeId = null,
            CancellationToken cancellationToken = default);

        Task<SmartRx_UserRewardBadgeEntity?> GetLatestUserRewardBadgeAsync(
            long userId,
            CancellationToken cancellationToken = default);
    }
}

