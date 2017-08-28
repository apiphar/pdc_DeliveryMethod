using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryVendorCreateModel
    {
        [StringLength(8)]
        public string SAPCode { get; set; }

        [StringLength(16)]
        public string Account { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        public DeliveryVendorLocationModel Location { get; set; }

        public string DeliveryVendorCode { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

    }
}
