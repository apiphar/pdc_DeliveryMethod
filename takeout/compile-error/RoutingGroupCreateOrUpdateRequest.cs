using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TAM.TANGO.Models
{
    public class RoutingGroupCreateOrUpdateRequest
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
    }
}
