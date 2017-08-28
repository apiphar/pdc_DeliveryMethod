using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class DMSSendKaroseriModel
    {
        public string FrameNumber { get; set; }
        public int DeliveryRequestType { get; set; }
        public DateTimeOffset DeliveryRequestDate{ get; set; }
        public DateTimeOffset PickUpDateTime { get; set; }
        public string LocationCode { get; set; }
        public DateTimeOffset VehicleReturnDate { get; set; }
        public int DriverReturnType { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public string ReasonUrgentMemo { get; set; }
    }

    public class DMSValidateKaroseriModel
    {
        public DateTimeOffset? CancelledAt { get; set; }
        public DateTimeOffset? CloseAt { get; set; }
    }

    public class DMSRetrieveKaroseriModel
    {
        public string FrameNumber { get; set; }
        public bool UrgenMemoStatus { get; set; }
        public string ConfirmationCode { get; set; }
    }

    public class DMSVehicleKaroseriModel
    {
        public int VehicleId { get; set; }
        public string BranchCode { get; set; }
    }
}
