using System.Collections.Generic;

namespace PMSBackend.Domain.SharedContract
{
    public class DoctorProfileWithCountContract
    {
        public long DoctorId { get; set; }
        public string DoctorCode { get; set; }
        public string DoctorTitle { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
        public string? ProfilePhotoName { get; set; }
        public string? ProfilePhotoPath { get; set; }
        public string RegistrationNumber { get; set; }
        public int? SmartRxCount { get; set; }
        public decimal? DoctorRating { get; set; }
        public int PrescriptionCount { get; set; }
    }
}


