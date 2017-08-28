using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class AfiRequestRevisiForm
    {
        public int? VehicleId { get; set; }
        public int? AFIApplicationId { get; set; }
        public int? AFIApplicationProcessId { get; set; }
        public string FrameNumber { get; set; }
        public DateTime? DODate { get; set; }
        public string Branch { get; set; }
        public string CarModelCode { get; set; }
        public string CarModelName { get; set; }
        public string Color { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public string ChassisModel { get; set; }



        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string KTP { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string RegionCodeAFI { get; set; }
        public string RegionNameAFI { get; set; }
        public string PostalCode { get; set; }
        public DateTime TanggalEfektif { get; set; }
    }
}
