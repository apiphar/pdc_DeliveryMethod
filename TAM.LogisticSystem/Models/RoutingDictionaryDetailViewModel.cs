using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class RoutingDictionaryDetailViewModel
    {
        [Key]
        public int RoutingDictionaryDetailId { get; set; }
        public int RoutingDictionaryId { get; set; }
        public string LocationCode { get; set; }
        public string RoutingMasterCode { get; set; }
        public string DeliveryMethodCode { get; set; }
        public int Ordering { get; set; }

        public string RoutingMasterName { get; set; }
        public string LocationName { get; set; }

        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string DeliveryMethodName { get; set; }

    }
}
