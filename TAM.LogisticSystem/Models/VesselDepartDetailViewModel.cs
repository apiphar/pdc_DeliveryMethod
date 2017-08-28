using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class VesselDepartDetailViewModel
    {
        public string VoyageNumber { get; set; }
        public string Vendor { get; set; }
        public string Vessel { get; set; }
        public DateTimeOffset EstimatedTimeDeparture { get; set; }
        public int Capacity { get; set; }
        public int Loaded { get; set; }
        public int Assigned { get; set; }
        public int PreBookPorted { get; set; }
        public int PreBookNotPorted { get; set; }
        public string VoyageStatus { get; set; }
        public int UnitListId { get; set; }
    }
}
