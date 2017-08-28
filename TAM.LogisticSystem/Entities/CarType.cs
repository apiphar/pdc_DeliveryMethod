using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class CarType
    {
        public string AFICarTypeCode { get; set; }

        public string Assembly { get; set; }

        public string CarSeriesCode { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string EngineDescription { get; set; }

        public string EngineVolume { get; set; }

        public bool IsFreeTaxZone { get; set; }

        public string Katashiki { get; set; }

        public string Name { get; set; }

        public string SteerPosition { get; set; }

        public string Suffix { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public string WheelDiameter { get; set; }

        public string WheelSize { get; set; }
    }
}
