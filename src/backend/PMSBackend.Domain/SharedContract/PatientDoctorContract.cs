using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Domain.SharedContract
{
    public class PatientDoctorContract
    {
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public long DoctorId { get; set; }
        public DoctorProfileContract PatientDoctor { get; set; }
        public List<DoctorEducationContract>? DoctorEducations { get; set; }
        public List<DoctorChamberContract>? DoctorChambers { get; set; }
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
        public string DoctorVisitingDaysInChamber {  get; set; }
        public string? Comments { get; set; }
    }
}
