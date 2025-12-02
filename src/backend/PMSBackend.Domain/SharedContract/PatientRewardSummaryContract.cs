using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Domain.SharedContract
{
    public class PatientRewardSummaryContract
    {
        public long UserId { get; set; }
        public long BadgeId { get; set; }
        public string BadgeName { get; set; }
        public long PatientId { get; set; }

        //// Initial Balances (from user's reward system)
        //public double InitialCashableBalance { get; set; }
        //public double InitialNonCashableBalance { get; set; }
        //public double InitialMoneyBalance { get; set; }

        //// Conversions TO each type (where ToType == target type)
        //public double TotalConvertedToCashable { get; set; }
        //public double TotalConvertedToNonCashable { get; set; }
        //public double TotalConvertedToMoney { get; set; }

        //// Deductions FROM each type (where FromType == source type)
        //public double TotalDeductedFromCashable { get; set; }
        //public double TotalDeductedFromNonCashable { get; set; }
        //public double TotalDeductedFromMoney { get; set; }

        //// Final Balances (Initial + Converted TO - Deducted FROM)
        //public double FinalCashableBalance { get; set; }
        //public double FinalNonCashableBalance { get; set; }
        //public double FinalMoneyBalance { get; set; }

        //// Net Conversion Effects (Converted TO - Deducted FROM)
        //public double NetCashableConversion { get; set; }
        //public double NetNonCashableConversion { get; set; }
        //public double NetMoneyConversion { get; set; }

        // Legacy fields for backward compatibility
        public double TotalEarnedNonCashablePoints { get; set; }
        public double TotalConsumedNonCashablePoints { get; set; }
        public double TotalNonCashablePoints { get; set; }
        public double TotalEarnedCashablePoints { get; set; }
        public double TotalConsumedCashablePoints { get; set; }
        public double TotalCashablePoints { get; set; }
        public double TotalEarnedMoney { get; set; }
        public double TotalConsumedMoney { get; set; }
        public double TotalMoney { get; set; }
        public double TotalEncashMoney { get; set; }

        public double GrandTotalConverted { get; set; }
        public int TotalConversionCount { get; set; }
    }
}
