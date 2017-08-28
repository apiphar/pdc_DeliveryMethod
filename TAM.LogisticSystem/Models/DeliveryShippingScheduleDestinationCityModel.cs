using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryShippingScheduleDestinationCityModel
    {
        [Key]
        public string CityForShipmentCode { get; set; }
        
        public string Name { get; set; }
    }
}
