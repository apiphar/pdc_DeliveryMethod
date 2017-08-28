using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterCompanyInsertUpdateModel
    {
        [Required]
        [StringLength(16)]
        [RegularExpression("^[A-Za-z0-9]*$")]
        public string CompanyCode { get; set; }
        [Required]
        [StringLength(16)]
        public string DealerCode { get; set; }
        [Required]
        [StringLength(255)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string CompanyName { get; set; }
        [Required]
        [StringLength(255)]
        public string NPWPAddress { get; set; }
        [StringLength(32)]
        [RegularExpression("^[0-9]*$")]
        public string Phone { get; set; }
        [StringLength(32)]
        [RegularExpression("^[0-9]*$")]
        public string Fax { get; set; }
        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        public string TradeName { get; set; }
        [Required]
        [StringLength(32)]
        [RegularExpression("^[0-9.-]*$")]
        public string NPWP { get; set; }
        [Required]
        public bool IsDealerFinancing { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$")]
        public int TermOfPaymentDay { get; set; }
        [Required]
        [StringLength(8)]
        [RegularExpression("^[A-Za-z0-9]*$")]
        public string SAPCode { get; set; }
    }
}
