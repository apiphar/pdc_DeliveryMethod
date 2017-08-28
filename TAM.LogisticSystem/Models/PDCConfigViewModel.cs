using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class PDCConfigViewModel
    {
        public string Name { get; set; }

        public string LocationCode { get; set; }
        public int PDCConfigId { get; set; }
        [Required]
        public int MaintenanceDay { get; set; }
        [Required]
        public int CarCarrierQuotaPerDay { get; set; }
        [Required]
        public int NonCarCarrierQuotaPerDay { get; set; }
        [Required]
        public int LeadDayPreDeliveryService { get; set; }

        //Concat Result
        public string MaintenanceDayResult { get; set; }
        public string CarCarrierQuotaPerDayResult { get; set; }
        public string NonCarCarrierQuotaPerDayResult { get; set; }
        public string LeadTimePreparation { get; set; }
        public string LeadTimePreDelivery { get; set; }
    }
}
