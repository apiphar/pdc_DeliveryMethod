using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryUnitAdvanceViewModel
    {
        [Required]
        [RegularExpression("^([a-zA-Z0-9]+)$")]
        public string FrameNumber { get; set; }
        [Required]
        public string Katashiki { get; set; }
        [Required]
        public string Suffix { get; set; }
        [Required]
        public string Tipe { get; set; }
        [Required]
        public string Branch { get; set; }
        [Required]
        public DateTimeOffset RequestedPDD { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Warna { get; set; }
        [Required]
        public bool CustomerAssign { get; set; }
    }
}
