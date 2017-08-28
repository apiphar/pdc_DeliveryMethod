using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class CompanyPlafondMutation
    {
        public decimal Balance { get; set; }

        public string CompanyCode { get; set; }

        [Key]
        public int CompanyPlafondMutationId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string Description { get; set; }

        public decimal Outstanding { get; set; }

        public decimal Plafond { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public decimal Value { get; set; }
    }
}
