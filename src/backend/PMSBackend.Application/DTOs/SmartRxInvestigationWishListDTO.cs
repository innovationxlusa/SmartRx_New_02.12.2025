using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Application.DTOs
{
    public class SmartRxInvestigationWishListDTO
    {
        public long Id { get; set; }
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        //public string WishListType { get; set; }
        public long TestId { get; set; }
        public string TestCode { get; set; }
        [Required]
        public string TestName { get; set; }
        public string? TestDescription { get; set; }
        public string? TestFullName { get; set; }
        public string? TestShortName { get; set; }
        public string? TestNameByDiagnosticCenter { get; set; }
        public decimal? TestUnitPrice { get; set; }
        public string? PriceName { get; set; }
        public string? PriceMeasurementUnit { get; set; }
        public decimal? NationalUnitPrice { get; set; } = 0;
        public string? NationalPriceName { get; set; }
        public string? NationalPriceMeasurementUnit { get; set; }
        public string? Speciality { get; set; }//any special machine/setup/environment etc.       
        public string? Specimen { get; set; }
        public string? Comments { get; set; }
        public bool IsActive { get; set; }

        public long? RecommendedTestCenterId { get; set; }
        public string? TestCenterIds { get; set; }
        [Column(TypeName = "nvarchar(5)")]
        public string? RecommendedTestCenterCode { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? RecommendedTestCenterName { get; set; }

        [Column(TypeName = "nvarchar(4000)")]
        public string? RecommendedTestCenterDescription { get; set; }

        [Column(TypeName = "nvarchar(2000)")]
        public string? RecommendedTestCenterAddress { get; set; }
        public long? RecommendedTestCenterCityId { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string? RecommendedTestCenterGoogleRating { get; set; }
        [Column(TypeName = "nvarchar(3000)")]
        public string? RecommendedTestCenterGoogleLocation { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string? RecommendedTestCenterOpenTime { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string? RecommendedTestCenterCloseTime { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string? RecommendedTestCenterOpenDay { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string? RecommendedTestCenterCloseDay { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string? RecommendedTestCenterWeekend { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string? RecommendedTestCenterMobile { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string? RecommendedTestCenterFax { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string? RecommendedTestCenterPhone { get; set; }
        public string? WishList { get; set; }
        public bool? Wished { get; set; }

    }
}
