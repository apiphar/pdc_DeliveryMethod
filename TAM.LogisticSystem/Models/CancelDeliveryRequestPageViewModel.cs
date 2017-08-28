using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class CancelDeliveryRequestPageViewModel
    {
        public List<CancelDeliveryRequestViewModel> CancelDeliveryRequest { get; set; }
        public List<CancelDeliveryRequestLocationModel> CancelDeliveryRequestLocation { get; set; }
    }
}
