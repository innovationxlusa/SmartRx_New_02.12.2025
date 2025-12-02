using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Databases.Repositories
{
    public class RewardRuleRepository : IRewardRuleRepository
    {
        private readonly PMSDbContext _dbContext;
        private readonly ICodeGenerationService _codeGenerationService;

        public RewardRuleRepository(PMSDbContext dbContext, ICodeGenerationService codeGenerationService)
        {
            _dbContext = dbContext;
            _codeGenerationService = codeGenerationService;
        }

        public async Task<Configuration_RewardRuleEntity> CreateRewardRuleAsync(Configuration_RewardRuleEntity rewardRule, CancellationToken cancellationToken)
        {
            try
            {
                rewardRule.ActivityCode = await _codeGenerationService.GenerateRewardCodeAsync(cancellationToken); 
                rewardRule.CreatedDate = DateTime.UtcNow;
                rewardRule.IsActive = true;

                await _dbContext.Configuration_RewardRule.AddAsync(rewardRule, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return rewardRule;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create reward rule: {ex.Message}", ex);
            }
        }

        public async Task<Configuration_RewardRuleEntity> UpdateRewardRuleAsync(Configuration_RewardRuleEntity rewardRule, CancellationToken cancellationToken)
        {
            try
            {
                var existingRule = await _dbContext.Configuration_RewardRule
                    .FirstOrDefaultAsync(r => r.Id == rewardRule.Id, cancellationToken);

                if (existingRule == null)
                {
                    throw new Exception($"Reward rule with ID '{rewardRule.ActivityName}' not found");
                }

                // Update fields
                existingRule.ActivityCode = existingRule.ActivityCode;
                existingRule.ActivityName = rewardRule.ActivityName;
                existingRule.ActivityTaken = rewardRule.ActivityTaken;
                existingRule.RewardType = rewardRule.RewardType;
                existingRule.RewardDetails = rewardRule.RewardDetails;
                existingRule.IsDeductible = rewardRule.IsDeductible;
                existingRule.IsVisibleBenifit = rewardRule.IsVisibleBenifit;
                existingRule.Points = rewardRule.Points;
                existingRule.IsActive = rewardRule.IsActive;
                existingRule.ModifiedById = rewardRule.ModifiedById;
                existingRule.ModifiedDate = DateTime.UtcNow;

                _dbContext.Configuration_RewardRule.Update(existingRule);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return existingRule;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update reward rule: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteRewardRuleAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                var rewardRule = await _dbContext.Configuration_RewardRule
                    .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

                if (rewardRule == null)
                {
                    return false;
                }

                _dbContext.Configuration_RewardRule.Remove(rewardRule);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete reward rule: {ex.Message}", ex);
            }
        }

        public async Task<Configuration_RewardRuleEntity?> GetRewardRuleByIdAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.Configuration_RewardRule
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get reward rule by ID: {ex.Message}", ex);
            }
        }

        public async Task<Configuration_RewardRuleEntity?> GetRewardRuleByActivityNameAsync(string activityName, CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.Configuration_RewardRule
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.ActivityName.ToLower() == activityName.ToLower(), cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get reward rule by activity name: {ex.Message}", ex);
            }
        }

        public async Task<PaginatedResult<Configuration_RewardRuleEntity>> GetAllRewardRulesAsync(
            PagingSortingParams pagingSorting,
            CancellationToken cancellationToken)
        {
            try
            {
                if (pagingSorting.PageNumber <= 0) pagingSorting.PageNumber = 1;
                if (pagingSorting.PageSize <= 0) pagingSorting.PageSize = 10;

                var query = _dbContext.Configuration_RewardRule
                    .AsNoTracking()
                    .Where(r=>r.IsActive==true && r.IsVisibleBenifit==true)
                    .AsQueryable();

                // Apply sorting
                IQueryable<Configuration_RewardRuleEntity> sortedQuery = pagingSorting.SortBy?.ToLower() switch
                {
                    "activitycode" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(r => r.ActivityCode)
                        : query.OrderBy(r => r.ActivityCode),
                    "activityname" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(r => r.ActivityName)
                        : query.OrderBy(r => r.ActivityName),
                    "rewardtype" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(r => r.RewardType)
                        : query.OrderBy(r => r.RewardType),
                    "points" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(r => r.Points)
                        : query.OrderBy(r => r.Points),
                    "createddate" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(r => r.CreatedDate)
                        : query.OrderBy(r => r.CreatedDate),
                    _ => query.OrderByDescending(r => r.CreatedDate)
                };

                var totalRecords = await sortedQuery.CountAsync(cancellationToken);

                var pagedData = await sortedQuery
                    .Skip((pagingSorting.PageNumber - 1) * pagingSorting.PageSize)
                    .Take(pagingSorting.PageSize)
                    .ToListAsync(cancellationToken);

                return new PaginatedResult<Configuration_RewardRuleEntity>(
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
                throw new Exception($"Failed to get all reward rules: {ex.Message}", ex);
            }
        }

        public async Task<bool> IsActivityCodeExistsAsync(string activityCode, long? excludeId = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = _dbContext.Configuration_RewardRule
                    .AsNoTracking()
                    .Where(r => r.ActivityCode.ToLower() == activityCode.ToLower());

                if (excludeId.HasValue)
                {
                    query = query.Where(r => r.Id != excludeId.Value);
                }

                return await query.AnyAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to check activity code existence: {ex.Message}", ex);
            }
        }

        public async Task<bool> IsActivityNameExistsAsync(string activityName, long? excludeId = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = _dbContext.Configuration_RewardRule
                    .AsNoTracking()
                    .Where(r => r.ActivityName.ToLower() == activityName.ToLower());

                if (excludeId.HasValue)
                {
                    query = query.Where(r => r.Id != excludeId.Value);
                }

                return await query.AnyAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to check activity name existence: {ex.Message}", ex);
            }
        }
    }
}

