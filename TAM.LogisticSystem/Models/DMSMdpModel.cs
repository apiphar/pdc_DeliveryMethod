using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DMSMdpModel
    {
        public string RRN { get; set; }
        public string Katashiki { get; set; }
        public string Suffix { get; set; }
        public string ExteriorColorCode { get; set; }
        public DateTime DTPLOD { get; set; }
        public string Outlet { get; set; }

    }
    public class DMSStatusMdpModel
    {
        public string RRN { get; set; }
        public DateTime DTPLOD { get; set; }
        public bool Status { get; set; }
    }
}
