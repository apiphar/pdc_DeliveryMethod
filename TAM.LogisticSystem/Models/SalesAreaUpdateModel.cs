﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class SalesAreaUpdateModel
    {
        [Required]
        public string Description { set; get; }
    }
}
