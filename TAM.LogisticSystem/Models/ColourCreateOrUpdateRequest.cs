using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class ColourCreateOrUpdateRequest
    {
        [Required]
        [StringLength(4)]
        public string ColorCode { get; set; }

        [Required]
        public string ColorType { get; set; }

        [Required]
        [StringLength(256)]
        public string EnglishName { get; set; }

        [Required]
        [StringLength(256)]
        public string IndonesianName { get; set; }
    }
}
