using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestTransitToOthersNormalSelfPickFromOthersModel
    {
        [Required]
        public DeliveryRequestTransitToOthersModel DeliveryTransitToOthers { get; set; }
        [Required]
        public DeliveryRequestTransitToOthersNormalSelfPickModel DeliveryRequestTransitToOthersNormalModel { get; set; }
    }
}
