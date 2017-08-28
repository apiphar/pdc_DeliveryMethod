using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterModelSeriesDelete
    {
        public string BrandId { get; set; }
        public string CarModelId { get; set; }

        public string CarSeriesId { get; set; }

        public string CarSeriesName { get; set; }
        public string CarModelName { get; set; }

        public string Name { get; set; }
    }
}
