using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryUnitLoadingFrameNumberUpdate
    {
      
        [Required]
        public List<int> VehicleId { get; set; }
        [Required]
        [RegularExpression("^[A-Za-z0-9]*$")]
        public string VoyageNumber { get; set; }
    }
}
