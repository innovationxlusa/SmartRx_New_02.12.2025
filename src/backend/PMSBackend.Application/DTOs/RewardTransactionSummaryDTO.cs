namespace PMSBackend.Application.DTOs
{
    public class RewardTransactionSummaryDTO
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
        public double EarnedTotalCashed { get; set; }// total cashed point= total money earned
        public double EarnedTotalMoney { get; set; }// total cashed point= total money earned


        public double TotalEarnedPoints { get; set; }
        public double TotalConsumedPoints { get; set; }
        public double TotalMoney { get; set; }

        // Badge Information
        public long? BadgeId { get; set; }
        public string? BadgeName { get; set; }
    }
}

