using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class CancelDeliveryRequestDirectDeliveryModel
    {
        public string DeliveryRequestNumber { get; set; }
        public DateTimeOffset? EstimasiPDCOut { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCity { get; set; }
        public string SalesmanName { get; set; }
        public string SalesmanContactNo { get; set; }

    }
}
