using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class UnitListViewModel
    {
        public string FrameNo { get; set; }
        public string Katashiki { get; set; }
        public string Suffix { get; set; }
        public string Model { get; set; }
        public string Tipe { get; set; }
        public string Warna { get; set; }
        public string Branch { get; set; }
        public DateTimeOffset PdcIn { get; set; }
        public bool CustomerAssign { get; set; }
        public DateTimeOffset RequestedPdd { get; set; }
        public string Status { get; set; }
        public string VoyageNumber { get; set; }
    }
}
