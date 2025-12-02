using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Domain.SharedContract
{
    public class RewardPointConversionsSummaryContract
    {
        public long UserId { get; set; }
        
        // Initial Balances (from user's reward system)
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
    }
}
