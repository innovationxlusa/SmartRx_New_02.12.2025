using PMSBackend.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Application.DTOs
{
    public class SmartRxInvestigationListDTO
    {
        public long Id { get; set; }
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public long TestId { get; set; }
        [Column(TypeName = "nchar(5)")]
        public string? TestCode { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string? TestName { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? TestDescription { get; set; }
        [Column(TypeName = "nvarchar(2000)")]
        public string? TestFullName { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? TestShortName { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? TestNameByDiagnosticCenter { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal TestUnitPrice { get; set; } = 0;


        public long? PriceUnitId { get; set; }
        [Column(TypeName = "nchar(4)")]
        public string? TestPriceName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? TestPriceMeasurementUnit { get; set; }
        public string? TestPriceType { get; set; }



        [Column(TypeName = "decimal(5,2)")]
        public decimal? TestNationalUnitPrice { get; set; } = 0;
        public long? NationalPriceUnitId { get; set; }
        [Column(TypeName = "nchar(4)")]
        public string? NationalTestPriceName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? NationalTestPriceMeasurementUnit { get; set; }
        public string? NationalTestPriceType { get; set; }
        [Column(TypeName = "nvarchar(2000)")]
        public string? TestSpeciality { get; set; }//any special machine/setup/environment etc.
        [Column(TypeName = "nvarchar(2000)")]
        public string? TestComments { get; set; }


        public string? TestCenterIds { get; set; }
        public long? TestCenterId { get; set; }
        public List<Configuration_HospitalEntity>? TestCenters { get; set; }
        public string? TestCenterCode { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? TestCenterName { get; set; }

        [Column(TypeName = "nvarchar(4000)")]
        public string? TestCenterDescription { get; set; }

        [Column(TypeName = "nvarchar(2000)")]
        public string? TestCenterAddress { get; set; }
        public long? TestCenterCityId { get; set; }
        [ForeignKey("CityId")]
        public virtual Configuration_CityEntity? TestCenterCity { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? TestCenterYearEstablished { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string? TestCenterGoogleRating { get; set; }
        [Column(TypeName = "nvarchar(3000)")]
        public string? TestCenterGoogleLocation { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string? TestCenterOpenTime { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string? TestCenterCloseTime { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string? TestCenterOpenDay { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string? TestCenterCloseDay { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string? TestCenterWeekend { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string? TestCenterMobile { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string? TestCenterFax { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string? TestCenterPhone { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? TestCenterWebAddress { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? TestCenterRemarks { get; set; }
        public bool? TestCenterIsActive { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? TestCenterLogoFileName { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? TestCenterLogoFilePath { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string? TestCenterLogoFileExtension { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? TestCenterIcon { get; set; }
        public bool IsDoctorRecommended { get; set; } = false;
        public List<Configuration_HospitalEntity>? RecommendedTestCenters { get; set; }
        public bool IsUserSelectd { get; set; } = false;

        public List<Configuration_HospitalEntity>? UserSelectedTestCenters { get; set; }
    }
}

