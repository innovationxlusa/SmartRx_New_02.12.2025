using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_Education")]
    public class Configuration_EducationEntity : BaseEntity
    {
        [Column(TypeName = "nvarchar(5)")]
        public string Code { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public String DegreeName { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        //public String? MajorSubject { get; set; }
        //[Column(TypeName = "nvarchar(200)")]
        public string? InstitutionName { get; set; }     // e.g., Dhaka Medical College

        //public int? PassingYear { get; set; }            // e.g., 2015

        //[Column(TypeName = "nvarchar(200)")]
        //public long CountryId { get; set; }
        //[ForeignKey("CountryId")]
        //public virtual Configuration_CountryEntity Country { get; set; }      

        [Column(TypeName = "nvarchar(1500)")]
        public String? Description { get; set; }

    }
}
