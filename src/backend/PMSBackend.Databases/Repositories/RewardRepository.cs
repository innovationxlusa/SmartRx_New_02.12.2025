using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Databases.Services;
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
    public class RewardRepository : IRewardRepository
    {
        private readonly PMSDbContext _dbContext;
        private readonly ICodeGenerationService _codeGenerationService;

        public RewardRepository(PMSDbContext dbContext, ICodeGenerationService codeGenerationService)
        {
            _dbContext = dbContext;
            _codeGenerationService = codeGenerationService;
        }

        public async Task<Configuration_Reward> CreateRewardAsync(Configuration_Reward reward, CancellationToken cancellationToken)
        {
            try
            {
                reward.CreatedDate = DateTime.UtcNow;
                reward.IsActive = true;

                await _dbContext.Configuration_Reward.AddAsync(reward, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                // Reload with UserActivity navigation property
                return await _dbContext.Configuration_Reward
                    .Include(r => r.UserActivity)
                    .FirstOrDefaultAsync(r => r.Id == reward.Id, cancellationToken) ?? reward;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create reward: {ex.Message}", ex);
            }
        }

        public async Task<Configuration_Reward> UpdateRewardAsync(Configuration_Reward reward, CancellationToken cancellationToken)
        {
            try
            {
                var existingReward = await _dbContext.Configuration_Reward
                    .FirstOrDefaultAsync(r => r.Id == reward.Id, cancellationToken);

                if (existingReward == null)
                {
                    throw new Exception($"Reward with ID {reward.Id} not found");
                }

                // Update fields
                existingReward.UserActivityId = reward.UserActivityId;
                if (!string.IsNullOrWhiteSpace(reward.RewardCode))
                {
                    existingReward.RewardCode = reward.RewardCode;
                }
                existingReward.Title = reward.Title;
                existingReward.Details = reward.Details;
                existingReward.IsDeduction = reward.IsDeduction;
                existingReward.NonCashablePoints = reward.NonCashablePoints;
                existingReward.IsCashable = reward.IsCashable;
                existingReward.CashablePoints = reward.CashablePoints;
                existingReward.IsCashedMoney = reward.IsCashedMoney;
                existingReward.CashedMoney = reward.CashedMoney;
                existingReward.IsVisibleToUser = reward.IsVisibleToUser;
                existingReward.IsActive = reward.IsActive;
                existingReward.ModifiedById = reward.ModifiedById;
                existingReward.ModifiedDate = DateTime.UtcNow;

                _dbContext.Configuration_Reward.Update(existingReward);
                await _dbContext.SaveChangesAsync(cancellationToken);

                // Reload with UserActivity navigation property
                return await _dbContext.Configuration_Reward
                    .Include(r => r.UserActivity)
                    .FirstOrDefaultAsync(r => r.Id == reward.Id, cancellationToken) ?? existingReward;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update reward: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteRewardAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                var reward = await _dbContext.Configuration_Reward
                    .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

                if (reward == null)
                {
                    return false;
                }

                _dbContext.Configuration_Reward.Remove(reward);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete reward: {ex.Message}", ex);
            }
        }

        public async Task<Configuration_Reward?> GetRewardByIdAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.Configuration_Reward
                    .AsNoTracking()
                    .Include(r => r.UserActivity)
                    .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get reward by ID: {ex.Message}", ex);
            }
        }

        public async Task<IList<Configuration_Reward>> GetRewardByUserActivityIdAsync(long userActivityId, CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.Configuration_Reward
                    .AsNoTracking()
                    .Include(r => r.UserActivity)
                    .Where(r => r.UserActivityId == userActivityId)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get reward by UserActivityId: {ex.Message}", ex);
            }
        }

        public async Task<PaginatedResult<Configuration_Reward>> GetAllRewardsAsync(
            PagingSortingParams pagingSorting,
            CancellationToken cancellationToken)
        {
            try
            {
                if (pagingSorting.PageNumber <= 0) pagingSorting.PageNumber = 1;
                if (pagingSorting.PageSize <= 0) pagingSorting.PageSize = 10;

                var query = _dbContext.Configuration_Reward
                    .AsNoTracking()
                    .Include(r => r.UserActivity)
                   // .Where(r => r.IsActive == true)
                    .AsQueryable();

                // Apply sorting
                IQueryable<Configuration_Reward> sortedQuery = pagingSorting.SortBy?.ToLower() switch
                {
                    "heading" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(r => r.Title)
                        : query.OrderBy(r => r.Title),
                    "noncashablepoints" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(r => r.NonCashablePoints)
                        : query.OrderBy(r => r.NonCashablePoints),
                    "cashablepoints" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(r => r.CashablePoints)
                        : query.OrderBy(r => r.CashablePoints),
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

                return new PaginatedResult<Configuration_Reward>(
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
                throw new Exception($"Failed to get all rewards: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the reward settings from Configuration_Settings table
        /// </summary>
        public async Task<Configuration_Settings?> GetRewardSettingsAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.Configuration_Settings
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get reward settings: {ex.Message}", ex);
            }
        }
    }
}

