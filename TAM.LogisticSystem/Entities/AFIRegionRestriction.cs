using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class AFIRegionRestriction
    {
        [Key]
        public int AFIRegionRestrictionId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public bool IsLocked { get; set; }

        public string RegionCode { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public DateTimeOffset? ValidFrom { get; set; }

        public DateTimeOffset? ValidTo { get; set; }
    }
}
