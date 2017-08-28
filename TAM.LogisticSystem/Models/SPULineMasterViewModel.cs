using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class SPULineMasterViewModel
    {
        public List<SPULineMasterModel> SpuLineMaster { get; set; }
        public List<Location> Location { get; set; }
    }
}
