using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestTransitToOthersNormalModel
    {
        [Required]
        public string DeliveryTransitType { get; set; }
        public DeliveryRequestOtherPdcLocationModel DeliveryOtherPdcLocation { get; set; }
        public bool ValidateTransitDetail { get; set; }
        public bool ValidateDetailSelfPickFromOthers { get; set; }
    }
}
