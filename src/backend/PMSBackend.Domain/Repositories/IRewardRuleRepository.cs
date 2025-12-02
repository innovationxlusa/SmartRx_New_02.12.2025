using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.SharedContract;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Domain.Repositories
{
    public interface IRewardRuleRepository
    {
        /// <summary>
        /// Creates a new reward rule
        /// </summary>
        Task<Configuration_RewardRuleEntity> CreateRewardRuleAsync(Configuration_RewardRuleEntity rewardRule, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing reward rule
        /// </summary>
        Task<Configuration_RewardRuleEntity> UpdateRewardRuleAsync(Configuration_RewardRuleEntity rewardRule, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a reward rule by ID
        /// </summary>
        Task<bool> DeleteRewardRuleAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a reward rule by ID
        /// </summary>
        Task<Configuration_RewardRuleEntity?> GetRewardRuleByIdAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a reward rule by Activity Name
        /// </summary>
        Task<Configuration_RewardRuleEntity?> GetRewardRuleByActivityNameAsync(string activityName, CancellationToken cancellationToken);

        /// <summary>
        /// Gets all reward rules with pagination
        /// </summary>
        Task<PaginatedResult<Configuration_RewardRuleEntity>> GetAllRewardRulesAsync(
            PagingSortingParams pagingSorting,
            CancellationToken cancellationToken);

        /// <summary>
        /// Checks if an activity code already exists
        /// </summary>
        Task<bool> IsActivityCodeExistsAsync(string activityCode, long? excludeId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if an activity name already exists
        /// </summary>
        Task<bool> IsActivityNameExistsAsync(string activityName, long? excludeId = null, CancellationToken cancellationToken = default);
    }
}

