using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class DeliveryVendorVehicle
    {
        public int Capacity { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string DeliveryMethodCode { get; set; }

        public string DeliveryVendorCode { get; set; }

        [Key]
        public int DeliveryVendorVehicleId { get; set; }

        public string PoliceNumberOrVesselName { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
