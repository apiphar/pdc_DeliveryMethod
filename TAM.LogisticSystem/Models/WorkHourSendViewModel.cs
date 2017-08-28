using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class WorkHourSendViewModel
    {
        public WorkHourTemplateViewModel WorkHourTemplate { get; set; }
        public List<WorkHourTemplateDetailViewModel> WorkHourTemplatesDetail { get; set; }

    }
}
