using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class FilterDateModel
    {
        public List<string> field { get; set; }
        public List<DateTime?> DateFrom { get; set; }
        public List<DateTime?> DateTo { get; set; }
    }
}
