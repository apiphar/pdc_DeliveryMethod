using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class AfiRequestCheckData
    {
        public int? VehicleId { get; set; }
        public DateTimeOffset? DODate { get; set; }
        public string Branch{ get; set; }
        public string Color { get; set; }

        public string CarModelName { get; set; }
        public string CarModelCode { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }

        public string FrameNumber { get; set; }
    }
}
