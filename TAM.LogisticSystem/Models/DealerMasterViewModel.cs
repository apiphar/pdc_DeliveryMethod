using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class DealerMasterViewModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9]*$")]
        public string DealerCode { get; set; }
        [Required]
        [MaxLength(255)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string DealerName { get; set; }
        [Required]
        [MaxLength(255)]
        public string DealerAddress { get; set; }
        [Required]
        public DealerMasterTypeCode DealerTypeCode { get; set; }

    }
}
