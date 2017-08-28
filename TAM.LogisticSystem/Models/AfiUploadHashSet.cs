using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class AfiUploadHashSet
    {
        public HashSet<string> AppliedFrameSet { get; set; }
        public HashSet<string> ExistedFrameSet { get; set; }
        public HashSet<string> ProvinsiSet { get; set; }
        public HashSet<string> KotaSet { get; set; }
        public HashSet<string> RegionAFISet { get; set; }
        public List<string> ExcelFrameList { get; set; }
    }
}
