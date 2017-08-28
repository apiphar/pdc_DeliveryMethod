using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterGroupDealerViewModel
    {
        [Required]
        [MaxLength(16)]
        [RegularExpression("^([a-zA-Z0-9]+)$")]
        public string KodeGroupDealer { get; set; }
        [Required]
        [MaxLength(255)]
        [RegularExpression(@"^([a-zA-Z0-9\s-/.,'&]+)$")]
        public string GroupDealer { get; set; }
    }    
}
