using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Databases.Services;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Databases.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly PMSDbContext _dbContext;
        private readonly ICodeGenerationService _codeGenerationService;

        public CountryRepository(PMSDbContext context, ICodeGenerationService codeGenerationService)
        {
            _dbContext = context;
            _codeGenerationService = codeGenerationService;
        }

        public async Task<IEnumerable<Configuration_CountryEntity>> GetAllCountriesAsync()
        {
            try
            {
                return await _dbContext.Configuration_Country
                    .AsNoTracking()
                    .OrderBy(c => c.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving countries: {ex.Message}");
            }
        }

        public async Task<Configuration_CountryEntity?> GetCountryByIdAsync(long id)
        {
            try
            {
                return await _dbContext.Configuration_Country
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving country by ID {id}: {ex.Message}");
            }
        }

        public async Task<Configuration_CountryEntity?> GetCountryByCodeAsync(string code)
        {
            try
            {
                return await _dbContext.Configuration_Country
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Code == code);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving country by code {code}: {ex.Message}");
            }
        }
    }
}
