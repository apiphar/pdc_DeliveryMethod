using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class VehicleRouting
    {
        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string DeliveryMethodCode { get; set; }

        public DateTimeOffset EstimatedTimeAdjusted { get; set; }

        public DateTimeOffset EstimatedTimeInitial { get; set; }

        public string LineNumber { get; set; }

        public string LocationCode { get; set; }

        public int Ordering { get; set; }

        public string ProcessMasterCode { get; set; }

        public DateTimeOffset? ScanTime { get; set; }

        public string ShiftCode { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public int VehicleId { get; set; }

        [Key]
        public long VehicleRoutingId { get; set; }
    }
}
