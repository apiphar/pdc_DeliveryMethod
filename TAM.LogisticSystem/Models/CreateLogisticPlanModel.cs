using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class CreateLogisticPlanModel
    {
        public int VehicleId { get; set; }
        public string  Katashiki { get; set; }
        public string  Suffix { get; set; }
        public string RoutingMasterCode { get; set; }
        public decimal LeadMinutes { get; set; }
        public int Decimal { get; set; }
        public string PhysicalLocationName{ get; set; }
        public string BranchCode { get; set; }
        public string RoutingLeadTimeById{ get; set; }
        public int Ordering { get; set; }

    }
}
