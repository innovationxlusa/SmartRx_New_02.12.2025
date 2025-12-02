using MediatR;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.RewardPointConversions
{
    public class GetRewardDetailsQuery : IRequest<ApiResponseResult>
    {
        public long UserId { get; set; }
    }

    public class GetRewardDetailsQueryHandler : IRequestHandler<GetRewardDetailsQuery, ApiResponseResult>
    {
        private readonly IPatientRewardRepository _patientRewardRepository;
        private readonly IRewardPointConversionsRepository _rewardPointConversionsRepository;

        public GetRewardDetailsQueryHandler(
            IPatientRewardRepository patientRewardRepository,
            IRewardPointConversionsRepository rewardPointConversionsRepository)
        {
            _patientRewardRepository = patientRewardRepository;
            _rewardPointConversionsRepository = rewardPointConversionsRepository;
        }

        public async Task<ApiResponseResult> Handle(GetRewardDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var detailsList = new List<RewardDetailsDTO>();

                // Get all patient rewards for the user
                var patientRewards = await _patientRewardRepository.GetPatientRewardsByUserIdAsync(
                    request.UserId,
                    new Domain.CommonDTO.PagingSortingParams { PageNumber = 1, PageSize = int.MaxValue, SortBy = "CreatedDate", SortDirection = "desc" },
                    cancellationToken);

                // Convert patient rewards to DTOs
                foreach (var reward in patientRewards.Data)
                {
                    var dto = new RewardDetailsDTO
                    {
                        Id = reward.Id,
                        RecordType = "PatientReward",
                        CreatedDate = reward.CreatedDate,
                        PatientRewardId = reward.Id,
                        PatientId = reward.PatientId,
                        PatientName = null, // PatientProfile not included in GetPatientRewardsByUserIdAsync, can be fetched separately if needed
                        BadgeId = reward.BadgeId,
                        BadgeName = reward.RewardBadge?.Name,
                        SmartRxMasterId = reward.SmartRxMasterId,
                        PrescriptionId = reward.PrescriptionId,
                        IsDeductPoints = reward.IsDeductPoints,
                        // Additions - show as is (EarnedNonCashablePoints is already negative if IsDeductPoints is true)
                        EarnedNonCashablePoints = reward.EarnedNonCashablePoints != 0 ? reward.EarnedNonCashablePoints : null,
                        EarnedCashablePoints = reward.EarnedCashablePoints != 0 ? reward.EarnedCashablePoints : null,
                        ConvertedCashableToMoney = reward.ConvertedCashableToMoney,
                        // Deductions - show with minus sign
                        ConsumedNonCashablePoints = reward.ConsumedNonCashablePoints != 0 ? -Math.Abs(reward.ConsumedNonCashablePoints) : null,
                        ConsumedCashablePoints = reward.ConsumedCashablePoints != 0 ? -Math.Abs(reward.ConsumedCashablePoints) : null,
                        EncashedMoney = reward.EncashedMoney.HasValue && reward.EncashedMoney.Value != 0 ? -Math.Abs(reward.EncashedMoney.Value) : null,
                        Remarks = reward.Remarks
                    };
                    detailsList.Add(dto);
                }

                // Get all conversions for the user
                var conversions = await _rewardPointConversionsRepository.GetByUserIdAsync(request.UserId);

                // Convert conversions to DTOs
                // For each conversion, show both deduction (FromType) and addition (ToType) in one entry
                foreach (var conversion in conversions)
                {
                    var dto = new RewardDetailsDTO
                    {
                        Id = conversion.Id,
                        RecordType = "Conversion",
                        CreatedDate = conversion.CreatedDate,
                        ConversionId = conversion.Id,
                        FromType = conversion.FromType,
                        FromTypeName = GetTypeName(conversion.FromType),
                        ToType = conversion.ToType,
                        ToTypeName = GetTypeName(conversion.ToType),
                        // Deduction amount - always negative (what was deducted FROM)
                        ConversionDeductionAmount = -Math.Abs(conversion.Amount),
                        // Addition amount - always positive (what was added TO)
                        ConversionAdditionAmount = conversion.Amount,
                        // Legacy field for backward compatibility - shows addition (positive)
                        ConversionAmount = conversion.Amount,
                        Remarks = $"Converted from {GetTypeName(conversion.FromType)} to {GetTypeName(conversion.ToType)}"
                    };
                    detailsList.Add(dto);
                }

                // Sort by CreatedDate descending (most recent first)
                detailsList = detailsList.OrderByDescending(d => d.CreatedDate).ToList();

                return new ApiResponseResult
                {
                    Data = detailsList,
                    StatusCode = 200,
                    Status = "Success",
                    Message = $"Reward details for user {request.UserId} retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving reward details for user {request.UserId}: {ex.Message}"
                };
            }
        }

        private string GetTypeName(RewardType type)
        {
            return type switch
            {
                RewardType.Noncashable => "Noncashable",
                RewardType.Cashable => "Cashable",
                RewardType.Money => "Money",
                _ => "Unknown"
            };
        }
    }
}

