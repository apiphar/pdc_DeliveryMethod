using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class PolaRangkaianTahapAwalPenerapanViewModel
    {
        public List<ProcessHeadTemplate> RoutingDictionaryHead { set; get; }
        public List<CarModel> CarModel { set; get; }
        public List<CarSeries> CarSeries { set; get; }
        public List<CarType> CarType { set; get; }
        public List<ProcessHeadTemplateMapping> RoutingDictionaryHeadVehicleMapping { set; get; }

        //insert
        public string RoutingDictionaryHeadCode { set; get; }
        public List<string> Katsu { set; get; }
    }
}
