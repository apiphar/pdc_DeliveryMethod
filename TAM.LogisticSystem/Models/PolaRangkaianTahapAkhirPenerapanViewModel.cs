using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class PolaRangkaianTahapAkhirPenerapanViewModel
    {
        public List<ProcessTailTemplate> RoutingDictionaryTail { set; get; }
        public List<CarModel> CarModel { set; get; }
        public List<CarSeries> CarSeries { set; get; }
        public List<CarType> CarType { set; get; }
        public List<CompanyJoinBranchModel> Branch { set; get; }
        public List<Dealer> Dealer { set; get; }
        public List<ProcessTailTemplateMapping> RoutingDictionaryTailVehicleMapping { set; get; }

        //insert
        public string RoutingDictionaryTailCode { set; get; }
        public List<string> Katsu { set; get; }
    }
}
