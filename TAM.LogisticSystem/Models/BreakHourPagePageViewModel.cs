using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class BreakHourPageViewModel
    {
        public List<BreakHourTemplateViewModel> BreakHourTemplates { get; set; }
        public List<BreakHourTemplateDetailViewModel> BreakHourTemplatesDetail { get; set; }
        public List<ShiftViewModel> ShiftList { get; set; }
    }
}
