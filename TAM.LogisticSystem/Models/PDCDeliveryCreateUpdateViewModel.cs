using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class PDCDeliveryInsertModel
    {
        //public List<PDCDeliveryCreateUpdateViewModel> Detail { get; set; }
    }

    public class PDCDeliveryCreateUpdateViewModel
    {    
        //public List<PDCLocationModel> Locations { get; set; }
        //public List<PDCDeliveryMethodModel> Deliveries { get; set; }

        public string BranchCode { get; set; }

        public string DeliveryMethodCode { get; set; }

        public string LocationCode { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }

}
