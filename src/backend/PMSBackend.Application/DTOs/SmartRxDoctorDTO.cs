using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.SharedContract;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Application.DTOs
{
    public class SmartRxDoctorDTO
    {
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public long DoctorId { get; set; }
        public DoctorProfileDTO PatientDoctor { get; set; }
        public List<EducationDTO>? DoctorEducations { get; set; }
        public  List<SmartRxDoctorChamberDTO>? Chambers { get; set; }
        public int? ChamberWaitTimeHour { get; set; }
        public int? ChamberWaitTimeMinute { get; set; }
        public decimal? ChamberFee { get; set; }
        public long? ChamberFeeMeasurementUnitId { get; set; }
        public string? ChamberFeeMeasurementUnit { get; set; }
        public decimal? DoctorRating { get; set; }
        public decimal? TransportFee { get; set; }

        public decimal? OtherExpense { get; set; }
        public decimal? TravelTimeMinute { get; set; }
        public decimal? ConsultingDurationInMinutes { get; set; }
        public string DoctorVisitingDaysInChamber { get; set; }
        public string? Comments { get; set; }

        public ApiResponseResult? ApiResponseResult { get; set; }

        public bool? IsRewardUpdated { get; set; }
        public string? RewardTitle { get; set; }
        public double? TotalRewardPoints { get; set; }
        public string? RewardMessage { get; set; }
    }
}
