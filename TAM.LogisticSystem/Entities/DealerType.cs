using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class DealerType
    {
        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        [Key]
        public string DealerTypeCode { get; set; }

        public string Name { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
