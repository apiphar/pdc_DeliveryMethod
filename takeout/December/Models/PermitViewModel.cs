using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class PermitViewModel : IValidatableObject
    {
       

        [Required(ErrorMessage = "Permit Id must be filled.")]
        public int PermitId { get; set; }

        [Required(ErrorMessage = "Quota must be filled.")]
        public int Quota { get; set; }

        public string Name { get; set; }

        public string Katashiki { get; set; }
        public string Suffix { get; set; }
        public string CarModelCode { get; set; }

        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true)]
        public DateTime EffectiveFrom { get; set; }

        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true)]
        public DateTime EffectiveUntil { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EffectiveUntil < EffectiveFrom)
            {
                yield return
                    new ValidationResult(errorMessage: "End Date must be greater than Start Date", memberNames: new[] { "EffectiveUntil" });
            }
        }
    }
}
