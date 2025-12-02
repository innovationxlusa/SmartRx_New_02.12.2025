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
    public class UpdateRewardRuleCommand : IRequest<RewardRulesDTO>
    {
        public long Id { get; set; }
        public string ActivityName { get; set; } = string.Empty;
        public string ActivityTaken { get; set; } = string.Empty;
        public RewardType RewardType { get; set; }
        public string RewardDetails { get; set; } = string.Empty;
        public bool IsDeductible { get; set; }
        public bool IsVisibleBenifit { get; set; }
        public double Points { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
    }

    public class UpdateRewardRuleCommandHandler : IRequestHandler<UpdateRewardRuleCommand, RewardRulesDTO>
    {
        private readonly IRewardRuleRepository _rewardRuleRepository;

        public UpdateRewardRuleCommandHandler(IRewardRuleRepository rewardRuleRepository)
        {
            _rewardRuleRepository = rewardRuleRepository;
        }

        public async Task<RewardRulesDTO> Handle(UpdateRewardRuleCommand request, CancellationToken cancellationToken)
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

            // Validation: Check if reward rule exists
            var existingRule = await _rewardRuleRepository.GetRewardRuleByIdAsync(request.Id, cancellationToken);
            if (existingRule == null)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = $"Reward rule with ID {request.Id} not found"
                };
                return responseResult;
            }
            // Validation: Check for duplicate ActivityName (excluding current rule)
            if (await _rewardRuleRepository.IsActivityNameExistsAsync(request.ActivityName, request.Id, cancellationToken))
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
                Id = request.Id,               
                ActivityName = request.ActivityName,
                ActivityTaken = request.ActivityTaken,
                RewardType = request.RewardType,
                RewardDetails = request.RewardDetails,
                IsDeductible = request.IsDeductible,
                IsVisibleBenifit = request.IsVisibleBenifit,
                Points = request.Points,
                IsActive = request.IsActive,
                ModifiedById = request.UserId
            };

            var updatedRewardRule = await _rewardRuleRepository.UpdateRewardRuleAsync(rewardRule, cancellationToken);

            var dto = new RewardRuleDTO
            {
                Id = updatedRewardRule.Id,
                ActivityCode = updatedRewardRule.ActivityCode,
                ActivityName = updatedRewardRule.ActivityName,
                ActivityTaken = updatedRewardRule.ActivityTaken,
                RewardType = updatedRewardRule.RewardType,
                RewardDetails = updatedRewardRule.RewardDetails,
                IsDeductible = updatedRewardRule.IsDeductible,
                IsVisibleBenifit = updatedRewardRule.IsVisibleBenifit,
                Points = updatedRewardRule.Points,
                IsActive = updatedRewardRule.IsActive,
                CreatedById = updatedRewardRule.CreatedById,
                CreatedDate = updatedRewardRule.CreatedDate,
                ModifiedById = updatedRewardRule.ModifiedById,
                ModifiedDate = updatedRewardRule.ModifiedDate
            };

            responseResult.ApiResponseResult = new ApiResponseResult
            {
                Data = dto,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "Reward rule updated successfully"
            };
            return responseResult;
        }
    }
}

