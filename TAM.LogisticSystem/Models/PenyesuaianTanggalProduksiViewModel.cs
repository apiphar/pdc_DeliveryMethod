using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class PenyesuaianTanggalProduksiViewModel
    {
        public int Id { get; set; }
        public string Plant { get; set; }
        public string Nama { get; set; }
        public bool BukanAkhirBulan { get; set; }
        public bool AkhirBulan { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
