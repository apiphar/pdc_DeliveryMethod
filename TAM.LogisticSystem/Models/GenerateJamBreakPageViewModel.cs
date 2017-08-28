﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class GenerateJamBreakPageViewModel
    {
        public List<BreakHourTemplateViewModel> BreakHourTemplates { get; set; }
        public List<GenerateHourLocationViewModel> Location { get; set; }
    }
}
