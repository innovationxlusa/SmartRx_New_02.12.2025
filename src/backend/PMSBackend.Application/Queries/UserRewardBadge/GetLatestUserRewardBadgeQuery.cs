using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.UserRewardBadge
{
    public class GetLatestUserRewardBadgeQuery : IRequest<UserRewardBadgesDTO?>
    {
        public int UserId { get; set; }
    }

    public class GetLatestUserRewardBadgeQueryHandler : IRequestHandler<GetLatestUserRewardBadgeQuery, UserRewardBadgesDTO?>
    {
        private readonly IUserRewardBadgeRepository _userRewardBadgeRepository;

        public GetLatestUserRewardBadgeQueryHandler(IUserRewardBadgeRepository userRewardBadgeRepository)
        {
            _userRewardBadgeRepository = userRewardBadgeRepository;
        }

        public async Task<UserRewardBadgesDTO?> Handle(GetLatestUserRewardBadgeQuery request, CancellationToken cancellationToken)
        {
            var responseResult = new UserRewardBadgesDTO();

            if (request.UserId <= 0)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Failed",
                    Message = "User ID must be greater than zero"
                };
                return responseResult;
            }

            var badge = await _userRewardBadgeRepository.GetLatestUserRewardBadgeAsync(request.UserId, cancellationToken);

            if (badge == null)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = $"No reward badge found for user {request.UserId}"
                };
                return responseResult;
            }

            var dto = badge.ToDto();
            responseResult.UserRewardBadge = dto;
            responseResult.ApiResponseResult = new ApiResponseResult
            {
                Data = dto,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "Latest user reward badge retrieved successfully"
            };

            return responseResult;
        }
    }
}

