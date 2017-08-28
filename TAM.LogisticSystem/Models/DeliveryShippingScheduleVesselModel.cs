using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryShippingScheduleVesselModel
    {
	    public string DeliveryVendorCode { get; set; }
        public int DeliveryVendorVehicleId { get; set; }
        public string PoliceNumberOrVesselName { get; set; }
    }
}
