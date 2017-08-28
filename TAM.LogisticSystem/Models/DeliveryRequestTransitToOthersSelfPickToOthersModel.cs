﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DeliveryRequestTransitToOthersSelfPickToOthersModel
    {
        [Required]
        public DeliveryRequestTransitToOthersModel DeliveryTransitToOthers { get; set; }
        [Required]
        public DeliveryRequestSelfPickToOthersModel DeliverySelfPickToOthers { get; set; }
    }
}
