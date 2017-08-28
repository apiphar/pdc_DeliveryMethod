using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class RegionAfiModelToAdd
    {
        

        public string Name { get; set; }

        
        public string RegionAFICode { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
