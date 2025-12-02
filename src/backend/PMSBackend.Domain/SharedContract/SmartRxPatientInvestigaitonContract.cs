using PMSBackend.Domain.Entities;

namespace PMSBackend.Domain.SharedContract
{
    public class SmartRxPatientInvestigaitonContract
    {
        public long Id { get; set; }
        public long SmartRxMasterId { get; set; }
        public long PresriptionId { get; set; }
        public long TestId { get; set; }
        public Configuration_InvestigationEntity InvestigationTest { get; set; }
        public long? DiagnosticCenterWiseTestId { get; set; }
        public Configuration_DiagnosisCenterWiseTestEntity? DiagnosticCenterWiseTest { get; set; }
        public long? TestCenterId { get; set; }
        public Configuration_HospitalEntity InvestigationTestCenter { get; set; }
        public decimal? DiscountByAuthority { get; set; }
        public decimal TestPrice { get; set; }
        public DateTime? TestDate { get; set; }
        public string? Result { get; set; }
        public string? Remarks { get; set; }
        public bool? IsCompleted { get; set; }
        public string? WishListType { get; set; }

        public long? PatientRecommendedTestCenterId { get; set; }
        public Configuration_HospitalEntity? RecommendedTestCenter { get; set; }
    }
}
