using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class GesekNoRangkaData
    {
        public int VehicleId { get; set; }
        public string Katashiki { get; set; }
        public string Suffix { get; set; }
        public string Color { get; set; }
        public string Jenis { get; set; }
        public string Model { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string lokasi { get; set; }
        public int JumlahGesek { get; set; }
        public DateTimeOffset ETDPDC { get; set; }
        public bool HasCustomer { get; set; }
        public DateTimeOffset RequestedPDD { get; set; }
    }
}
