using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class SPUDefaultLeadTimeConfigurationUpdateViewModel
    {
        [Required]
        public string OldLocationCode { get; set; }
        [Required]
        public int OldProcessForLineId { get; set; }
        [Required]
        public int TotalLeadTimeMinutes { get; set; }
        [Required]
        public string RoutingMasterCode { get; set; }
        public string NewLocationCode { get; set; }
        public int NewProcessForLineId { get; set; }
    }
}
