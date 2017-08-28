using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestLocationNameModel
    {
        public string LocationCode { get; set; }
        public string LocationTypeCode { get; set; }
        [Required]
        public string LocationName { get; set; }
    }
}
