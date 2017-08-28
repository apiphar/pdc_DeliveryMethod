using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestLocationModel
    {
        public string LocationTypeCode { get; set; }
        [Required]
        public string LocationType { get; set; }

        public string LocationCode { get; set; }
    }
}
