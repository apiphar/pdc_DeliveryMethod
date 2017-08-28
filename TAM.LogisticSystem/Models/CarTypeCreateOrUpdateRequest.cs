using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class CarTypeCreateOrUpdateRequest
    {
        public string Katashiki { get; set; }

        public string Suffix { get; set; }

        public string CarSeriesId { get; set; }

        public string VehicleModel { get; set; }

        public string VehicleSeries { get; set; }

        public string Name { get; set; }

        public string HarmonizeCode { get; set; }

        public decimal ImportPrice { get; set; }

        public decimal StandardPrice { get; set; }

        public decimal SpecialPrice { get; set; }

        public decimal ModelPrice { get; set; }

        public decimal DiscountPrice { get; set; }

        public decimal PPH22 { get; set; }

        public decimal LuxuryTax { get; set; }

        public string EngineDescription { get; set; }

        public string CC { get; set; }

        public string SteerPosition { get; set; }

        public string WheelDiametre { get; set; }

        public string WheelSize { get; set; }

        public string Assembly { get; set; }
    }
}
