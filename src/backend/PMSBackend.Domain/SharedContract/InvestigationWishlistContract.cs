using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Domain.SharedContract
{
    public class InvestigationWishlistContract
    {
        public long? Id { get; set; }      
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public bool? Wished { get; set; }
        public long? TestId { get; set; }
        public string? TestName { get; set; }
        public string? TestFullName { get; set; }

        public Dictionary<long, string>? WishedTestCenters { get; set; }
    }
}
