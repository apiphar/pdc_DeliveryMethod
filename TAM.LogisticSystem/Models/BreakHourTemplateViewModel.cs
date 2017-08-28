using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class BreakHourTemplateViewModel
    {
        [Required]
        [MaxLength(8)]
        [RegularExpression("^[A-Za-z0-9]*$")]
        public string BreakHourTemplateCode { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
