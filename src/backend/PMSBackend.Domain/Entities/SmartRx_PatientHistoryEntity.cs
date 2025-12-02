using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("SmartRx_PatientHistory")]
    public class SmartRx_PatientHistoryEntity : BaseEntity
    {
        public long SmartRxMasterId { get; set; }
        [ForeignKey("SmartRxMasterId")]
        public virtual SmartRx_MasterEntity SmartRxMaster { get; set; }
        public long PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription_UploadEntity Prescription { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string? Details { get; set; }
    }
}
