using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("SmartRx_PatientDoctor")]
    public class SmartRx_PatientDoctorEntity : BaseEntity
    {

        public long SmartRxMasterId { get; set; }
        [ForeignKey("SmartRxMasterId")]
        public virtual SmartRx_MasterEntity SmartRxMaster { get; set; }
        public long PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription_UploadEntity Prescription { get; set; }
        public long DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Configuration_DoctorEntity PatientDoctor { get; set; }
        public long? ActiveChamberId { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public int? ChamberWaitTimeHour { get; set; } = 0;
        public int? ChamberWaitTimeMinute { get; set; } = 0;
        public decimal? ChamberFee { get; set; } //Doctor fee
        public long? ChamberFeeMeasurementUnitId { get; set; }

        [ForeignKey("ChamberFeeMeasurementUnitId")]
        public virtual Configuration_UnitEntity? ChamberFeeMeasurementUnit { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal? DoctorRating { get; set; }
        public decimal? TransportExpense { get; set; }
        public decimal? OtherExpense { get; set; }
       
        public int? ConsultingDurationInMinutes { get; set; }
        public decimal? TravelTimeMinute { get; set; }

        public string? Comments { get; set; }
       

    }
}
