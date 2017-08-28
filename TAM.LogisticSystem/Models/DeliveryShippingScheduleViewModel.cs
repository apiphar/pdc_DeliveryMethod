using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryShippingScheduleViewModel
    {
        public List<DeliveryShippingScheduleVendorModel> Vendors { get; set; }
        public List<DeliveryShippingScheduleVesselModel> Vessels { get; set; }
        public List<DeliveryShippingScheduleLocationModel> Ports { get; set; }
        public List<DeliveryShippingScheduleDestinationCityModel> DestinationCities { get; set; }
        public List<DeliveryShippingScheduleLocationModel> SourceLocations { get; set; }
        public List<DeliveryShippingScheduleVoyageModel> Voyages { get; set; }
        public List<DeliveryShippingScheduleVoyageDestinationCityModel> VoyagesDestinationCities { get; set; }
        public List<DeliveryShippingScheduleVoyageDestinationSourceLocationModel> VoyagesDestinationsSourceLocations { get; set; }
    }
}
