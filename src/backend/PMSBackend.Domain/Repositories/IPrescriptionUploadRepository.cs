using PMSBackend.Domain.Entities;

namespace PMSBackend.Domain.Repositories
{
    public interface IPrescriptionUploadRepository : IBaseRepository<Prescription_UploadEntity>
    {
        Task<IQueryable<Prescription_UploadEntity>> GetByFilter();
        Task<Prescription_UploadEntity> GetLastSavedPrescriptionCode();
        Task<bool> HasSmartRxReferenceAsync(long prescriptionId, CancellationToken cancellationToken = default);
        Task<bool> HasReferencesInOtherTablesAsync(long prescriptionId, CancellationToken cancellationToken = default);
        //Task<PrescriptionUploadEntity> GenerateFileSequenceAsync(string uniqueFileId);
    }
}
