using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterModelDelete
    {
        public int BrandCode { set; get; }
        public string CarModelCode { set; get; }
        public string Name { set; get; }
        public int ManufacturingId { get; set; }
    }
}
