using System.Collections.Generic;
using PMSBackend.Domain.Entities;

namespace PMSBackend.Application.DTOs
{
    public class SmartRxChiefComplaintDTO
    {
        public long SmartRxMasterId { get; set; }
        public long UploadedPrescriptionId { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public string Acronym { get; set; }
        public string Elaboration { get; set; }
        public string Details { get; set; }
        public IList<AcronymsDTO>? Acronyms { get; set; }
    }
}
