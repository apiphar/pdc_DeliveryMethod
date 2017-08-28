using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class FileJob
    {
        public string BlobPath { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        [Key]
        public Guid FileJobId { get; set; }

        public DateTimeOffset? FinishAt { get; set; }

        public bool IsSuccess { get; set; }

        public string Menu { get; set; }

        public string Module { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
