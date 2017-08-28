using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class LocationViewModel
    {
        [Required]
        [MaxLength(16)]
        [RegularExpression("^[a-zA-Z0-9]*$")]
        public string LocationCode { set; get; }
        [Required]
        [MaxLength(255)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string LocationName { set; get; }
        [Required]
        [MaxLength(255)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string Alamat { set; get; }
        [Required]
        public string CityForLegCode { set; get; }
        public string CityForLegName { set; get; }
        [Required]
        public string CityForShipmentCode { set; get; }
        public string CityForShipmentName { set; get; }
        [Required]
        public string LocationTypeCode { set; get; }
        public string LocationTypeName { set; get; }
        public bool CetakSJKB { get; set; }
    }
}
