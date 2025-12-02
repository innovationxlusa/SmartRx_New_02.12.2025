using PMSBackend.Domain.SharedContract;
using PMSBackend.Domain.CommonDTO;

namespace PMSBackend.Domain.Repositories
{
    public interface IDoctorProfileRepository
    {
        Task<DoctorProfileContract?> GetDoctorProfileByIdAsync(long id);
        Task<IList<DoctorProfileWithCountContract>> GetDoctorProfilesByUserIdWithPrescriptionCountAsync(long userId, long? PatientId, CancellationToken cancellationToken);
        Task<PaginatedResult<DoctorProfileWithCountContract>> GetDoctorProfilesByUserIdWithPagingAsync(long userId, long? patientId, long? doctorId, string? searchKeyword, string? searchColumn, PagingSortingParams? pagingSorting, CancellationToken cancellationToken);
    }
}
