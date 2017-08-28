using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class CityLegViewModel
    {
        public string CityLegCode { get; set; }
        public string CityLegName { get; set; }
        public string CityFrom { get; set; }
        public string CityTo { get; set; }
        public string CalculatingSwappingCost { get; set; }
    }
}
