using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class WorkHourPageViewModel
    {
        public List<WorkHourTemplateViewModel> WorkHourTemplates { get; set; }
        public List<WorkHourTemplateDetailViewModel> WorkHourTemplatesDetail { get; set; }
        public List<ShiftViewModel> ShiftList { get; set; }
    }
}
