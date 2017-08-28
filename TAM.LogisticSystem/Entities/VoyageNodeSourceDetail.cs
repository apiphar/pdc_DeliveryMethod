using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class VoyageNodeSourceDetail
    {
        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public int VehicleId { get; set; }

        public int VehicleVoyageStatusEnumId { get; set; }

        public int VoyageNodeSourceId { get; set; }
    }
}
