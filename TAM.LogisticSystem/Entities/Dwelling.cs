using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class Dwelling
    {
        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public int LeadMinutes { get; set; }

        public string LocationFrom { get; set; }

        public string LocationTo { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
