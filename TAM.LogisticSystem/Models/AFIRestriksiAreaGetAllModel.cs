using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class AFIRestriksiAreaGetAllModel
    {
        public List<AFIRestriksiAreaViewModel> AFIRegionsRestriction { get; set; }
        public List<AFIRestriksiAreaRegionViewModel> Regions { get; set; }
    }
}
