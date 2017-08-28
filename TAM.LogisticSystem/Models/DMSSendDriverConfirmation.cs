using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DMSSendDriverConfirmation
    {
        public string FrameNumber { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public DateTime? PickUpDateTime { get; set; }
    }
}
