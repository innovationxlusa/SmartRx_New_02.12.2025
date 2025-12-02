using System;
using System.Collections.Generic;
using System.Linq;
using PMSBackend.Domain.Entities;

namespace PMSBackend.Domain.SharedContract
{
    public class RewardTransactionResult
    {
        public bool IsRewardUpdated { get; set; } = false;
        public string? RewardTitle { get; set; }
        public double Points { get; set; }
        public string? RewardMessage { get; set; }
    }

    public class RewardTransactionSummaryContract
    {
        public long UserId { get; set; }
        
        // Initial Balances (from RewardTransaction table)
        public double InitialCashableBalance { get; set; }
        public double InitialNonCashableBalance { get; set; }
        public double InitialMoneyBalance { get; set; }
        
        // Conversions TO each type (where ToType == target type)
        public double TotalConvertedToCashable { get; set; }
        public double TotalConvertedToNonCashable { get; set; }
        public double TotalConvertedToMoney { get; set; }
        
        // Deductions FROM each type (where FromType == source type)
        public double TotalDeductedFromCashable { get; set; }
        public double TotalDeductedFromNonCashable { get; set; }
        public double TotalDeductedFromMoney { get; set; }
        
        // Final Balances (Initial + Converted TO - Deducted FROM)
        public double FinalCashableBalance { get; set; }
        public double FinalNonCashableBalance { get; set; }
        public double FinalMoneyBalance { get; set; }
        
        // Net Conversion Effects (Converted TO - Deducted FROM)
        public double NetCashableConversion { get; set; }
        public double NetNonCashableConversion { get; set; }
        public double NetMoneyConversion { get; set; }
        
        public double GrandTotalConverted { get; set; }
        public int TotalConversionCount { get; set; } = 0;
        public int TotalTransactionCount { get; set; } = 0;
        
        // Additional summary properties
        public double TotalPoint { get; set; } // noncashable + cashable + encashed points
        public double TotalNonCashable { get; set; }
        public double TotalCashable { get; set; }
        public double EarnedTotalNonCashable { get; set; }
        public double EarnedTotalCashable { get; set; }
        public double EarnedTotalCashed { get; set; }        
        public double EarnedTotalMoney { get; set; }


        public double TotalEarnedPoints { get; set; }
        public double TotalConsumedPoints { get; set; }
        public double TotalMoney { get; set; }


        // Badge Information
        public long? BadgeId { get; set; }
        public string? BadgeName { get; set; }
    }

    public class RewardTransactionDetailContract
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

    public class RewardTransactionQueryResult
    {
        public IQueryable<SmartRx_RewardTransactionEntity> All { get; set; }
        public IQueryable<SmartRx_RewardTransactionEntity> Earned { get; set; }
        public IQueryable<SmartRx_RewardTransactionEntity> Consumed { get; set; }
    }
}


