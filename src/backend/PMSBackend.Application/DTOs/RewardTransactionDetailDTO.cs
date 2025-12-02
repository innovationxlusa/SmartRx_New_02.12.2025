using System;

namespace PMSBackend.Application.DTOs
{
    public class RewardTransactionDetailDTO
    {
        public long Id { get; set; }
        public string RecordType { get; set; } = string.Empty; // "RewardTransaction" or "Conversion"
        public DateTime? CreatedDate { get; set; }
        public string RewarDescription { get; set; }

        // RewardTransaction specific fields
        public long? RewardTransactionId { get; set; }
        public long? BadgeId { get; set; }
        public string? BadgeName { get; set; }
        public long? RewardRuleId { get; set; }
        public string? RewardRuleName { get; set; }
        public string? ActivityTaken { get; set; }
        public RewardType? RewardType { get; set; }
        public long? SmartRxMasterId { get; set; }
        public long? PrescriptionId { get; set; }
        public long? PatientId { get; set; }
        public string PatientName { get; set; }
        public bool IsDeductPoints { get; set; }
        public double? AmountChanged { get; set; }

        // Conversion specific fields
        public long? ConversionId { get; set; }
        public RewardType? FromType { get; set; }
        public string? FromTypeName { get; set; }
        public RewardType? ToType { get; set; }
        public string? ToTypeName { get; set; }

        // Points and Money values
        public double? NonCashablePoints { get; set; }
        public double? CashablePoints { get; set; }
        public double? CashedMoney { get; set; }

        public string? Remarks { get; set; }
    }
}

