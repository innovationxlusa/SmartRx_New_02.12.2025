using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.SharedContract;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Domain.Repositories
{
    public interface IPatientRewardRepository
    {
        /// <summary>
        /// Creates a new patient reward entry
        /// </summary>
        Task<SmartRx_PatientReward> CreatePatientRewardAsync(SmartRx_PatientReward patientReward, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing patient reward entry
        /// </summary>
        Task<SmartRx_PatientReward> UpdatePatientRewardAsync(SmartRx_PatientReward patientReward, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a patient reward entry by ID
        /// </summary>
        Task<bool> DeletePatientRewardAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets patient rewards by UserId and PatientId with pagination
        /// </summary>
        Task<PaginatedResult<SmartRx_PatientReward>> GetPatientRewardsByUserIdAndPatientIdAsync(
            long userId,
            long? patientId,
            PagingSortingParams pagingSorting,
            CancellationToken cancellationToken,
            DateTime? startDate = null,
            DateTime? endDate = null);

        /// <summary>
        /// Gets a single patient reward by ID
        /// </summary>
        Task<SmartRx_PatientReward?> GetPatientRewardByIdAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets patient rewards summary (total points) by UserId and PatientId
        /// </summary>
        Task<PatientRewardSummaryContract?> GetPatientRewardsSummaryAsync(long userId, long? patientId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets patient rewards by UserId only with pagination
        /// </summary>
        Task<PaginatedResult<SmartRx_PatientReward>> GetPatientRewardsByUserIdAsync(
            long userId,
            PagingSortingParams pagingSorting,
            CancellationToken cancellationToken);
    }

   
}

