using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterWarnaVehicleModelModel
    {
        public string KodeBrand { get; set; }
        [Required]
        public string KodeModel { get; set; }
        public string ModelName { get; set; }
    }
}
