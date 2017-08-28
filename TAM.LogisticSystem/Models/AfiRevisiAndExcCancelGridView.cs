﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class AfiRevisiAndExcCancelGridView
    {
        public int AfiApplicationId { get; set; }
        public DateTimeOffset DODate { get; set; }
        public int VehicleId { get; set; }
        public string FrameNumber { get; set; }
        public string ModelName { get; set; }
        public string Color { get; set; }
        public string Jenis { get; set; }
        public string Model { get; set; }
        public string ModelCode { get; set; }
        public string Chassis { get; set; }
        public string ApplicationNumber { get; set; }
        public string Branch { get; set; }
        public string CustomerName { get; set; }
        public string KTP { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }

        public string Province { get; set; }
        public string PostalCode { get; set; }
        public DateTimeOffset TanggalEfektif { get; set; }
        public string Region { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTimeOffset TanggalAjuAFI { get; set; }
        public int TipePengajuan { get; set; }
        public string TipePengajuanName { get; set; }
        public int ProcessId { get; set; }
    }
}