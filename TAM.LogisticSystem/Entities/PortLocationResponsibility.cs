using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class PortLocationResponsibility
    {
        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string PortLocationCode { get; set; }

        public string ResponsibleLocationCode { get; set; }
    }
}
