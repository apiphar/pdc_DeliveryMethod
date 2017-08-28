using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryLegCreateOrUpdateRequest
    {
        [Required]
        [MaxLength(16)]
        [RegularExpression("^[A-Za-z0-9]*$")]
        public string DeliveryLegCode { get; set; }
        [Required]
        [MaxLength(255)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string Name { get; set; }
        [Required]
        public string LocationFrom { get; set; }
        [Required]
        public string LocationTo { get; set; }
        [Required]
        public string CityLegCode { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int BufferMinutes { get; set; }
        [Required]
        public bool NeedSJKB { get; set; }
    }
}
