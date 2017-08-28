using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryShippingScheduleVoyageFormModel
    {
        public string voyageNumber { get; set; }
        public DeliveryShippingScheduleVendorModel vendor { get; set; }
        public DeliveryShippingScheduleVesselModel vessel { get; set; }
        public DeliveryShippingScheduleLocationModel port { get; set; }
        public DateTimeOffset estimatedDeparture { get; set; }
    }
}
