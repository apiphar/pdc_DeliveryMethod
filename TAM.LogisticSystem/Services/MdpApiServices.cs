using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class MdpApiServices
    {
        private readonly LogisticDbContext context;
        

        public MdpApiServices(LogisticDbContext context)
        {
            this.context = context;
        }

        // TIE: START
        //public async Task<List<string>> InsertAllMDPModel(List<MdpApiModel> listMdpApiModel)
        //{
        //    var errorMessage = new List<string>();
        //    var allModel = await GetAllModel();
        //    var index = 1;
        //    var dictionary = CreateDictionary(allModel);
        //    foreach (var item in listMdpApiModel)
        //    {
        //        if (CompareRRN(dictionary, item.Rrn)>0)
        //        {                    
        //            errorMessage.Add("Error update data or possible duplicate data with RRN:"+ item.Rrn);
        //        }
        //        else
        //        {
        //            index++;
        //            var newEntities = new Vehicle()
        //            {
        //                BranchAS400 = item.BranchCode,
        //                RRN = item.Rrn,
        //                Katashiki = item.Katashiki,
        //                Suffix = item.Suffix,
        //                ExteriorColorCode = item.Exteriorcolorcode,
        //                InteriorColorCode = item.Interiorcolorcode,
        //                DTPLOD = DateTime.ParseExact(item.Dtplod, "yyyymmdd", CultureInfo.InvariantCulture,
        //                                DateTimeStyles.None),
        //                REVPLOD = DateTime.ParseExact(item.Revplod, "yyyymmdd", CultureInfo.InvariantCulture,
        //                                DateTimeStyles.None),
        //                SpecialVehicleSign = item.SpecialVehiclesign,
        //                FrameNumber = item.Framenumber,
        //                DeliveredAt = null,
        //                TotalLossAt = null,
        //                SetUsedAt = null,
        //                PhysicalLocationCode = null,
        //                EstimatedArrivalBranch = null,
        //                EstimatedDeliveryTime = null,
        //                EstimatedPDCIn = null,
        //                EstimatedPDCOut = null,
        //                EnginePrefix = null,
        //                EngineNumber = index.ToString(),
        //                KeyNumber = null,
        //                HasCustomer = false,
        //                IsAdvanceUnit = false,
        //                IsHold = false,
        //                IsInAudit = false,
        //                IsInWorkshop = false,
        //                IsUrgentDeliveryRequest = false,
        //                Responsibility = null
        //            };
        //            this.context.Vehicle.Add(newEntities);
        //        }     
        //    }
        //    if (errorMessage.Count==0)
        //    {
        //        await this.context.SaveChangesAsync();
        //        errorMessage.Add("Success Add All Data");
        //    }
        //    return errorMessage;
        //}

        //public async Task<List<Vehicle>> GetAllModel()
        //{
        //    var allModel =  await this.context.Vehicle.ToListAsync();
        //    return allModel;
        //}

        //public int CompareRRN(Dictionary<string, string> dictionary, string input)
        //{
        //    var flag = 0;
        //    string error = "";
        //    if(dictionary.TryGetValue(input,out error)){
        //        flag++;
        //    }
        //    return flag;
        //}
        // TIE: END

        public Dictionary<string,string> CreateDictionary(List<Vehicle> listVehicle)
        {
            var comparer = new Dictionary<string, string>();
            foreach(var item in listVehicle)
            {
                comparer.Add(item.RRN, item.RRN);
            }
            return comparer;
        }
            
    }
}
