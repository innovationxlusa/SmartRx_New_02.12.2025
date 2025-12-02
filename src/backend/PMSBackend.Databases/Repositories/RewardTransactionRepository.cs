using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Databases.Repositories
{
    public class RewardTransactionRepository : IRewardTransactionRepository
    {
        private readonly PMSDbContext _dbContext;

        public RewardTransactionRepository(PMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SmartRx_RewardTransactionEntity> CreateRewardTransactionAsync(
            SmartRx_RewardTransactionEntity transaction,
            CancellationToken cancellationToken)
        {
            try
            {
                transaction.CreatedDate = DateTime.UtcNow;

                await _dbContext.SmartRx_RewardTransaction.AddAsync(transaction, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                await LoadNavigationPropertiesAsync(transaction, cancellationToken);

                return transaction;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create reward transaction: {ex.Message}", ex);
            }
        }

        public async Task<SmartRx_RewardTransactionEntity> UpdateRewardTransactionAsync(
            SmartRx_RewardTransactionEntity transaction,
            CancellationToken cancellationToken)
        {
            try
            {
                var existingTransaction = await _dbContext.SmartRx_RewardTransaction
                    .FirstOrDefaultAsync(rt => rt.Id == transaction.Id, cancellationToken);

                if (existingTransaction == null)
                {
                    throw new Exception($"Reward transaction with ID {transaction.Id} not found");
                }

                existingTransaction.UserId = transaction.UserId;
                existingTransaction.BadgeId = transaction.BadgeId;
                existingTransaction.RewardRuleId = transaction.RewardRuleId;
                existingTransaction.RewardType = transaction.RewardType;
                existingTransaction.SmartRxMasterId = transaction.SmartRxMasterId;
                existingTransaction.PrescriptionId = transaction.PrescriptionId;
                existingTransaction.PatientId = transaction.PatientId;
                existingTransaction.IsDeductPoints = transaction.IsDeductPoints;
                existingTransaction.AmountChanged = transaction.AmountChanged;
                existingTransaction.NonCashableBalance = transaction.NonCashableBalance;
                existingTransaction.CashableBalance = transaction.CashableBalance;
                existingTransaction.CashedMoneyBalance = transaction.CashedMoneyBalance;
                existingTransaction.Remarks = transaction.Remarks;
                existingTransaction.ModifiedById = transaction.ModifiedById;
                existingTransaction.ModifiedDate = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync(cancellationToken);
                await LoadNavigationPropertiesAsync(existingTransaction, cancellationToken);

                return existingTransaction;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update reward transaction: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteRewardTransactionAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                var transaction = await _dbContext.SmartRx_RewardTransaction
                    .FirstOrDefaultAsync(rt => rt.Id == id, cancellationToken);

                if (transaction == null)
                {
                    return false;
                }

                _dbContext.SmartRx_RewardTransaction.Remove(transaction);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete reward transaction: {ex.Message}", ex);
            }
        }

        public async Task<SmartRx_RewardTransactionEntity?> GetRewardTransactionByIdAsync(
            long id,
            CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.SmartRx_RewardTransaction
                    .AsNoTracking()
                    .Include(rt => rt.User)
                    .Include(rt => rt.RewardBadge)
                    .Include(rt => rt.RewardRule)
                    .Include(rt => rt.SmartRxMaster)
                    .FirstOrDefaultAsync(rt => rt.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get reward transaction by ID: {ex.Message}", ex);
            }
        }

        public async Task<PaginatedResult<SmartRx_RewardTransactionEntity>> GetAllRewardTransactionsAsync(
            PagingSortingParams pagingSorting,
            CancellationToken cancellationToken)
        {
            try
            {
                if (pagingSorting.PageNumber <= 0) pagingSorting.PageNumber = 1;
                if (pagingSorting.PageSize <= 0) pagingSorting.PageSize = 10;

                var query = _dbContext.SmartRx_RewardTransaction
                    .AsNoTracking()
                    .Include(rt => rt.User)
                    .Include(rt => rt.RewardBadge)
                    .Include(rt => rt.RewardRule)
                    .Include(rt => rt.SmartRxMaster)
                    .AsQueryable();

                IQueryable<SmartRx_RewardTransactionEntity> sortedQuery = pagingSorting.SortBy?.ToLower() switch
                {
                    "userid" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(rt => rt.UserId)
                        : query.OrderBy(rt => rt.UserId),
                    "rewardruleid" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(rt => rt.RewardRuleId)
                        : query.OrderBy(rt => rt.RewardRuleId),
                    "amountchanged" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(rt => rt.AmountChanged)
                        : query.OrderBy(rt => rt.AmountChanged),
                    "createddate" => pagingSorting.SortDirection?.ToLower() == "asc"
                        ? query.OrderBy(rt => rt.CreatedDate)
                        : query.OrderByDescending(rt => rt.CreatedDate),
                    _ => query.OrderByDescending(rt => rt.CreatedDate)
                };

                var totalRecords = await sortedQuery.CountAsync(cancellationToken);

                var pagedData = await sortedQuery
                    .Skip((pagingSorting.PageNumber - 1) * pagingSorting.PageSize)
                    .Take(pagingSorting.PageSize)
                    .ToListAsync(cancellationToken);

                return new PaginatedResult<SmartRx_RewardTransactionEntity>(
                    pagedData,
                    totalRecords,
                    pagingSorting.PageNumber,
                    pagingSorting.PageSize,
                    pagingSorting.SortBy ?? "CreatedDate",
                    pagingSorting.SortDirection ?? "desc",
                    null, null, null, null, null);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get all reward transactions: {ex.Message}", ex);
            }
        }

        public async Task<RewardTransactionResult?> CreateRewardTransactionForPrescriptionAsync(
            long userId,
            long? prescriptionId,
            long? smartRxMasterId,
            long? patientId,
            string activityName,
            string activityFor,
            CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(activityName))
                {
                    return null;
                }

                // Find the reward rule by activity name
                var rewardRule = await _dbContext.Configuration_RewardRule
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.ActivityName.ToLower() == activityName.ToLower(), cancellationToken);

                if (rewardRule == null || rewardRule.Points == 0)
                {
                    return null;
                }

                // For one-time events like DELETE_PRESCRIPTION, ensure we don't create
                // duplicate reward transactions for the same user + rule + prescription.
                if (string.Equals(activityName, "DELETE_PRESCRIPTION", StringComparison.OrdinalIgnoreCase))
                {
                    var alreadyExists = await _dbContext.SmartRx_RewardTransaction
                        .AsNoTracking()
                        .AnyAsync(rt =>
                            rt.UserId == userId &&
                            rt.RewardRuleId == rewardRule.Id &&
                            rt.PrescriptionId == prescriptionId,
                            cancellationToken);

                    if (alreadyExists)
                    {
                        return null;
                    }
                }

                // Get latest user badge (if any)
                var latestUserBadge = await _dbContext.SmartRx_UserRewardBadge
                    .AsNoTracking()
                    .OrderByDescending(urb => urb.EarnedDate)
                    .ThenByDescending(urb => urb.CreatedDate)
                    .FirstOrDefaultAsync(urb => urb.UserId == userId, cancellationToken);

                var isDeduct = rewardRule.IsDeductible;
                var points = rewardRule.Points;
                var amountChanged = isDeduct ? -points : points;

                var transaction = new SmartRx_RewardTransactionEntity
                {
                    UserId = userId,
                    BadgeId = latestUserBadge?.BadgeId,
                    RewardRuleId = rewardRule.Id,
                    RewardType = RewardType.Noncashable,
                    SmartRxMasterId = smartRxMasterId,
                    PrescriptionId = prescriptionId,
                    PatientId = patientId,
                    AmountChanged = amountChanged,
                    NonCashableBalance = amountChanged,
                    CashableBalance = 0,
                    CashedMoneyBalance = 0,
                    IsDeductPoints = isDeduct,
                    Remarks = null
                };

                await CreateRewardTransactionAsync(transaction, cancellationToken);

                var result = new RewardTransactionResult
                {
                    IsRewardUpdated = true,
                    RewardTitle = rewardRule.ActivityTaken,
                    Points = points,
                };

                result.RewardMessage = isDeduct
                    ? $"You have consumed {points} points for {activityFor}."
                    : $"You have earned {points} points for {activityFor}.";

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create reward transaction for prescription: {ex.Message}", ex);
            }
        }

        public async Task<RewardTransactionSummaryContract?> GetRewardTransactionSummaryByUserIdAsync(
            long userId,
            CancellationToken cancellationToken)
        {
            try
            {
                // Get all reward transactions for the user
                var transactions = await _dbContext.SmartRx_RewardTransaction
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .ToListAsync(cancellationToken);

                // Get all conversions for the user
                var conversions = await _dbContext.SmartRx_RewardPointConversions
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .ToListAsync(cancellationToken);

                // Calculate initial balances (sum of all user's reward transactions)
                var initialCashableBalance = transactions.Sum(x => x.CashableBalance);
                var initialNonCashableBalance = transactions.Sum(x => x.NonCashableBalance);
                var initialMoneyBalance = transactions.Sum(x => x.CashedMoneyBalance);

                // Calculate conversion totals
                var totalConvertedToCashable = conversions
                    .Where(x => x.ToType == RewardType.Cashable)
                    .Sum(x => x.ConvertedPoints);
                    
                var totalConvertedToNonCashable = conversions
                    .Where(x => x.ToType == RewardType.Noncashable)
                    .Sum(x => x.ConvertedPoints);
                    
                var totalConvertedToMoney = conversions
                    .Where(x => x.ToType == RewardType.Money)
                    .Sum(x => x.ConvertedPoints);

                var encashedMoney = conversions
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

                // Calculate earned totals (transactions where IsDeductPoints is false, meaning points were earned)
                var earnedTotalNonCashable = transactions
                    .Where(x => !x.IsDeductPoints)
                    .Sum(x => x.NonCashableBalance);
                
                var earnedTotalCashable = transactions
                    .Where(x => !x.IsDeductPoints)
                    .Sum(x => x.CashableBalance);
                
                var earnedTotalMoney = transactions
                    .Where(x => !x.IsDeductPoints)
                    .Sum(x => x.CashedMoneyBalance);

                var totalConvertedToCashed = conversions
                    .Where(x => x.ToType == RewardType.Money)
                    .Sum(x => x.ConvertedPoints);

                var totalConsumedPoints = Math.Abs(transactions
                    .Where(x => x.IsDeductPoints)
                    .Sum(x => x.NonCashableBalance))+ totalConvertedToCashable+ totalConvertedToCashed;
                // Calculate total point (noncashable + cashable + encashed money)
                var totalPoint = finalNonCashableBalance + finalCashableBalance + finalMoneyBalance;

                // Get the most recent badge information from UserRewardBadge table
                var latestUserBadge = await _dbContext.SmartRx_UserRewardBadge
                    .AsNoTracking()
                    .Include(urb => urb.Badge)
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.EarnedDate)
                    .ThenByDescending(x => x.CreatedDate)
                    .FirstOrDefaultAsync(cancellationToken);
                
                var badge = latestUserBadge?.Badge;

                var summary = new RewardTransactionSummaryContract
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
                    TotalConversionCount = conversions.Count,
                    TotalTransactionCount = transactions.Count,
                    
                    // Additional summary properties
                    TotalPoint = totalPoint,
                    TotalNonCashable = finalNonCashableBalance,
                    TotalCashable = finalCashableBalance,
                    TotalMoney= earnedTotalMoney,
                    EarnedTotalNonCashable = earnedTotalNonCashable,
                    EarnedTotalCashable = earnedTotalCashable,
                    EarnedTotalCashed= totalConvertedToMoney,
                    EarnedTotalMoney = earnedTotalMoney,

                    TotalEarnedPoints = earnedTotalNonCashable,// + earnedTotalCashable+ totalConvertedToMoney,
                    TotalConsumedPoints = totalConsumedPoints,
                    
                    // Badge Information
                    BadgeId = badge?.Id,
                    BadgeName = badge?.Name
                };

                return summary;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get reward transaction summary by user ID: {ex.Message}", ex);
            }
        }

        public async Task<List<RewardTransactionDetailContract>> GetRewardTransactionDetailsByUserIdAsync(
            long userId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            bool? earned = null,
            bool? consumed = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var detailsList = new List<RewardTransactionDetailContract>();

                // Build query for reward transactions with filters
                var transactionQuery = _dbContext.SmartRx_RewardTransaction
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .Include(rt => rt.RewardBadge)
                    .Include(rt => rt.RewardRule)
                    .AsQueryable();

                // Apply date range filter
                if (startDate.HasValue)
                {
                    transactionQuery = transactionQuery.Where(x => x.CreatedDate >= startDate.Value);
                }
                if (endDate.HasValue)
                {
                    // Add one day to endDate to include the entire end date
                    var endDateInclusive = endDate.Value.Date.AddDays(1).AddTicks(-1);
                    transactionQuery = transactionQuery.Where(x => x.CreatedDate <= endDateInclusive);
                }

                // Apply earned/consumed filter
                if (earned == true)
                {
                    transactionQuery = transactionQuery.Where(x => !x.IsDeductPoints);
                }
                if (consumed == true)
                {
                    transactionQuery = transactionQuery.Where(x => x.IsDeductPoints);
                }

                var transactions = await transactionQuery
                    .OrderByDescending(x => x.CreatedDate)
                    .ToListAsync(cancellationToken);

                // Convert reward transactions to contracts
                foreach (var transaction in transactions)
                {
                    var contract = new RewardTransactionDetailContract
                    {
                        Id = transaction.Id,
                        RecordType = "PatientReward",
                        CreatedDate = transaction.CreatedDate,
                        RewardTransactionId = transaction.Id,                        
                        PatientId = transaction.PatientId,
                        PatientName = "",
                        BadgeId = transaction.BadgeId,
                        BadgeName = transaction.RewardBadge?.Name,
                        RewardRuleId = transaction.RewardRuleId,
                        RewardRuleName = transaction.RewardRule?.ActivityName,
                        ActivityTaken = transaction.RewardRule?.ActivityTaken,
                        RewarDescription = transaction.RewardRule?.RewardDetails,
                        RewardType = transaction.RewardType,
                        SmartRxMasterId = transaction.SmartRxMasterId,
                        PrescriptionId = transaction.PrescriptionId,                       
                        IsDeductPoints = transaction.IsDeductPoints,
                        AmountChanged = transaction.AmountChanged,
                        NonCashablePoints = transaction.NonCashableBalance != 0 ? transaction.NonCashableBalance : null,
                        CashablePoints = transaction.CashableBalance != 0 ? transaction.CashableBalance : null,
                        CashedMoney = transaction.CashedMoneyBalance != 0 ? transaction.CashedMoneyBalance : null,
                        Remarks = transaction.Remarks
                    };
                    detailsList.Add(contract);
                }

                // Build query for conversions with filters
                var conversionQuery = _dbContext.SmartRx_RewardPointConversions
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .AsQueryable();

                // Apply date range filter for conversions
                if (startDate.HasValue)
                {
                    conversionQuery = conversionQuery.Where(x => x.CreatedDate >= startDate.Value);
                }
                if (endDate.HasValue)
                {
                    // Add one day to endDate to include the entire end date
                    var endDateInclusive = endDate.Value.Date.AddDays(1).AddTicks(-1);
                    conversionQuery = conversionQuery.Where(x => x.CreatedDate <= endDateInclusive);
                }

                // Note: Conversions don't have earned/consumed concept, so we don't filter by those
                var conversions = await conversionQuery
                    .OrderByDescending(x => x.CreatedDate)
                    .ToListAsync(cancellationToken);

                // Convert conversions to contracts
                foreach (var conversion in conversions)
                {
                    var contract = new RewardTransactionDetailContract
                    {
                        Id = conversion.Id,
                        RecordType = "Conversion",
                        CreatedDate = conversion.CreatedDate,
                        ConversionId = conversion.Id,
                        RewarDescription = GetTypeName(conversion.FromType) + " to " + GetTypeName(conversion.ToType),
                        PatientName = "",
                        FromType = conversion.FromType,
                        FromTypeName = GetTypeName(conversion.FromType),
                        ToType = conversion.ToType,
                        ToTypeName = GetTypeName(conversion.ToType),
                        NonCashablePoints = conversion.ToType==RewardType.Noncashable? Math.Abs(conversion.Amount) : null,
                        CashablePoints = conversion.ToType == RewardType.Cashable ? Math.Abs(conversion.Amount) : null,
                        CashedMoney = conversion.ToType == RewardType.Money ? Math.Abs(conversion.Amount) : null,                      
                        Remarks = $"Converted from {GetTypeName(conversion.FromType)} to {GetTypeName(conversion.ToType)}"
                    };
                    detailsList.Add(contract);
                }

                // Sort by CreatedDate descending (most recent first)
                return detailsList.OrderByDescending(d => d.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get reward transaction details by user ID: {ex.Message}", ex);
            }
        }

        public RewardTransactionQueryResult GetRewardTransactionQueriesByUserId(
            long userId,
            long? patientId = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                // Base query for all transactions
                var baseQuery = _dbContext.SmartRx_RewardTransaction
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .Include(rt => rt.RewardBadge)
                    .Include(rt => rt.RewardRule)
                    .AsQueryable();

                // Apply PatientId filter if provided
                if (patientId.HasValue && patientId.Value>0)
                {
                    baseQuery = baseQuery.Where(x => x.PatientId == patientId.Value);
                }

                // Apply date range filter
                if (startDate.HasValue)
                {
                    baseQuery = baseQuery.Where(x => x.CreatedDate >= startDate.Value);
                }
                if (endDate.HasValue)
                {
                    // Add one day to endDate to include the entire end date
                    var endDateInclusive = endDate.Value.Date.AddDays(1).AddTicks(-1);
                    baseQuery = baseQuery.Where(x => x.CreatedDate <= endDateInclusive);
                }

                // All transactions query
                var allQuery = baseQuery;

                // Earned transactions query (RewardRule.IsDeductible == false)
                var earnedQuery = baseQuery.Where(x => x.RewardRule == null || !x.RewardRule.IsDeductible);

                // Consumed transactions query (RewardRule.IsDeductible == true)
                var consumedQuery = baseQuery.Where(x => x.RewardRule != null && x.RewardRule.IsDeductible);

                return new RewardTransactionQueryResult
                {
                    All = allQuery,
                    Earned = earnedQuery,
                    Consumed = consumedQuery
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get reward transaction queries by user ID: {ex.Message}", ex);
            }
        }

        private string GetTypeName(RewardType type)
        {
            return type switch
            {
                RewardType.Noncashable => "Noncashable",
                RewardType.Cashable => "Cashable",
                RewardType.Money => "Money",
                _ => "Unknown"
            };
        }

        private async Task LoadNavigationPropertiesAsync(
            SmartRx_RewardTransactionEntity transaction,
            CancellationToken cancellationToken)
        {
            await _dbContext.Entry(transaction).Reference(rt => rt.User).LoadAsync(cancellationToken);
            await _dbContext.Entry(transaction).Reference(rt => rt.RewardBadge).LoadAsync(cancellationToken);
            await _dbContext.Entry(transaction).Reference(rt => rt.RewardRule).LoadAsync(cancellationToken);
            await _dbContext.Entry(transaction).Reference(rt => rt.SmartRxMaster).LoadAsync(cancellationToken);
        }
    }
}

