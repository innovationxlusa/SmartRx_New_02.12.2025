using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Commands.RewardRule
{
    public class CreateRewardRuleCommand : IRequest<RewardRulesDTO>
    {
        public string ActivityName { get; set; } = string.Empty;
        public string ActivityTaken { get; set; } = string.Empty;
        public RewardType RewardType { get; set; }
        public string RewardDetails { get; set; } = string.Empty;
        public bool IsDeductible { get; set; }
        public bool IsVisibleBenifit { get; set; }
        public double Points { get; set; }
        public long UserId { get; set; }
    }

    public class CreateRewardRuleCommandHandler : IRequestHandler<CreateRewardRuleCommand, RewardRulesDTO>
    {
        private readonly IRewardRuleRepository _rewardRuleRepository;

        public CreateRewardRuleCommandHandler(IRewardRuleRepository rewardRuleRepository)
        {
            _rewardRuleRepository = rewardRuleRepository;
        }

        public async Task<RewardRulesDTO> Handle(CreateRewardRuleCommand request, CancellationToken cancellationToken)
        {
            var responseResult = new RewardRulesDTO();

            // Validation: Check for null or empty ActivityName
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

            // Validation: Check ActivityName length
            if (request.ActivityName.Length > 250)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Activity name cannot exceed 250 characters"
                };
                return responseResult;
            }
         
            // Validation: Check for null or empty ActivityTaken
            if (string.IsNullOrWhiteSpace(request.ActivityTaken))
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Failed",
                    Message = "Activity taken is required"
                };
                return responseResult;
            }

            // Validation: Check for duplicate ActivityName
            if (await _rewardRuleRepository.IsActivityNameExistsAsync(request.ActivityName, null, cancellationToken))
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status409Conflict,
                    Status = "Failed",
                    Message = $"A reward rule with activity name '{request.ActivityName}' already exists"
                };
                return responseResult;
            }

            var rewardRule = new Configuration_RewardRuleEntity
            {               
                ActivityName = request.ActivityName,
                ActivityTaken = request.ActivityTaken,
                RewardType = request.RewardType,
                RewardDetails = request.RewardDetails,
                IsDeductible = request.IsDeductible,
                IsVisibleBenifit = request.IsVisibleBenifit,
                Points = request.Points,
                CreatedById = request.UserId,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            var newRewardRule = await _rewardRuleRepository.CreateRewardRuleAsync(rewardRule, cancellationToken);

            var dto = new RewardRuleDTO
            {
                Id = newRewardRule.Id,
                ActivityCode = newRewardRule.ActivityCode,
                ActivityName = newRewardRule.ActivityName,
                ActivityTaken = newRewardRule.ActivityTaken,
                RewardType = newRewardRule.RewardType,
                RewardDetails = newRewardRule.RewardDetails,
                IsDeductible = newRewardRule.IsDeductible,
                IsVisibleBenifit = newRewardRule.IsVisibleBenifit,
                Points = newRewardRule.Points,
                IsActive = newRewardRule.IsActive,
                CreatedById = newRewardRule.CreatedById,
                CreatedDate = newRewardRule.CreatedDate
            };

            responseResult.ApiResponseResult = new ApiResponseResult
            {
                Data = dto,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "Reward rule created successfully"
            };
            return responseResult;
        }
    }
}

