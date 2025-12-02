using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Databases.Services
{

    public class CodeGenerationService : ICodeGenerationService
    {
        private readonly PMSDbContext _dbContext;

        public CodeGenerationService(PMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Generates a Patient code (format: P + 9 digits, e.g., P000000001)
        /// </summary>
        public async Task<string> GeneratePatientCodeAsync(CancellationToken cancellationToken = default)
        {
            var lastPatientCode = await _dbContext.Smartrx_PatientProfile
                .AsNoTracking()
                .Where(p => p.PatientCode.StartsWith("P") && p.PatientCode.Length >= 2)
                .OrderByDescending(p => p.PatientCode)
                .Select(p => p.PatientCode)
                .FirstOrDefaultAsync(cancellationToken);

            int nextCode = 1;
            if (!string.IsNullOrEmpty(lastPatientCode))
            {
                var numericPart = lastPatientCode.Substring(1);
                if (long.TryParse(numericPart, out long lastCodeNumber))
                {
                    nextCode = (int)(lastCodeNumber + 1);
                }
            }

            // P + 9 digits = 10-character nchar(10) PatientCode
            return "P" + nextCode.ToString().PadLeft(9, '0');
        }

        /// <summary>
        /// Generates a Doctor code (format: D + 9 digits, e.g., D000000001)
        /// </summary>
        public async Task<string> GenerateDoctorCodeAsync(CancellationToken cancellationToken = default)
        {
            var lastDoctor = await _dbContext.Configuration_Doctor
                .AsNoTracking()
                .Where(d => d.Code.StartsWith("D") && d.Code.Length >= 2)
                .OrderByDescending(d => d.Code)
                .Select(d => d.Code)
                .FirstOrDefaultAsync(cancellationToken);

            int nextCode = 1;
            if (!string.IsNullOrEmpty(lastDoctor))
            {
                var numericPart = lastDoctor.Substring(1);
                if (long.TryParse(numericPart, out long lastCodeNumber))
                {
                    nextCode = (int)(lastCodeNumber + 1);
                }
            }

            return "D" + nextCode.ToString().PadLeft(9, '0');
        }

        /// <summary>
        /// Generates a Reward code (format: RW + 7 digits, e.g., RW0000001)
        /// </summary>
        public async Task<string> GenerateRewardCodeAsync(CancellationToken cancellationToken = default)
        {
            var lastReward = await _dbContext.Configuration_RewardRule
                .AsNoTracking()
                .Where(r => r.ActivityCode.StartsWith("RW") && r.ActivityCode.Length==10)
                .OrderByDescending(r => r.ActivityCode)
                .Select(r => r.ActivityCode)
                .FirstOrDefaultAsync(cancellationToken);

            int nextCode = 1;
            if (!string.IsNullOrEmpty(lastReward))
            {
                var numericPart = lastReward.Substring(2);
                if (long.TryParse(numericPart, out long lastCodeNumber))
                {
                    nextCode = (int)(lastCodeNumber + 1);
                }
            }

            return "RW" + nextCode.ToString().PadLeft(8, '0');
        }

        /// <summary>
        /// Generates a User code (format: 10 digits, e.g., 0000000001)
        /// </summary>
        public async Task<string> GenerateUserCodeAsync(CancellationToken cancellationToken = default)
        {
            var lastUserCode = await _dbContext.Security_PMSUsers
                .AsNoTracking()
                .OrderByDescending(u => u.UserCode)
                .Select(u => u.UserCode)
                .FirstOrDefaultAsync(cancellationToken);

            int nextCode = 1;
            if (!string.IsNullOrEmpty(lastUserCode) && int.TryParse(lastUserCode, out int lastCodeNumber))
            {
                nextCode = lastCodeNumber + 1;
            }

            return nextCode.ToString().PadLeft(10, '0');
        }

        /// <summary>
        /// Generates a UserActivity code (format: R + 9 digits, e.g., R000000001)
        /// </summary>
        public async Task<string> GenerateUserActivityCodeAsync(CancellationToken cancellationToken = default)
        {
            var lastActivityCode = await _dbContext.Configuration_UserActivity
                .AsNoTracking()
                .Where(ua => ua.ActivityCode.StartsWith("R") && ua.ActivityCode.Length >= 2)
                .OrderByDescending(ua => ua.ActivityCode)
                .Select(ua => ua.ActivityCode)
                .FirstOrDefaultAsync(cancellationToken);

            int nextCode = 1;
            if (!string.IsNullOrEmpty(lastActivityCode))
            {
                var numericPart = lastActivityCode.Substring(1);
                if (long.TryParse(numericPart, out long lastCodeNumber))
                {
                    nextCode = (int)(lastCodeNumber + 1);
                }
            }

            return "R" + nextCode.ToString().PadLeft(9, '0');
        }

        /// <summary>
        /// Generic method to generate codes with custom prefix and query
        /// </summary>
        public async Task<string> GenerateCodeAsync(string prefix, Func<IQueryable<string>, IQueryable<string>> queryBuilder, int digitLength, CancellationToken cancellationToken = default)
        {
            try
            {
                // This is a generic fallback - specific methods above are preferred
                // For now, this is a placeholder that can be extended
                throw new NotImplementedException("Use specific methods like GeneratePatientCodeAsync, GenerateDoctorCodeAsync, etc.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to generate code: {ex.Message}", ex);
            }
        }
    }
}

