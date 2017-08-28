using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryLegViewModel
    {
        public string DeliveryLegCode { get; set; }
        public string Name { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public string CityLegCode { get; set; }
        public int BufferMinutes { get; set; }
        public string NeedSJKB { get; set; }
    }
}
