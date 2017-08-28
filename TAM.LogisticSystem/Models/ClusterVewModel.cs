using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class ClusterViewModel
    {
        [Required]
        [StringLength(16)]
        public string AS400ClusterCode { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}
