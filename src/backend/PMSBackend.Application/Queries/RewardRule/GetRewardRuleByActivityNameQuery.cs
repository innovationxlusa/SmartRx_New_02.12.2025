using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.RewardRule
{
    public class GetRewardRuleByActivityNameQuery : IRequest<RewardRulesDTO?>
    {
        public string ActivityName { get; set; } = string.Empty;
    }

    public class GetRewardRuleByActivityNameQueryHandler : IRequestHandler<GetRewardRuleByActivityNameQuery, RewardRulesDTO?>
    {
        private readonly IRewardRuleRepository _rewardRuleRepository;

        public GetRewardRuleByActivityNameQueryHandler(IRewardRuleRepository rewardRuleRepository)
        {
            _rewardRuleRepository = rewardRuleRepository;
        }

        public async Task<RewardRulesDTO?> Handle(GetRewardRuleByActivityNameQuery request, CancellationToken cancellationToken)
        {
            var responseResult = new RewardRulesDTO();

            if (string.IsNullOrWhiteSpace(request.ActivityName))
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Failed",
                    Message = "Activity name is required"
                };
                return responseResult;
            }

            var rewardRule = await _rewardRuleRepository.GetRewardRuleByActivityNameAsync(request.ActivityName, cancellationToken);

            if (rewardRule == null)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = $"Reward rule with activity name '{request.ActivityName}' not found"
                };
                return responseResult;
            }

            var dto = new RewardRuleDTO
            {
                Id = rewardRule.Id,
                ActivityCode = rewardRule.ActivityCode,
                ActivityName = rewardRule.ActivityName,
                ActivityTaken = rewardRule.ActivityTaken,
                RewardType = rewardRule.RewardType,
                RewardDetails = rewardRule.RewardDetails,
                IsDeductible = rewardRule.IsDeductible,
                IsVisibleBenifit = rewardRule.IsVisibleBenifit,
                Points = rewardRule.Points,
                IsActive = rewardRule.IsActive,
                CreatedById = rewardRule.CreatedById,
                CreatedDate = rewardRule.CreatedDate,
                ModifiedById = rewardRule.ModifiedById,
                ModifiedDate = rewardRule.ModifiedDate
            };

            responseResult.RewardRule = dto;
            responseResult.ApiResponseResult = new ApiResponseResult
            {
                Data = dto,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "Reward rule retrieved successfully"
            };

            return responseResult;
        }
    }
}

