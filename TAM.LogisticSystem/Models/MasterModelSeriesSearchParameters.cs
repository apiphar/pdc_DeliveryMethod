using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Helpers;

namespace TAM.LogisticSystem.Models
{
    public class MasterModelSeriesSearchParameters : BasicSearchParameters
    {
        public string Query { set; get; }

        public string BrandId { set; get; }
        public string CarModelId { set; get; }
        public string Name { set; get; }
        public string CarSeriesId { set; get; }
    }
}
