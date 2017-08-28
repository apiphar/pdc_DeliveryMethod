using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryShippingScheduleVoyageModel
    {
        public string VoyageNumber { get; set; }
        public string DeliveryVendorCode { get; set; }
        public string DeliveryVendorName { get; set; }
        public int DeliveryVendorVehicleId { get; set; }
        public string PoliceNumberOrVesselName { get; set; }
        public string DepartureLocationCode { get; set; }
        public string DepartureLocationName { get; set; }
        public DateTimeOffset DepartureDate { get; set; }
    }
}
