using PMSBackend.Domain.Entities;

namespace PMSBackend.Domain.Repositories
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Configuration_CountryEntity>> GetAllCountriesAsync();
        Task<Configuration_CountryEntity?> GetCountryByIdAsync(long id);
        Task<Configuration_CountryEntity?> GetCountryByCodeAsync(string code);
    }
}
