using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{

    public class SchemeTable
    {
        public string Scheme { set; get; }
        public decimal BM { set; get; }
    }
    public class TariffViewModel
    {
        public string HSCode { get; set; }
        public string Scheme { get; set; }
        public decimal BM { get; set; }
        public decimal PPH { get; set; }
        public decimal PPn { get; set; }
        public decimal PPnBM { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public int TariffId { get; set; }
        public List<SchemeTable> schemeTable { set; get; }

    }
}
