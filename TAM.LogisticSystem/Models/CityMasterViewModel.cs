using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class CityMasterViewModel
    {
        [Required]
        [MaxLength(16)]
        [RegularExpression("^[A-Za-z0-9]*$")]
        public string CityCode { get; set; }
        [Required]
        [MaxLength(255)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string Name { get; set; } 
    }
}
