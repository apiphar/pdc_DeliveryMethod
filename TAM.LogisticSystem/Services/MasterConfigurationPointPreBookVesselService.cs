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
    public class MasterConfigurationPointPreBookVesselService
    {
        public MasterConfigurationPointPreBookVesselService(LogisticDbContext logisticDbContext)
        {
            this.LogisticDbContext = logisticDbContext;
        }
        private readonly LogisticDbContext LogisticDbContext;

        // TIE: START
        //public async Task<ConfigurationPointPreBookVesselViewModel> GetBookVesselDataList()
        //{
        //    var data = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<PointPreBookVesselListViewModel>(@"
        //        select pb.LocationCode as LocationCode, rm.Name as PointPreBookVesselName, 
        //        rm.RoutingMasterCode as PointPreBookVesselId
        //        from PreBookVesselLocationMapping pb
        //        join RoutingMaster rm on rm.RoutingMasterCode = pb.RoutingMasterCode 
        //    ")).ToList();

        //    var locationCode = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<string>(@"
        //           select l.locationCode from location l
        //            where l.LocationCode not in
        //            (select LocationCode from PreBookVesselLocationMapping)
        //    ")).ToList();

        //    var masterRoutingForPointPreBookVessel = (await LogisticDbContext.Database.GetDbConnection()
        //        .QueryAsync<MasterRoutingForPointPreBookVessel>(
        //        @"select Name as PointPreBookVesselName, RoutingMasterCode as PointPreBookVesselId
        //        from  RoutingMaster  
        //    ")).ToList();

        //    ConfigurationPointPreBookVesselViewModel all = new ConfigurationPointPreBookVesselViewModel
        //    {
        //        PointPreBookVesselsList = data,
        //        LocationCodes = locationCode,
        //        MasterRoutingDataNeeded = masterRoutingForPointPreBookVessel
        //    };
        //    return all;
        //}

        //public async Task AddBookVesselData(PointPreBookVesselListViewModel point)
        //{
        //    var data = new PreBookVesselLocationMapping
        //    {
        //        LocationCode = point.LocationCode,
        //        RoutingMasterCode = point.PointPreBookVesselId  
        //    };
        //    LogisticDbContext.PreBookVesselLocationMapping.Add(data);
        //    await LogisticDbContext.SaveChangesAsync();
        //}

        //public async Task DeleteBookVesselData(string location)
        //{
        //    var data = await LogisticDbContext.PreBookVesselLocationMapping.FirstOrDefaultAsync(Q => Q.LocationCode == location);
        //    LogisticDbContext.PreBookVesselLocationMapping.Remove(data);
        //    await LogisticDbContext.SaveChangesAsync();
        //}

        ///// <summary>
        ///// harus pake dapper karena table isinya yg diedit PK semua, katanya gara" Entity Framework nya
        ///// </summary>
        ///// <param name="point"></param>
        ///// <returns></returns>
        //public async Task UpdateBookVesselData(PointPreBookVesselListViewModel point)
        //{
        //    var data = await LogisticDbContext.PreBookVesselLocationMapping
        //        .FirstOrDefaultAsync(Q => Q.LocationCode == point.LocationCode);

        //    data.RoutingMasterCode = point.PointPreBookVesselId;

        //    await LogisticDbContext.Database.GetDbConnection().ExecuteAsync(@"
        //       update PreBookVesselLocationMapping
        //        set RoutingMasterCode = @routingMasterCode
        //        where LocationCode = @locationCode

        //    ", new { routingMasterCode = point.PointPreBookVesselId, locationCode = point.LocationCode });

        //}
        // TIE: END
    }
}
