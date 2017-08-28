using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DwellingTimeViewModel
    {
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public string LocationNameFrom { get; set; }
        public string LocationNameTo { get; set; }
        public int LeadMinutes { get; set; }
    }

    public class GetDwellingLocationViewModel {
        public string LocationCode { get; set; }
        public string Name { get; set; }
    }
    public class InsertDwellingViewModel
    {
        [Required]
        [RegularExpression("^[A-Za-z0-9]*$")]
        public string LocationFrom { get; set; }
        [Required]
        [RegularExpression("^[A-Za-z0-9]*$")]
        public string LocationTo { get; set; }
        [Required]
        [Range(0, 143999)] //max 99 hari, 23 jam, 59 menit = 143999 menit
        public int LeadMinutes { get; set; }
    }
}
