using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterWarnaVehicleColorModel
    {
        [Required]
        public string KodeWarna { get; set; }
        [Required]
        public string DeskripsiWarnaInd { get; set; }
        [Required]
        public string DeskripsiWarnaEng { get; set; }
    }
}
