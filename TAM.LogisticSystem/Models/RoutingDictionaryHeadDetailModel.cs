using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class RoutingDictionaryHeadDetailModel
    {
        public string RoutingDictionaryHeadCode { set; get; }
	    public int Ordering { set; get; }
	    public string ProcessMasterCode { set; get; }
	    public string RoutingMasterName { set; get; }
	    public string DeliveryMethodName { set; get; }
        public string LocationName { set; get; }
        public string DeliveryMethodCode { set; get; }
        public string LocationCode { set; get; }
    }
}
