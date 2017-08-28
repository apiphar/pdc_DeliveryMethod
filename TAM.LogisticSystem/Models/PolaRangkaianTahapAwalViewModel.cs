using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class PolaRangkaianTahapAwalViewModel
    {
        public List<ProcessHeadTemplate> RoutingDictionaryHead { set; get; }
        public List<RoutingDictionaryHeadDetailModel> RoutingDictionaryHeadDetail { set; get; }
        public List<Location> Location { set; get; }
        public List<DeliveryMethod> DeliveryMethod { set; get; }
        public List<ProcessMaster> RoutingMaster { set; get; }
    }
}
