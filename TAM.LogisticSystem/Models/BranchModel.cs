using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class BranchModel
    {
        [Required]
        public string BranchCode { get; set; }

        [Required]
        public string SalesAreaCode { get; set; }

        [Required]
        public string CompanyCode { get; set; }

        [Required]
        public string LocationCode { get; set; }

        [Required]
        public string DestinationCode { get; set; }

        [Required]
        public string RegionCode { get; set; }

        [Required]
        public string AS400ClusterCode { get; set; }

        [Required]
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        [Required]
        public string AFIBranchCode { get; set; }

        [Required]
        public string AS400BranchCode { get; set; }

        [Required]
        public string KabupatenCode { get; set; }

        public int MasterDataPrimaryKey { get; set; }


        public string CompanyName { get; set; }

        public string LocationName { get; set; }

        public string DestinationName { get; set; }

        public string RegionName { get; set; }

        public string ClusterName { get; set; }

        public string SalesAreaName { get; set; }
    }

    public class SalesAreaModel
    {
        public string SalesAreaCode { get; set; }

        public string SalesAreaName { get; set; }
    }

    public class CompanyModel
    {
        public string CompanyCode { get; set; }

        public string CompanyName { get; set; }
    }

    public class LocationModel
    {
        public string LocationCode { get; set; }

        public string LocationName { get; set; }
    }

    public class DestinationModel
    {
        public string DestinationCode { get; set; }

        public string DestinationName { get; set; }
    }

    public class RegionModel
    {
        public string RegionCode { get; set; }

        public string RegionName { get; set; }
    }

    public class ClusterModel
    {
        public string AS400ClusterCode { get; set; }

        public string ClusterName { get; set; }
    }
}
