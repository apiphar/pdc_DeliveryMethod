using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class CancelDeliveryRequestTransitToOthersModel
    {
        public string DeliveryRequestNumber { get; set; }
        public DateTimeOffset DeliveryRequestDate { get; set; }
        public string LocationType { get; set; }
        public string LocationTypeCode { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public string LocationCode { get; set; }
        public int LeadTimeDay { get; set; }
        public int LeadTimeHour { get; set; }
        public int LeadTimeMinute { get; set; }
        public DateTimeOffset? PickUpDate { get; set; }
        public int? DeliveryTransitTypeId { get; set; }
        public List<CancelDeliveryRequestLocationModel> LocationList { get; set; }
        public DateTimeOffset? TransitReturnDate { get; set; }

    }
}
