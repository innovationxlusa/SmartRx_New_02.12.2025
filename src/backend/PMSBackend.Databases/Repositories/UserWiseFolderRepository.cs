using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Databases.Repositories
{
    public class UserWiseFolderRepository : IUserWiseFolderRepository
    {
        private readonly PMSDbContext _context;
        private readonly IBaseRepository<Prescription_UserWiseFolderEntity> _folderRepository;
        private readonly ILogger<UserWiseFolderRepository> _logger;
        public UserWiseFolderRepository(PMSDbContext context, IBaseRepository<Prescription_UserWiseFolderEntity> folderRepository, ILogger<UserWiseFolderRepository> logger)
        {
            _context = context;
            _folderRepository = folderRepository;
            _logger = logger;
        }
        public async Task<Prescription_UserWiseFolderEntity> AddAsync(Prescription_UserWiseFolderEntity entity)
        {
            try
            {
                _logger.LogInformation("POST request received at {Time}", DateTime.UtcNow);
                _logger.LogInformation("Data before insert", entity);

                var result = await _folderRepository.AddAsync(entity);
                _logger.LogInformation("POST request received at {Time}", DateTime.UtcNow);
                _logger.LogInformation("Data after insert", result);
                await Task.CompletedTask;
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Prescription_UserWiseFolderEntity> UpdateAsync(Prescription_UserWiseFolderEntity entity)
        {
            try
            {
                var result = await _folderRepository.UpdateAsync(entity);
                await Task.CompletedTask;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task DeleteAsync(long id)
        {
            try
            {
                await _folderRepository.DeleteAsync(id);
                await Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Prescription_UserWiseFolderEntity>> GetAllAsync()
        {
            try
            {
                var result = await _folderRepository.GetAllAsync();
                await Task.CompletedTask;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Prescription_UserWiseFolderEntity>> GetAllAsync(long userId)
        {
            try
            {
                var result = await _context.Prescription_UserWiseFolders
                    .AsNoTracking()
                    .Where(data => data.UserId == userId)
                    .ToListAsync();
                await Task.CompletedTask;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Prescription_UserWiseFolderEntity?> GetDetailsByIdAsync(long id)
        {
            try
            {
                return await _context.Prescription_UserWiseFolders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(folder => folder.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Prescription_UserWiseFolderEntity?> GetPrimaryDetailsByIdAsync(long userId)
        {
            try
            {
                return await _context.Prescription_UserWiseFolders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(data => data.UserId == userId && data.FolderHierarchy == 0 && data.ParentFolderId == null);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Prescription_UserWiseFolderEntity?> GetDetailsByUserIdAsync(long userId, long folderId)
        {
            try
            {
                return await _context.Prescription_UserWiseFolders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(data => data.UserId == userId && data.Id == folderId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Prescription_UserWiseFolderEntity?> GetDetailsByUserIdAsync(long userId)
        {
            try
            {
                return await _context.Prescription_UserWiseFolders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(data => data.UserId == userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Prescription_UserWiseFolderEntity?> IsExistAnyParentFolderForThisUserAsync(long userId)
        {
            try
            {
                return await _context.Prescription_UserWiseFolders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(data => data.UserId == userId && data.FolderHierarchy == 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsExistsThisFolder(long folderId)
        {
            try
            {
                return await _context.Prescription_UserWiseFolders
                    .AsNoTracking()
                    .AnyAsync(data => data.Id == folderId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> IsUploadedAnyFileForThisUser(long userId)
        {
            try
            {
                var data = await _context.Prescription_UserWiseFolders
                        .AsNoTracking()
                        .Join(_context.Prescription_UploadedPrescription.AsNoTracking(),
                            u => new { u.UserId, FolderId = u.Id },
                            p => new { p.UserId, p.FolderId },
                            (u, p) => new { u, p })
                        .Where(joined => joined.u.UserId == userId)
                        .Select(joined => new
                        {
                            joined.p.Id
                        })
                        .ToListAsync();
                //var data = await (from u in _context.UserWiseFolders
                //                  join p in _context.Prescription_UploadedPrescription
                //                      on new { u.UserId, FolderId = u.Id }
                //                      equals new { UserId = p.Id, p.FolderId }
                //                  where u.UserId == userId
                //                  select new
                //                  {
                //                      p.Id
                //                  }).ToListAsync();
                return data.Count > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> IsUploadedAnyFileForThisUser(long userId, long folderId)
        {
            try
            {
                var data = await _context.Prescription_UserWiseFolders
                        .AsNoTracking()
                        .Join(_context.Prescription_UploadedPrescription.AsNoTracking(),
                            u => new { u.UserId, FolderId = u.Id },
                            p => new { p.UserId, p.FolderId },
                            (u, p) => new { u, p })
                        .Where(joined => joined.u.UserId == userId && joined.u.Id == folderId)
                        .Select(joined => new
                        {
                            joined.p.Id
                        })
                        .ToListAsync();
                return data.Count > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsExistsFolderName(string folderName, long userId, long parentFolderId)
        {
            try
            {
                return await _context.Prescription_UserWiseFolders
                    .AsNoTracking()
                    .AnyAsync(data => data.FolderName == folderName && data.UserId == userId && data.ParentFolderId==parentFolderId);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
