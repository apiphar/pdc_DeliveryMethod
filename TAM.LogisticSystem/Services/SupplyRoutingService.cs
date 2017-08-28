using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Enums;

namespace TAM.LogisticSystem.Services
{
    public class SupplyRoutingService
    {
        private readonly LogisticDbContext _context;

        public SupplyRoutingService(LogisticDbContext c)
        {
            this._context = c;
        }

        private class LeadTimeBy
        {
            public string MasterCode { get; set; }
            public int LeadId { get; set; }
        }

        public async Task SupplyRouting(Vehicle vehicle)
        {
            var vehicleInVehicleRouting = this._context.VehicleRouting
                .Where(Q => Q.VehicleId == vehicle.VehicleId)
                .FirstOrDefault();

            if (vehicleInVehicleRouting != null)
            {
                return;
            }

            var processDictionaryDetailList = await GetProcessDictionaryDetail(vehicle);
            var leadby = GetLeadBy();
            var downstream = new List<VehicleRouting>();
            
            foreach (var item in processDictionaryDetailList)
            {
                var lineNumber = "0";

                var routingLeadTimeById = leadby[item.ProcessMasterCode];

                //PIO
                if (routingLeadTimeById == (int) ProcessLeadTimeBy.PIO)
                {
                    lineNumber = await GetPIOLineNumberAsync(vehicle);
                }

                //SPU
                if (routingLeadTimeById == (int) ProcessLeadTimeBy.SPU)
                {
                    lineNumber = await GetSPULineNumberAsync(vehicle);
                }

                var vehicleRouting = new VehicleRouting
                {
                    VehicleId = vehicle.VehicleId,
                    Ordering = item.Ordering,
                    ProcessMasterCode = item.ProcessMasterCode,
                    DeliveryMethodCode = item.DeliveryMethodCode,
                    LocationCode = item.LocationCode,
                    ShiftCode = null,
                    LineNumber = lineNumber,
                    EstimatedTimeInitial = (DateTimeOffset)vehicle.REVPLOD,
                    EstimatedTimeAdjusted = (DateTimeOffset)vehicle.REVPLOD,
                    ScanTime = null,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "SYSTEM",
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = "SYSTEM"
                };

                downstream.Add(vehicleRouting);
            }
            //downstream.Add(this.GetVehicleRoutingDLO(vehicle, downstream));
            //downstream.Add(this.GetVehicleRoutingBranchIn(vehicle, downstream));

            _context.VehicleRouting.AddRange(downstream);

            await _context.SaveChangesAsync();
        }

        private VehicleRouting GetVehicleRoutingDLO(Vehicle vehicle, List<VehicleRouting> downstream)
        {
            var vehicleRoutingDLO = new VehicleRouting
            {
                VehicleId = vehicle.VehicleId,
                ProcessMasterCode = "DLO",
                DeliveryMethodCode = null,
                LineNumber = "0",
                Ordering = downstream.Last().Ordering + 1,
                LocationCode = downstream.Last().LocationCode,
                ShiftCode = "SHIFT01",
                EstimatedTimeAdjusted = (DateTimeOffset)vehicle.REVPLOD,
                EstimatedTimeInitial = (DateTimeOffset)vehicle.REVPLOD,
                ScanTime = null,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "SYSTEM",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "SYSTEM"
            };

            return vehicleRoutingDLO;
        }

        private VehicleRouting GetVehicleRoutingBranchIn(Vehicle vehicle, List<VehicleRouting> downstream)
        {
            var locationCode = this._context.Database.GetDbConnection().Query<string>(@"
               	SELECT l.LocationCode FROM Vehicle v JOIN Branch b
	ON v.BranchCode = b.BranchCode
	JOIN Location l ON b.LocationCode = l.LocationCode
	WHERE v.BranchCode = @branchCode", new { branchCode = vehicle.BranchCode }).FirstOrDefault();

            var vehicleRoutingBranchIn = new VehicleRouting
            {
                VehicleId = vehicle.VehicleId,
                ProcessMasterCode = "BRCHIN",
                DeliveryMethodCode = null,
                LineNumber = "0",
                Ordering = downstream.Last().Ordering + 1,
                LocationCode = locationCode,
                ShiftCode = "SHIFT01",
                EstimatedTimeAdjusted = (DateTimeOffset)vehicle.REVPLOD,
                EstimatedTimeInitial = (DateTimeOffset)vehicle.REVPLOD,
                ScanTime = null,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "SYSTEM",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "SYSTEM"
            };

            return vehicleRoutingBranchIn;
        }

        private async Task<List<ProcessDictionaryDetail>> GetProcessDictionaryDetail(Vehicle vehicle)
        {
            var processDictionary = await _context.ProcessDictionary.Where(Q => Q.Katashiki == vehicle.Katashiki &&
            Q.Suffix == vehicle.Suffix &&
            Q.BranchCode == vehicle.BranchCode &&
            Q.ValidFrom < DateTime.UtcNow).OrderByDescending(X => X.ValidFrom).FirstOrDefaultAsync();

            return await _context.ProcessDictionaryDetail.Where(Q => Q.ProcessDictionaryId == processDictionary.ProcessDictionaryId).
                OrderBy(X => X.Ordering).ToListAsync();
        }
        private Dictionary<string, int> GetLeadBy()
        {
            return _context.Database.GetDbConnection().Query<LeadTimeBy>(@"
SELECT pm.ProcessMasterCode AS MasterCode, pm.ProcessLeadTimeByEnumId AS LeadId FROM ProcessMaster pm
JOIN ProcessLeadTimeByEnum pltbe ON pm.ProcessLeadTimeByEnumId = pltbe.ProcessLeadTimeByEnumId
").ToDictionary(Q => Q.MasterCode, Q => Q.LeadId);
        }

        private async Task<string> GetPIOLineNumberAsync(Vehicle vehicle)
        {
            return await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<string>(@"
SELECT pl.LineNumber FROM PIOLine pl 
JOIN PIOLineDetail pld ON pl.PIOLineId = pld.PIOLineId
WHERE pld.Katashiki = @katashiki AND pld.Suffix = @suffix
", new { katashiki = vehicle.Katashiki, suffix = vehicle.Suffix });
        }

        private async Task<string> GetSPULineNumberAsync(Vehicle vehicle)
        {
            return await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<string>(@"
SELECT pl.LineNumber FROM PIOLine pl 
JOIN PIOLineDetail pld ON pl.PIOLineId = pld.PIOLineId
WHERE pld.Katashiki = @katashiki AND pld.Suffix = @suffix
", new { katashiki = vehicle.Katashiki, suffix = vehicle.Suffix, branchCode = vehicle.BranchCode });
        }
    }
}
