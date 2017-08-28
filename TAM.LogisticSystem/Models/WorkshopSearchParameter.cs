using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Helpers;

namespace TAM.LogisticSystem.Models
{
    public class WorkshopSearchParameter : BasicSearchParameters
    {
        public string Name { get; set; }
        
        public int? WorkshopId { get; set; }

        public int? WorkshopTypeId { get; set; }
    }
}
