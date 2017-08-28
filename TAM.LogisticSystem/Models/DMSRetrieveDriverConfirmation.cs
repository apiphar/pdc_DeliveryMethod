using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{    
    public class DMSRetrieveDriverConfirmation
    {
        public string FrameNumber { get; set; }
        public string ConfirmationCode { get; set; }
        public string DriverId { get; set; }
        public DateTime? PickUpDateTime { get; set; }
    }
}
