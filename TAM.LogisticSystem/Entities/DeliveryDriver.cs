using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class DeliveryDriver
    {
        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        [Key]
        public string DeliveryDriverCode { get; set; }

        public string DeliveryMethodCode { get; set; }

        public string DeliveryVendorCode { get; set; }

        public bool HasSIMA { get; set; }

        public bool HasSIMB { get; set; }

        public string Name { get; set; }

        public DateTime SIMAExpiration { get; set; }

        public string SIMANumber { get; set; }

        public DateTime SIMBExpiration { get; set; }

        public string SIMBNumber { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
