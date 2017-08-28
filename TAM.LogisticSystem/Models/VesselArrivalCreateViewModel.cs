using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class VesselArrivalCreateViewModel
    {
        [Required]
        public string VoyageNumber { get; set; }
        [Required]
        public string DestinationCity { get; set; }
        [Required]
        public DateTime EstimatedTimeArrival { get; set; }
    }
}
