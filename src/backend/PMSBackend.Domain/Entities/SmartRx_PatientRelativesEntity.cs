using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("SmartRx_PatientRelatives")]
    public class SmartRx_PatientRelativesEntity : BaseEntity
    {
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual SmartRx_PatientProfileEntity PatientProfile { get; set; }
        public long? PatientRelativeId { get; set; }

        [ForeignKey("PatientRelativeId")]
        public virtual SmartRx_PatientProfileEntity? ExistingRelativePatient { get; set; }
    }
}
