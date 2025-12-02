using PMSBackend.Domain.Entities;

namespace PMSBackend.Domain.Repositories
{
    public interface IConfigurationThanaRepository
    {
        Task<List<Configuration_DistrictEntity>> GetAllDistrict();
        Task<Configuration_DistrictEntity> GetAllDistrict(string districtCode);

    }
}
