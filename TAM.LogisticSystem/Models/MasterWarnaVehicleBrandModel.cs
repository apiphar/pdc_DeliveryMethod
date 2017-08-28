using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterWarnaVehicleBrandModel
    {
        [Required]
        public string KodeBrand { get; set; }
        public string BrandName { get; set; }
    }
}
