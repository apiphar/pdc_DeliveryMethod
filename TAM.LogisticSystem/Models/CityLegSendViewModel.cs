using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class CityLegSendViewModel
    {
        [Required]
        [MaxLength(16)]
        [RegularExpression("^[A-Za-z0-9]*$")]
        public string CityLegCode { get; set; }
        [Required]
        [MaxLength(255)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string CityLegName { get; set; }
        [Required]
        public string CityFrom { get; set; }
        [Required]
        public string CityTo { get; set; }
        [Required]
        public bool CalculatingSwappingCost { get; set; }
    }
}
