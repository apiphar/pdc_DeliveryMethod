using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestDirectDeliveryModel
    {
        [Required]
        public DateTime EstimasiPDCOut { get; set; }
        public string EstimasiPDCOutView { get; set; }
        [Required]
        [MaxLength(255)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string CustomerName { get; set; }
        [Required]
        [MaxLength(255)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string CustomerAddress { get; set; }
        [Required]
        [MaxLength(255)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string CustomerCity { get; set; }
        [Required]
        [MaxLength(255)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string SalesmanName { get; set; }
        [Required]
        [MaxLength(255)]
        [Phone]
        public string SalesmanContactNo { get; set; }
        public bool ValidateDetail { get; set; }
    }
}
