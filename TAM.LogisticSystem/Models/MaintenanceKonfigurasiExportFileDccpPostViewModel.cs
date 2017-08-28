using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MaintenanceKonfigurasiExportFileDccpPostViewModel
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public string RangeStart { get; set; }
        [Required]
        public string RangeEnd { get; set; }
    }
}
