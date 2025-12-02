using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("SmartRx_PatientAdvice")]
    public class SmartRx_PatientAdviceEntity : BaseEntity
    {
        public long SmartRxMasterId { get; set; }
        [ForeignKey("SmartRxMasterId")]
        public virtual SmartRx_MasterEntity SmartRxMaster { get; set; }
        public long PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription_UploadEntity Prescription { get; set; }

        public string Advice { get; set; }
        public string AdviceKeywordToRecommend { get; set; }

    }
}
