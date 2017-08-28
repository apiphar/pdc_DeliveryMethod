using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class BreakHourTemplateDetail
    {
        public string BreakHourTemplateCode { get; set; }

        [Key]
        public int BreakHourTemplateDetailId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public bool IsFriday { get; set; }

        public bool IsMonday { get; set; }

        public bool IsSaturday { get; set; }

        public bool IsSunday { get; set; }

        public bool IsThursday { get; set; }

        public bool IsTuesday { get; set; }

        public bool IsWednesday { get; set; }

        public string ShiftCode { get; set; }

        public TimeSpan TimeFinish { get; set; }

        public TimeSpan TimeStart { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
