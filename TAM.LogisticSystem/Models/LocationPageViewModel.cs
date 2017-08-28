using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class LocationPageViewModel
    {
        public List<LocationViewModel> Location { get; set; }
        public List<CityForLeg> CityForLeg { get; set; }
        public List<CityForShipment> CityForShipment { get; set; }
        public List<LocationType> LocationType { get; set; }
    }
}
