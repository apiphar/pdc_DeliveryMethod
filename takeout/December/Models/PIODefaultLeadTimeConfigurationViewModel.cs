using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class PIODefaultLeadTimeConfigurationViewModel
    {
        public string RoutingMasterCode { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public int ProcessForLineId { get; set; }
        public string ProcessStatus { get; set; }
        public int LeadTime { get; set; }
    }
}
