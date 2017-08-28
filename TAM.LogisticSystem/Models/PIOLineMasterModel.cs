using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Models
{
    public class PIOLineMasterModel
    {
        [Required]
        public int PIOLineDictionaryId { set; get; }
        public Location Location { set; get; }
        [Required,MaxLength(16)]
        public string LineNumber { set; get; }
        [Required]
        public int TaktSeconds { set; get; }
        [Required]
        public int Post { get; set; }
    }
}
