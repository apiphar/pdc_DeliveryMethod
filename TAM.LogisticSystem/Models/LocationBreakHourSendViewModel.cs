using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class LocationBreakHourSendViewModel
    {
        [Required]
        public string BreakHourTemplateCode { get; set; }
        [Required]
        public GenerateHourLocationViewModel Location { get; set; }
        [Required]
        public DateTimeOffset ValidFrom { get; set; }
        [Required]
        public DateTimeOffset ValidTo { get; set; }
    }
}
