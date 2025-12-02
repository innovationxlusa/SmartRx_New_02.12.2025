using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Domain.Repositories
{
    public interface IDashboardRepository
    {
        Task<DashboardSummaryContract> GetDashboardSummaryAsync(long userId, long? patientId, CancellationToken cancellationToken);
        Task<DashboardCountsContract> GetDashboardSummaryCountAsync(long userId, long? patientId, CancellationToken cancellationToken);
        
    }
}