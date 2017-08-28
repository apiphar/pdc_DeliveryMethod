using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class SPUDefaultLeadTimeConfigurationPageViewModel
    {
        public List<SPUDefaultLeadTimeConfigurationViewModel> SPUDefaultLeadTimes { get; set; }
        public List<SPUDefaultLeadTimeConfigurationLocationModel> Locations { get; set; }
        public List<ProcessForLine> ProcessStatusList { get; set; }
    }
}
