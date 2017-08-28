using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MaintenaceShiftKerjaPostModel
    {
        public int LocationWorkHourId { get; set; }
        public string LocationCode { get; set; }
        public string ShiftCode { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset Finish { get; set; }
    }
}
