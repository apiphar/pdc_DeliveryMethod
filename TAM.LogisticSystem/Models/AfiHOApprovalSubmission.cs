using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class AfiHOApprovalSubmission
    {
        [Required]
        public int AFIApplicationId { get; set; }
        public string Model { get; set; }
        public string FrameNumber { get; set; }
        public string ModelName { get; set; }
        public string CustomerName { get; set; }
        public string ApplicationNo { get; set; }
        public string NoId { get; set; }
        public string Address { get; set; }
        public string TipePengajuan { get; set; }
        public string TipePengajuanName { get; set; }
        public string ReferenceNo { get; set; }

    }
}
