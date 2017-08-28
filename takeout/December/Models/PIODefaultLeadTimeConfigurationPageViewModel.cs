using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class PIODefaultLeadTimeConfigurationPageViewModel
    {
        public List<PIODefaultLeadTimeConfigurationViewModel> PIODefaultLeadTimes { get; set; }
        public List<PIODefaultLeadTimeConfigurationLocationModel> Locations { get; set; }
        public List<ProcessForLine> ProcessStatusList { get; set; }
    }
}
