using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class SalesAreaViewModel
    {
        [RegularExpression("^([a-zA-Z0-9]+)$")]
        [MaxLength(16)]
		[Required]
        public string SalesAreaCode { set; get; }

        [RegularExpression(@"^([a-zA-Z0-9\s-.,&'/]+)$")]
        [MaxLength(255)]
        [Required]
        public string Description { set; get; }
    }
}
