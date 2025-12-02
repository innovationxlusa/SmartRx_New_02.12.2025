using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_UserActivity")]
    public class Configuration_UserActivityEntity : BaseEntity
    {
        [Column(TypeName = "nchar(10)")]
        public string ActivityCode { get; set; } //R000000001
        public string ActivityName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public double ActivityPoint { get; set; }

        public string Remarks { get; set; }


    }
}
