using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestCarModel
    {
        [Required]
        [MaxLength(17)]
        [RegularExpression("^[A-Za-z0-9]*$")]
        public string FrameNumber { get; set; }
        public int VehicleId { get; set; }
        public string Katashiki { get; set; }
        public string Suffix { get; set; }
        public string Model { get; set; }
        public string Tipe { get; set; }
        public string Warna { get; set; }
        public string Branch { get; set; }
        public string BranchCode { get; set; }
        public DateTimeOffset RequestedPdd { get; set; }
        public DateTimeOffset EstimatedPdcIn { get; set; }
        public bool CustomerAssign { get; set; }
        public string PosisiTerakhir { get; set; }
        public string LokasiTerakhir { get; set; }
        public DateTimeOffset? ScanTime { get; set; }
    }
}
