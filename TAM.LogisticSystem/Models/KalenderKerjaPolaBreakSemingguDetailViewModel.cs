using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class KalenderKerjaPolaBreakSemingguDetailViewModel
    {
        public string ShiftCode { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-dd HH:mm:ss}")]
        public DateTime TimeFinish { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-dd HH:mm:ss}")]
        public DateTime TimeStart { get; set; }

        public string IdleDictionaryCode { get; set; }

       
        public int IdleDictionaryDetailId { get; set; }

        public List<int> Days { get; set; }

        public int Senin { get; set; }
        public int Selasa { get; set; }
        public int Rabu { get; set; }
        public int Kamis { get; set; }
        public int Jumat { get; set; }
        public int Sabtu { get; set; }
        public int Minggu { get; set; }

        public string ShiftName { get; set; }
       
    }
}
