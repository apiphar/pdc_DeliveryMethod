using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterLeadTimeViewModel
    {
        public int LeadMinutes { get; set; }

        public string LocationCode { get; set; }

        public string RoutingMasterCode { get; set; }

        public string NamaRute { get; set; }

    }
}
