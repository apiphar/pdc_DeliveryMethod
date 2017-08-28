using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DealerMasterPageViewModel
    {
        public List<DealerMasterViewModel> ViewModels { get; set; }
        public List<DealerMasterTypeCode> DealerTypeCodes { get; set; }
    }
}
