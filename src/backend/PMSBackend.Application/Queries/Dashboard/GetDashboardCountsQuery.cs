using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using System.Net.Sockets;

namespace PMSBackend.Application.Queries.Dashboard
{
    public class GetDashboardCountsQuery : IRequest<DashboardCountsDTO>
    {
        public long UserId { get; set; }
        public long? PatientId { get; set; }
    }

    public class GetDashboardCountsQueryHandler : IRequestHandler<GetDashboardCountsQuery, DashboardCountsDTO>
    {
        private readonly IDashboardRepository _dashboardRepository;

        public GetDashboardCountsQueryHandler(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardCountsDTO> Handle(GetDashboardCountsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dashboardCountsContract = await _dashboardRepository.GetDashboardSummaryCountAsync(request.UserId, request.PatientId, cancellationToken);

                // Convert contract to DTO
                var dashboardCountsDTO = new DashboardCountsDTO
                {                    
                    ApiResponseResult = new ApiResponseResult
                    {
                        Data = dashboardCountsContract,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "Dashboard counts retrieved successfully"
                    }
                };

                return dashboardCountsDTO;
            }
            catch (Exception ex)
            {
                return new DashboardCountsDTO
                {
                    ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Status = "Failed",
                        Message = "Failed to retrieve dashboard counts: " + ex.Message
                    }
                };
            }
        }
    }
}
