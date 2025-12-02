using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.Reward
{
    public class CreateRewardCommand : IRequest<RewardsDTO>
    {
        public long UserActivityId { get; set; }
        public string Title { get; set; }
        public string? Details { get; set; }
        public double? NonCashablePoints { get; set; }
        public bool IsCashable { get; set; }
        public double? CashablePoints { get; set; }
        public bool? IsCashedMoney { get; set; }
        public double? CashedMoney { get; set; }
        public bool IsVisibleToUser { get; set; }
        public long UserId { get; set; }
    }

    public class CreateRewardCommandHandler : IRequestHandler<CreateRewardCommand, RewardsDTO>
    {
        private readonly IRewardRepository _rewardRepository;
        private readonly ICodeGenerationService _codeGenerationService;

        public CreateRewardCommandHandler(IRewardRepository rewardRepository, ICodeGenerationService codeGenerationService)
        {
            _rewardRepository = rewardRepository;
            _codeGenerationService = codeGenerationService;
        }

        public async Task<RewardsDTO> Handle(CreateRewardCommand request, CancellationToken cancellationToken)
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

            // Validation: Check for duplicate title
            var allRewards = await _rewardRepository.GetAllRewardsAsync(
                new Domain.CommonDTO.PagingSortingParams { PageNumber = 1, PageSize = int.MaxValue, SortBy = "Title", SortDirection = "asc" },
                cancellationToken);

            var duplicateExists = allRewards.Data.Any(r =>
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

            // Generate RewardCode if not provided
         
            string rewardCode = await _codeGenerationService.GenerateRewardCodeAsync(cancellationToken);          

            // Get IsNegativePointAllowed value from Configuration_Settings
            var settings = await _rewardRepository.GetRewardSettingsAsync(cancellationToken);
            bool isNegativePointAllowed = settings != null && settings.IsRewardNegetivePointAllowed;

            var reward = new Configuration_Reward
            {
                UserActivityId = request.UserActivityId,
                RewardCode = rewardCode,
                Title = request.Title,
                Details = request.Details,
                IsDeduction = isNegativePointAllowed,
                NonCashablePoints = request.NonCashablePoints,
                IsCashable = request.IsCashable,
                CashablePoints = request.CashablePoints,
                IsCashedMoney = request.IsCashedMoney,
                CashedMoney = request.CashedMoney,
                IsVisibleToUser = request.IsVisibleToUser,
                CreatedById = request.UserId,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            var newReward = await _rewardRepository.CreateRewardAsync(reward, cancellationToken);

            var dto = new RewardDTO
            {
                Id = newReward.Id,
                UserActivityId = newReward.UserActivityId,
               
                RewardCode = newReward.RewardCode,
                Title = newReward.Title,
                Details = newReward.Details,
                IsDeduction = newReward.IsDeduction,
                NonCashablePoints = newReward.NonCashablePoints,
                IsCashable = newReward.IsCashable,
                CashablePoints = newReward.CashablePoints,
                IsCashedMoney = newReward.IsCashedMoney,
                CashedMoney = newReward.CashedMoney,
                IsVisibleToUser = newReward.IsVisibleToUser,
                CreatedById = newReward.CreatedById ?? 0,
                CreatedDate = newReward.CreatedDate,
                IsActive = newReward.IsActive
            };

            responseResult.ApiResponseResult = new ApiResponseResult
            {
                Data = dto,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "Reward created successfully"
            };
            return responseResult;
        }
    }
}

