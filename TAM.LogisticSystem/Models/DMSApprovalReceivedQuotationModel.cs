using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DMSApprovalReceivedQuotationModel
    {
        public string FrameNumber { get; set; }
        public string OutletDestination { get; set; }
        public string LocationUnit { get; set; }
    }

    public class DMSSentApprovalReceivedQuotationModel
    {
        public decimal Price { get; set; }
        public string LocationUnit { get; set; }
        public bool ChangeLocationFlag { get; set; }
    }
}
