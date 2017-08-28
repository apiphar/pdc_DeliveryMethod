using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class PDILeadTimeConfigurationCreateModel
    {
        [Required]
        [StringLength(16)]
        public string LocationCode { get; set; }
        [Required]
        [StringLength(32)]
        public string Katashiki { get; set; }
        [Required]
        [StringLength(4)]
        public string Suffix { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$")]
        public int TotalTaktTimes { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$")]
        public int Post { get; set; }
    }
}
