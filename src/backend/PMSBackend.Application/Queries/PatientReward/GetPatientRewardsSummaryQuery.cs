using MediatR;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.PatientReward
{
    public class GetPatientRewardsSummaryQuery : IRequest<ApiResponseResult>
    {
        public long UserId { get; set; }
        public long? PatientId { get; set; }
    }

    public class GetPatientRewardsSummaryQueryHandler 
        : IRequestHandler<GetPatientRewardsSummaryQuery, ApiResponseResult>
    {
        private readonly IPatientRewardRepository _patientRewardRepository;

        public GetPatientRewardsSummaryQueryHandler(IPatientRewardRepository patientRewardRepository)
        {
            _patientRewardRepository = patientRewardRepository;
        }

        public async Task<ApiResponseResult> Handle(
            GetPatientRewardsSummaryQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _patientRewardRepository.GetPatientRewardsSummaryAsync(
                    request.UserId,
                    request.PatientId,
                    cancellationToken);

                if (result == null)
                {
                    return new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 404,
                        Status = "Failed",
                        Message = $"No reward summary found for user {request.UserId}"
                    };
                }

                // Map to DTO
                var responseDTO = new PatientRewardSummaryDTO
                {
                    UserId = result.UserId,
                    BadgeId = result.BadgeId,
                    BadgeName = result.BadgeName,
                    PatientId = result.PatientId,
                    
                    //// Initial Balances
                    //InitialCashableBalance = result.InitialCashableBalance,
                    //InitialNonCashableBalance = result.InitialNonCashableBalance,
                    //InitialMoneyBalance = result.InitialMoneyBalance,
                    
                    //// Conversions TO each type
                    //TotalConvertedToCashable = result.TotalConvertedToCashable,
                    //TotalConvertedToNonCashable = result.TotalConvertedToNonCashable,
                    //TotalConvertedToMoney = result.TotalConvertedToMoney,
                    
                    //// Deductions FROM each type
                    //TotalDeductedFromCashable = result.TotalDeductedFromCashable,
                    //TotalDeductedFromNonCashable = result.TotalDeductedFromNonCashable,
                    //TotalDeductedFromMoney = result.TotalDeductedFromMoney,
                    
                    //// Final Balances
                    //FinalCashableBalance = result.FinalCashableBalance,
                    //FinalNonCashableBalance = result.FinalNonCashableBalance,
                    //FinalMoneyBalance = result.FinalMoneyBalance,
                    
                    //// Net Conversion Effects
                    //NetCashableConversion = result.NetCashableConversion,
                    //NetNonCashableConversion = result.NetNonCashableConversion,
                    //NetMoneyConversion = result.NetMoneyConversion,
                    
                    // Legacy fields for backward compatibility
                    TotalEarnedNonCashablePoints = result.TotalEarnedNonCashablePoints,
                    TotalConsumedNonCashablePoints = result.TotalConsumedNonCashablePoints,
                    TotalNonCashablePoints = result.TotalNonCashablePoints,
                    TotalEarnedCashablePoints = result.TotalEarnedCashablePoints,
                    TotalConsumedCashablePoints = result.TotalConsumedCashablePoints,
                    TotalCashablePoints = result.TotalCashablePoints,
                    TotalEarnedMoney = result.TotalEarnedMoney,
                    TotalConsumedMoney = result.TotalConsumedMoney,
                    TotalMoney = result.TotalMoney,
                    TotalEncashMoney = result.TotalEncashMoney,
                    
                    GrandTotalConverted = result.GrandTotalConverted,
                    TotalConversionCount = result.TotalConversionCount
                };

                return new ApiResponseResult
                {
                    Data = responseDTO,
                    StatusCode = 200,
                    Status = "Success",
                    Message = $"Patient rewards summary for user {request.UserId} retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving patient rewards summary for user {request.UserId}: {ex.Message}"
                };
            }
        }
    }
}

