using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestSelfPickFromOtherModel
    {
        [Required]
        public string DriverType { get; set; }
        [Required]
        [MaxLength(32)]
        [RegularExpression("^[0-9]*$")]
        public string DriverId { get; set; }
        [Required]
        [MaxLength(255)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string DriverName { get; set; }
        public bool ValidateTransitDetail { get; set; }
    }
}
