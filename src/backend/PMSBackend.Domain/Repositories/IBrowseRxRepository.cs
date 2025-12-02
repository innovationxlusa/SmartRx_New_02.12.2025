using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.SharedContract;


namespace PMSBackend.Domain.Repositories
{
    public interface IBrowseRxRepository
    {
        Task<List<PrescriptionAllListModel>> GetAllFilesAsync(long userId, long? patientId, long? folderId, long? parentFolderId, long? folderHierarchy);
        Task<PaginatedResult<PrescriptionAllListModel>> GetAllFilesWithPagingAsync(long userId, long? folderId, long? parentFolderId, long? folderHierarchy, PagingSortingParams pagingSorting);
        Task GetAllFolderListByPatient(long userId);      
        Task<IEnumerable<Prescription_UserWiseFolderEntity>> GetAllFoldersAsync(long userId, long? folderId, long? folderHierarchy, long? parentFolderId);
        Task GetAllFileListByPatient(long userId);
        Task<List<FolderNode>> GetAllFolderListByUserIdAsync(long userId, long? patientId);
        Task<PaginatedResult<PatientPrescriptionContract>> GetPatientPrescriptionsWithPagingAsync(PatientPrescriptionSearchContract searchParams);
        Task<PaginatedResult<PrescriptionContract>> GetPatientPrescriptionsByTypeAsync(long userId, long? patientId, string prescriptionType, PagingSortingParams? pagingSorting);
    }
}
