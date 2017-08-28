using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class AfiRequestRevisiInsert
    {
        public int? afiApplicationId { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string branch { get; set; }
        public string carModelCode { get; set; }
        public string carModelName { get; set; }
        public string chassisModel { get; set; }
        public Region city { get; set; }
        public ExteriorColor color { get; set; }
        public int customerId { get; set; }
        public string customerName { get; set; }
        public string doDate { get; set; }
        public string frameNumber { get; set; }
        public string ktp { get; set; }
        public AFICarType model { get; set; }
        public AFICarType name { get; set; }
        public string postalCode { get; set; }
        public Region province { get; set; }
        public AFIRegion regionCodeAFI { get; set; }
        public string regionNameAFI { get; set; }
        public int? revisi { get; set; }
        public DateTime tanggalEfektif { get; set; }
        public int vehicleId { get; set; }
    }
}
