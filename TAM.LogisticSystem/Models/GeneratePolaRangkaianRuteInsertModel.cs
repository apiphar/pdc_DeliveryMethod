using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class GeneratePolaRangkaianRuteInsertModel
    {
        //insert header
        public List<GeneratePolaRangkaianRuteViewModel> JoinData { set; get; }

        public string ValidFrom { set; get; }

        //insert detail
        public string RoutingDictionaryHeadCode { set; get; }
	    public string RoutingMasterCode { set; get; }
	    public string LocationCode { set; get; }
	    public string DeliveryMethodCode { set; get; }
        public int Ordering { set; get; }
    }
}
