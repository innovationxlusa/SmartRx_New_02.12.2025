using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using System.Threading;

namespace PMSBackend.Databases.Repositories
{
    public class PrescriptionUploadRepository : IPrescriptionUploadRepository
    {
        private readonly PMSDbContext _dbContext;
        private readonly IBaseRepository<Prescription_UploadEntity> _prescriptionRepository;


        public PrescriptionUploadRepository(PMSDbContext context)
        {
            this._dbContext = context;
            _prescriptionRepository = new BaseRepository<Prescription_UploadEntity>(_dbContext);
        }
        /// <summary>
        /// Insert, update, delete, query of file upload/scan/capture
        /// </summary>
        /// <returns></returns>
        public async Task<Prescription_UploadEntity> AddAsync(Prescription_UploadEntity entity)
        {
            try
            {
                //entity.CreatedById = entity.UserId;
                //entity.CreatedDate = DateTime.Now;
                await _prescriptionRepository.AddAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Prescription_UploadEntity> UpdateAsync(Prescription_UploadEntity entity)
        {
            try
            {
                var result = await _prescriptionRepository.UpdateAsync(entity);
                return result;
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

        public async Task<IEnumerable<Prescription_UploadEntity>> GetAllAsync()
        {
            try
            {
                return await _dbContext.Prescription_UploadedPrescription
                    .AsNoTracking()
                    .Where(p => !p.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Prescription_UploadEntity>? GetDetailsByIdAsync(long id)
        {
            try
            {
                var result = await _dbContext.Prescription_UploadedPrescription
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
                return result ?? new Prescription_UploadEntity();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

           
        }
        public async Task<IQueryable<Prescription_UploadEntity>> GetByFilter()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Insert, update, delete, query of file upload sequence
        /// </summary>
        /// <returns></returns>
        public async Task<Prescription_UploadEntity> GetLastSavedPrescriptionCode()
        {
            try
            {
                var result = await _dbContext.Prescription_UploadedPrescription
                    .AsNoTracking()
                    .Where(p => !p.IsDeleted)
                    .OrderByDescending(data => data.PrescriptionCode)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> HasSmartRxReferenceAsync(long prescriptionId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Smartrx_Master
                    .AsNoTracking()
                    .AnyAsync(sm => sm.PrescriptionId == prescriptionId, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to check SmartRx reference: {ex.Message}", ex);
            }
        }

        public async Task<bool> HasReferencesInOtherTablesAsync(long prescriptionId, CancellationToken cancellationToken = default)
        {
            try
            {
              

                // Check PatientDoctor
                var hasPatientDoctor = await _dbContext.Smartrx_Doctor
                    .AsNoTracking()
                    .AnyAsync(pd => pd.PrescriptionId == prescriptionId, cancellationToken);

                if (hasPatientDoctor)
                    return true;

                //// Check PatientChiefComplaint
                //var hasChiefComplaint = await _dbContext.Smartrx_ChiefComplaint
                //    .AsNoTracking()
                //    .AnyAsync(pcc => pcc.PrescriptionId == prescriptionId, cancellationToken);

                //if (hasChiefComplaint)
                //    return true;

                // Check PatientVitals
                var hasVitals = await _dbContext.Smartrx_Vital
                    .AsNoTracking()
                    .AnyAsync(pv => pv.PrescriptionId == prescriptionId, cancellationToken);

                if (hasVitals)
                    return true;

                // Check PatientHistory
                var hasHistory = await _dbContext.Smartrx_PatientHistory
                    .AsNoTracking()
                    .AnyAsync(ph => ph.PrescriptionId == prescriptionId, cancellationToken);

                if (hasHistory)
                    return true;

                // Check PatientMedicine
                var hasMedicine = await _dbContext.SmartRx_PatientMedicine
                    .AsNoTracking()
                    .AnyAsync(pm => pm.PrescriptionId == prescriptionId, cancellationToken);

                if (hasMedicine)
                    return true;

                // Check PatientInvestigation
                var hasInvestigation = await _dbContext.SmartRx_PatientInvestigation
                    .AsNoTracking()
                    .AnyAsync(pi => pi.PrescriptionId == prescriptionId, cancellationToken);

                if (hasInvestigation)
                    return true;

                // Check PatientAdvice
                var hasAdvice = await _dbContext.SmartRx_PatientAdvice
                    .AsNoTracking()
                    .AnyAsync(pa => pa.PrescriptionId == prescriptionId, cancellationToken);

                if (hasAdvice)
                    return true;

                // Check PatientOtherExpense
                var hasOtherExpense = await _dbContext.SmartRx_PatientOtherExpenses
                    .AsNoTracking()
                    .AnyAsync(poe => poe.PrescriptionId == prescriptionId, cancellationToken);

                if (hasOtherExpense)
                    return true;

                // Check PatientWishlist
                var hasWishlist = await _dbContext.SmartRx_PatientWishList
                    .AsNoTracking()
                    .AnyAsync(pw => pw.PrescriptionId == prescriptionId, cancellationToken);

                if (hasWishlist)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to check references in other tables: {ex.Message}", ex);
            }
        }

    }
}
