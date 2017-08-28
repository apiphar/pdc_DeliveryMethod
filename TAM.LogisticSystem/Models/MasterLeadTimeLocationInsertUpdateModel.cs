using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterLeadTimeLocationInsertUpdateModel
    {
        [Required]
        public string LocationCode { get; set; }
        [Required]
        public string ProcessMasterCode { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$")]
        public int LeadMinutes { get; set; }
    }
}
