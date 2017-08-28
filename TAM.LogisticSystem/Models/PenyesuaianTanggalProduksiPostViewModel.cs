using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class PenyesuaianTanggalProduksiPostViewModel
    {
        [Required]
        public string Plant { get; set; }
        [Required]
        public bool BukanAkhirBulan { get; set; }
        [Required]
        public bool AkhirBulan { get; set; }
        [Required]
        public DateTime DateStart { get; set; }
        [Required]
        public DateTime DateEnd { get; set; }
    }
}
