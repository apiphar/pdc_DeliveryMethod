using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class CarTypeViewModel
    {
        [Required]
        public string Katashiki { get; set; }

        [Required]
        public string Suffix { get; set; }

        [Required]
        public string CarSeriesCode { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string AfiCarTypeCode { get; set; }

        //[Required]
        //public string HSCode { get; set; }

        [Required]
        public string EngineDescription { get; set; }

        [Required]
        public string EngineVolume { get; set; }

        [Required]
        public string SteerPosition { get; set; }

        [Required]
        public string WheelDiameter { get; set; }

        [Required]
        public string WheelSize { get; set; }

        public string Assembly { get; set; }

        [Required]
        public bool IsFTZ { get; set; }
        public bool IsFreeTaxZone { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public string CarSeriesName { get; set; }
    }

    public class CarSeriesModel
    {
        public string CarSeriesCode { get; set; }

        public string CarSeriesName { get; set; }
    }

    public class AfiCarTypeModel
    {
        public string AfiCarTypeCode { get; set; }

        public string aficartypeName { get; set; }
    }
}
