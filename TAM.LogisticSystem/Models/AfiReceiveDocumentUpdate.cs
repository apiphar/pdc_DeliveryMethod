using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class AfiReceiveDocumentUpdate
    {
        [Required]
        public List<AfiGridViewModel> AfiApplicationList { get; set; }
        [Required]
        public string rbStatus { get; set; }
    }
}
