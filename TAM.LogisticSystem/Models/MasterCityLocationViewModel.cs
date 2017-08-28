using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterCityLocationViewModel
    {
        [Required]
        [MaxLength(16)]
        [RegularExpression("^([a-zA-Z0-9]+)$")]
        public string KodeCityLocation { get; set; }
        [Required]
        [MaxLength(255)]
        [RegularExpression(@"^([a-zA-Z0-9\s-/.,'&]+)$")]
        public string CityLocation { get; set; }
    }
}
