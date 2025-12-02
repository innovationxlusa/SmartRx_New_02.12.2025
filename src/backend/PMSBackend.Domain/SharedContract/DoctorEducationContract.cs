using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Domain.SharedContract
{
    public class DoctorEducationContract
    {
        public long EducationId { get; set; }
        public string EducationCode { get; set; }
        public String EducationDegreeName { get; set; }
        public string? EducationInstitutionName { get; set; }     // e.g., Dhaka Medical College       
        public String? EducationDescription { get; set; }
    }
}
