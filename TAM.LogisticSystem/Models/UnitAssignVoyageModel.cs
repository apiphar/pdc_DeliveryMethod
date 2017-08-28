using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class UnitAssignVoyageModel
    {
        [Required]
        [RegularExpression("^[A-Za-z0-9]*$")]
        [MaxLength(16)]
        public string Voyage { get; set; }
        public int VoyageNodeSourceId { get; set; }//for pairing with Voyage Number, because for the save needed this (new table)
        public string Vendor { get; set; }
        public string Vessel { get; set; }
        public DateTimeOffset EstimationTimeDeparture { get; set; }
        public int CapacityVessel { get; set; }
        public int TotalUnitAssign { get; set; }
        public int TotalUnitPreBookedInPorted { get; set; }
        public int TotalUnitPreBookedNotPorted { get; set; }
        public int TotalAll { get; set; }

    }
}
