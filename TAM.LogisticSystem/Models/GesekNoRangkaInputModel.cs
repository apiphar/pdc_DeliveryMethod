using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class GesekNoRangkaInputModel
    {
        [Required]
        public int VehicleId { get; set; }
        [Required]
        public string LocationCode { get; set; }
    }
}
