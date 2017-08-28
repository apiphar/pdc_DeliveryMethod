using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class AfiRequestInsertUpload
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string BranchCodeAFI { get; set; }
        public string BranchCode { get; set; }
        public string Chassis { get; set; }
        public string City { get; set; }
        public string CustomerName { get; set; }
        public string FrameNo { get; set; }
       
        public string Ktp { get; set; }
        public string PostCode { get; set; }
        public string Province { get; set; }
        
        public string RegionCodeAFI { get; set; }
        public DateTime? EffectiveDate { get; set; }
        
        public int? VehicleId { get; set; }
        
        
    }
}
