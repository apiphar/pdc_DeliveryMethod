using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryShippingScheduleVoyageDestinationSourceLocationModel
    {
        public int VoyageNodeId { get; set; }
        public int TempVoyageNodeId { get; set; }
        public int VoyageNodeSourceId { get; set; }
        public int TempVoyageNodeSourceId { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public int Capacity { get; set; }
    }
}
