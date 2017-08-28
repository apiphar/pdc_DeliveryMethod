using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class InspectionItemCreateOrUpdateRequest
    {
        [Key]
        public int InspectionItemId { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public int InspectionSideId { get; set; }
    }
}
