using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryLegLeadTimeViewModel
    {
        public int DeliveryLeadTimeId { get; set; }
        [Required]
        public string DeliveryMethodCode { get; set; }
        public int LeadMinutes { get; set; }
        public string ParentDeliveryMethodCode { get; set; }
        public string DeliveryLegCode { get; set; }
    }
    public class InsertDeliveryLegLeadTimeViewModel
    {
        [Required]
        public string DeliveryLegCode { get; set; }
        [Required]
        public string DeliveryMethodCode { get; set; }
        public int LeadMinutes { get; set; }
    }
    public class GetLocationLeadTimeViewModel
    {
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public string NameLocationFrom { get; set; }
        public string NameLocationTo { get; set; }
        public string DeliveryLegCode { get; set; }
        public string DeliveryLegName { get; set; }
    }
    public class GetDeliveryMethodViewModel
    {
        public string DeliveryMethodCode { get; set; }
        public string DeliveryMethodName { get; set; }
        public string ParentDeliveryMethodCode { get; set; }
    }
    public class ValitdateLeadTimeViewModel
    {
        public string DeliveryLegCode { get; set; }
        public string DeliveryMethodCode { get; set; }
    }
}
