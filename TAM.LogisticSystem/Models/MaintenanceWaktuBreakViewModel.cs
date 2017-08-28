using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MaintenanceWaktuBreakViewModel
    {
        public int IdleTimeCustomId { get; set; }
        [Required]
        public string LocationCode { get; set; }
        public string Name { get; set; }

        [Required]
        public string ShiftCode { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime DateFrom { get; set;}
        public string ConvertDateFrom { get; set; }

        [Required]
        public DateTime DateTo { get; set; }
        public string ConvertDateTo { get; set; }
    }
}
