using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class Branch
    {
        public string AS400BranchCode { get; set; }

        public string AS400ClusterCode { get; set; }

        [Key]
        public string BranchCode { get; set; }

        public string CompanyCode { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string DestinationCode { get; set; }

        public string Fax { get; set; }

        public string KabupatenCode { get; set; }

        public int MasterDataPrimaryKey { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string RegionCode { get; set; }

        public string SalesAreaCode { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
