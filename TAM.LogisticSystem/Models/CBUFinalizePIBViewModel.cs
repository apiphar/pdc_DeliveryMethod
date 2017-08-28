using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class CBUFinalizePIBViewModel
    {
        public string InvoiceNumber { get; set; }
        public string FrameNumber { get; set; }
        public string EngineNumber { get; set; }
        public string Katashiki { get; set; }
        public string GlobalSuffix { get; set; }
        public string LocalSuffix { get; set; }
        public string HSCode { get; set; }
        public string EDNumber { get; set; }
        public decimal Price { get; set; }
        [Required]
        public decimal PriceRupiah { get; set; }
        [Required]
        public decimal BM { get; set; }
        [Required]
        public decimal ImportValue { get; set; }
        [Required]
        public decimal PPH { get; set; }
        [Required]
        public decimal PPN { get; set; }
        [Required]
        public decimal PPNBM { get; set; }
    }
    public class CBUImportFinalizePIBViewModel
    {
        public string NoAju { get; set; }
        public DateTime AjuDate { get; set; }
        public int TotalQty { get; set; }
        public string CurrencySymbol { get; set; }
        public string Schema { get; set; }
        public decimal CurrencyRate { get; set; }
    }

    public class CurrencyViewModel
    {
        public string Currency { get; set; }
        public decimal NDPBM { get; set; }
    }

    public class PercentageViewModel
    {
        public decimal BeaMasukPercentage { get; set; }
        public string HSCode { get; set; }
        public string Schema { get; set; }
        public decimal PphPercentage { get; set; }
        public decimal PpnPercentage { get; set; }
        public decimal PpnBmPercentage { get; set; }
    }

    public class FinalizePIBViewModel{
        public List<CBUFinalizePIBViewModel> FinalizeTable { get; set; }
        public FinalizeInfoViewModel FinalizeInfo { get; set; }
    }

    public class FinalizeTableViewModel
    {
        [Required]
        public decimal PPHFinal { get; set; }
        [Required]
        public decimal PPNFinal { get; set; }
        [Required]
        public decimal PPNBMFinal { get; set; }
        [Required]
        public decimal ImportValueFinal { get; set; }
        [Required]
        public decimal BeaMasukFinal { get; set; }
        [Required]
        public decimal PriceRupiahFinal { get; set; }
    }

    public class FinalizeInfoViewModel
    {
        [Required]
        public string NomorAju { get; set; }
        [Required]
        public string SchemaFinal { get; set; }
        [Required]
        public DateTime TanggalAjuApproved { get; set; }
        [Required]
        public decimal CurrencyRateFinal { get; set; }
    }
}
