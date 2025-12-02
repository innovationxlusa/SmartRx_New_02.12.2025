using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Databases.Repositories
{
    public class SmartRxVitalRepository : ISmartRxVitalRepository
    {
        private readonly PMSDbContext _dbContext;
        private readonly IBaseRepository<SmartRx_PatientVitalsEntity> _smartRxVitalRepository;

        public SmartRxVitalRepository(PMSDbContext context)
        {
            this._dbContext = context;
            _smartRxVitalRepository = new BaseRepository<SmartRx_PatientVitalsEntity>(_dbContext);
        }

        public async Task<SmartRx_PatientVitalsEntity> AddAsync(SmartRx_PatientVitalsEntity entity)
        {
            try
            {
                await _smartRxVitalRepository.AddAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SmartRx_PatientVitalsEntity> UpdateAsync(SmartRx_PatientVitalsEntity entity)
        {
            try
            {
                var result = await _smartRxVitalRepository.UpdateAsync(entity);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                await _smartRxVitalRepository.DeleteAsync(id);
                await Task.CompletedTask;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<SmartRx_PatientVitalsEntity>> GetAllAsync()
        {
            try
            {
                return await _smartRxVitalRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SmartRx_PatientVitalsEntity?> GetDetailsByIdAsync(long id)
        {
            try
            {
                var result = await _dbContext.Smartrx_Vital
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);
                return result ?? new SmartRx_PatientVitalsEntity();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SmartRx_PatientVitalsEntity?> GetDetailsBySmartRxAsync(long smartRxId)
        {
            try
            {
                var result = await _dbContext.Smartrx_Vital
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.SmartRxMasterId == smartRxId);
                return result ?? new SmartRx_PatientVitalsEntity();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<SmartRx_PatientVitalsEntity?> GetDetailsBySmartRxAndPrescriptionAsync(long smartRxId, long prescriptionId)
        {
            try
            {
                var result = await _dbContext.Smartrx_Vital
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.SmartRxMasterId == smartRxId && c.PrescriptionId == prescriptionId);
                return result ?? new SmartRx_PatientVitalsEntity();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SmartRx_PatientVitalsEntity?> GetPatientVitalBySmartRxId(long smartRxMasterId, long prescriptionId, long vitalId)
        {
            try
            {
                var patientVitals = await _dbContext.Smartrx_Vital
                                                   .AsNoTracking()
                                                   .Where(m => m.SmartRxMasterId == smartRxMasterId && m.PrescriptionId == prescriptionId && m.VitalId == vitalId)
                                                   .Include(p => p.Vital)
                                                   .Include(pv => pv.Vital.Unit)
                                                   .FirstOrDefaultAsync();
                if (patientVitals == null)
                    return null;

                await Task.CompletedTask;
                return patientVitals!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> IsExistsVital(long smartRxId, long prescriptionId, string vitalName)
        {
            try
            {
                bool hasValue = false;
                bool hasFound = Common.DescriptionContainsText<Vitals>(vitalName);
                if (hasFound)
                {
                    var vitals = await _dbContext.Smartrx_Vital
                        .AsNoTracking()
                        .Where(data => data.SmartRxMasterId == smartRxId && data.PrescriptionId == prescriptionId)
                        .Include(data => data.Vital)
                        .ToListAsync();
                    if (vitals.Any() && vitals.Any(data => data.Vital.Name == vitalName))
                    {
                        hasValue = true;
                    }
                }
                await Task.CompletedTask;
                return hasValue;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SmartRx_PatientVitalsEntity?> IsExistsVital(long smartRxId, long prescriptionId, long vitalId)
        {
            try
            {
                var vital = await _dbContext.Smartrx_Vital
                    .AsNoTracking()
                    .Where(v => v.SmartRxMasterId == smartRxId
                                && v.PrescriptionId == prescriptionId
                                && v.VitalId == vitalId)
                    .Include(v => v.Vital)
                    .FirstOrDefaultAsync();

                return vital;
            }
            catch (Exception)
            {
                throw;
            }
        }

        Task IBaseRepository<SmartRx_PatientVitalsEntity>.DeleteAsync(long id)
        {
            return DeleteAsync(id);
        }
    }
}
