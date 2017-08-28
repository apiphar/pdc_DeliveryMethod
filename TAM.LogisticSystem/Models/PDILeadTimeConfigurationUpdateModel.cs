using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class PDILeadTimeConfigurationUpdateModel
    {
        [Required]
        public int PDIKatsuDictionaryDetailId { get; set; }
        [Required]
        [StringLength(16)]
        public string LocationCode { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$")]
        public int TotalTaktTimes { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$")]
        public int Post { get; set; }
    }
}
