using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryShippingScheduleVoyageDestinationCityModel
    {
        public string VoyageNumber { get; set; }
        public int VoyageNodeId { get; set; }
        public int TempVoyageNodeId { get; set; }
        public string CityForShipmentCode { get; set; }
        public string CityName { get; set; }
        public DateTimeOffset EstimatedTimeOfArrival { get; set; }
    }
}
