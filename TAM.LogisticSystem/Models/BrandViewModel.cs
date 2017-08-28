using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class BrandViewModel
    {
        [StringLength(16)]
        public string BrandCode { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}
