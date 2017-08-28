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
    public class LegPriceMasterService
    {
        private readonly LogisticDbContext LogisticDbContext;

        public LegPriceMasterService(LogisticDbContext logisticDbContext)
        {
            this.LogisticDbContext = logisticDbContext;
        }

        public async Task<List<LegPriceMasterViewModel>> GetAllCityLegCost()
        {
            var cityLegCostList = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<LegPriceMasterViewModel>(@"
                            SELECT 
		clg.CityLegCostCode,
		clg.CityLegCode,
		cl.Name AS [CityLegName],
		clg.ValidDate,
		clg.Nominal,
		clg.DeliveryVendorCode,
		dv.Name AS [DeliveryVendorName],
		clg.DeliveryMethodCode,
		dm.Name AS [DeliveryMethodName],
		clg.CurrencySymbol,
		cr.Name AS [CurrencyName],
        clg.NeedAdditionalCityLegCostCode,
		cs.CarSeriesCode,
		cs.Name AS [CarSeriesName]		                            
FROM dbo.CityLegCost clg
    JOIN CarSeries cs on clg.CarSeriesCode = cs.CarSeriesCode
JOIN dbo.Currency c ON clg.CurrencySymbol = c.CurrencySymbol
	JOIN CityLeg cl ON clg.CityLegCode = cl.CityLegCode
	JOIN DeliveryVendor dv ON clg.DeliveryVendorCode = dv.DeliveryVendorCode
	JOIN DeliveryMethod dm ON clg.DeliveryMethodCode = dm.DeliveryMethodCode
	JOIN Currency cr ON clg.CurrencySymbol = cr.CurrencySymbol")).ToList();

            foreach (var item in cityLegCostList)
            {
                item.ValidDate = DateTime.SpecifyKind(
                    item.ValidDate,
                    DateTimeKind.Utc);
                item.CarSeriesNameView = item.CarSeriesCode + " - " + item.CarSeriesName;
                item.CityLegNameView = item.CityLegCode + " - " + item.CityLegName;
                item.CurrencyNameView = item.CurrencySymbol + " - " + item.CurrencyName;
                item.DeliveryMethodNameView = item.DeliveryMethodCode + " - " + item.DeliveryMethodName;
                item.DeliveryVendorNameView = item.DeliveryVendorCode + " - " + item.DeliveryVendorName;
                item.ValidDateView = item.ValidDate.ToLocalTime().ToString("dd-MMM-yyyy");
            };

            return cityLegCostList;
        }

        public async Task<List<LegPriceMasterDeliveryVendorModel>> GetAllDeliveryVendor()
        {
            var deliveryVendorList = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<LegPriceMasterDeliveryVendorModel>(@"
                            
SELECT 
	dv.DeliveryVendorCode,
	dv.Name AS [DeliveryVendorName]
FROM DeliveryVendor dv")).ToList();

            return deliveryVendorList;
        }

        public async Task<List<LegPriceMasterCityLegModel>> GetAllCityLeg()
        {
            var cityLegList = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<LegPriceMasterCityLegModel>(@"
SELECT
	cl.CityLegCode,
	cl.Name AS [CityLegName]
FROM CityLeg cl")).ToList();

            return cityLegList;
        }

        public async Task<List<LegPriceMasterDeliveryMethodModel>> GetDeliveryMethod()
        {
            var deliveryMethodList = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<LegPriceMasterDeliveryMethodModel>(@"

SELECT
	dm.DeliveryMethodCode,
	dm.Name AS [DeliveryMethodName]
FROM DeliveryMethod dm
WHERE dm.DeliveryMethodCode = 'CC'
	OR dm.DeliveryMethodCode = 'SD'
	OR dm.DeliveryMethodCode = 'SH'
	OR dm.DeliveryMethodCode = 'SC'")).ToList();

            return deliveryMethodList;
        }

        public async Task<List<LegPriceMasterCarSeriesModel>> GetAllCarSeries()
        {
            var carSeriesList = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<LegPriceMasterCarSeriesModel>(@"
SELECT
	cs.CarSeriesCode,
	cs.Name AS [CarSeriesName]
FROM CarSeries cs")).ToList();

            return carSeriesList;
        }

        public async Task<List<LegPriceMasterCurrencyModel>> GetAllCurrency()
        {
            var currencyList = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<LegPriceMasterCurrencyModel>(@"
SELECT
	c.CurrencySymbol,
	c.Name AS [CurrencyName]
FROM Currency c")).ToList();

            return currencyList;
        }


        public List<CarSeries> GetSeries()
        {
            var data = LogisticDbContext.CarSeries.ToList();
            return data;
        }

        // TIE: START
        //public List<LegPriceMasterViewModel> GetCarSeriesCode()
        //{
        //    var dbconnection = LogisticDbContext.Database.GetDbConnection();
        //    {
        //        string query = @"select a.DeliveryVendorCode
        //                                from DeliveryVendor as a";

        //        var result = dbconnection.Query<LegPriceMasterViewModel>(query, new
        //        {
        //        }).ToList();

        //        return result;
        //    }
        //}
        //public List<LegPriceMasterViewModel> GetCurrencySymbol()
        //{
        //    var dbconnection = LogisticDbContext.Database.GetDbConnection();
        //    {
        //        string query = @"select a.DeliveryVendorCode
        //                                from DeliveryVendor as a";

        //        var result = dbconnection.Query<LegPriceMasterViewModel>(query, new
        //        {
        //        }).ToList();

        //        return result;
        //    }
        //}

        //public async Task<CityLegCost> Get(string id)
        //{
        //    return await LogisticDbContext.CityLegCost.FirstOrDefaultAsync(m => m.CityLegCostCode == id);
        //}

        //public async Task<int> Add(LegPriceMasterCreateModel model)
        //{

        //    var cityLegCostCode = await LogisticDbContext.CityLegCost
        //        .Where(Q => Q.CityLegCostCode == model.CityLegCostCode)
        //        .Select(Q => Q.CityLegCostCode)
        //        .FirstOrDefaultAsync();

        //    if (cityLegCostCode != null)
        //    {
        //        return 2;
        //    }

        //    var entity = new CityLegCost();
        //    {           
        //        entity.CityLegCostCode = model.CityLegCostCode;
        //        entity.DeliveryVendorCode = model.DeliveryVendor.DeliveryVendorCode;
        //        entity.DeliveryMethodCode = model.DeliveryMethod.DeliveryMethodCode;
        //        entity.CarSeriesCode = model.CarSeries.CarSeriesCode;
        //        entity.ValidDate = model.ValidDate.ToUniversalTime();
        //        entity.CityLegCode = model.CityLeg.CityLegCode;
        //        entity.Nominal = model.Nominal;
        //        entity.NeedAdditionalCityLegCostCode = model.NeedAdditionalCityLegCostCode;
        //        entity.CurrencySymbol = model.Currency.CurrencySymbol;
        //        entity.CreatedAt = DateTime.UtcNow;
        //        entity.CreatedBy = "SYSTEM";
        //        entity.UpdatedAt = DateTime.UtcNow;
        //        entity.UpdatedBy = "SYSTEM";
        //    };

        //    LogisticDbContext.Add(entity);
        //    await LogisticDbContext.SaveChangesAsync();

        //    return 0;
        //}

        //public async Task<int> Update(string id, LegPriceMasterCreateModel model)
        //{
        //    var existingLegPrice= await LogisticDbContext.CityLegCost.Where(x => x.CityLegCostCode == id).FirstOrDefaultAsync();
        //    int rowsAffected = 0;

        //    if (existingLegPrice != null)
        //    {            
        //        existingLegPrice.Nominal = model.Nominal;
        //        existingLegPrice.CurrencySymbol = model.Currency.CurrencySymbol;
        //        existingLegPrice.NeedAdditionalCityLegCostCode = model.NeedAdditionalCityLegCostCode;
        //        existingLegPrice.CityLegCode = model.CityLeg.CityLegCode;
        //        existingLegPrice.DeliveryMethodCode = model.DeliveryMethod.DeliveryMethodCode;
        //        existingLegPrice.DeliveryVendorCode = model.DeliveryVendor.DeliveryVendorCode;
        //        existingLegPrice.CarSeriesCode = model.CarSeries.CarSeriesCode;
        //        existingLegPrice.ValidDate = model.ValidDate;

        //        rowsAffected = await LogisticDbContext.SaveChangesAsync();
        //    }
        //    return rowsAffected;
        //}
        // TIE: END

        public async Task<int> Remove(string id)
        {
            var existingCompany = await LogisticDbContext.CityLegCost.Where(x => x.CityLegCostCode == id).FirstOrDefaultAsync();
            if (existingCompany != null)
            {
                LogisticDbContext.Remove(existingCompany);
            }

            return await LogisticDbContext.SaveChangesAsync();
        }

    }
}
