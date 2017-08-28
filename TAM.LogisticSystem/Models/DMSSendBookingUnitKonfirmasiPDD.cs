using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DMSSendBookingUnitKonfirmasiPDD
    {
        public string RRN { get; set; }
        public DateTime? DTPLOD { get; set; }
        public DateTime RequestedPDD { get; set; }
        public string FrameNumber { get; set; }
        public string PaketAksesorisTAM { get; set; }
        public int DeliveryCategory { get; set; }
    }
}
