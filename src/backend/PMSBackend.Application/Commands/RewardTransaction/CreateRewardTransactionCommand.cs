using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Commands.RewardTransaction
{
    public class CreateRewardTransactionCommand : IRequest<RewardTransactionsDTO>
    {
        public long UserId { get; set; }
        public long? BadgeId { get; set; }
        public long RewardRuleId { get; set; }
        public RewardType RewardType { get; set; }
        public long? SmartRxMasterId { get; set; }
        public long? PrescriptionId { get; set; }
        public long? PatientId { get; set; }
        public bool IsDeductPoints { get; set; }
        public double AmountChanged { get; set; }
        public double NonCashableBalance { get; set; }
        public double CashableBalance { get; set; }
        public double CashedMoneyBalance { get; set; }
        public string? Remarks { get; set; }
        public long CreatedById { get; set; }
    }

    public class CreateRewardTransactionCommandHandler : IRequestHandler<CreateRewardTransactionCommand, RewardTransactionsDTO>
    {
        private readonly IRewardTransactionRepository _rewardTransactionRepository;
        private readonly IRewardRuleRepository _rewardRuleRepository;
        private readonly IRewardBadgeRepository _rewardBadgeRepository;

        public CreateRewardTransactionCommandHandler(
            IRewardTransactionRepository rewardTransactionRepository,
            IRewardRuleRepository rewardRuleRepository,
            IRewardBadgeRepository rewardBadgeRepository)
        {
            _rewardTransactionRepository = rewardTransactionRepository;
            _rewardRuleRepository = rewardRuleRepository;
            _rewardBadgeRepository = rewardBadgeRepository;
        }

        public async Task<RewardTransactionsDTO> Handle(CreateRewardTransactionCommand request, CancellationToken cancellationToken)
        {
            var responseResult = new RewardTransactionsDTO();

            if (request.UserId <= 0)
            {
                responseResult.ApiResponseResult = BuildError(StatusCodes.Status400BadRequest, "User ID must be greater than zero");
                return responseResult;
            }

            if (request.RewardRuleId <= 0)
            {
                responseResult.ApiResponseResult = BuildError(StatusCodes.Status400BadRequest, "Reward rule ID must be greater than zero");
                return responseResult;
            }

            if (!Enum.IsDefined(typeof(RewardType), request.RewardType))
            {
                responseResult.ApiResponseResult = BuildError(StatusCodes.Status400BadRequest, "Invalid reward type");
                return responseResult;
            }

            if (request.AmountChanged == 0)
            {
                responseResult.ApiResponseResult = BuildError(StatusCodes.Status400BadRequest, "Amount changed cannot be zero");
                return responseResult;
            }

            if (request.NonCashableBalance < 0 || request.CashableBalance < 0 || request.CashedMoneyBalance < 0)
            {
                responseResult.ApiResponseResult = BuildError(StatusCodes.Status400BadRequest, "Balance values cannot be negative");
                return responseResult;
            }

            var rewardRule = await _rewardRuleRepository.GetRewardRuleByIdAsync(request.RewardRuleId, cancellationToken);
            if (rewardRule == null)
            {
                responseResult.ApiResponseResult = BuildError(StatusCodes.Status404NotFound, $"Reward rule with ID {request.RewardRuleId} not found");
                return responseResult;
            }

            if (request.BadgeId.HasValue && request.BadgeId.Value > 0)
            {
                var badge = await _rewardBadgeRepository.GetRewardBadgeByIdAsync((int)request.BadgeId.Value, cancellationToken);
                if (badge == null)
                {
                    responseResult.ApiResponseResult = BuildError(StatusCodes.Status404NotFound, $"Badge with ID {request.BadgeId} not found");
                    return responseResult;
                }
            }

            var entity = new SmartRx_RewardTransactionEntity
            {
                UserId = request.UserId,
                BadgeId = request.BadgeId,
                RewardRuleId = request.RewardRuleId,
                RewardType = request.RewardType,
                SmartRxMasterId = request.SmartRxMasterId,
                PrescriptionId = request.PrescriptionId,
                PatientId = request.PatientId,
                IsDeductPoints = request.IsDeductPoints,
                AmountChanged = request.AmountChanged,
                NonCashableBalance = request.NonCashableBalance,
                CashableBalance = request.CashableBalance,
                CashedMoneyBalance = request.CashedMoneyBalance,
                Remarks = request.Remarks,
                CreatedById = request.CreatedById,
                CreatedDate = DateTime.UtcNow
            };

            var created = await _rewardTransactionRepository.CreateRewardTransactionAsync(entity, cancellationToken);

            var dto = created.ToDto();
            responseResult.RewardTransaction = dto;
            responseResult.ApiResponseResult = new ApiResponseResult
            {
                Data = dto,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "Reward transaction created successfully"
            };

            return responseResult;
        }

        private static ApiResponseResult BuildError(int statusCode, string message) =>
            new ApiResponseResult
            {
                Data = null,
                StatusCode = statusCode,
                Status = "Failed",
                Message = message
            };
    }
}

