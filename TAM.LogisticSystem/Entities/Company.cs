using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class Company
    {
        [Key]
        public string CompanyCode { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string DealerCode { get; set; }

        public string Email { get; set; }

        public string Fax { get; set; }

        public bool IsDealerFinancing { get; set; }

        public int MasterDataPrimaryKey { get; set; }

        public string Name { get; set; }

        public string NPWP { get; set; }

        public string NPWPAddress { get; set; }

        public string Phone { get; set; }

        public string SAPCode { get; set; }

        public int? TermOfPaymentDay { get; set; }

        public string TradeName { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
