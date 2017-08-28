using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterRitasePriceViewModel
    {
        public string CityLegCode { get; set; }

        public int CityLegRitaseCostId { get; set; }

        public string CurrencySymbol { get; set; }

        public string DeliveryMethodCode { get; set; }

        public string DeliveryVendorCode { get; set; }

        public bool IsSingleTrip { get; set; }

        public decimal Nominal { get; set; }

        public DateTime ValidDate { get; set; }
    }
}
