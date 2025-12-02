using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.SharedContract;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Domain.Repositories
{
    public interface IRewardBadgeRepository
    {
        /// <summary>
        /// Creates a new reward badge
        /// </summary>
        Task<Configuration_RewardBadgeEntity> CreateRewardBadgeAsync(Configuration_RewardBadgeEntity rewardBadge, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing reward badge
        /// </summary>
        Task<Configuration_RewardBadgeEntity> UpdateRewardBadgeAsync(Configuration_RewardBadgeEntity rewardBadge, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a reward badge by ID
        /// </summary>
        Task<bool> DeleteRewardBadgeAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a reward badge by ID
        /// </summary>
        Task<Configuration_RewardBadgeEntity?> GetRewardBadgeByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets all reward badges with pagination
        /// </summary>
        Task<PaginatedResult<Configuration_RewardBadgeEntity>> GetAllRewardBadgesAsync(
            PagingSortingParams pagingSorting,
            CancellationToken cancellationToken);
    }
}

