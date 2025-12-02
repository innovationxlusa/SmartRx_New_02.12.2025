using PMSBackend.Databases.Services;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Databases.Repositories
{
    public class ConfigurationThanaRepository : IConfigurationThanaRepository
    {
        private readonly ICodeGenerationService _codeGenerationService;

        public ConfigurationThanaRepository(ICodeGenerationService codeGenerationService)
        {
            _codeGenerationService = codeGenerationService;
        }
        public Task<Configuration_PoliceStationEntity> AddAsync(Configuration_PoliceStationEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Configuration_PoliceStationEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Configuration_DistrictEntity>> GetAllDistrict()
        {
            throw new NotImplementedException();
        }

        public Task<Configuration_DistrictEntity> GetAllDistrict(string districtCode)
        {
            throw new NotImplementedException();
        }

        public Task<Configuration_PoliceStationEntity?> GetDetailsByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Configuration_PoliceStationEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
