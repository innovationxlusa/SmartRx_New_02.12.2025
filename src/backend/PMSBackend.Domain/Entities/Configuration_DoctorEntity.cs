using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_Doctor")]
    public class Configuration_DoctorEntity : BaseEntity
    {
        [Column(TypeName = "nchar(10)")]
        public string Code { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string FirstName { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string LastName { get; set; }


        // Comma-separated or join table for multiple degrees
        [Column(TypeName = "nvarchar(200)")]
        public string? EducationDegreeIds { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? SpecializedArea { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? ProfilePhotoName { get; set; }
        [Column(TypeName = "nvarchar(2000)")]
        public string? ProfilePhotoPath { get; set; }
        public string? ChamberIds { get; set; }
        public int YearOfExperiences { get; set; }
        public string? Experiences { get; set; }
        public string? BMDCRegNo { get; set; }
        public string? ProfessionalSummary { get; set; }


        public decimal? Rating { get; set; } // e.g., 4.5 stars, google rating      

        [Column(TypeName = "nvarchar(max)")]
        public string? Comments { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
