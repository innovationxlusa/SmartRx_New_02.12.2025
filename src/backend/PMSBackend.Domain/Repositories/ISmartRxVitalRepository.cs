using PMSBackend.Domain.Entities;

namespace PMSBackend.Domain.Repositories
{
    public interface ISmartRxVitalRepository : IBaseRepository<SmartRx_PatientVitalsEntity>
    {
        Task<SmartRx_PatientVitalsEntity?> GetDetailsBySmartRxAsync(long smartRxId);
        Task<SmartRx_PatientVitalsEntity?> GetDetailsBySmartRxAndPrescriptionAsync(long smartRxId, long prescriptionId);
        Task<SmartRx_PatientVitalsEntity?> GetPatientVitalBySmartRxId(long smartRxMasterId, long prescriptionId, long vitalId);
        Task<bool> IsExistsVital(long smartRxId, long prescriptionId, string vitalName);
        Task<SmartRx_PatientVitalsEntity> IsExistsVital(long smartRxId, long prescriptionId, long vitalId);
        Task<bool> DeleteAsync(long id);
    }
}
