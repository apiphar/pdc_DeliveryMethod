using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class CancelDeliveryRequestSelfPickFromOthersModel
    {
        public string DeliveryRequestNumber { get; set; }
        public string DriverType { get; set; }
        public bool? DriverIdIsKtp { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
    }
}
