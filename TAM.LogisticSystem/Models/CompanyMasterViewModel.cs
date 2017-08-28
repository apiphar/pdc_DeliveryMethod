using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class CompanyMasterViewModel
    {
        public string CompanyCode { get; set; }
        public string NPWPAddress { get; set; }
        public string NPWP { get; set; }
        public string SAPCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string DealerCode { get; set; }
        public string TradeName { get; set; }
        public Boolean IsDealerFinancing { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
