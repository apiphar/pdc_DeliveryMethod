using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class PDILeadTimeConfigurationViewModel
    {
        public int PDILeadTimeId { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string Katashiki { get; set; }
        public string Suffix { get; set; }
        public int TaktSeconds { get; set; }
        public int Post { get; set; }

    }
}
