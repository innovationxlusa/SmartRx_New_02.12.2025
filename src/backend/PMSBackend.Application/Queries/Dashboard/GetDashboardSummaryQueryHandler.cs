using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.Dashboard
{
    public class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDTO>
    {
        private readonly IDashboardRepository _dashboardRepository;

        public GetDashboardSummaryQueryHandler(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardSummaryDTO> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
        {
            var contract = await _dashboardRepository.GetDashboardSummaryAsync(request.UserId, request.PatientId, cancellationToken);
            return new DashboardSummaryDTO
            {
                UserSummary = new DashboardUserSummaryDTO
                {
                    UserId = contract.UserSummary.UserId,
                    TotalPatients = contract.UserSummary.TotalPatients,
                    TotalDoctors = contract.UserSummary.TotalDoctors,
                    TotalRxFileOnly = contract.UserSummary.TotalRxFileOnly,
                    TotalSmartRx = contract.UserSummary.TotalSmartRx,
                    TotalPending = contract.UserSummary.TotalPending,
                    TotalEdex = contract.UserSummary.TotalEdex
                },
                ExpenseSummary = new DashboardExpenseSummaryDTO
                {
                    UserId = contract.ExpenseSummary.UserId,
                    TotalDoctorsCost = contract.ExpenseSummary.TotalDoctorsFee,
                    TotalMedicinesCost = contract.ExpenseSummary.TotalMedicinesCost,
                    TotalTestsCost = contract.ExpenseSummary.TotalTestsCost,                    
                    TotalTransportCost = contract.ExpenseSummary.TotalTransportCost,
                    TotalOtherCosts = contract.ExpenseSummary.TotalOtherCosts
                }
            };
        }
    }
}


