using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("SmartRx_PatientVitals")]
    public class SmartRx_PatientVitalsEntity : BaseEntity
    {
        public long SmartRxMasterId { get; set; }
        [ForeignKey("SmartRxMasterId")]
        public virtual SmartRx_MasterEntity SmartRxMaster { get; set; }

        public long PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription_UploadEntity Prescription { get; set; }

        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual SmartRx_PatientProfileEntity PatientProfile { get; set; }

        public long VitalId { get; set; }
        [ForeignKey("VitalId")]
        public virtual Configuration_VitalEntity Vital { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal VitalValue { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? VitalStatus { get; set; }

        // 👇 New height properties
        public int? HeightFeet { get; set; }
        public decimal? HeightInches { get; set; }

        [NotMapped]
        public string HeightDisplay
        {
            get
            {
                if (HeightFeet.HasValue || HeightInches.HasValue)
                {
                    return $"{HeightFeet ?? 0} ft {HeightInches ?? 0} in";
                }
                return string.Empty;
            }
        }
    }
}
