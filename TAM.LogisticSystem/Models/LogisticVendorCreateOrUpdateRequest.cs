using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class LogisticVendorCreateOrUpdateRequest
    {

        [StringLength(8)]
        public string SAPCode { get; set; }

        [StringLength(16)]
        public string Account { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(16)]
        [Required]
        public string CityCode { get; set; }

        public string DeliveryVendorCode { get; set; }

        [StringLength(32)]
        public string Fax { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string PaymentTerm { get; set; }

        [StringLength(32)]
        public string Phone { get; set; }

    }

}