using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class ProcessTailTemplateMapping
    {
        public string BranchCode { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string Katashiki { get; set; }

        public string ProcessTailTemplateCode { get; set; }

        [Key]
        public int ProcessTailTemplateMappingId { get; set; }

        public string Suffix { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
