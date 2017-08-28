using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class FormARequestInvoicesViewModel
    {
        public string NomorAju { get; set; }
        public string NomorPIB { get; set; }
        public DateTime TanggalPIB { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int ShipmentInvoiceDetailId { get; set; }
        public string Jenis { get; set; }
        public string Model { get; set; }
        public string FrameNumber { get; set; }
        public string EngineNumber { get; set; }
        public DateTime DTPLOD { get; set; }
        public bool IsAction { get; set; }
    }
}
