using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestTransitToOthersNormalSelfPickModel
    {
        [Required]
        public string DeliveryTransitType { get; set; }
        [Required]
        public DeliveryRequestSelfPickFromOtherModel DeliverySelfPickFromOthers { get; set; }
    }
}
