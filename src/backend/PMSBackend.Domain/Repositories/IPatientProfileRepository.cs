using PMSBackend.Domain.SharedContract;
using PMSBackend.Domain.CommonDTO;

namespace PMSBackend.Domain.Repositories
{
    public interface IPatientProfileRepository
    {
        Task<PatientWithRelativesContract?> GetPatientProfileWithRelativesById(long id, CancellationToken cancellationToken);

        Task<IList<PatientDropdown>> GetPatientDropdownInfoAsync(long userId, CancellationToken cancellationToken);

        Task<bool> IsExistsPatientProfileDetails(long patientId);

        Task<PatientWithRelativesContract> EditPatientDetailsAsync(long patientId, long loginUserId, PatientWithRelativesContract patientDetails, CancellationToken cancellationToken);

        Task<IList<PatientWithRelativesContract>> GetAllPatientProfilesByUserIdAsync(long userId, CancellationToken cancellationToken);

        Task<PaginatedResult<PatientWithRelativesContract>> GetAllPatientProfilesByUserIdWithPagingAsync(long userId, string? RxType, string? searchKeyword, string? searchColumn, PagingSortingParams? pagingSorting, CancellationToken cancellationToken);

        Task<PatientWithRelativesContract> CreatePatientDetailsAsync(long userId, PatientWithRelativesContract patientDetails, CancellationToken cancellationToken);
    }
}
