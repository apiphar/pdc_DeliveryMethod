using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class RegionAndRegionAFIViewModel
    {
        public List<Region> RegionList { get; set; }
        public List<AFIRegion> RegionAFIList { get; set; }
    }
}
