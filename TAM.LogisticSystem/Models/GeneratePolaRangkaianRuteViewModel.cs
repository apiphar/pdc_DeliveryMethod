using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class GeneratePolaRangkaianRuteViewModel
    {
        public string BranchCode { set; get; }
	    public string BranchName { set; get; }
	    public string Katashiki { set; get; }
	    public string Suffix { set; get; }
	    public string KodeTahapAwal { set; get; }
	    public string KodeTahapAkhir { set; get; }
    }
}
