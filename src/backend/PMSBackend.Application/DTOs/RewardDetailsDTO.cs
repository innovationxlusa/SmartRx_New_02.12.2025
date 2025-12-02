using System;

namespace PMSBackend.Application.DTOs
{
    public class RewardDetailsDTO
    {
        public long Id { get; set; }
        public string RecordType { get; set; } = string.Empty; // "PatientReward" or "Conversion"
        public DateTime? CreatedDate { get; set; }
        
        // Patient Reward specific fields
        public long? PatientRewardId { get; set; }
        public long? PatientId { get; set; }
        public string? PatientName { get; set; }
        public long? BadgeId { get; set; }
        public string? BadgeName { get; set; }
        public long? SmartRxMasterId { get; set; }
        public long? PrescriptionId { get; set; }
        public bool IsDeductPoints { get; set; }
        
        // Conversion specific fields
        public long? ConversionId { get; set; }
        public RewardType? FromType { get; set; }
        public string? FromTypeName { get; set; }
        public RewardType? ToType { get; set; }
        public string? ToTypeName { get; set; }
        
        // Points and Money values (with proper signs)
        // For PatientReward: Additions show as is, Deductions show as negative
        public double? EarnedNonCashablePoints { get; set; }
        public double? ConsumedNonCashablePoints { get; set; }
        public double? EarnedCashablePoints { get; set; }
        public double? ConsumedCashablePoints { get; set; }
        public double? ConvertedCashableToMoney { get; set; }
        public double? EncashedMoney { get; set; }
        
        // For Conversions: Show both deduction (FromType) and addition (ToType)
        public double? ConversionAmount { get; set; } // Positive for addition (ToType), negative for deduction (FromType)
        public double? ConversionDeductionAmount { get; set; } // Always negative (FromType)
        public double? ConversionAdditionAmount { get; set; } // Always positive (ToType)
        
        public string? Remarks { get; set; }
    }
}

