using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryLegPageVIewModel
    {
        public List<DeliveryLegViewModel> ViewModels { get; set; }
        public List<DeliveryLegLocationViewModel> DeliveryLegLocations { get; set; }
        public List<DeliveryLegCityListViewModel> CityLegCodes { get; set; }
    }
}
