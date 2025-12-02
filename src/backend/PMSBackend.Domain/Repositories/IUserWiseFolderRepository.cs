using PMSBackend.Domain.Entities;

namespace PMSBackend.Domain.Repositories
{

    public interface IUserWiseFolderRepository : IBaseRepository<Prescription_UserWiseFolderEntity>
    {
        Task<IEnumerable<Prescription_UserWiseFolderEntity>> GetAllAsync(long userId);
        Task<bool> IsExistsThisFolder(long folderId);
        Task<Prescription_UserWiseFolderEntity> IsExistAnyParentFolderForThisUserAsync(long userId);
        Task<bool> IsUploadedAnyFileForThisUser(long userId);
        Task<bool> IsUploadedAnyFileForThisUser(long userId, long folderId);
        Task<Prescription_UserWiseFolderEntity?> GetDetailsByUserIdAsync(long userId, long folderId);
        Task<Prescription_UserWiseFolderEntity?> GetDetailsByUserIdAsync(long userId);
        Task<Prescription_UserWiseFolderEntity?> GetPrimaryDetailsByIdAsync(long userId);
        Task<bool> IsExistsFolderName(string folderName, long userId, long parentFolderId);
    }
}
