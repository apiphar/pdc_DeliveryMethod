using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Helpers;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Models
{
    public class MasterModelSearchResult
    {
        
        public string CarModelCode{ get; set; }
        
        public string Name { get; set; }

        public string MasterName { get; set; }
       
        public string BrandCode { get; set; }

        public string BrandName { get; set; }

        public int ManufacturingId { get; set; }

        public string ManufacturingName { get; set; }

        public string Query { get; set; }

      public string PlantCode { get; set; }
        public string PlantName { get; set; }
    }
}
