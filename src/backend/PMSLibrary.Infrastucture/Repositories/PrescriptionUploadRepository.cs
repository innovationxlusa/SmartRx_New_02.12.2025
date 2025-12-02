using Microsoft.EntityFrameworkCore;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Infrastucture.Data;

namespace PMSBackend.Infrastructure.Repositories
{
    public class PrescriptionUploadRepository// : IPrescriptionUploadRepository
    {
        private readonly PMSDbContext dbContext;
        private readonly IBaseRepository<PrescriptionUploadEntity> _prescriptionRepository;
        private readonly IBaseRepository<PrescriptionSequenceEntity> _prescriptionSequenceRepository;

        public PrescriptionUploadRepository(PMSDbContext context)
        {
            this.dbContext = context;
            _prescriptionRepository = new BaseRepository<PrescriptionUploadEntity>(dbContext);
            _prescriptionSequenceRepository = new BaseRepository<PrescriptionSequenceEntity>(dbContext);

        }
        /// <summary>
        /// Insert, update, delete, query of file upload/scan/capture
        /// </summary>
        /// <returns></returns>
        public async Task<PrescriptionUploadEntity> AddAsync(PrescriptionUploadEntity entity)
        {
            try
            {
                await _prescriptionRepository.AddAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateAsync(PrescriptionUploadEntity entity)
        {
            try
            {
                await _prescriptionRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAsync(long id)
        {
            try
            {
                await _prescriptionRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<PrescriptionUploadEntity>> GetAllAsync()
        {
            try
            {
                return await _prescriptionRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PrescriptionUploadEntity> GetDetailsByIdAsync(long id)
        {
            try
            {
                return await dbContext.Prescription_UploadedPrescription
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id) ?? new PrescriptionUploadEntity();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IQueryable<PrescriptionUploadEntity>> GetByFilter()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Insert, update, delete, query of file upload sequence
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetLastSavedPrescriptionCode()
        {
            try
            {
                var result = await dbContext.Prescription_UploadedFileSequence
                    .AsNoTracking()
                    .LastOrDefaultAsync();
                if (result == null) return "";
                return result.SeqNo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateFileSequence(string uniqueFileId)
        {
            try
            {
                PrescriptionSequenceEntity entity = new PrescriptionSequenceEntity() { SeqNo = uniqueFileId };
                await _prescriptionSequenceRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
