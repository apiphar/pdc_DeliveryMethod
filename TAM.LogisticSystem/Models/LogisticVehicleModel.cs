using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class LogisticVehicleModel
    {
        [Required]
        [StringLength(16)]
        public string DeliveryMethodCode { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public int DeliveryMethodTypeId { get; set; }

        [Required]
        public bool NeedSJKBValidation { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }


        public string DeliveryMethodTypeName { get; set; }
    }
    public class DeliveryMethodTypeModel
    {
        public int DeliveryMethodTypeId { get; set; }

        public string DeliveryMethodTypeName { get; set; }
    }
}
