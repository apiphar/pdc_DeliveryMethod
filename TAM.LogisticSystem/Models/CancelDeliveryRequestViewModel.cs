using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class CancelDeliveryRequestViewModel
    {
        [Required]
        [MaxLength(32)]
        [RegularExpression("^[A-Za-z0-9/]*$")]
        public string DeliveryRequestNumber { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? CancelledAt { get; set; }
        public int VehicleId { get; set; }
        public int DeliveryRequestTypeId { get; set; }
        public int DeliveryRequestTransitTypeId { get; set; }
        public string FrameNumber { get; set; }
        public string Katashiki { get; set; }
        public string Suffix { get; set; }
        public string Model { get; set; }
        public string Tipe { get; set; }
        public string Warna { get; set; }
        public string Branch { get; set; }
        public DateTimeOffset RequestedPdd { get; set; }
        public DateTimeOffset EstimatedPDCIn { get; set; }
        public bool CustomerAssign { get; set; }
        public string PosisiTerakhir { get; set; }
        public string LokasiTerakhir { get; set; }
        public DateTimeOffset? RequestedDeliveryTimeToBranch { get; set; }
        public DateTimeOffset? PickUpDate { get; set; }
        public bool PickUpIdentityIsKtp { get; set; }
        public string DriverType { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public string ConfirmationCode { get; set; }
        public DateTimeOffset? EstimasiPDCOut { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCity { get; set; }
        public string SalesmanName { get; set; }
        public string SalesmanContactNo { get; set; }
        public string LocationType { get; set; }
        public string LocationTypeCode { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public string LocationCode { get; set; }
        public int LeadTimeDay { get; set; }
        public int LeadTimeHour { get; set; }
        public int LeadTimeMinute { get; set; }
        public DateTimeOffset? TransitReturnDate { get; set; }
        public string OtherPdcLocation { get; set; }
        public string OtherPdcLocationCode { get; set; }    
    }
}
