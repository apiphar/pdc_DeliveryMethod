using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class PolaRangkaianTahapAwalInsertModel
    {
        public InsertPolaHeaderModel Header { set; get; }

        public List<InsertPolaDetailModel> Detail { set; get; }
    }

    public class InsertPolaHeaderModel
    {
        [RegularExpression("^[a-zA-Z0-9]+$")]
        [Required]
        public string RoutingDictionaryHeadCode { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9\s\-.,&\'\/]+$")]
        [Required]
        public string Description { get; set; }
    }

    public class InsertPolaDetailModel
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
