using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestPageViewModel
    {
        public List<DeliveryRequestModel> DeliveryRequest { get; set; }
        public List<DeliveryRequestCarModel> DeliveryRequestCar { get; set; }
        public List<DeliveryRequestLocationModel> DeliveryRequestLocationType { get; set; }
        public List<DeliveryRequestLocationNameModel> DeliveryRequestLocationName { get; set; }
        public List<DeliveryRequestTransitToOthersModel> DeliveryRequestLocationAddress { get; set; }
        public List<DeliveryRequestOtherPdcLocationModel> DeliveryRequestOtherPdcLocation { get; set; }
    }
}
