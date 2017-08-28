using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class PolaRangkaianTahapAkhirInsertModel
    {
        public InsertPolaHeaderTailModel Header { set; get; }

        public List<InsertPolaDetailTailModel> Detail { set; get; }
    }
    public class InsertPolaHeaderTailModel
    {
        [RegularExpression("^[a-zA-Z0-9]+$")]
        [Required]
        public string RoutingDictionaryTailCode { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9\s\-.,&\'\/]+$")]
        [Required]
        public string Description { get; set; }
    }

    public class InsertPolaDetailTailModel
    {
        public string RoutingDictionaryTailCode { set; get; }
        public int Ordering { set; get; }
        public string ProcessMasterCode { set; get; }
        public string RoutingMasterName { set; get; }
        public string DeliveryMethodName { set; get; }
        public string LocationName { set; get; }
        public string DeliveryMethodCode { set; get; }
        public string LocationCode { set; get; }
    }
}
