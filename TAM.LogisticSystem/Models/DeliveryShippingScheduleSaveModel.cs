using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryShippingScheduleSaveModel
    {
        public DeliveryShippingScheduleVoyageFormModel voyageForm { get; set; }
        public List<DeliveryShippingScheduleVoyageDestinationCityModel> tempDestinationCities { get; set; }
        public List<DeliveryShippingScheduleVoyageDestinationSourceLocationModel> tempSourceLocations { get; set; }
    }

    public class DeliveryShippingScheduleDestionation
    {
        public int VoyageDestinationId { get; set; }
        public string VoyageNumber { get; set; }
        public string DestinationCity { get; set; }
        public DateTimeOffset EstimatedTimeArrivalDate { get; set;}
    }

    public class DeliveryShippingScheduleSourceLocation
    {
        public int VoyageCapacityPerDestinationId { get; set; }
        public int VoyageDestinationId { get; set; }
        public string SourceLocationCode { get; set; }
        public int Capacity { get; set; }
    }

    public class DeliveryShippingScheduleVoyageNodeSourceVoyageNumber
    {
        public int Capacity { get; set; }

        public string LocationCode { get; set; }
        
        public int VoyageNodeSourceId { get; set; }

        public int VoyageNodeId { get; set; }

        public string VoyageNumber { get; set; }
    }
}
