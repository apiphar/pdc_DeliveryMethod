using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterRegionAFIViewModel
    {
        public string RegionCode { get; set; }
        public string PostCode { get; set; }
        public string Kelurahan { get; set; }
        public string Kota { get; set; }
        public string ParentRegionCode { get; set; }
        public string AFIRegionCode { get; set; }
        public string AFIRegionName { get; set; }
        public string AFIRegion { get; set; }
        public string Name { get; set; }
    }
    public class MasterRegionAFIPostCodeModel
    {
        public string PostCode { get; set; }
    }
}
