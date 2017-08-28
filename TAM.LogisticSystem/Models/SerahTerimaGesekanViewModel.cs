using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class SerahTerimaGesekanViewModel
    {
        public int ScratchId { get; set; }
        public int VehicleId { get; set; }
        public string FrameNumber { get; set; }
        public DateTimeOffset TanggalGesek { get; set; }
        public int JumlahGesek { get; set; }
        public string Lokasi { get; set; }
        public string Katashiki { get; set; }
        public string Suffix { get; set; }
        public string ModelName { get; set; }
        public string Color { get; set; }
        public string Branch { get; set; }
        public bool CustomerAssign { get; set; }
        public DateTimeOffset RequestedPDD { get; set; }
    }
}
