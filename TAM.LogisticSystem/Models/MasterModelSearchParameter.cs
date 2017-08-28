using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Helpers;

namespace TAM.LogisticSystem.Models
{
    public class MasterModelSearchParameter : BasicSearchParameters
    {
        public string Name { set; get; }

        public int BrandCode { get; set; }
        public int ManufacturingId { get; set; }

        
    
    }
}
