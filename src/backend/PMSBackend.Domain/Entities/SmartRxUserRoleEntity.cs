using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Security_PMSUserWiseRole")]
    public class SmartRxUserRoleEntity : BaseEntity
    {
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual SmartRxUserEntity User { get; set; }

        public long RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual SmartRxRoleEntity Role { get; set; }
    }
}
