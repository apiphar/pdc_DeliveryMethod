using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterModelCreateOrUpdate
    {
        [Required]
        public string BrandCode { set; get; }
        [Required]
        public string MasterModelId { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public string PlantCode { set; get; }

       
    }
}
