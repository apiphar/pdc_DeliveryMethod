using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestTransitToOthersModel
    {
        [Required]
        public DeliveryRequestLocationModel DeliveryLocation { get; set; }
        [Required]
        public DeliveryRequestLocationNameModel DeliveryLocationName { get; set; }
        [Required]
        public string LocationAddress { get; set; }
        public string LocationCode { get; set; }
        [Required]
        [Range(0, 1000000000)]
        [RegularExpression("^[0-9]*$")]
        public int LeadTimeDay { get; set; }
        [Required]
        [Range(0, 23)]
        [RegularExpression("^[0-9]*$")]
        public int LeadTimeHour { get; set; }
        [Required]
        [Range(0, 59)]
        [RegularExpression("^[0-9]*$")]
        public int LeadTimeMinute { get; set; }
        [Required]
        public DateTime PickUpDate { get; set; }
        public string PickUpDateView { get; set; }
        [Required]
        public string DeliveryRequestTransitType { get; set; }
        public bool ValidateDetail { get; set; }
        public bool ValidateDetailSelfPickToOthers { get; set; }
        public bool ValidateDetailTransitNormal { get; set; }
        public bool ValidateDetailSelfPickFromOthers { get; set; }
        public bool ValidateTransitDetail { get; set; }
    }
}
