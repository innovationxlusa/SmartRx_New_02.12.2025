using PMSBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Domain.SharedContract
{
    public class InvestigationWithWishlistContract
    {
        public List<SmartRx_PatientInvestigationEntity> Tests { get; set; }
        public List<InvestigationWishlistContract> TestsWishtlist { get; set; }
    }
}
