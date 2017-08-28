using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class UnitAssignDataModel
    {
        public UnitAssignVoyageModel AllVoyage { get; set; }
        public List<UnitAssignUnitListModel> AllUnit { get; set; }
    }
}
