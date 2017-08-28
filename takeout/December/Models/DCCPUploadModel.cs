using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DCCPUploadModel
    {
        public DateTime TransInOutDate { get; set; }
        public string LocationFrom { get; set; }
    	public string LocationTo { get; set; }
        public int Trip { get; set; }
        public int Load { get; set; }
        public string ShiftCode { get; set; }
        public int UnitReadyQuantity { get; set; }
        public int EstimatedUnit { get; set; }
    }
}
