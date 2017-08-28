using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class Voyage
    {
        public DateTimeOffset? CancelledAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public int DeliveryVendorVehicleId { get; set; }

        public DateTimeOffset DepartureDate { get; set; }

        public string DepartureLocationCode { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [Key]
        public string VoyageNumber { get; set; }

        public int VoyageStatusEnumId { get; set; }
    }
}
