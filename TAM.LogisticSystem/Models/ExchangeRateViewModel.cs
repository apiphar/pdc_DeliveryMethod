using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class ExchangeRateViewModel
    {
        public int ExchangeRateId { get; set; }
        [Required]
        public string CurrencySymbol { get; set; }
        public string Name { get; set; }
        [Required]
        public DateTime ValidFrom { get; set; }
        [Required]
        public DateTime ValidUntil { get; set; }
        [Required]
        public Decimal ToRupiah { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
