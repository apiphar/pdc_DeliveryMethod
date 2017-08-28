using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class PreDeliveryCenter
    {
        public int CarCarrierQuotaPerDay { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public int LeadPreparationDeliveryMinutes { get; set; }

        [Key]
        public string LocationCode { get; set; }

        public int MaintenanceDay { get; set; }

        public int NonCarCarrierQuotaPerDay { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
