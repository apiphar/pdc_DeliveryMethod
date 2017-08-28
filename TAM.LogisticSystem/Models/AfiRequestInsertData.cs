using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class AfiRequestInsertData
    {
        [Required]
        [MaxLength(30)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string Name { get; set; }
        public int CustomerId { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression("^[A-Za-z0-9]*$")]
        public string Ktp { get; set; }
        [Required]
        [MaxLength(30)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string Address1 { get; set; }
        [Required]
        [MaxLength(30)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string Address2 { get; set; }
        [Required]
        [MaxLength(30)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string Address3 { get; set; }
        [Required]
        public Region City { get; set; }
        [Required]
        public Region Province { get; set; }
        [Required]
        [MaxLength(5)]
        [RegularExpression("^[0-9]*$")]
        public string PostalCode { get; set; }
        [Required]
        public AFIRegion RegionAFI { get; set; }
        [Required]
        public DateTime TanggalEfektif { get; set; }
        [Required]
        public int VehicleId { get; set; }
        public string Branch { get; set; }
        public string Chassis { get; set; }
        [Required]
        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string Color { get; set; }
        public string Jenis { get; set; }
        public string Model { get; set; }
    }
}
