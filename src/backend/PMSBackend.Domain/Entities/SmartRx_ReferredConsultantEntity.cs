using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("SmartRx_ReferredConsultant")]
    public class SmartRx_ReferredConsultantEntity : BaseEntity
    {
        public long SmartRxMasterId { get; set; }
        [ForeignKey("SmartRxMasterId")]
        public virtual SmartRx_MasterEntity SmartRxMaster { get; set; }
        public long? ReferredConsultantId { get; set; }
        [ForeignKey("ReferredConsultantId")]
        public virtual SmartRx_PatientDoctorEntity? ReferredConsultant { get; set; }

        public string ReferredBy { get; set; }

    }
}
