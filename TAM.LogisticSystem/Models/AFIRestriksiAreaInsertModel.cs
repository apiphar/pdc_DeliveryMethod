using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class AFIRestriksiAreaInsertModel
    {
        [Required]
        public string RegionCode { get; set; }
        [Required]
        public bool IsLocked { get; set; }
        [Required]
        public DateTimeOffset? ValidFrom { get; set; }
        [Required]
        public DateTimeOffset? ValidTo { get; set; }
    }
}
