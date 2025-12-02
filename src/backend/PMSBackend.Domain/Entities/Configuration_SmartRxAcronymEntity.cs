using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_SmartRxAcronym")]
    public class Configuration_SmartRxAcronymEntity : BaseEntity
    {
       
        [Column(TypeName = "nvarchar(100)")]
        public string? Acronym { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? Abbreviation { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Elaboration { get; set; }
        [Column(TypeName = "nvarchar(2000)")]
        public string? Details { get; set; }

        //public virtual Configuration_ChiefComplaintEntity ChiefComplaint { get; set; }
    }
}
