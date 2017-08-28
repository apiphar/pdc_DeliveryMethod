using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class FormAModel
    {
        [Required]
        [StringLength(32)]
        public string FrameNumber { get; set; }

        public string FormANumber { get; set; }

        public DateTime FormADate { get; set; }

    }
}
