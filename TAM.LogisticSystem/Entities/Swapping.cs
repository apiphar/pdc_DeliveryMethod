using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class Swapping
    {
        public decimal Cost { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset EstimatedPDD { get; set; }

        public string FromBranch { get; set; }

        public bool IsApproved { get; set; }

        [Key]
        public int SwappingId { get; set; }

        public string ToBranch { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public int VehicleId { get; set; }
    }
}
