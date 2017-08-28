using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class Scratch
    {
        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string LocationCode { get; set; }

        public string ScratchHandOverNumber { get; set; }

        [Key]
        public int ScratchId { get; set; }

        public int VehicleId { get; set; }
    }
}
