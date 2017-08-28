using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterRitasePriceInputModel
    {
        [Required]
        public string CityLegCode { get; set; }

        [Required]
        public string CurrencySymbol { get; set; }
        
        [Required]
        public string DeliveryMethodCode { get; set; }

        [Required]
        public string DeliveryVendorCode { get; set; }

        [Required]
        public bool IsSingleTrip { get; set; }

        [Required]
        public decimal Nominal { get; set; }

        [Required]
        public DateTime ValidDate { get; set; }



    }
}
