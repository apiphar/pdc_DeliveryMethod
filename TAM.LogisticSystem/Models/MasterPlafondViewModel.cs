using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterPlafondViewModel
    {
        public int PlafondMasterId { get; set; }
        [Required]
        public string KodeCompany { get; set; }
        [Required]
        public decimal Plafond { get; set; }
    }

    public class UpdateMasterPlafondViewModel
    {
        public int PlafondMasterId { get; set; }
        [Required]
        public string KodeCompany { get; set; }
        [Required]
        public decimal Plafond { get; set; }
        public decimal Outstanding { get; set; }
        public decimal Balance { get; set; }
    }

    public class CompanyCodeMasterPlafondViewModel
    {
        public string KodeCompany { get; set; }
        public string Name { get; set; }
    }
}
