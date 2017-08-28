using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Helpers;

namespace TAM.LogisticSystem.Models
{
    public class InspectionItemViewModel
    {
        [Key]
        public int InspectionItemId { get; set; }

        public string ItemName { get; set; }

        public string Category { get; set; }

        public int InspectionSideId { get; set; }

        public string SideName { get; set; }

    }
}
