using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DealerBranchViewModel
    {
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string CompanyCode { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string CarModelCode { get; set; }
        public string CarModelName { get; set; }
        public int? JumlahGesek { get; set; }

    }
}
