using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Security_PMSRole")]
    public class SmartRxRoleEntity : BaseEntity
    {
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? Description { get; set; }
        public bool IsSelfService { get; set; }
        public virtual IList<SmartRxUserRoleEntity>? UserRoles { get; set; }
    }
}
