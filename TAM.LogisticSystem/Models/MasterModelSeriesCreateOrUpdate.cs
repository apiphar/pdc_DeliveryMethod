using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterModelSeriesCreateOrUpdate
    {
        public string BrandId { get; set; }
        public string carModelCode { get; set; }

        public string CarModelName { get; set; }

        public string CarSeriesCode { get; set; }

        public string Name { get; set; }

        public string CarSeriesName { get; set; }

        public string MasterModelSeriesId { get; set; }

        public string MasterModelName { get; set; }
    }
}
