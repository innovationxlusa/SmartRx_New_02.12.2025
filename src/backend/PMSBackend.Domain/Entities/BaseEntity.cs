using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? CreatedById { get; set; }
        [ForeignKey(nameof(CreatedById))]
        public virtual SmartRxUserEntity? CreatedBy { get; set; } = default!;
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedById { get; set; }
        [ForeignKey(nameof(ModifiedById))]
        public virtual SmartRxUserEntity? ModifiedBy { get; set; } = default!;
        
    }
}
