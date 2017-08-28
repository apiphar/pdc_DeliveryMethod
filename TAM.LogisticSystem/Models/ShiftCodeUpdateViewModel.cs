using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class ShiftCodeUpdateViewModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        [MaxLength(255)]
        public string Description { get; set; }
    }
}
