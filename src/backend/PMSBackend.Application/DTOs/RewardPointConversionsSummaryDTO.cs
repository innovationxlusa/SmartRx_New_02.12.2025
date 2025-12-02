namespace PMSBackend.Application.DTOs
{
    public class RewardPointConversionsSummaryDTO
    {
        public long UserId { get; set; }
        
        // Initial Balances (from user's reward system)
        public decimal InitialCashableBalance { get; set; }
        public decimal InitialNonCashableBalance { get; set; }
        public decimal InitialMoneyBalance { get; set; }
        
        // Conversions TO each type (where ToType == target type)
        public decimal TotalConvertedToCashable { get; set; }
        public decimal TotalConvertedToNonCashable { get; set; }
        public decimal TotalConvertedToMoney { get; set; }
        
        // Deductions FROM each type (where FromType == source type)
        public decimal TotalDeductedFromCashable { get; set; }
        public decimal TotalDeductedFromNonCashable { get; set; }
        public decimal TotalDeductedFromMoney { get; set; }
        
        // Final Balances (Initial + Converted TO - Deducted FROM)
        public decimal FinalCashableBalance { get; set; }
        public decimal FinalNonCashableBalance { get; set; }
        public decimal FinalMoneyBalance { get; set; }
        
        // Net Conversion Effects (Converted TO - Deducted FROM)
        public decimal NetCashableConversion { get; set; }
        public decimal NetNonCashableConversion { get; set; }
        public decimal NetMoneyConversion { get; set; }
        
        public decimal GrandTotalConverted { get; set; }
        public int TotalConversionCount { get; set; }
    }

    public class RewardPointConversionsSummaryQuery
    {
        public long UserId { get; set; }
    }
}
