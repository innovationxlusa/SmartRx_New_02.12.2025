namespace PMSBackend.Domain.SharedContract
{
    public class DiagnosticCenterWiseTestContract
    {
        public long SmartRxId { get; set; }
        public long PrescriptionId { get; set; }

        public long InvestigationId { get; set; }
        public string DiagnosticTestCenterCode { get; set; }
        public long DiagnosticTestCenterId { get; set; }
        public string DiagnosticTestCenterName { get; set; }
        public string? DiagnosticTestCenterDescription { get; set; }
        public string DiagnosticTestCenterLocation { get; set; }
        public string? DiagnosticTestCenterIcon { get; set; }
        public string? DiagnosticTestCenterShortName { get; set; }
        public string? DiagnosticTestCenterAddress { get; set; }

        //public long? DiagnosticTestCenterBranchId { get; set; }
        public string? DiagnosticTestCenterBranchName { get; set; }
        //public string? DiagnosticTestCenterBranchLocation { get; set; }
        //public string? DiagnosticTestCenterBranchAddress { get; set; }


        public long? DiagnosticTestCenterCityId { get; set; }
        public string? DiagnosticTestCenterCityCode { get; set; }
        public string? DiagnosticTestCenterCityName { get; set; }


        //public long? DiagnosticTestCenterBranchCityId { get; set; }
        //public string? DiagnosticTestCenterBranchCityCode { get; set; }
        //public string? DiagnosticTestCenterBranchCityName { get; set; }



        public long? DiagnosticTestCenterDistrictId { get; set; }
        public string? DiagnosticTestCenterDistrictCode { get; set; }
        public string? DiagnosticTestCenterDistrictName { get; set; }


        //public long? DiagnosticTestCenterBranchDistrictId { get; set; }
        //public string? DiagnosticTestCenterBranchDistrictCode { get; set; }
        //public string? DiagnosticTestCenterBranchDistrictName { get; set; }



        public long? DiagnosticTestCenterDivisionId { get; set; }
        //public long? DiagnosticTestCenterBranchDivisionId { get; set; }


        public long? DiagnosticTestCenterCountryId { get; set; }
        public string? DiagnosticTestCenterCountryCode { get; set; }
        public string? DiagnosticTestCenterCountryName { get; set; }


        //public long? DiagnosticTestCenterBranchCountryId { get; set; }
        //public string? DiagnosticTestCenterBranchCountryCode { get; set; }
        //public string? DiagnosticTestCenterBranchCountryName { get; set; }


        public string? DiagnosticTestCenterYearEstablished { get; set; }
        //public string? DiagnosticTestCenterBranchYearEstablished { get; set; }


        public string? DiagnosticTestCenterGoogleRating { get; set; }
        //public string? DiagnosticTestCenterBranchGoogleRating { get; set; }


        public string DiagnosticTestCenterGoogleLocation { get; set; }
        //public string? DiagnosticTestCenterBranchGoogleLocation { get; set; }


        public bool DiagnosticTestCenterIsActive { get; set; } = false;
        //public bool DiagnosticTestCenterBranchIsActive { get; set; } = false;


        public long? DiagnosticTestCenterWiseTestId { get; set; }
        //public long? DiagnosticTestCenterBranchWiseTestId { get; set; }


        public decimal? DiagnosticeTestCenterWiseTestUnitPrice { get; set; }
        public string? DiagnosticeTestCenterTestUnitPriceMeasurementUnit { get; set; }

        public string? DiagnosticeTestCenterWiseTestPriceType { get; set; }

        public long DiagnosticeTestCenterTestId { get; set; }
        public string? DiagnosticeTestCenterTestCode { get; set; }
        public string? DiagnosticeTestCenterTestName { get; set; }
        public string? DiagnosticeTestCenterTestDescription { get; set; }
        public string? DiagnosticeTestCenterTestFullName { get; set; }
        public string? DiagnosticeTestCenterTestShortName { get; set; }
        public string? TestNameByDiagnosticCenter { get; set; }
        public decimal? TestUnitPrice { get; set; }
        public string? TestPriceName { get; set; }
        public string? TestPriceMeasurementUnit { get; set; }
        public string? TestPriceType { get; set; }



        public bool? Wished { get; set; }
        public bool IsMinimum { get; set; } = false;
        public bool IsDoctorRecommended { get; set; } = false;
        public bool IsUserSelected { get; set; } = false;
    }
}
