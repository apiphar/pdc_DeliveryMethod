using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class DeliveryOrder
    {
        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string DebitAdviceNumber { get; set; }

        [Key]
        public string DeliveryOrderNumber { get; set; }

        public bool IsManual { get; set; }

        public string ReferenceNumber { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
