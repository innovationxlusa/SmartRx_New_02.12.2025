using MediatR;
using PMSBackend.Application.DTOs;

namespace PMSBackend.Application.Queries.Dashboard
{
    public class GetDashboardSummaryQuery : IRequest<DashboardSummaryDTO>
    {
        public long UserId { get; set; }
        public long? PatientId { get; set; }
    }
}


