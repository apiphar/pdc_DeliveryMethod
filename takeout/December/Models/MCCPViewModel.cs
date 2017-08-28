using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MCCPViewModel
    {
        public int Baris { set; get; }
        public String LokAsal { set; get; }
        public string LokAsalName { set; get; }
        public String LokTujuan { set; get; }
        public string LokTujuanName { set; get; }
        public string Keterangan { set; get; }
        public String SqlField { set; get; }
        public string DealerCode { set; get; }
        public string DealerName { set; get; }
        public string BranchCode { set; get; }
        public string BranchName { set; get; }

    }
}
