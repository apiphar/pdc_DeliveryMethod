using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class CancelDeliveryRequestLocationModel
    {
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string LocationTypeName { get; set; }
        public string LocationAddress { get; set; }
        public string LocationTypeCode { get; set; }
    }
}
