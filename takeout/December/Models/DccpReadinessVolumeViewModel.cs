﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DccpReadinessVolumeViewModel
    {
        public int Id { get; set; }
        public string Tanggal { get; set; }
        public string Asal { get; set; }
        public string Tujuan { get; set; }
        public int Trip { get; set; }
        public int Load { get; set; }
        public string Shift { get; set; }
        public int Qty { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        public int Adjust { get; set; }
        public int Estimasi { get; set; }
    }
}
