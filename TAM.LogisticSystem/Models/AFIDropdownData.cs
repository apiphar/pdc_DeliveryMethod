using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class AFIDropdownData
    {
        public List<Region> Region { get; set; }
        public List<ExteriorColor> Color { get; set; }
        public List<AFICarType> CarCategory { get; set; }
    }
}
