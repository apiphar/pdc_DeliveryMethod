using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DownloadDccpReadinessVolumeExcelModel
    {
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public string LocationToName { get; set; }
        public int Trip { get; set; }
        public int Load { get; set; }
        public string ShiftCode { get; set; }
        public int? Adjusted { get; set; }
        public int Quantity { get; set; }
        public int EstimatedUnit { get; set; }
    }
}
