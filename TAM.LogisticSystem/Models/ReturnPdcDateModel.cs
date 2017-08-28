using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class ReturnPdcDateModel
    {
        public DateTime Date { get; set; }
        public int LeadTimeDay { get; set; }
        public int LeadTimeHour { get; set; }
        public int LeadTimeMinute { get; set; }
    }
}
