using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class AccordionVehicleViewModel
    {
        // TIE: START
        // public List<RoutingDictionary> routingDictionary { set; get; }
        // TIE: END

        public List<CarModel> carModel { set; get; }
        public List<CarSeries> carSeries { set; get; }
        public List<CarType> carType { set; get; }
        public List<BranchModel> branch { set; get; }
        public List<Dealer> dealer { set; get; }
        public List<Company> company { set; get; }

        public List<KatashikiModel> katashiki { set; get; }


        // insert & update
        public int RoutingDictionaryId { get; set; }
        // public string BranchCode { get; set; }
        // public string Suffix { get; set; }
        // public string Katashiki { get; set; }

        // tambahan dari DB baru
        public DateTime ValidFrom { get; set; }


        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string CarModelCode { get; set; }
        public string CarModelName { get; set; }
    }
}
