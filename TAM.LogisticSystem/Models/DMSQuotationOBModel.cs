using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DMSQuotationOBModel
    {
        public string FrameNumber { get; set; }
        public string OutletDestination { get; set; }
    }

    public class DMSSentQuotationModel
    {
        public decimal Price { get; set; }
        public string LocationUnit { get; set; }
    }
}
