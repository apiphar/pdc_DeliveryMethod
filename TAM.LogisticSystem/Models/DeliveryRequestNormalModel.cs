using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestNormalModel
    {
        [Required]
        public DateTime RequestedDeliveryTimeToBranch { get; set; }
        public string RequestedDeliveryTimeToBranchView { get; set; }
        public bool ValidateDetail { get; set; }
    }
}
