using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Helpers;

namespace TAM.LogisticSystem.Models
{
    public class ColourSearchParameters : BasicSearchParameters
    {
        public string Query { set; get; }
        public string ColorId { set; get; }
        public string ColorType { get; set; }
        public string EnglishName { get; set; }
        public string IndonesianName { get; set; }
    }
}
