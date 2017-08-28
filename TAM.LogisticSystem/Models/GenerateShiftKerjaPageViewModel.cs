using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class GenerateShiftKerjaPageViewModel
    {
        public List<WorkHourTemplateViewModel> WorkHourTemplates { get; set; }
        public List<string> LocationCodes { get; set; }
    }
}
