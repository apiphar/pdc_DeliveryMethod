using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class KalkulasiVehicleRoutingModel
    {
        public int VehicleRoutingId { get; set; }
        public int VehicleId { get; set; }
        public int Ordering { get; set; }
        public string ProcessMasterCode { get; set; }
        public string DeliveryMethodCode { get; set; }
        public string LocationCode { get; set; }
        public string ShiftCode { get; set; }
        public string LineNumber { get; set; }
        public DateTimeOffset EstimatedTimeInitial { get; set; }
        public DateTimeOffset EstimatedTimeAdjusted { get; set; }
        public DateTimeOffset ScanTime { get; set; }
        public int BufferMinutes { get; set; }
    }
}
