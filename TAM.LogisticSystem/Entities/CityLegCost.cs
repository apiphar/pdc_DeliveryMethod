using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class CityLegCost
    {
        public int Capacity { get; set; }

        public string CarSeriesCode { get; set; }

        public string CityLegCode { get; set; }

        [Key]
        public string CityLegCostCode { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string Currency { get; set; }

        public string DeliveryMethodCode { get; set; }

        public string DeliveryVendorCode { get; set; }

        public string NeedAdditionalCityLegCostCode { get; set; }

        public decimal Nominal { get; set; }

        public string ShiftCode { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public DateTimeOffset ValidFrom { get; set; }
    }
}
