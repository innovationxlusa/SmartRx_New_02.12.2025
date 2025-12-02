using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Application.DTOs
{
    public class PrescriptionAcronymDTO
    {
        public long Id { get; set; }
        public string Acronym { get; set; }
        public string Abbreviaiton { get; set; }

        public string Elaboration { get; set; }
        public string Details { get; set; }


    }
}
