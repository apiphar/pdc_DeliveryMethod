using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class ConfigurationPlanningViewModel
    {
        public string RoutingMasterCodeAndName { get; set; }
        public string RoutingMasterCode { get; set; }
        [Required]
        public Boolean DoMonthlyCarCarrierPlan { get; set; }
        [Required]
        public Boolean DoDailyCarCarrierPlan { get; set; }
        public string Name { get; set; }
        public string Config { get; set; }
    }
}
