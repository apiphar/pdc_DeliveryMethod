using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class MasterCompanyViewModel
    {
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string NPWPAddress { get; set; }
        public string SAPCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string TradeName { get; set; }
        public string NPWP { get; set; }
        public bool IsDealerFinancing { get; set; }
        public int? TermOfPaymentDay { get; set; }
        
    }
}
