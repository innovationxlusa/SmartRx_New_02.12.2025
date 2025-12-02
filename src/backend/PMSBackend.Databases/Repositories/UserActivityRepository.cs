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
    public class UserActivityRepository : IUserActivityRepository
    {
        private readonly PMSDbContext _dbContext;

        public UserActivityRepository(PMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Configuration_UserActivityEntity> CreateUserActivityAsync(Configuration_UserActivityEntity userActivity, CancellationToken cancellationToken)
        {
            try
            {
                userActivity.CreatedDate = DateTime.UtcNow;
                await _dbContext.Configuration_UserActivity.AddAsync(userActivity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return userActivity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create user activity: {ex.Message}", ex);
            }
        }

        public async Task<Configuration_UserActivityEntity?> GetUserActivityByIdAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.Configuration_UserActivity
                    .AsNoTracking()
                    .FirstOrDefaultAsync(ua => ua.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get user activity by ID: {ex.Message}", ex);
            }
        }

        public async Task<Configuration_UserActivityEntity?> GetUserActivityByTitleAsync(string activityName, CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.Configuration_UserActivity
                    .AsNoTracking()
                    .FirstOrDefaultAsync(ua => ua.ActivityName.ToLower() == activityName.ToLower(), cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get user activity by title: {ex.Message}", ex);
            }
        }
       
        public async Task<PaginatedResult<Configuration_UserActivityEntity>> GetAllUserActivitiesAsync(PagingSortingParams pagingSorting, CancellationToken cancellationToken)
        {
            try
            {
                if (pagingSorting.PageNumber <= 0) pagingSorting.PageNumber = 1;
                if (pagingSorting.PageSize <= 0) pagingSorting.PageSize = 10;

                var query = _dbContext.Configuration_UserActivity
                    .AsNoTracking()
                    .AsQueryable();

                // Apply sorting
                IQueryable<Configuration_UserActivityEntity> sortedQuery = pagingSorting.SortBy?.ToLower() switch
                {
                    "activityname" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(ua => ua.ActivityName)
                        : query.OrderBy(ua => ua.ActivityName),
                    "activitycode" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(ua => ua.ActivityCode)
                        : query.OrderBy(ua => ua.ActivityCode),
                    "activitypoint" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(ua => ua.ActivityPoint)
                        : query.OrderBy(ua => ua.ActivityPoint),
                    "createddate" => pagingSorting.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(ua => ua.CreatedDate)
                        : query.OrderBy(ua => ua.CreatedDate),
                    _ => query.OrderBy(ua => ua.ActivityCode)
                };

                var totalRecords = await sortedQuery.CountAsync(cancellationToken);

                var pagedData = await sortedQuery
                    .Skip((pagingSorting.PageNumber - 1) * pagingSorting.PageSize)
                    .Take(pagingSorting.PageSize)
                    .ToListAsync(cancellationToken);

                return new PaginatedResult<Configuration_UserActivityEntity>(
                    pagedData,
                    totalRecords,
                    pagingSorting.PageNumber,
                    pagingSorting.PageSize,
                    pagingSorting.SortBy ?? "ActivityCode",
                    pagingSorting.SortDirection ?? "asc",
                    null, null, null, null, null);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get user activities: {ex.Message}", ex);
            }
        }

        public async Task<bool> IsActivityNameExistsAsync(string activityName, long? excludeId = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = _dbContext.Configuration_UserActivity
                    .AsNoTracking()
                    .Where(ua => ua.ActivityName.ToLower() == activityName.ToLower());

                if (excludeId.HasValue)
                {
                    query = query.Where(ua => ua.Id != excludeId.Value);
                }

                return await query.AnyAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to check if activity name exists: {ex.Message}", ex);
            }
        }

      
    }
}

