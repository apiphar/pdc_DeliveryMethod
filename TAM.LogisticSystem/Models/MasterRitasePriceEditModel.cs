using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterRitasePriceEditModel
    {
        [Required]
        public string CityLegCode { get; set; }

        [Required]
        public int CityLegRitaseCostId { get; set; }

        [Required]
        public string CurrencySymbol { get; set; }

        [Required]
        public string DeliveryMethodCode { get; set; }

        [Required]
        public string DeliveryVendorCode { get; set; }

        [Required]
        public string IsSingleTrip { get; set; }

        [Required]
        public decimal Nominal { get; set; }

        [Required]
        public DateTime ValidDate { get; set; }
    }
}
