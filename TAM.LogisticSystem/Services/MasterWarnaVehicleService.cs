using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class MasterWarnaVehicleService
    {
        private readonly LogisticDbContext LogisticDbContext;

        public MasterWarnaVehicleService(LogisticDbContext logisticDbContext)
        {
            this.LogisticDbContext = logisticDbContext;
        }

        // TIE: START
        ///// <summary>
        ///// Add Vehicle Color to DB
        ///// </summary>
        ///// <param name="vehicleColorViewModel"></param>
        ///// <returns></returns>
        //public async Task AddNewVehicleColor(MasterWarnaVehicleCreateModel vehicleColorViewModel)
        //{
        //    var carTypeColor = new CarTypeColor
        //    {
        //        CarTypeColorCode = vehicleColorViewModel.KodeWarnaVehicle,
        //        CarModelCode = vehicleColorViewModel.Model.KodeModel,
        //        BrandCode = vehicleColorViewModel.Brand.KodeBrand,
        //        ExteriorColorCode = vehicleColorViewModel.Warna.KodeWarna
        //    };
        //    LogisticDbContext.CarTypeColor.Add(carTypeColor);
        //    await LogisticDbContext.SaveChangesAsync();
        //}
        ///// <summary>
        ///// Update Vehicle Color by KodeWarnaVehicle
        ///// </summary>
        ///// <param name="vehicleColorViewModel"></param>
        ///// <returns></returns>
        //public async Task UpdateVehicleColor(MasterWarnaVehicleCreateModel vehicleColorViewModel)
        //{
        //    var vehicleColor = await LogisticDbContext.CarTypeColor
        //        .Where(Q => Q.CarTypeColorCode == vehicleColorViewModel.KodeWarnaVehicle)
        //        .FirstOrDefaultAsync();

        //    vehicleColor.CarTypeColorCode = vehicleColorViewModel.KodeWarnaVehicle;
        //    vehicleColor.CarModelCode = vehicleColorViewModel.Model.KodeModel;
        //    vehicleColor.BrandCode = vehicleColorViewModel.Brand.KodeBrand;
        //    vehicleColor.ExteriorColorCode = vehicleColorViewModel.Warna.KodeWarna;

        //    LogisticDbContext.CarTypeColor.Update(vehicleColor);
        //    await LogisticDbContext.SaveChangesAsync();
        //}
        ///// <summary>
        ///// Delete Vehicle Color by KodeWarnaVehicle
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //public async Task<int> RemoveVehicleColor(string code)
        //{
        //    var vehicleColor = await LogisticDbContext.CarTypeColor
        //        .Where(Q => Q.CarTypeColorCode == code)
        //        .FirstOrDefaultAsync();
        //    var result = LogisticDbContext.CarTypeColor.Remove(vehicleColor);
        //    return await LogisticDbContext.SaveChangesAsync();
        //}
        // TIE: END

        /// <summary>
        /// Get All Vehicle Color Data from DB
        /// </summary>
        /// <returns></returns>
        public async Task<List<MasterWarnaVehicleViewModel>> GetAllVehicleColor()
        {
            var con = LogisticDbContext.Database.GetDbConnection();
            {
                var vehicleColor = (await con.QueryAsync<MasterWarnaVehicleViewModel>(@"
	SELECT
	ctc.CarTypeColorCode AS [KodeWarnaVehicle],
	ctc.BrandCode AS [KodeBrand],
	b.Name AS [BrandName],
	ctc.CarModelCode AS [KodeModel],
	cm.Name AS [ModelName],
	ctc.ExteriorColorCode AS [KodeWarna],
	ec.IndonesianName AS [DeskripsiWarnaInd],
	ec.EnglishName AS [DeskripsiWarnaEng]	
FROM CarTypeColor ctc
	JOIN ExteriorColor ec ON ctc.ExteriorColorCode = ec.ExteriorColorCode
	JOIN Brand b ON ctc.BrandCode = b.BrandCode
	JOIN CarModel cm ON ctc.CarModelCode = cm.CarModelCode
                ")).ToList();

                if (vehicleColor != null)
                {
                    foreach (var item in vehicleColor)
                    {
                        item.BrandDetail = item.KodeBrand + " - " + item.BrandName;
                        item.ModelDetail = item.KodeModel + " - " + item.ModelName;
                    }
                }

               

                return vehicleColor;
            }
        }

        /// <summary>
        /// Get All Brand Data from DB
        /// </summary>
        /// <returns></returns>
        public async Task<List<MasterWarnaVehicleBrandModel>> GetAllKodeBrand()
        {
            var con = LogisticDbContext.Database.GetDbConnection();
            {
                var brandModel = (await con.QueryAsync<MasterWarnaVehicleBrandModel>(@"
                    SELECT  
	b.BrandCode AS [KodeBrand],
    b.Name AS [BrandName]
FROM Brand b
                ")).ToList();
                return brandModel;
            }
        }
        /// <summary>
        /// Get All Model Data from DB
        /// </summary>
        /// <returns></returns>
        public async Task<List<MasterWarnaVehicleModelModel>> GetAllKodeModel()
        {
            var con = LogisticDbContext.Database.GetDbConnection();
            {
                var modelModel = (await con.QueryAsync<MasterWarnaVehicleModelModel>(@"
                    SELECT  
	cm.CarModelCode AS [KodeModel],
	cm.BrandCode AS [KodeBrand],
    cm.Name AS [ModelName]
FROM CarModel cm
                ")).ToList();
                return modelModel;
            }
        }
        /// <summary>
        /// Get All Color Data from DB
        /// </summary>
        /// <returns></returns>
        public async Task<List<MasterWarnaVehicleColorModel>> GetAllKodeWarna()
        {
            var con = LogisticDbContext.Database.GetDbConnection();
            {
                var colorModel = (await con.QueryAsync<MasterWarnaVehicleColorModel>(@"
                    SELECT
	ec.ExteriorColorCode AS [KodeWarna],
	ec.IndonesianName AS [DeskripsiWarnaInd],
	ec.EnglishName AS [DeskripsiWarnaEng]
FROM ExteriorColor ec
                ")).ToList();
                return colorModel;
            }
        }

        // TIE: START
        ///// <summary>
        ///// Check if Vehicle Color already exists
        ///// </summary>
        ///// <param name="kodeWarnaVehicle"></param>
        ///// <returns></returns>
        //public async Task<bool> IsVehicleColorExist(string kodeWarnaVehicle)
        //{
        //    var data = await LogisticDbContext.CarTypeColor.FirstOrDefaultAsync(Q => Q.CarTypeColorCode == kodeWarnaVehicle);
        //    if (data != null)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        // TIE: END
    }
}
