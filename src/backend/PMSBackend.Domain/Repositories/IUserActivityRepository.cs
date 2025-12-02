using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.SharedContract;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Domain.Repositories
{
    public interface IUserActivityRepository
    {
        Task<Configuration_UserActivityEntity> CreateUserActivityAsync(Configuration_UserActivityEntity userActivity, CancellationToken cancellationToken);
        Task<Configuration_UserActivityEntity?> GetUserActivityByIdAsync(long id, CancellationToken cancellationToken);
        Task<Configuration_UserActivityEntity?> GetUserActivityByTitleAsync(string activityName, CancellationToken cancellationToken);
        Task<PaginatedResult<Configuration_UserActivityEntity>> GetAllUserActivitiesAsync(PagingSortingParams pagingSorting, CancellationToken cancellationToken);
        Task<bool> IsActivityNameExistsAsync(string activityName, long? excludeId = null, CancellationToken cancellationToken = default);
    }
}

