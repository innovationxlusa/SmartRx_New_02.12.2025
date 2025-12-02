using PMSBackend.Domain.CommonDTO;

namespace PMSBackend.Application.DTOs
{
    public class PatientPrescriptionSearchParams : PagingSortingParams
    {
        public long UserId { get; set; }
        public long PatientId { get; set; }
        public string? SearchKeyword { get; set; }
        public string? SearchColumn { get; set; } // "all", "patientname", "prescriptioncode", "filename", "createddate", "tag1", "tag2", "tag3", "tag4", "tag5"
    }
}
