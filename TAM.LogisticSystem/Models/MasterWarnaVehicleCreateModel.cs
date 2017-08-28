using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterWarnaVehicleCreateModel
    {
        [Required]
        public string KodeWarnaVehicle { get; set; }
        [Required]
        public MasterWarnaVehicleBrandModel Brand { get; set; }
        [Required]
        public MasterWarnaVehicleModelModel Model { get; set; }
        [Required]
        public MasterWarnaVehicleColorModel Warna { get; set; }
    }
}
