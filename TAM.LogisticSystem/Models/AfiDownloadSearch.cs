using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class AfiDownloadSearch
    {
        public string FrameNo { get; set; }
        public int? Quantity { get; set; }
        public string Type { get; set; }
        public AfiBranchViewModel Branch { get; set; }
        public DateTime? TanggalPengajuan { get; set; }
        public DateTime? Sampai { get; set; }
        public string StatusPengajuan { get; set; }
        public string Revisi { get; set; }
    }
}
