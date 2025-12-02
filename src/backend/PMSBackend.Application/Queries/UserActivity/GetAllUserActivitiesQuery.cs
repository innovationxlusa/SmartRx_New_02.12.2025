using MediatR;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.UserActivity
{
    public class GetAllUserActivitiesQuery : IRequest<ApiResponseResult>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = "ActivityCode";
        public string? SortDirection { get; set; } = "asc";
    }

    public class GetAllUserActivitiesQueryHandler : IRequestHandler<GetAllUserActivitiesQuery, ApiResponseResult>
    {
        private readonly IUserActivityRepository _userActivityRepository;

        public GetAllUserActivitiesQueryHandler(IUserActivityRepository userActivityRepository)
        {
            _userActivityRepository = userActivityRepository;
        }

        public async Task<ApiResponseResult> Handle(GetAllUserActivitiesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var pagingParams = new PagingSortingParams
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    SortBy = request.SortBy ?? "ActivityCode",
                    SortDirection = request.SortDirection ?? "asc"
                };

                var result = await _userActivityRepository.GetAllUserActivitiesAsync(pagingParams, cancellationToken);

                // Map to DTOs
                var dtos = result.Data.Select(ua => new UserActivityDTO
                {
                    Id = ua.Id,
                    ActivityCode = ua.ActivityCode,
                    ActivityName = ua.ActivityName,
                    ActivityPoint = ua.ActivityPoint,
                    Remarks = ua.Remarks,
                    CreatedDate = ua.CreatedDate,
                    CreatedById = ua.CreatedById,
                    ModifiedDate = ua.ModifiedDate,
                    ModifiedById = ua.ModifiedById
                }).ToList();

                var paginatedResponse = new PaginatedResult<UserActivityDTO>(
                    dtos,
                    result.TotalRecords,
                    result.PageNumber,
                    result.PageSize,
                    result.SortBy,
                    result.SortDirection,
                    null, null, null, null, null);

                return new ApiResponseResult
                {
                    Data = paginatedResponse,
                    StatusCode = 200,
                    Status = "Success",
                    Message = "User activities retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving user activities: {ex.Message}"
                };
            }
        }
    }
}

