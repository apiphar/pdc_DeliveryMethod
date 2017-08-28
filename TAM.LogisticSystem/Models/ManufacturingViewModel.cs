using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class ManufacturingViewModel
    {
        public string Country { get; set; }

        public string Name { get; set; }

        [Key]
        public string PlantCode { get; set; }
    }
}
