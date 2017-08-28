using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DMSSentUrgentModel
    {
        public string FrameNumber { get; set; }
        public int DeliveryRequestType { get; set; }
        public DateTimeOffset DeliveryRequestDate { get; set; }
        public DateTimeOffset PickUpDate { get; set; }
        public string KaroseriLocationCode { get; set; }
        public DateTimeOffset VehicleReturnDate { get; set; }
        public int DriverReturnType { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public string ReasonUrgentMemo { get; set; }
    }
}
