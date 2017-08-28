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
    public class MasterRitasePriceService
    {

        public MasterRitasePriceService( LogisticDbContext logisticDbContext)
        {
            _TangoDbContext = logisticDbContext;
        }
        private readonly LogisticDbContext _TangoDbContext;

        // TIE: START
        ///// <summary>
        ///// Delete Ritase data by id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public async Task DeleteData(int id)
        //{
        //    var selected = await _TangoDbContext.CityLegRitaseCost.AsNoTracking()
        //                    .FirstOrDefaultAsync(q => q.CityLegRitaseCostId == id);

        //    _TangoDbContext.Remove(selected);
        //    await _TangoDbContext.SaveChangesAsync();
        //}

        ///// <summary>
        ///// Get all Delivery vendor code
        ///// </summary>
        ///// <returns></returns>
        //public async Task<List<MasterRitasePriceDeliveryVendor>> GetDeliveryVendorCode()
        //{
        //    var data = await _TangoDbContext.DeliveryVendor.AsNoTracking().Select(q =>
        //    new MasterRitasePriceDeliveryVendor
        //    {
        //        DeliveryVendorCode = q.DeliveryVendorCode
        //    }).ToListAsync();

        //    return data;
        //}

        ///// <summary>
        ///// Get All Delivery Method code
        ///// </summary>
        ///// <returns></returns>
        //public async Task<List<MasterRitasePriceDeliveryMethod>> GetDeliveryMethodCode()
        //{
        //    var data = await _TangoDbContext.DeliveryMethod.AsNoTracking().Select(q =>
        //    new MasterRitasePriceDeliveryMethod
        //    {
        //        DeliveryMethodCode = q.DeliveryMethodCode
        //    }).ToListAsync();

        //    return data;
        //}


        ///// <summary>
        ///// Get All City Leg code
        ///// </summary>
        ///// <returns></returns>
        //public async Task<List<MasterRitasePriceCityLeg>> GetCityLegCode()
        //{
        //    var data = await _TangoDbContext.CityLeg.AsNoTracking().Select(q =>
        //    new MasterRitasePriceCityLeg
        //    {
        //        CityLegCode = q.CityLegCode
        //    }).ToListAsync();

        //    return data;
        //}

        ///// <summary>
        ///// Get all Currency Symbol
        ///// </summary>
        ///// <returns></returns>
        //public async Task<List<MasterRitasePriceCurrencySymbol>> GetCurrencySymbol()
        //{
        //    var data = await _TangoDbContext.Currency.AsNoTracking().Select(q =>
        //    new MasterRitasePriceCurrencySymbol
        //    {
        //         CurrencySymbol= q.CurrencySymbol
        //    }).ToListAsync();

        //    return data;
        //}

        ///// <summary>
        ///// Add data Ritase
        ///// </summary>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //public async Task AddData(MasterRitasePriceInputModel data)
        //{
        //    var newData = new CityLegRitaseCost
        //    {
        //        CityLegCode = data.CityLegCode,
        //        CurrencySymbol = data.CurrencySymbol,
        //        DeliveryMethodCode = data.DeliveryMethodCode,
        //        DeliveryVendorCode = data.DeliveryVendorCode,
        //        IsSingleTrip = data.IsSingleTrip,
        //        Nominal = data.Nominal,
        //        ValidDate = data.ValidDate
        //    };
        //    _TangoDbContext.CityLegRitaseCost.Add(newData);
        //    await _TangoDbContext.SaveChangesAsync();
        //}

        ////Update Data 
        //public async Task UpdateData( MasterRitasePriceEditModel data)
        //{
        //    var dataSelected = await _TangoDbContext.CityLegRitaseCost.FirstOrDefaultAsync(q => q.CityLegRitaseCostId == data.CityLegRitaseCostId);
        //    dataSelected.CityLegCode = data.CityLegCode;
        //    dataSelected.CurrencySymbol = data.CurrencySymbol;
        //    dataSelected.DeliveryMethodCode = data.DeliveryMethodCode;
        //    dataSelected.DeliveryVendorCode = data.DeliveryVendorCode;
        //    if(data.IsSingleTrip=="Single Trip")
        //    {
        //        dataSelected.IsSingleTrip = false;
        //    } else
        //    {
        //        dataSelected.IsSingleTrip = true;
        //    }
        //    dataSelected.ValidDate = data.ValidDate;
        //    dataSelected.Nominal = data.Nominal;

        //    _TangoDbContext.CityLegRitaseCost.Update(dataSelected);
        //    await _TangoDbContext.SaveChangesAsync();

        //}


        ////Get all Ritase data
        //public async Task<List<MasterRitasePriceViewModel>> GetAllRitaseData()
        //{
        //    var ritaseData = await _TangoDbContext.CityLegRitaseCost.AsNoTracking().Select(q => new MasterRitasePriceViewModel
        //    { CityLegCode = q.CityLegCode,
        //    CityLegRitaseCostId = q.CityLegRitaseCostId,
        //    CurrencySymbol = q.CurrencySymbol,
        //    DeliveryMethodCode=q.DeliveryMethodCode,
        //    DeliveryVendorCode=q.DeliveryVendorCode,
        //    IsSingleTrip = q.IsSingleTrip,
        //    Nominal= q.Nominal,
        //    ValidDate = q.ValidDate})
        //    .ToListAsync();
        //    return ritaseData;
        //}
        // TIE: END
    }
}
