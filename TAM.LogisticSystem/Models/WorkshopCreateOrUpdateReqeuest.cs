using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class WorkshopCreateOrUpdateReqeuest
    {
        public string Name { get; set; }
        
        public int WorkshopId { get; set; }

        public int WorkshopTypeId { get; set; }
    }
}
