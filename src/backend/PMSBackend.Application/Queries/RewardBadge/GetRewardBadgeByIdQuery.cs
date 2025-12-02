using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.RewardBadge
{
    public class GetRewardBadgeByIdQuery : IRequest<RewardBadgesDTO?>
    {
        public int Id { get; set; }
    }

    public class GetRewardBadgeByIdQueryHandler : IRequestHandler<GetRewardBadgeByIdQuery, RewardBadgesDTO?>
    {
        private readonly IRewardBadgeRepository _rewardBadgeRepository;

        public GetRewardBadgeByIdQueryHandler(IRewardBadgeRepository rewardBadgeRepository)
        {
            _rewardBadgeRepository = rewardBadgeRepository;
        }

        public async Task<RewardBadgesDTO?> Handle(GetRewardBadgeByIdQuery request, CancellationToken cancellationToken)
        {
            var responseResult = new RewardBadgesDTO();

            var result = await _rewardBadgeRepository.GetRewardBadgeByIdAsync(request.Id, cancellationToken);

            if (result == null)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = $"Reward badge with ID {request.Id} not found"
                };
                return responseResult;
            }           

            var dto= new RewardBadgeDTO
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                BadgeType = result.BadgeType,
                Heirarchy = result.Heirarchy,
                RequiredPoints = result.RequiredPoints,
                RequiredActivities = result.RequiredActivities,
                IsActive = result.IsActive
            };

            responseResult.ApiResponseResult = new ApiResponseResult
            {
                Data = dto,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "Reward badge data found"
            };
            return responseResult;
        }
    }
}

