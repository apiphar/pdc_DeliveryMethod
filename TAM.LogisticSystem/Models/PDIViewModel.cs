using System.Collections.Generic;

namespace TAM.LogisticSystem.Models
{
    public class InspectionMasterChecklist
    {
        public int InspectionMasterDetailId { get; set; }
        public string Area { get; set; }
        public string Description { get; set; }
        public bool IsChecked { get; set; }
    }

    public class InspectionParameter
    {
        public string Prefix { get; set; }
        public int AreaId { get; set; }
    }

    public class ActiveUser
    {
        public string Username { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
    }

    public class VehicleData
    {
        public string VehicleModel { get; set; }
        public string VehicleType { get; set; }
        public string VehicleColor { get; set; }
        public string VehicleOutlet { get; set; }
    }

    public class InspectionData
    {
        public string InspectionChecklist { get; set; }
        public Dictionary<int, bool> FormattedInspectionChecklist { get; set; }
        public string FrameNumber { get; set; }
        public string EngineNumber { get; set; }
        public string KeyNumber { get; set; }
    }

    public class PDIParkingData
    {
        public string ParkingLineSuggestion { get; set; }
        public string ParkingLineOptional { get; set; }
    }

    public class ResponseResult
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public object Data { get; set; }
    }
}
