using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class ManufacturingUpdateViewModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        [MaxLength(255)]
        public string Country { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z0-9]*$")]
        [MaxLength(8)]
        public string PlantCode { get; set; }
    }
}
