using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class RetreiveGetDeliveryInfofromTLSViewModel
    {
        public string RRN { get; set; }
        public DateTime PLOD { get; set; }
        public string FrameNo { get; set; }
        public DateTime ActualPDD { get; set; }
        public DateTime EstimateBranchReceivedDate { get; set; }
        public string ActualLocation { get; set; }

    }
    public class SentGetDeliveryInfofromTLSViewModel
    {
        public bool Status { get; set; }
        public string RRN { get; set; }
        public DateTime PLOD { get; set; }
        public string FrameNo { get; set; }
    }
}
