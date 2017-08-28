using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class LocationService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;

        public LocationService(LogisticDbContext tangoDb, WebEnvironmentService webEnvironmentService)
        {
            this.LogisticDbContext = tangoDb;
            this.WebEnvironmentService = webEnvironmentService;
        }

        /// <summary>
        /// Get All Data yang diperlukan untuk GET
        /// </summary>
        /// <returns></returns>
        public async Task<LocationPageViewModel> GetAllData()
        {
            var locationPage = new LocationPageViewModel()
            {
                Location = await this.GetLocation(),
                LocationType = await this.GetLocationType(),
                CityForLeg = await this.GetCityForLeg(),
                CityForShipment = await this.GetCityForShipment()
            };

            return locationPage;
        }

        /// <summary>
        /// Get All Location
        /// </summary>
        /// <returns></returns>

        public async Task<List<LocationViewModel>> GetLocation()
        {
            _ = nameof(Location.LocationCode);
            _ = nameof(Location.Name);
            _ = nameof(Location.Address);
            _ = nameof(Location.CityForLegCode);
            _ = nameof(CityForLeg.CityForLegCode);
            _ = nameof(CityForLeg.Name);
            _ = nameof(Location.CityForShipmentCode);
            _ = nameof(CityForShipment.CityForShipmentCode);
            _ = nameof(CityForShipment.Name);
            _ = nameof(Location.LocationTypeCode);
            _ = nameof(LocationType.LocationTypeCode);

            var data = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<LocationViewModel>(@"
SELECT 
	l.LocationCode,
	l.Name AS [LocationName],
	l.Address AS [Alamat],
	l.CityForLegCode,
	cfl.Name AS [CityForLegName],
	l.CityForShipmentCode,
	cfs.Name AS [CityForShipmentName],
	l.LocationTypeCode,
	lt.Name AS [LocationTypeName],
    l.CanPrintSJKB AS [CetakSJKB]
FROM Location l
JOIN CityForLeg cfl ON l.CityForLegCode = cfl.CityForLegCode
JOIN CityForShipment cfs ON l.CityForShipmentCode = cfs.CityForShipmentCode
JOIN LocationType lt ON l.LocationTypeCode = lt.LocationTypeCode
")).ToList();

            
            return data;
        }
        /// <summary>
        /// Get All City For Leg
        /// </summary>
        /// <returns></returns>
        public async Task<List<CityForLeg>> GetCityForLeg()
        {
            var cityForLegs = await LogisticDbContext.CityForLeg.OrderBy(x => x.CityForLegCode).Select(x => new CityForLeg()
            {
                CityForLegCode = x.CityForLegCode,
                Name = x.Name
            }).ToListAsync();

            return cityForLegs;
        }

        /// <summary>
        /// Get All City For Shipment
        /// </summary>
        /// <returns></returns>
        public async Task<List<CityForShipment>> GetCityForShipment()
        {
            var cityForShipments = await LogisticDbContext.CityForShipment.OrderBy(x => x.CityForShipmentCode).Select(x => new CityForShipment()
            {
                CityForShipmentCode = x.CityForShipmentCode,
                Name = x.Name
            }).ToListAsync();

            return cityForShipments;
        }

        /// <summary>
        /// Get All Location Type
        /// </summary>
        /// <returns></returns>
        public async Task<List<LocationType>> GetLocationType()
        {
            var clusters = await LogisticDbContext.LocationType.OrderBy(x => x.LocationTypeCode).Select(x => new LocationType()
            {
                LocationTypeCode = x.LocationTypeCode,
                Name = x.Name
            }).ToListAsync();

            return clusters;
        }
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> Add(LocationViewModel model)
        {
            var username = this.WebEnvironmentService.UserHumanName;

            var checkLocation = await LogisticDbContext.Location
                .Where(Q => Q.LocationCode == model.LocationCode)
                .FirstOrDefaultAsync();

            if (checkLocation != null)
            {
                return 2;
            }

            var entity = new Location();
            {
                entity.LocationCode = model.LocationCode.ToUpper();
                entity.Name = model.LocationName.ToUpper();
                entity.CityForLegCode = model.CityForLegCode.ToUpper();
                entity.CityForShipmentCode = model.CityForShipmentCode.ToUpper();
                entity.LocationTypeCode = model.LocationTypeCode.ToUpper();
                entity.Address = model.Alamat.ToUpper();
                entity.CanPrintSJKB = model.CetakSJKB;
                entity.CreatedAt = DateTimeOffset.UtcNow;
                entity.UpdatedAt = DateTimeOffset.UtcNow;
                entity.CreatedBy = username;
                entity.UpdatedBy = username;
            };

            LogisticDbContext.Add(entity);
            await LogisticDbContext.SaveChangesAsync();

            return 0;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<int> Remove(string code)
        {
            var data = await LogisticDbContext.Location.Where(x => x.LocationCode.Equals(code)).FirstOrDefaultAsync();
            if (data != null)
            {
                LogisticDbContext.Remove(data);
                await LogisticDbContext.SaveChangesAsync();
            }
            else
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> Update(LocationViewModel model)
        {
            var username = this.WebEnvironmentService.UserHumanName;

            var data = await LogisticDbContext.Location.Where(x => x.LocationCode.Equals(model.LocationCode)).FirstOrDefaultAsync();

            if (data != null)
            {
                data.CityForLegCode = model.CityForLegCode.ToUpper();
                data.CityForShipmentCode = model.CityForShipmentCode.ToUpper();
                data.LocationTypeCode = model.LocationTypeCode.ToUpper();
                data.Address = model.Alamat.ToUpper();
                data.Name = model.LocationName.ToUpper();
                data.UpdatedAt = DateTimeOffset.UtcNow;
                data.UpdatedBy = username;
                data.CanPrintSJKB = model.CetakSJKB;
                this.LogisticDbContext.Location.Update(data);
                await LogisticDbContext.SaveChangesAsync();
            }
            else
            {
                return 2;
            }

            return 0;
        }


    }
}
