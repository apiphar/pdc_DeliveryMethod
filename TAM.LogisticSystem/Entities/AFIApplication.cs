using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class AFIApplication
    {
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        [Key]
        public int AFIApplicationId { get; set; }

        public int AFIApplicationProcessEnumId { get; set; }

        public string AFIBranchCode { get; set; }

        public string AFIRegionCode { get; set; }

        public int AFISubmissionTypeEnumId { get; set; }

        public string ApplicationNumber { get; set; }

        public string ChassisModel { get; set; }

        public string City { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string Description { get; set; }

        public DateTimeOffset? DocumentReceivedAt { get; set; }

        public DateTimeOffset? DocumentSentAt { get; set; }

        public DateTimeOffset EffectiveUntil { get; set; }

        public string FakturNumber { get; set; }

        public string Jenis { get; set; }

        public string KTP { get; set; }

        public string Model { get; set; }

        public string Name { get; set; }

        public string PostalCode { get; set; }

        public string Province { get; set; }

        public string ReferenceNumber { get; set; }

        public DateTimeOffset? STNKAjuAt { get; set; }

        public DateTimeOffset? STNKReceivedAt { get; set; }

        public DateTimeOffset? TamReceivedAt { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public int VehicleId { get; set; }

        public string Warna { get; set; }
    }
}
