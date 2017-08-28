using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class ConfigurationPointPreBookVesselViewModel
    {
        public List<PointPreBookVesselListViewModel> PointPreBookVesselsList { get; set; }

        public List<string> LocationCodes { get; set; }

        //public List<string> PointPreBookVesselNames { get; set; }

        public List<MasterRoutingForPointPreBookVessel> MasterRoutingDataNeeded { get; set; }

        //public   List<string> Katashikis { get; set; }

        //  public List<string> Suffixes { get; set; }

        //  public List<string> Branches { get; set; }
    }
}
