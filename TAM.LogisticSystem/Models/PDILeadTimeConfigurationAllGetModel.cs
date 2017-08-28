using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class PDILeadTimeConfigurationAllGetModel
    {
        public List<PDILeadTimeConfigurationViewModel> PDILeadTimeConfigurationViewModels { get; set; }
        public List<PDILeadTimeConfigurationCarModelViewModel> PDILeadTimeConfigurationCarModels { get; set; }
        public List<PDILeadTimeConfigurationCarSeriesViewModel> PDILeadTimeConfigurationCarSeries { get; set; }
        public List<PDILeadTimeConfigurationCarTypeViewModel> PDILeadTimeConfigurationCarTypes { get; set; }
        public List<PDILeadTimeConfigurationKatashikiModel> PDILeadTimeConfigurationKatashikis { get; set; }
        public List<PDILeadTimeConfigurationLocationViewModel> PDILeadTimeConfigurationLocations { get; set; }
    }
}
