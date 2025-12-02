using PMSBackend.Application.CommonServices;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Application.DTOs
{
    public class SmartRxVitalDTO
    {
        public long Id { get; set; }
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public long VitalId { get; set; }
        public string VitalName { get; set; }
        public string ApplicableEntity { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal VitalValue { get; set; }
        public string VitalUnit { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string VitalStatus { get; set; }
        public int? HeightFeet { get; set; }
        public decimal? HeightInches { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateStr { get; set; }
        public ApiResponseResult ApiResponseResult { get; set; } = new ApiResponseResult();

        public bool? IsRewardUpdated { get; set; }
        public string? RewardTitle { get; set; }
        public double? TotalRewardPoints { get; set; }
        public string? RewardMessage { get; set; }
    }
}
