using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class SerahTerimaGesekanInputViewModel
    {
        [Required]
        public List<int> VehicleId { get; set; }
        [Required]
        [RegularExpression("^[A-Za-z0-9]*$")]
        [MaxLength(16)]
        public string NoSurat { get; set; }
        [Required]
        public DateTime Tanggal { get; set; }
        [Required]
        public List<SerahTerimaGesekanExcelViewModel> ExcelModel { get; set; }
    }
}
