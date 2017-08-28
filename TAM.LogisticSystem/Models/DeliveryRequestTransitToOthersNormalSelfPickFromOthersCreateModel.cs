using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel
    {
        [Required]
        public DeliveryRequestModel DeliveryRequest { get; set; }
        [Required]
        public DeliveryRequestTransitToOthersNormalSelfPickFromOthersModel DeliveryRequestTransitToOthersNormalSelfPickFromOthers { get; set; }
    }
}
