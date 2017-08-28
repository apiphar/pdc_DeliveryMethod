using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class PolaRangkaianTahapAkhirViewModel
    {
        public List<ProcessTailTemplate> RoutingDictionaryTail { set; get; }
        public List<RoutingDictionaryTailDetailModel> RoutingDictionaryTailDetail { set; get; }
        public List<Location> Location { set; get; }
        public List<DeliveryMethod> DeliveryMethod { set; get; }
        public List<ProcessMaster> RoutingMaster { set; get; }
        public ProcessTailTemplate Header { set; get; }
        public List<RoutingDictionaryTailDetailModel> Detail { set; get; }
    }
}
