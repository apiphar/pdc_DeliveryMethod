using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class AS400FrameNumber
    {
        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset? DTPLOD { get; set; }

        public string ExteriorColorCode { get; set; }

        [Key]
        public string FrameNumber { get; set; }

        public int? IdNumber { get; set; }

        public string Katashiki { get; set; }

        public string RRN { get; set; }

        public string Suffix { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
