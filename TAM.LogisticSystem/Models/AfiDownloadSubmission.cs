using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class AfiDownloadSubmission
    { 
        public int AFIApplicationId { get; set; }
        public string Katashiki { get; set; }
        public string FakturNo { get; set; }
        public DateTimeOffset EffectiveDate { get; set; }
        public string Model { get; set; }
        public string FrameNumber { get; set; }
        public string ModelName { get; set; }
        public string EngineNumber { get; set; }
        public string Color { get; set; }
        public string CustomerName { get; set; }
        public string ApplicationNo { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string NoId { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string TipePengajuan { get; set; }
        public string TipePengajuanName { get; set; }
        public string ReferenceNo { get; set; }

        public string DONumber { get; set; }
        public DateTimeOffset? DODate { get; set; }
        public string BranchCode { get; set; }
        public string BranchCodeAFI { get; set; }
        public string RegionCodeAFI { get; set; }

    }
}
