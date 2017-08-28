using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class DefectMaintenanceViewModel
    {
        public List<Defect> defects { get; set; }

        [Key]
        public int DefectId { get; set; }
   
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }


    }
}
