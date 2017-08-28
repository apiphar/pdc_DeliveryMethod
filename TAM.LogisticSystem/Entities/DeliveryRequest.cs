using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class DeliveryRequest
    {
        public DateTimeOffset? CancelledAt { get; set; }

        public DateTimeOffset? ClosedAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        [Key]
        public string DeliveryRequestNumber { get; set; }

        public int? DeliveryRequestTransitTypeEnumId { get; set; }

        public int DeliveryRequestTypeEnumId { get; set; }

        public string DirectCustomerAddress { get; set; }

        public string DirectCustomerCity { get; set; }

        public string DirectCustomerName { get; set; }

        public DateTimeOffset? DirectEstimatedPDCOut { get; set; }

        public string DirectSalesmanContactNumber { get; set; }

        public string DirectSalesmanName { get; set; }

        public string PickUpConfirmationCode { get; set; }

        public DateTimeOffset? PickUpDate { get; set; }

        public string PickUpIdentityCardNumber { get; set; }

        public bool? PickUpIdentityIsKtp { get; set; }

        public string PickUpIdentityName { get; set; }

        public DateTimeOffset? RequestedDeliveryTimeToBranch { get; set; }

        public string TransitLocationCode { get; set; }

        public DateTimeOffset? TransitReturnDate { get; set; }

        public string TransitReturnToOtherPdc { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public int VehicleId { get; set; }
    }
}
