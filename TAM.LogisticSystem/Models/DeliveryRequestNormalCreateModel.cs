﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestNormalCreateModel
    {
        [Required]
        public DeliveryRequestModel DeliveryRequest { get; set; }
        [Required]
        public DeliveryRequestNormalModel DeliveryRequestNormal { get; set; }
    }
}