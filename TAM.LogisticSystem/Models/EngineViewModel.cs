using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class EngineViewModel
    {
        public string EnginePrefix { get; set; }

        public string FrameCode { get; set; }

        public string Katashiki { get; set; }

        public string CarModelId { get; set; }

        public string CarModelName { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
        [Key]
        public int KatashikiValidationId { get; set; }
    }
}
