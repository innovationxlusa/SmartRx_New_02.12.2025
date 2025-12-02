using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.UserRewardBadge
{
    public class GetUserRewardBadgeByIdQuery : IRequest<UserRewardBadgesDTO?>
    {
        public long Id { get; set; }
    }

    public class GetUserRewardBadgeByIdQueryHandler : IRequestHandler<GetUserRewardBadgeByIdQuery, UserRewardBadgesDTO?>
    {
        private readonly IUserRewardBadgeRepository _userRewardBadgeRepository;

        public GetUserRewardBadgeByIdQueryHandler(IUserRewardBadgeRepository userRewardBadgeRepository)
        {
            _userRewardBadgeRepository = userRewardBadgeRepository;
        }

        public async Task<UserRewardBadgesDTO?> Handle(GetUserRewardBadgeByIdQuery request, CancellationToken cancellationToken)
        {
            var responseResult = new UserRewardBadgesDTO();

            var result = await _userRewardBadgeRepository.GetUserRewardBadgeByIdAsync(request.Id, cancellationToken);

            if (result == null)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = $"User reward badge with ID {request.Id} not found"
                };
                return responseResult;
            }

            var dto = result.ToDto();
            responseResult.UserRewardBadge = dto;
            responseResult.ApiResponseResult = new ApiResponseResult
            {
                Data = dto,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "User reward badge retrieved successfully"
            };

            return responseResult;
        }
    }
}

