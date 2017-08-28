using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class KalenderkKerjaPolaKerjaDalamSemingguGenerateViewModel
    {
        public string LocationCode { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public string WorkingDictionaryCode { get; set; }

        public string Description { get; set; }

        public string Name { set; get; }

        public string LocationName { get; set; }
    }
}
