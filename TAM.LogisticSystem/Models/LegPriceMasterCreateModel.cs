using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class LegPriceMasterCreateModel
    {
        public string CityLegCostCode { get; set; }
        public LegPriceMasterDeliveryVendorModel DeliveryVendor { get; set; }
        public LegPriceMasterCityLegModel CityLeg { get; set; }
        public LegPriceMasterDeliveryMethodModel DeliveryMethod { get; set; }
        public LegPriceMasterCarSeriesModel CarSeries { get; set; }
        public LegPriceMasterCurrencyModel Currency { get; set; }
        public int Nominal { get; set; }
        public DateTime ValidDate { get; set; }
        public string NeedAdditionalCityLegCostCode { get; set; }
    }
}
