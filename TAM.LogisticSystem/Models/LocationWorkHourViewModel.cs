using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class LocationWorkHourViewModel
    {
        [Required]
        public string WorkHourTemplateCode { get; set; }
        [Required]
        public string LocationCode { get; set; }
        [Required]
        public DateTime ValidFrom { get; set; }
        [Required]
        public DateTime ValidTo { get; set; }
    }
}
