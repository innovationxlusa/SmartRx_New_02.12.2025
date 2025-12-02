using MediatR;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.RewardTransaction
{
    public class GetRewardTransactionSummaryQuery : IRequest<ApiResponseResult>
    {
        public long UserId { get; set; }
    }

    public class GetRewardTransactionSummaryQueryHandler : IRequestHandler<GetRewardTransactionSummaryQuery, ApiResponseResult>
    {
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public GetRewardTransactionSummaryQueryHandler(IRewardTransactionRepository rewardTransactionRepository)
        {
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<ApiResponseResult> Handle(GetRewardTransactionSummaryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var summary = await _rewardTransactionRepository.GetRewardTransactionSummaryByUserIdAsync(request.UserId, cancellationToken);

                if (summary == null)
                {
                    return new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 404,
                        Status = "Failed",
                        Message = $"No reward transaction summary found for user {request.UserId}"
                    };
                }

                var summaryDto = new RewardTransactionSummaryDTO
                {
                    UserId = summary.UserId,
                    InitialCashableBalance = summary.InitialCashableBalance,
                    InitialNonCashableBalance = summary.InitialNonCashableBalance,
                    InitialMoneyBalance = summary.InitialMoneyBalance,
                    TotalConvertedToCashable = summary.TotalConvertedToCashable,
                    TotalConvertedToNonCashable = summary.TotalConvertedToNonCashable,
                    TotalConvertedToMoney = summary.TotalConvertedToMoney,
                    TotalDeductedFromCashable = summary.TotalDeductedFromCashable,
                    TotalDeductedFromNonCashable = summary.TotalDeductedFromNonCashable,
                    TotalDeductedFromMoney = summary.TotalDeductedFromMoney,
                    FinalCashableBalance = summary.FinalCashableBalance,
                    FinalNonCashableBalance = summary.FinalNonCashableBalance,
                    FinalMoneyBalance = summary.FinalMoneyBalance,
                    NetCashableConversion = summary.NetCashableConversion,
                    NetNonCashableConversion = summary.NetNonCashableConversion,
                    NetMoneyConversion = summary.NetMoneyConversion,
                    GrandTotalConverted = summary.GrandTotalConverted,
                    TotalConversionCount = summary.TotalConversionCount,
                    TotalTransactionCount = summary.TotalTransactionCount,
                    TotalPoint = summary.TotalPoint,
                    TotalNonCashable = summary.TotalNonCashable,
                    TotalCashable = summary.TotalCashable,
                    EarnedTotalNonCashable = summary.EarnedTotalNonCashable,
                    EarnedTotalCashable = summary.EarnedTotalCashable,
                    EarnedTotalCashed=summary.EarnedTotalCashed,
                    EarnedTotalMoney = summary.EarnedTotalMoney,
                    TotalEarnedPoints = summary.TotalEarnedPoints,
                    TotalConsumedPoints = summary.TotalConsumedPoints,
                    BadgeId = summary.BadgeId,
                    BadgeName = summary.BadgeName
                };

                return new ApiResponseResult
                {
                    Data = summaryDto,
                    StatusCode = 200,
                    Status = "Success",
                    Message = $"Reward transaction summary for user {request.UserId} retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving reward transaction summary for user {request.UserId}: {ex.Message}"
                };
            }
        }
    }
}

