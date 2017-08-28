using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class PDILeadTime
    {
        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string Katashiki { get; set; }

        public string LocationCode { get; set; }

        [Key]
        public int PDILeadTimeId { get; set; }

        public int Post { get; set; }

        public string Suffix { get; set; }

        public int TaktSeconds { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
