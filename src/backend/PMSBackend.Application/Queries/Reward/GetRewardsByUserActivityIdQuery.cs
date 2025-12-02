using MediatR;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.Reward
{
    public class GetRewardsByUserActivityIdQuery : IRequest<ApiResponseResult>
    {
        public long UserActivityId { get; set; }
    }

    public class GetRewardsByUserActivityIdQueryHandler : IRequestHandler<GetRewardsByUserActivityIdQuery, ApiResponseResult>
    {
        private readonly IRewardRepository _rewardRepository;

        public GetRewardsByUserActivityIdQueryHandler(IRewardRepository rewardRepository)
        {
            _rewardRepository = rewardRepository;
        }

        public async Task<ApiResponseResult> Handle(GetRewardsByUserActivityIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var rewards = await _rewardRepository.GetRewardByUserActivityIdAsync(request.UserActivityId, cancellationToken);

                if (rewards == null || rewards.Count == 0)
                {
                    return new ApiResponseResult
                    {
                        Data = new List<RewardDTO>(),
                        StatusCode = 200,
                        Status = "Success",
                        Message = $"No rewards found for UserActivityId {request.UserActivityId}"
                    };
                }

                // Map to DTOs
                var dtos = rewards.Select(r => new RewardDTO
                {
                    Id = r.Id,
                    UserActivityId = r.UserActivityId,                    
                    RewardCode = r.RewardCode,
                    Title = r.Title,
                    Details = r.Details,
                    IsDeduction = r.IsDeduction,
                    NonCashablePoints = r.NonCashablePoints,
                    IsCashable = r.IsCashable,
                    CashablePoints = r.CashablePoints,
                    IsCashedMoney = r.IsCashedMoney,
                    CashedMoney = r.CashedMoney,
                    IsVisibleToUser = r.IsVisibleToUser,
                    CreatedById = r.CreatedById ?? 0,
                    CreatedDate = r.CreatedDate,
                    ModifiedById = r.ModifiedById,
                    ModifiedDate = r.ModifiedDate,
                    IsActive = r.IsActive
                }).ToList();

                return new ApiResponseResult
                {
                    Data = dtos,
                    StatusCode = 200,
                    Status = "Success",
                    Message = $"Rewards for UserActivityId {request.UserActivityId} retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving rewards by UserActivityId: {ex.Message}"
                };
            }
        }
    }
}

