using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterLeadTimeLocationViewModel
    {
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string ProcessMasterCode { get; set; }
        public string RouteName { get; set; }
        public int LeadMinutes { get; set; }
    }
}
