using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestModel
    {
        [Required]
        public string DeliveryRequestNumber { get; set; }
        public int VehicleId { get; set; }
        public string FrameNumber { get; set; }
        public string BranchCode { get; set; }
        [Required]
        public string DeliveryRequestType { get; set; }
        public int DeliveryRequestTypeEnumId { get; set; }
        public DateTimeOffset? CancelledAt { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public int SequentialNumber { get; set; }
    }
}
