using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class BreakHourSendViewModel
    {
        public BreakHourTemplateViewModel BreakHourTemplate { get; set; }
        public List<BreakHourTemplateDetailViewModel> BreakHourTemplatesDetail { get; set; }
    }
}
