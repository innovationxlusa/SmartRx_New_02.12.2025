using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_ChiefComplaint")]
    public class Configuration_ChiefComplaintEntity : BaseEntity
    {
        [Column(TypeName = "nchar(10)")]
        public string Code { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Abbreviation { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string FullForm { get; set; }
        [Column(TypeName = "nvarchar(1500)")]
        public string Details { get; set; }
        //public virtual IList<Configuration_SmartRxAcronymEntity> SmartRxAcronym { get; set; }

    }
}
