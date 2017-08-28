using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestDirectDeliveryCreateModel
    {
        [Required]
        public DeliveryRequestModel DeliveryRequest { get; set; }
        [Required]
        public DeliveryRequestDirectDeliveryModel DeliveryRequestDirectDelivery { get; set; }
    }
}
