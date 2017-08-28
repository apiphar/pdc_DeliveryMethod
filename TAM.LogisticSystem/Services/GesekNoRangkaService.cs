using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class GesekNoRangkaService
    {
        private readonly LogisticDbContext logisticDbContext;
        private readonly WebEnvironmentService WebEnvirontmentService;
        public GesekNoRangkaService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvirontmentService)
        {
            this.logisticDbContext = logisticDbContext;
            this.WebEnvirontmentService = webEnvirontmentService;
        }

        /// <summary>
        /// Save gesekan 
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public async Task SaveGesekan(GesekNoRangkaInputModel data)
        {
            var scratch = new Scratch
            {
                VehicleId = data.VehicleId,
                LocationCode = data.LocationCode,
                CreatedAt = DateTimeOffset.UtcNow,
                ScratchHandOverNumber = null,
                CreatedBy = WebEnvirontmentService.UserHumanName
            };
            logisticDbContext.Scratch.Add(scratch);
            await logisticDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// get all GesekanNoRangka Data
        /// </summary>
        /// <param name="frameNumber"></param>
        /// <returns></returns>
        public async Task<GesekNoRangkaData> CheckDataByFrameNumber(string frameNumber)
        {
            _ = nameof(Vehicle.VehicleId);
            _ = nameof(CarType.Katashiki);
            _ = nameof(CarType.Suffix);
            _ = nameof(ExteriorColor.IndonesianName);
            _ = nameof(AFICarType.Jenis);
            _ = nameof(AFICarType.Model);
            _ = nameof(Branch.BranchCode);
            _ = nameof(Branch.Name);
            _ = nameof(CarSeries.CarModelCode);
            _ = nameof(ScratchConfiguration.NumberOfScratch);
            _ = nameof(Vehicle.EstimatedPDCIn);
            _ = nameof(Vehicle.RequestedDeliveryTime);
            _ = nameof(Vehicle.HasCustomer);
            _ = nameof(Scratch.VehicleId);
            _ = nameof(ScratchConfiguration.CarModelCode);
            _ = nameof(ScratchConfiguration.BranchCode);
            _ = nameof(CarType.CarSeriesCode);
            _ = nameof(CarSeries.CarSeriesCode);
            _ = nameof(Vehicle.Katashiki);
            _ = nameof(Vehicle.Suffix);
            _ = nameof(ExteriorColor.ExteriorColorCode);
            _ = nameof(Vehicle.ExteriorColorCode);
            _ = nameof(AFICarType.AFICarTypeCode);
            _ = nameof(CarType.AFICarTypeCode);
            _ = nameof(Vehicle.FrameNumber);

            var data = (await logisticDbContext.Database.GetDbConnection().QueryAsync<GesekNoRangkaData>
                ($@"         
SELECT e.VehicleId,d.Katashiki,d.Suffix,[Color]=f.IndonesianName,[Jenis] = g.Jenis,[Model] = g.Model ,
                                c.BranchCode,[BranchName]=c.Name,b.CarModelCode,[JumlahGesek]=a.NumberOfScratch,
                                [ETDPDC]=e.EstimatedPDCIn,
                                [RequestedPDD]=e.RequestedDeliveryTime,e.HasCustomer
                                FROM ScratchConfiguration a
                                JOIN CarSeries b ON a.CarModelCode = b.CarModelCode
                                JOIN Branch c ON c.BranchCode = a.BranchCode
                                JOIN CarType d ON d.CarSeriesCode = b.CarSeriesCode
                                JOIN Vehicle e ON e.Katashiki = d.Katashiki AND e.Suffix = d.Suffix
                                JOIN ExteriorColor f ON f.ExteriorColorCode = e.ExteriorColorCode
                                JOIN AFICarType g ON g.AFICarTypeCode = d.AFICarTypeCode
                                WHERE E.FrameNumber = @frameNumber
", new { frameNumber = frameNumber })).FirstOrDefault();
            return data;
        }

        /// <summary>
        /// get location code and name based on username login
        /// </summary>
        /// <returns></returns>
        public async Task<GesekNoRangkaLocationViewModel> GetLocationData()
        {
            _ = nameof(Location.LocationCode);
            _ = nameof(Location.Name);
            _ = nameof(UserMapping.LocationCode);
            _ = nameof(UserMapping.Username);
            var username = WebEnvirontmentService.UserHumanName;
            var locationData = (await logisticDbContext.Database.GetDbConnection().
                               QueryAsync<GesekNoRangkaLocationViewModel>
                               ($@"      
SELECT L.LocationCode AS [LocationCode],L.Name AS [Name] from UserMapping UM
JOIN Location L
ON UM.LocationCode = L.LocationCode
WHERE UM.Username = @username
", new { username = username })).FirstOrDefault();
            return locationData;
        }

        /// <summary>
        /// check jika frameNumber sudah di save sebelumnya
        /// </summary>
        /// <param name="frameNumber"></param>
        /// <returns></returns>
        public async Task<bool> IsFrameNumberExists(string frameNumber)
        {
            _ = nameof(Scratch.VehicleId);
            _ = nameof(Vehicle.VehicleId);
            _ = nameof(Vehicle.FrameNumber);
            var dataChecked = (await logisticDbContext.Database.GetDbConnection()
                                .QueryAsync(
                 $@"SELECT a.VehicleId
                                FROM Scratch a
                                JOIN Vehicle b ON a.VehicleId = b.VehicleId
                                WHERE b.frameNumber = @frameNumber"
                                , new { frameNumber = frameNumber })).FirstOrDefault();
            if (dataChecked == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
