using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.SharedContract;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Domain.Repositories
{
    public interface IRewardTransactionRepository
    {
        Task<SmartRx_RewardTransactionEntity> CreateRewardTransactionAsync(
            SmartRx_RewardTransactionEntity transaction,
            CancellationToken cancellationToken);

        Task<SmartRx_RewardTransactionEntity> UpdateRewardTransactionAsync(
            SmartRx_RewardTransactionEntity transaction,
            CancellationToken cancellationToken);

        Task<bool> DeleteRewardTransactionAsync(long id, CancellationToken cancellationToken);

        Task<SmartRx_RewardTransactionEntity?> GetRewardTransactionByIdAsync(long id, CancellationToken cancellationToken);

        Task<PaginatedResult<SmartRx_RewardTransactionEntity>> GetAllRewardTransactionsAsync(
            PagingSortingParams pagingSorting,
            CancellationToken cancellationToken);

        /// <summary>
        /// Creates a reward transaction for a given prescription and activity name,
        /// and returns a summary that can be used to update API DTOs.
        /// </summary>
        Task<RewardTransactionResult?> CreateRewardTransactionForPrescriptionAsync(
            long userId,
            long? prescriptionId,
            long? smartRxMasterId,
            long? patientId,
            string activityName,
            string activityFor,
            CancellationToken cancellationToken);

        Task<RewardTransactionSummaryContract?> GetRewardTransactionSummaryByUserIdAsync(
            long userId,
            CancellationToken cancellationToken);

        Task<List<RewardTransactionDetailContract>> GetRewardTransactionDetailsByUserIdAsync(
            long userId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            bool? earned = null,
            bool? consumed = null,
            CancellationToken cancellationToken = default);

        RewardTransactionQueryResult GetRewardTransactionQueriesByUserId(
            long userId,
            long? patientId = null,
            DateTime? startDate = null,
            DateTime? endDate = null);
    }
}

