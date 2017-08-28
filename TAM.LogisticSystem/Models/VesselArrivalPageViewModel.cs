using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class VesselArrivalPageViewModel
    {
        public List<UnitListViewModel> UnitLists { get; set; }
        public List<CityListViewModel> CityLists { get; set; }
        public List<VesselArrivalViewModel> ViewModels { get; set; }
    }
}
