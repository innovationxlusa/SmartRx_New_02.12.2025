using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("SmartRx_PatientChiefComplaint")]
    public class SmartRx_PatientChiefComplaintEntity : BaseEntity
    {
        public long SmartRxMasterId { get; set; }
        [ForeignKey("SmartRxMasterId")]
        public virtual SmartRx_MasterEntity SmartRx_Master { get; set; }

        //public long ChiefComplaintId { get; set; }
        //[ForeignKey("ChiefComplaintId")]
        //public virtual Configuration_ChiefComplaintEntity Configuration_ChiefComplaint { get; set; }
        public long UploadedPrescriptionId { get; set; }
        [ForeignKey("UploadedPrescriptionId")]
        public virtual Prescription_UploadEntity Prescription_Upload { get; set; }

        public string Description { get; set; }

    }
}
