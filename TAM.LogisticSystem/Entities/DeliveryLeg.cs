using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class DeliveryLeg
    {
        public int BufferMinutes { get; set; }

        public string CityLegCode { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        [Key]
        public string DeliveryLegCode { get; set; }

        public string LocationFrom { get; set; }

        public string LocationTo { get; set; }

        public string Name { get; set; }

        public bool NeedSJKB { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
