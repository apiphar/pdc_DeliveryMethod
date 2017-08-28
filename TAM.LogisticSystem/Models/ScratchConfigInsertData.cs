using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class ScratchConfigInsertData
    {
        public int jumlahGesekan { get; set; }
        public List<List<string>> UpdateData { get; set; }
        public List<List<string>> InsertData { get; set; }
    }
}
