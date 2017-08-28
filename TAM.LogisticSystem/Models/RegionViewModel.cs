using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class RegionViewModel
    {
        [Required, MaxLength(16), RegularExpression("^[A-Za-z0-9]+$")]
        public string RegionCode { set; get; }
        [Required, MaxLength(4), RegularExpression("^[A-Za-z0-9-.&,/' ]+$")]
        public string RegionType { set; get; }
        [Required, MaxLength(255), RegularExpression("^[a-zA-Z0-9-.&,/' ]+$")]
        public string Name { set; get; }
        public Region ParentCode { set; get; }
        [Required, MaxLength(8), RegularExpression("^[0-9]+$")]
        public string PostCode { set; get; }
    }
}
