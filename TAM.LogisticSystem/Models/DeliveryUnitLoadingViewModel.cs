using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryUnitLoadingViewModel
    {
        public int Capacity { get; set; }
        public string VoyageNumber { get; set; }
        public string Vessel { get; set; }
        public  string Vendor { get; set; }
        public DateTimeOffset EstimationDeparture { get; set; }
        public int  TotalUnitAssign { get; set; }
        public int TotalUnitPreBookedInPorted { get; set; }
        public int TotalUnitPreBookedNotPorted { get; set; }
    }
}
