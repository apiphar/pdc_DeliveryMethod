using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class ShiftCodeViewModel
    {
        [Required]
        [RegularExpression("^[A-Za-z0-9]*$")]
        [MaxLength(16)]
        public string ShiftCode { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        [MaxLength(255)]
        public string Description { get; set; }
    }
}
