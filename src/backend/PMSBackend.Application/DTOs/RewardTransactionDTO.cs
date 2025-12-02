using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.Entities;
using System;

namespace PMSBackend.Application.DTOs
{
    public class RewardTransactionDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public long? BadgeId { get; set; }
        public string? BadgeName { get; set; }
        public long RewardRuleId { get; set; }
        public string? RewardRuleName { get; set; }
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
        public DateTime? CreatedDate { get; set; }
        public long? CreatedById { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class RewardTransactionsDTO
    {
        public RewardTransactionDTO? RewardTransaction { get; set; }
        public ApiResponseResult? ApiResponseResult { get; set; }
    }

    public static class RewardTransactionMapper
    {
        public static RewardTransactionDTO ToDto(this SmartRx_RewardTransactionEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new RewardTransactionDTO
            {
                Id = entity.Id,
                UserId = entity.UserId,
                UserName = "",// entity.User?.FullName,
                BadgeId = entity.BadgeId,
                BadgeName = entity.RewardBadge?.Name,
                RewardRuleId = entity.RewardRuleId,
                RewardRuleName = entity.RewardRule?.ActivityName,
                RewardType = entity.RewardType,
                SmartRxMasterId = entity.SmartRxMasterId,
                PrescriptionId = entity.PrescriptionId,
                PatientId = entity.PatientId,
                IsDeductPoints = entity.IsDeductPoints,
                AmountChanged = entity.AmountChanged,
                NonCashableBalance = entity.NonCashableBalance,
                CashableBalance = entity.CashableBalance,
                CashedMoneyBalance = entity.CashedMoneyBalance,
                Remarks = entity.Remarks,
                CreatedDate = entity.CreatedDate,
                CreatedById = entity.CreatedById,
                ModifiedById = entity.ModifiedById,
                ModifiedDate = entity.ModifiedDate
            };
        }
    }
}

