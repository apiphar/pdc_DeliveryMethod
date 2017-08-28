using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class PDCBranchModel
    {
        [Key]
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        //public string LocationCode { get; set; }
        //public string LocationName { get; set; }
        public string BranchData { get; set; }
    }
}
