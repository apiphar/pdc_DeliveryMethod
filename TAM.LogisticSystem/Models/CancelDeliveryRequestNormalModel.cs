using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class CancelDeliveryRequestNormalModel
    {
        public DateTimeOffset? RequestedDeliveryTimeToBranch { get; set; }
        public string DeliveryRequestNumber { get; set; }

    }
}
