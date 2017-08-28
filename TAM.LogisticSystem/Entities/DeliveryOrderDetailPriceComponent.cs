using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class DeliveryOrderDetailPriceComponent
    {
        public string Category { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public int DeliveryOrderDetailId { get; set; }

        [Key]
        public int DeliveryOrderDetailPriceComponentId { get; set; }

        public decimal Price { get; set; }

        public string PricingComponentCode { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
