using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestOtherPdcLocationModel
    {
        [Required]
        public string OtherPdcLocation { get; set; }
        public string OtherPdcLocationCode { get; set; }
    }
}
