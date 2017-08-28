using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Helpers;

namespace TAM.LogisticSystem.Models
{
    public class DefectMaintenanceSearchParameters : BasicSearchParameters
    {
        public int DefectId { get; set; }
        public string Name { get; set; }
    }
}
