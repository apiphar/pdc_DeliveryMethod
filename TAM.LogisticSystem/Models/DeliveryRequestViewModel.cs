using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestViewModel
    {
        [Required]
        public DeliveryRequestModel DeliveryRequest { get; set; }
        [Required]
        public DeliveryRequestCarModel DeliveryRequestCar { get; set; }
    }
}
