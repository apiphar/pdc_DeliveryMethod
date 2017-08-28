using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class VesselDepartPageViewModel
    {
        public List<VesselDepartDetailViewModel> ViewModels { get; set; }
        public List<UnitListViewModel> UnitLists { get; set; }
    }
}
