using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Helpers;

namespace TAM.LogisticSystem.Models
{
    public class InspectionPartViewModel
    {
        [Key]
        public int InspectionPartId { get; set; }

        public string Name { get; set; }

        public int InspectionCategoryId { get; set; }

		public string Category { get; set; }

		public int InspectionSideId { get; set; }

		public string SideName { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }
		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

	}
}
