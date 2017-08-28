using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class AfiRequestRevisiSearchRO
    {
        public string FrameNumber { get; set; }
        public DateTime? TanggalPengajuan { get; set; }
        public DateTime? Sampai { get; set; }
        public string RbStatus { get; set; }
    }
    
}
