using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class LegPriceMasterViewModel
    {

        public string CarSeriesCode { get; set; }
        public string CarSeriesName { get; set; }
        public string CarSeriesNameView { get; set; }
        public string CityLegCode { get; set; }
        public string CityLegName { get; set; }
        public string CityLegNameView { get; set; }

        public string CityLegCostCode { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyNameView { get; set; }

        public string DeliveryMethodCode { get; set; }
        public string DeliveryMethodName { get; set; }
        public string DeliveryMethodNameView { get; set; }

        public string DeliveryVendorCode { get; set; }
        public string DeliveryVendorName { get; set; }
        public string DeliveryVendorNameView { get; set; }

        public string NeedAdditionalCityLegCostCode { get; set; }

        public decimal Nominal { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime ValidDate { get; set; }
        public string ValidDateView { get; set; }
    }
}
