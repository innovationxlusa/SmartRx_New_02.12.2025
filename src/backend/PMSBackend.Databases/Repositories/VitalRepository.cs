using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Databases.Services;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Databases.Repositories
{
    public class VitalRepository : IVitalRepository
    {
        private readonly PMSDbContext _dbContext;
        private readonly IBaseRepository<Configuration_VitalEntity> _smartRxVitalRepository;
        private readonly ICodeGenerationService _codeGenerationService;

        public VitalRepository(PMSDbContext context, ICodeGenerationService codeGenerationService)
        {
            this._dbContext = context;
            _smartRxVitalRepository = new BaseRepository<Configuration_VitalEntity>(_dbContext);
            _codeGenerationService = codeGenerationService;
        }

        public async Task<Configuration_VitalEntity> AddAsync(Configuration_VitalEntity entity)
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

        public async Task<Configuration_VitalEntity> UpdateAsync(Configuration_VitalEntity entity)
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

        public async Task DeleteAsync(long id)
        {
            try
            {
                await _smartRxVitalRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Configuration_VitalEntity>> GetAllAsync()
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

        public async Task<Configuration_VitalEntity?> GetDetailsByIdAsync(long id)
        {
            try
            {
                var result = await _dbContext.Configuration_Vital
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);
                return result ?? new Configuration_VitalEntity();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<List<Configuration_VitalEntity>> GetVitalByName(string vitalName)
        {
            try
            {
                Configuration_VitalEntity vital = new Configuration_VitalEntity();
                List<Configuration_VitalEntity> vitals = new List<Configuration_VitalEntity>();
                bool hasFound = Common.DescriptionContainsText<Vitals>(vitalName);
                if (hasFound)
                    vitals = await _dbContext.Configuration_Vital
                        .AsNoTracking()
                        .Where(data => data.Name == vitalName)
                        .Include(data => data.Unit)
                        .ToListAsync();

                await Task.CompletedTask;
                return vitals;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
