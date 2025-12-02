using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Domain.SharedContract
{
    public class MedicineWishlistContract
    {
        public long? Id { get; set; }
        public long? MedicineId { get; set; }
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public bool? Wished { get; set; }
        public string? MedicineName { get; set; }
        public Dictionary<long, string>? WishedMedicines { get; set; }
    }
}
