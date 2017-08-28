using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class AfiHOApprovalSearch
    {
        public string frameNo { get; set; }
        [Required]
        public string type { get; set; }
        public AfiBranchViewModel branch { get; set; }
        public DateTime? tanggalPengajuan { get; set; }
        public DateTime? sampai { get; set; }
        [Required]
        public string statusPengajuan { get; set; }
    }
    
}
