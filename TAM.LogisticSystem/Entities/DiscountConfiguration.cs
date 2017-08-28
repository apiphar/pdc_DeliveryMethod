using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class DiscountConfiguration
    {
        public decimal Amount { get; set; }

        public decimal AmountRunning { get; set; }

        public decimal? Budget { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string DealerCode { get; set; }

        public DateTime? EndPeriod { get; set; }

        public bool IsBudget { get; set; }

        public bool IsPeriod { get; set; }

        public string Katashiki { get; set; }

        [Key]
        public string NomorSurat { get; set; }

        public DateTime? StartPeriod { get; set; }

        public string Suffix { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
