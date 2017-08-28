using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DMSDOUpdateViewModel
    {
        public string FrameNumber { get; set; }
        public string EnginePrefix { get; set; }
        public string EngineNumber { get; set; }
        public string Katashiki { get; set; }
        public string Suffix { get; set; }
        public string ExteriorColorCode { get; set; }
        public decimal WPBT { get; set; }
        public decimal ValueAddedTax { get; set; }
        public decimal LuxuryTax { get; set; }
        public string CancellationNumber { get; set; }
        public string DeliveryOrderNumber { get; set; }        
        public DateTime DeliveryOrderDate { get; set; }
        public string KeyNumber { get; set; }
        public DateTime DeliveryOrderDueDate { get; set; }
        public string DebitAdviceNumber { get; set; }
        public string BranchCode { get; set; }
        public string RRN { get; set; }
        public decimal InvoicedPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal PPH22 { get; set; }
        public DateTime DTPLOD { get; set; }
    }
}
