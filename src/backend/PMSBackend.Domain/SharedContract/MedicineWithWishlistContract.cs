using PMSBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Domain.SharedContract
{
    public class MedicineWithWishlistContract
    {
        public List<SmartRx_PatientMedicineEntity> Medicines {get;set;}
        public List<MedicineWishlistContract>? MedicineWishtlist {get;set;}
    }
}
