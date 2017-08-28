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
    public class PenyesuaianTanggalProduksiService
    {
        private readonly LogisticDbContext context;

        public PenyesuaianTanggalProduksiService(LogisticDbContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// Query Dapper to get Plant list and All Data List
        /// </summary>
        /// <returns></returns>
        public async Task<PenyesuaianTanggalProduksiTotalModel> GetAllData()
        {
            var allTanggalProduksi = (await this.context.Database.GetDbConnection().QueryAsync<PenyesuaianTanggalProduksiViewModel>($@"select 
PlantProductionTimeId as Id,
ppt.PlantCode as Plant,
p.Name as Nama,
IsDailyConvertProductionTime as BukanAkhirBulan,
IsEndOfMonthConvertProductionTime as AkhirBulan,
CustomProductionTimeFrom as DateStart,
CustomProductionTimeTo as DateEnd
 from PlantProductionTime ppt
 join Plant p on p.PlantCode = ppt.PlantCode
")).ToList();
            var allPlant = (await this.context.Database.GetDbConnection().QueryAsync<PenyesuaianTanggalProduksi_AllPlantViewModel>($@"select 
PlantCode as PlantName,
Name as Nama
 from Plant
")).ToList();
            foreach (var item in allTanggalProduksi)
            {
                item.DateEnd = item.DateEnd.ToLocalTime();
                item.DateStart = item.DateStart.ToLocalTime();
            }
            var allModel = new PenyesuaianTanggalProduksiTotalModel();
            allModel.AllPlantViewModel = allPlant;
            allModel.AllViewModel = allTanggalProduksi;
            return allModel;

        }

        /// <summary>
        /// Create New Data
        /// </summary>
        /// <param name="penyesuaianTanggalProduksiViewModel"></param>
        /// <returns></returns>
        public async Task PostData(PenyesuaianTanggalProduksiPostViewModel penyesuaianTanggalProduksiViewModel)
        {
            var newPlantProductionTime = new PlantProductionTime();
            newPlantProductionTime.PlantCode = penyesuaianTanggalProduksiViewModel.Plant;
            newPlantProductionTime.IsDailyConvertProductionTime = penyesuaianTanggalProduksiViewModel.BukanAkhirBulan;
            newPlantProductionTime.IsEndOfMonthConvertProductionTime = penyesuaianTanggalProduksiViewModel.AkhirBulan;
            newPlantProductionTime.CustomProductionTimeFrom = TimeZoneInfo.ConvertTimeToUtc(penyesuaianTanggalProduksiViewModel.DateStart);
            newPlantProductionTime.CustomProductionTimeTo = TimeZoneInfo.ConvertTimeToUtc(penyesuaianTanggalProduksiViewModel.DateEnd);

            this.context.Add(newPlantProductionTime);
            await this.context.SaveChangesAsync();
        }
        /// <summary>
        /// Method for delete Data
        /// </summary>
        /// <param name="plant"></param>
        /// <returns></returns>
        public async Task<int>DeleteData(string plant)
        {
            var deletedPlant = this.context.PlantProductionTime.Where(Q => Q.PlantCode == plant).FirstOrDefault();
            this.context.Remove(deletedPlant);
            return await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Update Existing Data
        /// </summary>
        /// <param name="penyesuaianTanggalProduksiViewModel"></param>
        /// <returns></returns>
        public async Task UpdateData(PenyesuaianTanggalProduksiPostViewModel penyesuaianTanggalProduksiViewModel)
        {
            var selectedUpdate = await this.context.PlantProductionTime.Where(Q => Q.PlantCode == penyesuaianTanggalProduksiViewModel.Plant).FirstOrDefaultAsync();
            selectedUpdate.IsEndOfMonthConvertProductionTime = penyesuaianTanggalProduksiViewModel.AkhirBulan ;
            selectedUpdate.IsDailyConvertProductionTime= penyesuaianTanggalProduksiViewModel.BukanAkhirBulan;
            selectedUpdate.CustomProductionTimeFrom= TimeZoneInfo.ConvertTimeToUtc(penyesuaianTanggalProduksiViewModel.DateStart);
            selectedUpdate.CustomProductionTimeTo = TimeZoneInfo.ConvertTimeToUtc(penyesuaianTanggalProduksiViewModel.DateEnd);

            this.context.Update(selectedUpdate);
            await this.context.SaveChangesAsync();
        }
    }
}
