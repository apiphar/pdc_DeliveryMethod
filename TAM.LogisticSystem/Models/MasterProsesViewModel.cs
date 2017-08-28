using System;
using System.ComponentModel.DataAnnotations;

namespace TAM.LogisticSystem.Models
{
    public class MasterProsesViewModel
    {
        [Required]
        public string processMasterCode { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public bool isScan { get; set; }

        [Required]
        public int processLeadTimeByEnumId { get; set; }

        [Required]
        public int bufferMinutes { get; set; }

        [Required]
        public bool swappingPoint { get; set; }

        public DateTime createdAt { get; set; }

        public string createdBy { get; set; }

        public DateTime updatedAt { get; set; }

        public string updatedBy { get; set; }
    }
}
