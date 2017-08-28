using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class WorkHourTemplateDetailViewModel
    {
        [Required]
        public string WorkHourTemplateCode { get; set; }
        [Required]
        public ShiftViewModel ShiftCode { get; set; }
        [Required]
        public TimeSpan TimeStart { get; set; }
        [Required]
        public TimeSpan TimeFinish { get; set; }
        [Required]
        public bool IsMonday { get; set; }
        [Required]
        public bool IsTuesday { get; set; }
        [Required]
        public bool IsWednesday { get; set; }
        [Required]
        public bool IsThursday { get; set; }
        [Required]
        public bool IsFriday { get; set; }
        [Required]
        public bool IsSaturday { get; set; }
        [Required]
        public bool IsSunday { get; set; }

    }
}
