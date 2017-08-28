using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class PointPreBookVesselListViewModel
    {
        [Required]
        public string LocationCode { get; set; }
        //public string Katashiki { get; set; }
        //public string Suffix { get; set; }
        //public string Branch { get; set; }
        public string PointPreBookVesselName { get; set; }
        [Required]
        public string PointPreBookVesselId { get; set; }

    }

}
