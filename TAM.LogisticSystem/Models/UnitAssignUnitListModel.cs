using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class UnitAssignUnitListModel
    {
        [Required]
        public string FrameNumber { get; set; }
        public string Katashiki { get; set; }
        public string Suffix { get; set; }
        public string Model { get; set; }
        public string Tipe { get; set; }
        public string Warna { get; set; }
        public string Branch { get; set; }
        public bool CustomerAssign { get; set; }
        public DateTimeOffset RequestedPDD { get; set; }
        public string RequestedPDDString { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public int VehicleId { get; set; }
        public int StatusId { get; set; }
        public DateTimeOffset ETD { get; set; }
        public string ETDString { get; set; }
    }
}
