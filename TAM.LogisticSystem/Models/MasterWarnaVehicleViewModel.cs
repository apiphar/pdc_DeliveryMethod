using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterWarnaVehicleViewModel
    {
        [Required]
        public string KodeWarnaVehicle { get; set; }
        [Required]
        public string KodeBrand { get; set; }
        public string BrandName { get; set; }
        public string BrandDetail { get; set; }
        [Required]
        public string KodeModel { get; set; }
        public string ModelName { get; set; }
        public string ModelDetail { get; set; }
        [Required]
        public string KodeWarna { get; set; }
        public string DeskripsiWarnaInd { get; set; }
        public string DeskripsiWarnaEng { get; set; }
    }
}
