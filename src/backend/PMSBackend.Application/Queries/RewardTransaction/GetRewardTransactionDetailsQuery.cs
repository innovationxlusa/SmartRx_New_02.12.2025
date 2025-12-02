using MediatR;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.PatientReward;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.Queries.RewardTransaction
{
    public class GetRewardTransactionDetailsQuery : IRequest<ApiResponseResult>
    {
        public long UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Earned { get; set; }
        public bool? Consumed { get; set; }
        public long? PatientId { get; set; }
    }

    public class GetRewardTransactionDetailsQueryHandler : IRequestHandler<GetRewardTransactionDetailsQuery, ApiResponseResult>
    {
        private readonly IRewardTransactionRepository _rewardTransactionRepository;
        private readonly IMediator _mediator;

        public GetRewardTransactionDetailsQueryHandler(
            IRewardTransactionRepository rewardTransactionRepository,
            IMediator mediator)
        {
            _rewardTransactionRepository = rewardTransactionRepository;
            _mediator = mediator;
        }

        public async Task<ApiResponseResult> Handle(GetRewardTransactionDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var detailsList = await _rewardTransactionRepository.GetRewardTransactionDetailsByUserIdAsync(
                    request.UserId,
                    request.StartDate,
                    request.EndDate,
                    request.Earned,
                    request.Consumed,
                    cancellationToken);

                if (detailsList == null || !detailsList.Any())
                {
                    return new ApiResponseResult
                    {
                        Data = new List<RewardTransactionDetailDTO>(),
                        StatusCode = 200,
                        Status = "Success",
                        Message = $"No reward transaction details found for user {request.UserId}"
                    };
                }

                // Convert RewardTransactionDetailDTO to PatientRewardContract format
                var allContracts = new List<PatientRewardContract>();
                var conversionContracts = new List<PatientRewardContract>();

                foreach (var d in detailsList)
                {
                    if (d.RecordType == "Conversion")
                    {
                        // For conversions, create separate contracts for earned (ToType) and consumed (FromType)
                        // Similar to GetPatientRewardsByUserIdAndPatientId logic

                        // Create earned contract if ToType is Cashable or NonCashable
                        if (d.ToType == RewardType.Cashable || d.ToType == RewardType.Noncashable)
                        {
                            var earnedContract = new PatientRewardContract
                            {
                                Id = d.Id,
                                SmartRxMasterId = d.SmartRxMasterId,
                                PrescriptionId = d.PrescriptionId,
                                PatientId = d.PatientId ?? request.PatientId ?? 0,
                                RewardId = 0,
                                RewardName = d.ToType == RewardType.Noncashable ? "Cashable to NonCashable": d.ToType == RewardType.Cashable ? "Non-cashable to Cashable" : d.ToType == RewardType.Money ? "Cashable to Money":null,
                                BadgeId = 0,
                                BadgeName =d.BadgeName,
                                BadgeDescription = $"Converted from {d.FromTypeName} to {d.ToTypeName}",
                                PatientFirstName = null,
                                PatientLastName = null,
                                PatientCode = null,
                                Remarks = d.Remarks,
                                CreatedDate = d.CreatedDate,
                                ModifiedDate = null
                            };

                            if (d.ToType == RewardType.Cashable)
                            {
                                earnedContract.EarnedCashablePoints = (int)(d.CashablePoints ?? 0);
                                earnedContract.TotalCashablePoints = (int)(d.CashablePoints ?? 0);
                                earnedContract.TotalPoints = d.CashablePoints;
                            }
                            else if (d.ToType == RewardType.Noncashable)
                            {
                                earnedContract.EarnedNonCashablePoints = (int)(d.NonCashablePoints ?? 0);
                                earnedContract.TotalNonCashablePoints = (int)(d.NonCashablePoints ?? 0);
                                earnedContract.TotalPoints = d.NonCashablePoints;
                            }

                            allContracts.Add(earnedContract);
                            conversionContracts.Add(earnedContract);
                        }

                        // Create consumed contract if FromType is Cashable or NonCashable
                        if (d.FromType == RewardType.Cashable || d.FromType == RewardType.Noncashable)
                        {
                            var consumedContract = new PatientRewardContract
                            {
                                Id = d.Id,
                                SmartRxMasterId = d.SmartRxMasterId,
                                PrescriptionId = d.PrescriptionId,
                                PatientId = d.PatientId ?? request.PatientId ?? 0,
                                RewardId = 0,
                                RewardName = d.ToType == RewardType.Noncashable ? "Cashable to NonCashable" : d.ToType == RewardType.Cashable ? "Non-cashable to Cashable" : d.ToType == RewardType.Money ? "Cashable to Money" : null,
                                BadgeId = 0,
                                BadgeName = "Conversion",
                                BadgeDescription = $"Converted from {d.FromTypeName} to {d.ToTypeName}",
                                PatientFirstName = null,
                                PatientLastName = null,
                                PatientCode = null,
                                Remarks = d.Remarks,
                                CreatedDate = d.CreatedDate,
                                ModifiedDate = null
                            };

                            // For consumed, use the Amount value (same as earned side for simplicity)
                            // Note: In a real scenario, we'd use ConvertedPoints for consumed, but Amount is what's available in the DTO
                            var consumedAmount = d.CashablePoints ?? d.NonCashablePoints ?? d.CashedMoney ?? 0;

                            if (d.FromType == RewardType.Cashable)
                            {
                                consumedContract.ConsumedCashablePoints = (int)Math.Abs(consumedAmount);
                                consumedContract.TotalCashablePoints = -(int)Math.Abs(consumedAmount);
                                consumedContract.TotalPoints = -Math.Abs(consumedAmount);
                            }
                            else if (d.FromType == RewardType.Noncashable)
                            {
                                consumedContract.ConsumedNonCashablePoints = (int)Math.Abs(consumedAmount);
                                consumedContract.TotalNonCashablePoints = -(int)Math.Abs(consumedAmount);
                                consumedContract.TotalPoints = -Math.Abs(consumedAmount);
                            }

                            allContracts.Add(consumedContract);
                            conversionContracts.Add(consumedContract);
                        }
                    }
                    else
                    {
                        // Handle reward transactions
                        var contract = new PatientRewardContract
                        {
                            Id = d.Id,
                            SmartRxMasterId = d.SmartRxMasterId,
                            PrescriptionId = d.PrescriptionId,
                            PatientId = d.PatientId ?? request.PatientId ?? 0,
                            RewardId = d.RewardRuleId ?? 0,
                            RewardName = d.ActivityTaken,
                            BadgeId = d.BadgeId ?? 0,
                            BadgeName = d.BadgeName,
                            BadgeDescription = d.RewarDescription,
                            PatientFirstName = null,
                            PatientLastName = null,
                            PatientCode = null,
                            Remarks = d.Remarks,
                            CreatedDate = d.CreatedDate,
                            ModifiedDate = null
                        };

                        if (d.IsDeductPoints)
                        {
                            // Consumed points
                            if (d.NonCashablePoints.HasValue && d.NonCashablePoints.Value != 0)
                            {
                                contract.ConsumedNonCashablePoints = Math.Abs(d.NonCashablePoints.Value);
                                contract.TotalNonCashablePoints = -Math.Abs(d.NonCashablePoints.Value);
                            }
                            if (d.CashablePoints.HasValue && d.CashablePoints.Value != 0)
                            {
                                contract.ConsumedCashablePoints = Math.Abs(d.CashablePoints.Value);
                                contract.TotalCashablePoints = -Math.Abs(d.CashablePoints.Value);
                            }
                            if (d.CashedMoney.HasValue && d.CashedMoney.Value != 0)
                            {
                                contract.EncashedMoney = Math.Abs(d.CashedMoney.Value);
                                contract.TotalMoney = -Math.Abs(d.CashedMoney.Value);
                            }
                        }
                        else
                        {
                            // Earned points
                            if (d.NonCashablePoints.HasValue && d.NonCashablePoints.Value != 0)
                            {
                                contract.EarnedNonCashablePoints = d.NonCashablePoints.Value;
                                contract.TotalNonCashablePoints = d.NonCashablePoints.Value;
                            }
                            if (d.CashablePoints.HasValue && d.CashablePoints.Value != 0)
                            {
                                contract.EarnedCashablePoints = d.CashablePoints.Value;
                                contract.TotalCashablePoints = d.CashablePoints.Value;
                            }
                            if (d.CashedMoney.HasValue && d.CashedMoney.Value != 0)
                            {
                                contract.ConvertedCashableToMoney = d.CashedMoney.Value;
                                contract.TotalMoney = d.CashedMoney.Value;
                            }
                        }

                        // Calculate TotalPoints
                        contract.TotalPoints = contract.EarnedNonCashablePoints > 0 ? contract.EarnedNonCashablePoints
                            : contract.EarnedCashablePoints > 0 ? contract.EarnedCashablePoints
                            : contract.TotalMoney ?? 0;

                        allContracts.Add(contract);
                    }
                }

                // Split into Earned and Consumed
                // Get original transactions (excluding conversions)
                var originalTransactions = allContracts.Except(conversionContracts).ToList();
                var originalEarned = originalTransactions
                    .Where(c => c.EarnedNonCashablePoints > 0 || c.EarnedCashablePoints > 0 || (c.ConvertedCashableToMoney.HasValue && c.ConvertedCashableToMoney.Value > 0))
                    .ToList();
                var originalConsumed = originalTransactions
                    .Where(c => c.ConsumedNonCashablePoints > 0 || c.ConsumedCashablePoints > 0 || (c.EncashedMoney.HasValue && c.EncashedMoney.Value > 0))
                    .ToList();

                // Get conversion contracts for earned and consumed
                var conversionEarned = conversionContracts
                    .Where(c => c.EarnedNonCashablePoints > 0 || c.EarnedCashablePoints > 0)
                    .ToList();
                var conversionConsumed = conversionContracts
                    .Where(c => c.ConsumedNonCashablePoints > 0 || c.ConsumedCashablePoints > 0)
                    .ToList();

                // Combine original and conversion data
                var earnedList = originalEarned.Concat(conversionEarned).ToList();
                var consumedList = originalConsumed.Concat(conversionConsumed).ToList();

                // Get summary information from PatientReward summary API
                var patientRewardSummaryQuery = new GetPatientRewardsSummaryQuery
                {
                    UserId = request.UserId,
                    PatientId = request.PatientId
                };
                var summaryResult = await _mediator.Send(patientRewardSummaryQuery, cancellationToken);

                // Extract PatientRewardSummaryDTO from the ApiResponseResult
                PatientRewardSummaryDTO? summary = null;
                if (summaryResult?.StatusCode == 200 && summaryResult.Data != null)
                {
                    summary = summaryResult.Data as PatientRewardSummaryDTO;
                }

                // Create PatientRewardSplitResult similar to GetPatientRewardsByUserIdAndPatientId
                var splitResult = new PatientRewardSplitResult
                {
                    All = new PaginatedResult<PatientRewardContract>(
                        allContracts,
                        allContracts.Count,
                        1,
                        allContracts.Count > 0 ? allContracts.Count : 1,
                        "CreatedDate",
                        "desc",
                        null, null, null, null, null),
                    Earned = new PaginatedResult<PatientRewardContract>(
                        earnedList,
                        earnedList.Count,
                        1,
                        earnedList.Count > 0 ? earnedList.Count : 1,
                        "CreatedDate",
                        "desc",
                        null, null, null, null, null),
                    Consumed = new PaginatedResult<PatientRewardContract>(
                        consumedList,
                        consumedList.Count,
                        1,
                        consumedList.Count > 0 ? consumedList.Count : 1,
                        "CreatedDate",
                        "desc",
                        null, null, null, null, null)
                };

                // Return PatientRewardSplitResult directly, matching GetPatientRewardsByUserIdAndPatientId format
                return new ApiResponseResult
                {
                    Data = splitResult,
                    StatusCode = 200,
                    Status = "Success",
                    Message = $"Reward transaction details for user {request.UserId} retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving reward transaction details for user {request.UserId}: {ex.Message}"
                };
            }
        }
    }
}


