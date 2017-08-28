using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class Vehicle
    {
        public DateTimeOffset? ActualArrivalBranch { get; set; }

        public DateTimeOffset? ActualDeliveryTime { get; set; }

        public string BranchCode { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset? CustomerReceivedTime { get; set; }

        public DateTimeOffset DTPLOD { get; set; }

        public string EngineNumber { get; set; }

        public string EnginePrefix { get; set; }

        public DateTimeOffset? EstimatedArrivalBranch { get; set; }

        public DateTimeOffset? EstimatedPDCIn { get; set; }

        public DateTimeOffset? EstimatedPDCOut { get; set; }

        public string ExteriorColorCode { get; set; }

        public string FrameNumber { get; set; }

        public bool HasCustomer { get; set; }

        public string InteriorColorCode { get; set; }

        public bool IsAdvanceUnit { get; set; }

        public bool IsClaimLostToInsurance { get; set; }

        public bool IsHold { get; set; }

        public bool IsInAudit { get; set; }

        public bool IsInWorkshop { get; set; }

        public bool IsSentBoardPDSToDMS { get; set; }

        public bool IsSentDeliveryInfoToDMS { get; set; }

        public bool IsSentMDPToDMS { get; set; }

        public bool IsSentRevisedPDDToDMS { get; set; }

        public bool IsUrgentDeliveryRequest { get; set; }

        public string Katashiki { get; set; }

        public string KeyNumber { get; set; }

        public string NomorIndukKendaraan { get; set; }

        public string PaketAksesorisTAM { get; set; }

        public string PhysicalLocationCode { get; set; }

        public int ProductionYear { get; set; }

        public DateTimeOffset? RequestedDeliveryTime { get; set; }

        public string Responsibility { get; set; }

        public DateTimeOffset? REVPLOD { get; set; }

        public string RRN { get; set; }

        public DateTimeOffset? SetUsedAt { get; set; }

        public string SpecialVehicleSign { get; set; }

        public string Suffix { get; set; }

        public DateTimeOffset? TotalLossAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [Key]
        public int VehicleId { get; set; }
    }
}
