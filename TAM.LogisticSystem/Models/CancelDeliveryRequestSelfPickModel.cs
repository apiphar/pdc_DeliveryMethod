using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class CancelDeliveryRequestSelfPickModel
    {
        public string DeliveryRequestNumber { get; set; }
        public DateTimeOffset? PickUpDate { get; set; }
        public bool? DriverIdIsKTP { get; set; }
        public string DriverType { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }

        public string ConfirmationCode { get; set; }

    }
}
