using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DealerListViewModel
    {
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public List<DealerBranchViewModel> Branch { get; set; }
    }
}
