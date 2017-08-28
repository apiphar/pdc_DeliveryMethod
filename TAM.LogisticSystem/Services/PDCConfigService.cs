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
    public class PDCConfigService
    {
        private readonly LogisticDbContext logisticDbContext;

        public PDCConfigService(LogisticDbContext tangoDBContext)
        {
            this.logisticDbContext = tangoDBContext;
        }

        // TIE: START
        //public List<PDCConfigViewModel> GetPDCConfig()
        //{
        //    List<PDCConfigViewModel> lists = new List<PDCConfigViewModel>();
        //    var DbConnection = logisticDbContext.Database.GetDbConnection();

        //    string query = @"SELECT DataSources.PDCConfigId
        //                 , DataSources.LocationCode
        //                 , DataSources.Name
        //                 , DataSources.LeadDayPreDeliveryService
        //                 , concat(DataSources.MaintenanceDay, ' Hari') as MaintenanceDayResult
        //                 , concat(DataSources.CarCarrierQuotaPerDay, ' Unit') as CarCarrierQuotaPerDayResult
        //                 , concat(DataSources.NonCarCarrierQuotaPerDay, ' Unit') as NonCarCarrierQuotaPerDayResult
        //                    , DataSources.MaintenanceDay
        //                    , DataSources.CarCarrierQuotaPerDay
        //                 , DataSources.NonCarCarrierQuotaPerDay
        //                     , concat(DataSources.Days2, ' Hari ', DataSources.Hours2, ' Jam ', DataSources.Minutes2, ' Menit ') as LeadTimePreDelivery
        //                  from (
        //                 select a.PDCConfigId
        //                   , b.LocationCode
        //                   , b.Name
        //                   , a.MaintenanceDay
        //                   , a.CarCarrierQuotaPerDay
        //                   , a.NonCarCarrierQuotaPerDay
        //                   , a.LeadDayPreDeliveryService
        //                   , (((a.LeadDayPreDeliveryService - (a.LeadDayPreDeliveryService % 60)) / 60) - (((a.LeadDayPreDeliveryService - (a.LeadDayPreDeliveryService % 60)) / 60) % 24)) / 24 as Days2
        //                   , ((a.LeadDayPreDeliveryService - (a.LeadDayPreDeliveryService % 60)) / 60) % 24 as Hours2
        //                   , a.LeadDayPreDeliveryService % 60 as Minutes2
        //                   from PDCConfig as a 
        //                   left join Location as b 
        //                  on a.LocationCode = b.LocationCode
        //                ) as DataSources;";

        //    var data = DbConnection.Query<PDCConfigViewModel>(query);
        //    lists = data.ToList();

        //    return lists;
        //}

        //public async Task<PreDeliveryCenter> Get(string id)
        //{
        //    return await logisticDbContext.PreDeliveryCenter.FirstOrDefaultAsync(m => m.LocationCode == id);
        //}

        //public List<PDCConfigViewModel> GetPDC()
        //{
        //    var dbconnection = logisticDbContext.Database.GetDbConnection();
        //    {
        //        string query = @"select LocationCode, Name
        //                                from Location";

        //        var result = dbconnection.Query<PDCConfigViewModel>(query, new
        //        {
        //        }).ToList();

        //        return result;
        //    }
        //}
        ////public List<SelectListItem> GetPDC()
        ////{
        ////    var PDC = logisticDbContext.Location.OrderBy(x => x.LocationCode).Select(x => new SelectListItem()
        ////    {
        ////        Text = x.Name,
        ////        Value = x.LocationCode.ToString(),
        ////        Selected = false
        ////    }).ToList();

        ////    PDC.Insert(0, new SelectListItem()
        ////    {
        ////        Value = "0",
        ////        Text = "(Pilih Salah Satu)",
        ////        Selected = true
        ////    });

        ////    return PDC;
        ////}

        //public async Task<int> Add(string LocationCode, int MaintenanceDay, int CarCarrierQuotaPerDay, int NonCarCarrierQuotaPerDay, int LeadDayPreDeliveryService)
        //{
        //    logisticDbContext.Add(new PreDeliveryCenter
        //    {
        //        LocationCode = LocationCode,
        //        MaintenanceDay = MaintenanceDay,
        //        CarCarrierQuotaPerDay = CarCarrierQuotaPerDay,
        //        NonCarCarrierQuotaPerDay =  NonCarCarrierQuotaPerDay
        //    });
        //    return await logisticDbContext.SaveChangesAsync();
        //}

        //public async Task<int> Update(int id, PDCConfigViewModel model)
        //{
        //    var entity = await logisticDbContext.PreDeliveryCenter.Where(x => x.PDCConfigId == id).FirstOrDefaultAsync();
        //    int rowsAffected = 0;

        //    if (entity != null)
        //    {
        //        entity.MaintenanceDay = model.MaintenanceDay;
        //        entity.CarCarrierQuotaPerDay = model.CarCarrierQuotaPerDay;
        //        entity.NonCarCarrierQuotaPerDay = model.NonCarCarrierQuotaPerDay;
        //        entity.LeadDayPreDeliveryService = model.LeadDayPreDeliveryService;

        //        rowsAffected = await logisticDbContext.SaveChangesAsync();
        //    }

        //    return rowsAffected;
        //}

        //public async Task<int> Remove(int id)
        //{
        //    var entity = await logisticDbContext.PreDeliveryCenter.Where(x => x.PDCConfigId == id).FirstOrDefaultAsync();
        //    if (entity != null)
        //    {
        //        logisticDbContext.Remove(entity);
        //    }

        //    int rowsAffected = await logisticDbContext.SaveChangesAsync();
        //    return rowsAffected;
        //}
        // TIE: END
    }
}
