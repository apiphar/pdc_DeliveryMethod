using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class RoutingDictionaryViewModel
    {
        [Key]
        public int RoutingDictionaryId { get; set; }
        public string BranchCode { get; set; }
        public string Suffix { get; set; }
        public string Katashiki { get; set; }

        //tambahan dari DB baru
        public DateTime ValidFrom { get; set; }

        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string CarModelCode { get; set; }
        public string CarModelName { get; set; }

    }
}
