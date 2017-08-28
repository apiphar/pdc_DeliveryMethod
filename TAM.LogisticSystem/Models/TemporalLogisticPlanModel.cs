using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class TemporalLogisticPlanModel
    {
        public int TaktSeconds { get; set; }
        public int Post { get; set; }
        public int LeadMinutes{ get; set; }
        public string LineNumber { get; set; }
        public DateTime EstimatedTimeInitial{ get; set; }
        public int BufferMinutes { get; set; }
        public string RoutingMasterCode { get; set; }
        public int VehicleId { get; set; }
        public int Ordering { get; set; }
        public string LocationCode { get; set; }
    }
}
