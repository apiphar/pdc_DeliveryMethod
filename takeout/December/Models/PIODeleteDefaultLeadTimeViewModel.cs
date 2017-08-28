using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class PIODeleteDefaultLeadTimeViewModel
    {
        [Required]
        public string LocationCode { get; set; }
        [Required]
        public int ProcessForLineId { get; set; }
        [Required]
        public string RoutingMasterCode { get; set; }
    }
}
