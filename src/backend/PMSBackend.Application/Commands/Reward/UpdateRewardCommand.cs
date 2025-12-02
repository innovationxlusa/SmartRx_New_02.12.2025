using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Commands.Reward
{
    public class UpdateRewardCommand : IRequest<RewardsDTO>
    {
        public long Id { get; set; }
        public long UserActivityId { get; set; }
        public string? RewardCode { get; set; }
        public string Title { get; set; }
        public string? Details { get; set; }
        public double? NonCashablePoints { get; set; }
        public bool IsCashable { get; set; }
        public double? CashablePoints { get; set; }
        public bool? IsCashedMoney { get; set; }
        public double? CashedMoney { get; set; }
        public bool IsVisibleToUser { get; set; }
        public bool? IsActive { get; set; }
        public long UserId { get; set; }
    }

    public class UpdateRewardCommandHandler : IRequestHandler<UpdateRewardCommand, RewardsDTO>
    {
        private readonly IRewardRepository _rewardRepository;

        public UpdateRewardCommandHandler(IRewardRepository rewardRepository)
        {
            _rewardRepository = rewardRepository;
        }

        public async Task<RewardsDTO> Handle(UpdateRewardCommand request, CancellationToken cancellationToken)
        {
            var responseResult = new RewardsDTO();

            // Validation: Check for null or empty title
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = "Reward title is required"
                };
                return responseResult;
            }

            // Validation: Check title length
            if (request.Title.Length > 250)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Reward title cannot exceed 250 characters"
                };
                return responseResult;
            }

            // Validation: Check details length
            if (!string.IsNullOrEmpty(request.Details) && request.Details.Length > 1000)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Reward details cannot exceed 1000 characters"
                };
                return responseResult;
            }

            // Validation: Check points are non-negative
            if (request.NonCashablePoints.HasValue && request.NonCashablePoints.Value < 0)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Non-cashable points cannot be negative"
                };
                return responseResult;
            }

            if (request.IsCashable && request.CashablePoints.HasValue && request.CashablePoints.Value < 0)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Cashable points cannot be negative"
                };
                return responseResult;
            }

            // Validation: Check if reward exists
            var existingReward = await _rewardRepository.GetRewardByIdAsync(request.Id, cancellationToken);
            if (existingReward == null)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = $"Reward with ID {request.Id} not found"
                };
                return responseResult;
            }

            // Validation: Check for duplicate title (excluding current reward)
            var allRewards = await _rewardRepository.GetAllRewardsAsync(
                new Domain.CommonDTO.PagingSortingParams { PageNumber = 1, PageSize = int.MaxValue, SortBy = "Title", SortDirection = "asc" },
                cancellationToken);

            var duplicateExists = allRewards.Data.Any(r =>
                r.Id != request.Id &&
                r.Title.ToLower() == request.Title.ToLower());

            if (duplicateExists)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = $"A reward with the title '{request.Title}' already exists"
                };
                return responseResult;
            }

            // Get IsNegativePointAllowed value from Configuration_Settings
            var settings = await _rewardRepository.GetRewardSettingsAsync(cancellationToken);
            bool isNegativePointAllowed = settings != null && settings.IsRewardNegetivePointAllowed;

            var reward = new Configuration_Reward
            {
                Id = request.Id,
                UserActivityId = request.UserActivityId,
                RewardCode = request.RewardCode ?? existingReward.RewardCode,
                Title = request.Title,
                Details = request.Details,
                IsDeduction = isNegativePointAllowed,
                NonCashablePoints = request.NonCashablePoints,
                IsCashable = request.IsCashable,
                CashablePoints = request.CashablePoints,
                IsCashedMoney = request.IsCashedMoney,
                CashedMoney = request.CashedMoney,
                IsVisibleToUser = request.IsVisibleToUser,
                IsActive = request.IsActive,
                ModifiedById = request.UserId
            };

            var updatedReward = await _rewardRepository.UpdateRewardAsync(reward, cancellationToken);

            var dto = new RewardDTO
            {
                Id = updatedReward.Id,
                UserActivityId = updatedReward.UserActivityId,             
                RewardCode = updatedReward.RewardCode,
                Title = updatedReward.Title,
                Details = updatedReward.Details,
                IsDeduction = updatedReward.IsDeduction,
                NonCashablePoints = updatedReward.NonCashablePoints,
                IsCashable = updatedReward.IsCashable,
                CashablePoints = updatedReward.CashablePoints,
                IsCashedMoney = updatedReward.IsCashedMoney,
                CashedMoney = updatedReward.CashedMoney,
                IsVisibleToUser = updatedReward.IsVisibleToUser,
                CreatedById = updatedReward.CreatedById ?? 0,
                CreatedDate = updatedReward.CreatedDate,
                ModifiedById = updatedReward.ModifiedById,
                ModifiedDate = updatedReward.ModifiedDate,
                IsActive = updatedReward.IsActive
            };

            responseResult.ApiResponseResult = new ApiResponseResult
            {
                Data = dto,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "Reward updated successfully"
            };
            return responseResult;
        }
    }
}


