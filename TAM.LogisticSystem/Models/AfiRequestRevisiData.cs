using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class AfiRequestRevisiData
    {
        public int? AfiApplicationId { get; set; }
        public string ApplicationNumber{get;set;}
        public string FakturNumber { get; set; }
        public string ModelName { get; set; }
        public string FrameNumber { get; set; }
        public string CustomerName { get; set; }
        public string KTP { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }

        public string type { get; set; }

    }
}
