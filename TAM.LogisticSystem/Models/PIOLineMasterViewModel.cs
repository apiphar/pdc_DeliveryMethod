using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class PIOLineMasterViewModel
    {
        public List<PIOLineMasterModel> PioLineMaster { get; set; }
        public List<Location> Location { get; set; }
    }
}
