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
    public class UserRewardBadgeRepository : IUserRewardBadgeRepository
    {
        private readonly PMSDbContext _dbContext;

        public UserRewardBadgeRepository(PMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SmartRx_UserRewardBadgeEntity> CreateUserRewardBadgeAsync(
            SmartRx_UserRewardBadgeEntity userRewardBadge,
            CancellationToken cancellationToken)
        {
            try
            {
                if (userRewardBadge.EarnedDate == default)
                {
                    userRewardBadge.EarnedDate = DateTime.UtcNow;
                }

                await _dbContext.SmartRx_UserRewardBadge.AddAsync(userRewardBadge, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await _dbContext.Entry(userRewardBadge).Reference(urb => urb.Badge).LoadAsync(cancellationToken);

                return userRewardBadge;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create user reward badge: {ex.Message}", ex);
            }
        }

        public async Task<SmartRx_UserRewardBadgeEntity> UpdateUserRewardBadgeAsync(
            SmartRx_UserRewardBadgeEntity userRewardBadge,
            CancellationToken cancellationToken)
        {
            try
            {
                var existingBadge = await _dbContext.SmartRx_UserRewardBadge
                    .FirstOrDefaultAsync(urb => urb.Id == userRewardBadge.Id, cancellationToken);

                if (existingBadge == null)
                {
                    throw new Exception($"User reward badge with ID {userRewardBadge.Id} not found");
                }

                existingBadge.UserId = userRewardBadge.UserId;
                existingBadge.BadgeId = userRewardBadge.BadgeId;
                existingBadge.EarnedDate = userRewardBadge.EarnedDate;
                existingBadge.ModifiedById = userRewardBadge.ModifiedById;
                existingBadge.ModifiedDate = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync(cancellationToken);
                await _dbContext.Entry(existingBadge).Reference(urb => urb.Badge).LoadAsync(cancellationToken);

                return existingBadge;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update user reward badge: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteUserRewardBadgeAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                var badge = await _dbContext.SmartRx_UserRewardBadge
                    .FirstOrDefaultAsync(urb => urb.Id == id, cancellationToken);

                if (badge == null)
                {
                    return false;
                }

                _dbContext.SmartRx_UserRewardBadge.Remove(badge);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete user reward badge: {ex.Message}", ex);
            }
        }

        public async Task<SmartRx_UserRewardBadgeEntity?> GetUserRewardBadgeByIdAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.SmartRx_UserRewardBadge
                    .AsNoTracking()
                    .Include(urb => urb.Badge)
                    .FirstOrDefaultAsync(urb => urb.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get user reward badge by ID: {ex.Message}", ex);
            }
        }

        public async Task<PaginatedResult<SmartRx_UserRewardBadgeEntity>> GetAllUserRewardBadgesAsync(
            PagingSortingParams pagingSorting,
            CancellationToken cancellationToken)
        {
            try
            {
                if (pagingSorting.PageNumber <= 0) pagingSorting.PageNumber = 1;
                if (pagingSorting.PageSize <= 0) pagingSorting.PageSize = 10;

                var query = _dbContext.SmartRx_UserRewardBadge
                    .AsNoTracking()
                    .Include(urb => urb.Badge)
                    .AsQueryable();

                IQueryable<SmartRx_UserRewardBadgeEntity> sortedQuery = pagingSorting.SortBy?.ToLower() switch
                {
                    "userid" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(urb => urb.UserId)
                        : query.OrderBy(urb => urb.UserId),
                    "badgeid" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(urb => urb.BadgeId)
                        : query.OrderBy(urb => urb.BadgeId),
                    "earneddate" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(urb => urb.EarnedDate)
                        : query.OrderBy(urb => urb.EarnedDate),
                    _ => query.OrderByDescending(urb => urb.CreatedDate)
                };

                var totalRecords = await sortedQuery.CountAsync(cancellationToken);

                var pagedData = await sortedQuery
                    .Skip((pagingSorting.PageNumber - 1) * pagingSorting.PageSize)
                    .Take(pagingSorting.PageSize)
                    .ToListAsync(cancellationToken);

                return new PaginatedResult<SmartRx_UserRewardBadgeEntity>(
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
                throw new Exception($"Failed to get all user reward badges: {ex.Message}", ex);
            }
        }

        public async Task<bool> IsBadgeAlreadyAssignedAsync(
            long userId,
            long badgeId,
            long? excludeId = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var query = _dbContext.SmartRx_UserRewardBadge
                    .AsNoTracking()
                    .Where(urb => urb.UserId == userId && urb.BadgeId == badgeId);

                if (excludeId.HasValue)
                {
                    query = query.Where(urb => urb.Id != excludeId.Value);
                }

                return await query.AnyAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to check user reward badge duplication: {ex.Message}", ex);
            }
        }

        public async Task<SmartRx_UserRewardBadgeEntity?> GetLatestUserRewardBadgeAsync(
            long userId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.SmartRx_UserRewardBadge
                    .AsNoTracking()
                    .Include(urb => urb.Badge)
                    .Where(urb => urb.UserId == userId)
                    .OrderByDescending(urb => urb.EarnedDate)
                    .ThenByDescending(urb => urb.CreatedDate)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get latest user reward badge: {ex.Message}", ex);
            }
        }
    }
}

