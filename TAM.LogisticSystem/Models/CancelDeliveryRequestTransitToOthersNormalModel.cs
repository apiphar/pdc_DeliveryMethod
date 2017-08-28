using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class CancelDeliveryRequestTransitToOthersNormalModel
    {
        public int? DeliveryRequestTransitTypeId { get; set; }
        public string DeliveryRequestNumber { get; set; }
        public string OtherPdcLocation { get; set; }
        public string OtherPdcLocationCode { get; set; }
    }
}
