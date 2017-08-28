using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class ProcessMaster
    {
        public int BufferMinutes { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public bool IsScan { get; set; }

        public string Name { get; set; }

        public int ProcessLeadTimeByEnumId { get; set; }

        [Key]
        public string ProcessMasterCode { get; set; }

        public bool SwappingPoint { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
