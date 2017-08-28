using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class PricingComponentLookup
    {
        public string AS400ClusterCode { get; set; }

        public string CarSeriesCode { get; set; }

        public string Category { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string Katashiki { get; set; }

        public decimal Price { get; set; }

        public string PricingComponentCode { get; set; }

        [Key]
        public int PricingComponentLookupId { get; set; }

        public string Suffix { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
