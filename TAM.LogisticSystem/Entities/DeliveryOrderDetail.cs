using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class DeliveryOrderDetail
    {
        public string CancellationNumber { get; set; }

        public DateTimeOffset? CancelledAt { get; set; }

        public string CompanyCode { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string DebitAdviceCancellationNumber { get; set; }

        [Key]
        public int DeliveryOrderDetailId { get; set; }

        public string DeliveryOrderNumber { get; set; }

        public decimal DiscountPrice { get; set; }

        public decimal InvoicePrice { get; set; }

        public bool IsDutyFree { get; set; }

        public bool IsPPH22BarangMewah { get; set; }

        public bool IsSentToDMS { get; set; }

        public DateTimeOffset IssuedDate { get; set; }

        public decimal LuxuryTax { get; set; }

        public decimal NormalPrice { get; set; }

        public decimal PPH22 { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public decimal ValueAddedTax { get; set; }

        public int VehicleId { get; set; }

        public bool Waiver { get; set; }

        public decimal WholePriceBeforeTax { get; set; }
    }
}
