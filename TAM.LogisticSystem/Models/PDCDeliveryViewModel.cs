using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class PDCDeliveryViewModel
    {
        public string BranchCode { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string DeliveryMethodCode { get; set; }

        public string LocationCode { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
        public string LocationName { get; set; }
        public string BranchName { get; set; }
        public string DeliveryMethodName { get; set; }

        public string LocationData { get; set; }
        public string BranchData { get; set; }
        public string DeliveryMethodData { get; set; }

    }

}
