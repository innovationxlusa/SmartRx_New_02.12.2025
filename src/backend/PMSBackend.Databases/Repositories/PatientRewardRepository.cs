using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Databases.Repositories
{
    public class PatientRewardRepository : IPatientRewardRepository
    {
        private readonly PMSDbContext _dbContext;

        public PatientRewardRepository(PMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SmartRx_PatientReward> CreatePatientRewardAsync(SmartRx_PatientReward patientReward, CancellationToken cancellationToken)
        {
            try
            {
                patientReward.CreatedDate = DateTime.UtcNow;

                // If IsDeductPoints is true, make EarnedNonCashablePoints negative
                if (patientReward.IsDeductPoints)
                {
                    // Ensure the value is negative (take absolute value and negate it)
                    patientReward.EarnedNonCashablePoints = -Math.Abs(patientReward.EarnedNonCashablePoints);
                }

                // Calculate totals
                patientReward.TotalNonCashablePoints = patientReward.EarnedNonCashablePoints - patientReward.ConsumedNonCashablePoints;
                patientReward.TotalCashablePoints = patientReward.EarnedCashablePoints - patientReward.ConsumedCashablePoints;
                patientReward.TotalMoney = (patientReward.ConvertedCashableToMoney ?? 0) - (patientReward.EncashedMoney ?? 0);

                await _dbContext.SmartRx_PatientReward.AddAsync(patientReward, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                // Reload with navigation properties
                return await GetPatientRewardByIdAsync(patientReward.Id, cancellationToken) ?? patientReward;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create patient reward: {ex.Message}", ex);
            }
        }

        public async Task<SmartRx_PatientReward> UpdatePatientRewardAsync(SmartRx_PatientReward patientReward, CancellationToken cancellationToken)
        {
            try
            {
                var existingReward = await _dbContext.SmartRx_PatientReward
                    .FirstOrDefaultAsync(pr => pr.Id == patientReward.Id, cancellationToken);

                if (existingReward == null)
                {
                    throw new Exception($"Patient reward with ID {patientReward.Id} not found");
                }

                // Update fields
               // existingReward.BadgeId = patientReward.BadgeId;
                existingReward.IsDeductPoints = patientReward.IsDeductPoints;
                
                // If IsDeductPoints is true, make EarnedNonCashablePoints negative
                if (patientReward.IsDeductPoints)
                {
                    // Ensure the value is negative (take absolute value and negate it)
                    existingReward.EarnedNonCashablePoints = -Math.Abs(patientReward.EarnedNonCashablePoints);
                }
                else
                {
                    existingReward.EarnedNonCashablePoints = patientReward.EarnedNonCashablePoints;
                }
                
                existingReward.ConsumedNonCashablePoints = patientReward.ConsumedNonCashablePoints;
                existingReward.EarnedCashablePoints = patientReward.EarnedCashablePoints;
                existingReward.ConsumedCashablePoints = patientReward.ConsumedCashablePoints;                
                existingReward.EncashedMoney = patientReward.EncashedMoney;
                existingReward.Remarks = patientReward.Remarks;
                existingReward.ModifiedById = patientReward.ModifiedById;
                existingReward.ModifiedDate = DateTime.UtcNow;

                // Recalculate totals
                existingReward.TotalNonCashablePoints = existingReward.EarnedNonCashablePoints - existingReward.ConsumedNonCashablePoints-existingReward.ConvertedCashableToNonCashablePoints;
                existingReward.TotalCashablePoints = existingReward.EarnedCashablePoints - existingReward.ConsumedCashablePoints- existingReward.ConvertedNonCashableToCashablePoints;
                existingReward.TotalMoney = (existingReward.ConvertedCashableToMoney ?? 0) - (existingReward.EncashedMoney ?? 0);

                _dbContext.SmartRx_PatientReward.Update(existingReward);
                await _dbContext.SaveChangesAsync(cancellationToken);

                // Reload with navigation properties
                return await GetPatientRewardByIdAsync(existingReward.Id, cancellationToken) ?? existingReward;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update patient reward: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeletePatientRewardAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                var reward = await _dbContext.SmartRx_PatientReward
                    .FirstOrDefaultAsync(pr => pr.Id == id, cancellationToken);

                if (reward == null)
                {
                    return false;
                }

                _dbContext.SmartRx_PatientReward.Remove(reward);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete patient reward: {ex.Message}", ex);
            }
        }

        public async Task<PaginatedResult<SmartRx_PatientReward>> GetPatientRewardsByUserIdAndPatientIdAsync(
            long userId,
            long? patientId,
            PagingSortingParams pagingSorting,
            CancellationToken cancellationToken,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                if (pagingSorting.PageNumber <= 0) pagingSorting.PageNumber = 1;
                if (pagingSorting.PageSize <= 0) pagingSorting.PageSize = 10;

                var query = _dbContext.SmartRx_PatientReward
                    .AsNoTracking()
                    .Where(pr => pr.CreatedById == userId && (patientId == null))// || pr.PatientId == patientId))
                    .Include(pr => pr.RewardBadge)
                    //.Include(pr => pr.PatientProfile)
                    //.Include(pr => pr.SmartRxMaster)
                    //.Include(pr => pr.Prescription)
                    .AsQueryable();

                if (startDate.HasValue)
                {
                    query = query.Where(pr => pr.CreatedDate >= startDate.Value);
                }
                if (endDate.HasValue)
                {
                    // include entire day for endDate
                    var inclusiveEnd = endDate.Value.Date.AddDays(1).AddTicks(-1);
                    query = query.Where(pr => pr.CreatedDate <= inclusiveEnd);
                }

                // Apply sorting
                IQueryable<SmartRx_PatientReward> sortedQuery = pagingSorting.SortBy?.ToLower() switch
                {
                    "createddate" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(pr => pr.CreatedDate)
                        : query.OrderBy(pr => pr.CreatedDate),
                    "totalnoncashablepoints" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(pr => pr.TotalNonCashablePoints)
                        : query.OrderBy(pr => pr.TotalNonCashablePoints),
                    "totalcashablepoints" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(pr => pr.TotalCashablePoints)
                        : query.OrderBy(pr => pr.TotalCashablePoints),
                    "totalmoney" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(pr => pr.TotalMoney)
                        : query.OrderBy(pr => pr.TotalMoney),
                    _ => query.OrderByDescending(pr => pr.CreatedDate)
                };

                var totalRecords = await sortedQuery.CountAsync(cancellationToken);

                var pagedData = await sortedQuery
                    .Skip((pagingSorting.PageNumber - 1) * pagingSorting.PageSize)
                    .Take(pagingSorting.PageSize)
                    .ToListAsync(cancellationToken);

                // Recompute totals per requested formulas before returning
                foreach (var reward in pagedData)
                {
                    var convertedToMoney = reward.ConvertedCashableToMoney ?? 0;
                    var encashedMoney = reward.EncashedMoney ?? 0;

                    // total non-cashable = earned - consumed - converted to cashable
                    reward.TotalNonCashablePoints = reward.EarnedNonCashablePoints
                        - reward.ConsumedNonCashablePoints
                        - reward.ConvertedNonCashableToCashablePoints;

                    // total cashable = earned - consumed - converted to non-cashable - converted to money
                    // Note: convertedToMoney is money; without a rate we cast to int for point deduction
                    reward.TotalCashablePoints = reward.EarnedCashablePoints
                        - reward.ConsumedCashablePoints
                        - reward.ConvertedCashableToNonCashablePoints
                        - Convert.ToInt32(convertedToMoney);

                    // total money = converted cashable to money - encashed money
                    reward.TotalMoney = convertedToMoney - encashedMoney;
                }

                return new PaginatedResult<SmartRx_PatientReward>(
                    pagedData,
                    totalRecords,
                    pagingSorting.PageNumber,
                    pagingSorting.PageSize,
                    pagingSorting.SortBy,
                    pagingSorting.SortDirection,
                    null, null, null, null, null);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get patient rewards: {ex.Message}", ex);
            }
        }

        public async Task<SmartRx_PatientReward?> GetPatientRewardByIdAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.SmartRx_PatientReward
                    .AsNoTracking()
                    .Include(pr => pr.RewardBadge)
                    //.Include(pr => pr.PatientProfile)
                    //.Include(pr => pr.SmartRxMaster)
                    //.Include(pr => pr.Prescription)
                    .FirstOrDefaultAsync(pr => pr.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get patient reward by ID: {ex.Message}", ex);
            }
        }

        public async Task<PatientRewardSummaryContract?> GetPatientRewardsSummaryAsync(long userId, long? patientId, CancellationToken cancellationToken)
        {
            try
            {
                var rewards = await _dbContext.SmartRx_PatientReward
                    .AsNoTracking()
                    .Where(pr => pr.CreatedById == userId && (patientId == null || pr.PatientId == patientId))
                    .ToListAsync(cancellationToken);

                if (!rewards.Any())
                {
                    return null;
                }

                // Get the most recent badge information
                var latestReward = rewards.OrderByDescending(r => r.CreatedDate).FirstOrDefault();
                var badge = latestReward?.BadgeId != null
                    ? await _dbContext.Configuration_RewardBadge.AsNoTracking().FirstOrDefaultAsync(b => b.Id == latestReward.BadgeId, cancellationToken)
                    : null;

                // Get conversion data from RewardPointConversions table
                var conversions = await _dbContext.SmartRx_RewardPointConversions
                    .AsNoTracking()
                    .Where(x => x.CreatedById == userId)
                    .ToListAsync(cancellationToken);

                // Calculate initial balances (sum of all user's reward records)
                var initialCashableBalance = rewards.Sum(x => x.EarnedCashablePoints);
                var initialNonCashableBalance = rewards.Sum(x => x.EarnedNonCashablePoints);
                var initialMoneyBalance = rewards.Sum(x => x.ConvertedCashableToMoney ?? 0);

                // Calculate conversion totals
                var totalConvertedToCashable = conversions
                    .Where(x => x.ToType ==RewardType.Cashable)
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
                var netCashableConversion = totalConvertedToCashable - totalDeductedFromCashable;
                var netNonCashableConversion = totalConvertedToNonCashable - totalDeductedFromNonCashable;
                var netMoneyConversion = totalConvertedToMoney - totalDeductedFromMoney;

                // Calculate final balances (Initial + Net Conversion Effects)
                var finalCashableBalance = initialCashableBalance + netCashableConversion;
                var finalNonCashableBalance = initialNonCashableBalance + netNonCashableConversion;
                var finalMoneyBalance = initialMoneyBalance + netMoneyConversion;

                //// Legacy calculations for backward compatibility
                //var totalNonCashablePoints = rewards.Sum(r =>
                //    (r.EarnedNonCashablePoints)
                //    - (r.ConsumedNonCashablePoints)
                //    - (r.ConvertedNonCashableToCashablePoints))+finalNonCashableBalance;

                //var totalCashablePoints = rewards.Sum(r =>
                //    (r.EarnedCashablePoints)
                //    - (r.ConsumedCashablePoints)
                //    - (r.ConvertedCashableToNonCashablePoints))+finalCashableBalance;

                //var totalMoney = rewards.Sum(r => (r.ConvertedCashableToMoney ?? 0) - (r.EncashedMoney ?? 0))+finalMoneyBalance;

                return new PatientRewardSummaryContract
                {
                    UserId = userId,
                    BadgeId = badge?.Id ?? 0,
                    BadgeName = badge?.Name ?? string.Empty,
                    PatientId = patientId ?? 0,

                    //// Initial Balances
                    //InitialCashableBalance = initialCashableBalance,
                    //InitialNonCashableBalance = initialNonCashableBalance,
                    //InitialMoneyBalance = initialMoneyBalance,

                    //// Conversions TO each type
                    //TotalConvertedToCashable = totalConvertedToCashable,
                    //TotalConvertedToNonCashable = totalConvertedToNonCashable,
                    //TotalConvertedToMoney = totalConvertedToMoney,

                    //// Deductions FROM each type
                    //TotalDeductedFromCashable = totalDeductedFromCashable,
                    //TotalDeductedFromNonCashable = totalDeductedFromNonCashable,
                    //TotalDeductedFromMoney = totalDeductedFromMoney,

                    //// Final Balances
                    //FinalCashableBalance = finalCashableBalance,
                    //FinalNonCashableBalance = finalNonCashableBalance,
                    //FinalMoneyBalance = finalMoneyBalance,

                    //// Net Conversion Effects
                    //NetCashableConversion = netCashableConversion,
                    //NetNonCashableConversion = netNonCashableConversion,
                    //NetMoneyConversion = netMoneyConversion,

                    // Legacy fields for backward compatibility
                    TotalEarnedNonCashablePoints = rewards.Sum(r => r.EarnedNonCashablePoints),
                    TotalConsumedNonCashablePoints = rewards.Sum(r => r.ConsumedNonCashablePoints),
                    TotalNonCashablePoints = finalNonCashableBalance,
                    TotalEarnedCashablePoints = rewards.Sum(r => r.EarnedCashablePoints),
                    TotalConsumedCashablePoints = rewards.Sum(r => r.ConsumedCashablePoints),
                    TotalCashablePoints = finalCashableBalance,
                    TotalEarnedMoney = rewards.Sum(r => r.ConvertedCashableToMoney ?? 0),
                    TotalConsumedMoney = rewards.Sum(r => r.EncashedMoney ?? 0),
                    TotalMoney = finalMoneyBalance,
                    TotalEncashMoney = rewards.Sum(r => r.EncashedMoney ?? 0),
                    
                    GrandTotalConverted = totalConvertedToCashable + totalConvertedToNonCashable + totalConvertedToMoney,
                    TotalConversionCount = conversions.Count
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get patient rewards summary: {ex.Message}", ex);
            }
        }

        public async Task<PaginatedResult<SmartRx_PatientReward>> GetPatientRewardsByUserIdAsync(
            long userId,
            PagingSortingParams pagingSorting,
            CancellationToken cancellationToken)
        {
            try
            {
                if (pagingSorting.PageNumber <= 0) pagingSorting.PageNumber = 1;
                if (pagingSorting.PageSize <= 0) pagingSorting.PageSize = 10;

                var query = _dbContext.SmartRx_PatientReward
                    .AsNoTracking()
                    .Where(pr => pr.CreatedById == userId)
                    .Include(pr => pr.RewardBadge)
                    //.Include(pr => pr.PatientProfile)
                    //.Include(pr => pr.SmartRxMaster)
                    //.Include(pr => pr.Prescription)
                    .AsQueryable();

                // Apply sorting
                IQueryable<SmartRx_PatientReward> sortedQuery = pagingSorting.SortBy?.ToLower() switch
                {
                    "createddate" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(pr => pr.CreatedDate)
                        : query.OrderBy(pr => pr.CreatedDate),
                    "patientid" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(pr => pr.PatientId)
                        : query.OrderBy(pr => pr.PatientId),
                    "totalnoncashablepoints" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(pr => pr.TotalNonCashablePoints)
                        : query.OrderBy(pr => pr.TotalNonCashablePoints),
                    "totalcashablepoints" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(pr => pr.TotalCashablePoints)
                        : query.OrderBy(pr => pr.TotalCashablePoints),
                    "totalmoney" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(pr => pr.TotalMoney)
                        : query.OrderBy(pr => pr.TotalMoney),
                    _ => query.OrderByDescending(pr => pr.CreatedDate)
                };

                var totalRecords = await sortedQuery.CountAsync(cancellationToken);

                var pagedData = await sortedQuery
                    .Skip((pagingSorting.PageNumber - 1) * pagingSorting.PageSize)
                    .Take(pagingSorting.PageSize)
                    .ToListAsync(cancellationToken);

                return new PaginatedResult<SmartRx_PatientReward>(
                    pagedData,
                    totalRecords,
                    pagingSorting.PageNumber,
                    pagingSorting.PageSize,
                    pagingSorting.SortBy,
                    pagingSorting.SortDirection,
                    null, null, null, null, null);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get patient rewards by user ID: {ex.Message}", ex);
            }
        }
    }
}

