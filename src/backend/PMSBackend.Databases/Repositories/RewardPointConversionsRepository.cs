using Microsoft.EntityFrameworkCore;
using PMSBackend.Application.DTOs;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Databases.Repositories
{
    public class RewardPointConversionsRepository : IRewardPointConversionsRepository
    {
        private readonly PMSDbContext _context;

        public RewardPointConversionsRepository(PMSDbContext context)
        {
            _context = context;
        }

        public async Task<SmartRx_RewardPointConversionsEntity> CreateAsync(SmartRx_RewardPointConversionsEntity entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
            _context.SmartRx_RewardPointConversions.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<SmartRx_RewardPointConversionsEntity?> GetByIdAsync(long id)
        {
            return await _context.SmartRx_RewardPointConversions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<SmartRx_RewardPointConversionsEntity>> GetAllAsync()
        {
            return await _context.SmartRx_RewardPointConversions
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SmartRx_RewardPointConversionsEntity>> GetByUserIdAsync(long userId)
        {
            return await _context.SmartRx_RewardPointConversions
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<SmartRx_RewardPointConversionsEntity> UpdateAsync(SmartRx_RewardPointConversionsEntity entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;
            _context.SmartRx_RewardPointConversions.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.SmartRx_RewardPointConversions.FindAsync(id);
            if (entity == null)
                return false;

            _context.SmartRx_RewardPointConversions.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(long id)
        {
            return await _context.SmartRx_RewardPointConversions
                .AnyAsync(x => x.Id == id);
        }

        public async Task<RewardPointConversionsSummaryContract> GetConversionSummaryByUserIdAsync(long userId)
        {
            // Get user's initial reward balances from SmartRx_PatientReward
            var userRewards = await _context.SmartRx_PatientReward
                .AsNoTracking()
                .Where(x => x.CreatedById == userId)
                .ToListAsync();

            // Get conversion data
            var conversions = await _context.SmartRx_RewardPointConversions
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            // Calculate initial balances (sum of all user's reward records)
            var initialCashableBalance = userRewards.Sum(x => x.EarnedCashablePoints);
            var initialNonCashableBalance = userRewards.Sum(x => x.EarnedNonCashablePoints);
            var initialMoneyBalance = userRewards.Sum(x => x.ConvertedCashableToMoney ?? 0);

            // Calculate conversion totals
            var totalConvertedToCashable = conversions
                .Where(x => x.ToType == RewardType.Cashable)
                .Sum(x => x.Amount);
                
            var totalConvertedToNonCashable = conversions
                .Where(x => x.ToType == RewardType.Noncashable)
                .Sum(x => x.Amount);
                
            var totalConvertedToMoney = conversions
                .Where(x => x.ToType == RewardType.Money)
                .Sum(x => x.Amount);

            // Calculate deductions
            var totalDeductedFromCashable = conversions
                .Where(x => x.FromType == RewardType.Cashable)
                .Sum(x => x.Amount);
                
            var totalDeductedFromNonCashable = conversions
                .Where(x => x.FromType == RewardType.Noncashable)
                .Sum(x => x.Amount);
                
            var totalDeductedFromMoney = conversions
                .Where(x => x.FromType == RewardType.Money)
                .Sum(x => x.Amount);

            // Calculate net conversion effects
            var netCashableConversion =totalConvertedToCashable - totalDeductedFromCashable;
            var netNonCashableConversion = totalConvertedToNonCashable - totalDeductedFromNonCashable;
            var netMoneyConversion = totalConvertedToMoney - totalDeductedFromMoney;

            // Calculate final balances (Initial + Net Conversion Effects)
            var finalCashableBalance = initialCashableBalance + netCashableConversion;
            var finalNonCashableBalance = initialNonCashableBalance + netNonCashableConversion;
            var finalMoneyBalance = initialMoneyBalance + netMoneyConversion;

            var summary = new RewardPointConversionsSummaryContract
            {
                UserId = userId,
                
                // Initial Balances
                InitialCashableBalance = initialCashableBalance,
                InitialNonCashableBalance = initialNonCashableBalance,
                InitialMoneyBalance = initialMoneyBalance,
                
                // Conversions TO each type
                TotalConvertedToCashable = totalConvertedToCashable,
                TotalConvertedToNonCashable = totalConvertedToNonCashable,
                TotalConvertedToMoney = totalConvertedToMoney,
                
                // Deductions FROM each type
                TotalDeductedFromCashable = totalDeductedFromCashable,
                TotalDeductedFromNonCashable = totalDeductedFromNonCashable,
                TotalDeductedFromMoney = totalDeductedFromMoney,
                
                // Final Balances
                FinalCashableBalance = finalCashableBalance,
                FinalNonCashableBalance = finalNonCashableBalance,
                FinalMoneyBalance = finalMoneyBalance,
                
                // Net Conversion Effects
                NetCashableConversion = netCashableConversion,
                NetNonCashableConversion = netNonCashableConversion,
                NetMoneyConversion = netMoneyConversion,
                
                GrandTotalConverted = totalConvertedToCashable + totalConvertedToNonCashable + totalConvertedToMoney,
                TotalConversionCount = conversions.Count
            };

            return summary;
        }
    }
}
