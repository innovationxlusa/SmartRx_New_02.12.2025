namespace PMSBackend.Application.DTOs
{
    public class EducationDTO
    {
        public long EducationId { get; set; }
        public string EducationCode { get; set; }
        public String EducationDegreeName { get; set; }
        public string? EducationInstitutionName { get; set; }     // e.g., Dhaka Medical College       
        public String? EducationDescription { get; set; }
    }
}
