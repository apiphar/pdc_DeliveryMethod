using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class RoutingProductionLeadTimeViewModel
    {
        public int RoutingDictionaryProductionId { get; set; }
        public string LocationCode { get; set; }
        public string Katashiki { get; set; }
        public string Suffix { get; set; }
        public string RoutingMasterCode { get; set; }
        public int Ordering { get; set; }
        public int LeadMinutes { get; set; }
        public string CarModelCode { get; set; }
        public string NamaType { get; set; }
        
        public string NamaRute { get; set; }

        public string NamaLocation { get; set; }


    }
}
