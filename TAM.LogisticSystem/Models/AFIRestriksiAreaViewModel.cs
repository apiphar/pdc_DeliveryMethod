using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class AFIRestriksiAreaViewModel
    {
        public int AFIRegionRestrictionId { get; set; }
        public string RegionCode { get; set; }
        public string Name { get; set; }
        public bool IsLocked { get; set; }
        public DateTimeOffset? ValidFrom { get; set; }
        public DateTimeOffset? ValidTo { get; set; }
    }
}
