using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Domain.Repositories
{
    public interface IMedicineCompareRepository
    {
        Task<Configuration_MedicineEntity> CheckThisMedicineExists(long medicineId);
        Task<PaginatedResult<MedicineInfoModel>> ListOfSameGenericOtherBrandOfAMedicine(long smartRxMasterId, long prescriptionId, long medicineId, PagingSortingParams pagingAndSorting, CancellationToken cancellationToken);
        Task CompareMedicine(long compareToMedicineId);
        Task<SmartRx_PatientMedicineEntity> GetMedicineWishlist(long smartRxMasterId, long PrescriptionId, long medicineId);
        Task<SmartRx_PatientInvestigationEntity> GetInvestigationWishlist(long smartRxMasterId, long PrescriptionId, long investigatonId);
    }
}
