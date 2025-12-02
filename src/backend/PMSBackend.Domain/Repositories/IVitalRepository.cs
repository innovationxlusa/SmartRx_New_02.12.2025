using PMSBackend.Domain.Entities;

namespace PMSBackend.Domain.Repositories
{
    public interface IVitalRepository : IBaseRepository<Configuration_VitalEntity>
    {
        Task<List<Configuration_VitalEntity>> GetVitalByName(string vitalName);
    }
}
