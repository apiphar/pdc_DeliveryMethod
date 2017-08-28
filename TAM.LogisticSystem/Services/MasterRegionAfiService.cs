using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using Dapper;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class MasterRegionAfiService
    {
        public MasterRegionAfiService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironmentService = webEnvironmentService;
        }
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;

        public async Task<List<AFIRegion>> GetAllRegionAfiData()
        {
            
            var dataRegionAfi = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<AFIRegion>(
                @"select * from AFIRegion
                ")).ToList();

            return dataRegionAfi;
        }

        public async Task<List<MasterRegionAFIPostCodeModel>> GetPostCode()
        {
            _ = nameof(Region.PostCode);
            var dataPostCode = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<MasterRegionAFIPostCodeModel>(
                @"SELECT DISTINCT postcode
                    FROM Region WHERE PostCode != '' 
                ")).ToList();
            //var dataPostCode1 = await LogisticDbContext.Region.Where(Q => Q.PostCode != "").Distinct()
            //    .Select(J => new MasterRegionAFIPostCodeModel
            //    {
            //        PostCode = J.PostCode
            //    }).ToListAsync();
            return dataPostCode;
        }
        public async Task<List<MasterRegionAFIViewModel>> GetAllRegionData()
        {
            var dataRegion = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<MasterRegionAFIViewModel>(
                @"SELECT a.Name AS Kelurahan, 
		c.Name AS Kota,
		a.PostCode,
		a.RegionCode,
		a.ParentRegionCode,
		a.AFIRegionCode,
		d.name AS afiregionname
FROM Region AS a 
INNER JOIN region AS b
ON a.ParentRegionCode = b.RegionCode
INNER JOIN dbo.Region AS c
ON b.ParentRegionCode = c.RegionCode
INNER JOIN dbo.AFIRegion AS d 
ON a.AFIRegionCode = d.AFIRegionCode
WHERE a.Type = 'KEL'
                ")).ToList();
            if (dataRegion != null)
            {
                foreach (var data in dataRegion)
                {
                    data.AFIRegion = data.AFIRegionCode + " - " + data.AFIRegionName;
                }
            }
            return dataRegion;
        }

        public async Task AddRegionAfiData(string posCode, MasterRegionAFIViewModel regionAfi)
        {
            var selected = await LogisticDbContext.Region.Where(Q => Q.PostCode == regionAfi.PostCode).ToListAsync();
            var userName = this.WebEnvironmentService.UserHumanName;
            foreach (var data in selected)
            {
                data.PostCode = regionAfi.PostCode;
                data.AFIRegionCode = regionAfi.AFIRegionCode;
                data.UpdatedAt = DateTime.UtcNow;
                data.UpdatedBy = userName;
            }
            LogisticDbContext.Region.UpdateRange(selected);
            await LogisticDbContext.SaveChangesAsync();

            //var userName = this.WebEnvironmentService.UserHumanName;

            //var addingRegion = new Region
            //{
            //    //PostCode= regionAfi.PostCode,
            //    AFIRegionCode = regionAfi.AFIRegionCode,
            //    CreatedBy = userName,
            //    CreatedAt = DateTime.UtcNow,
            //    UpdatedAt = DateTime.UtcNow,
            //    UpdatedBy = userName
            //};

            //LogisticDbContext.Region.Add(addingRegion);

            //await LogisticDbContext.SaveChangesAsync();
        }

        //public async Task DeleteRegionAfiData(MasterRegionAfiPostViewModel regionAfi)
        //{
        //    var selected = await LogisticDbContext.Region.FirstOrDefaultAsync(Q => Q.PostCode == regionAfi.PostCode);
        //    var userName = this.WebEnvironmentService.UserHumanName;


        //    selected.AFIRegionCode = regionAfi.AFIRegionCode=null;


        //    LogisticDbContext.Region.Update(selected);
        //    await LogisticDbContext.SaveChangesAsync();
        //}

        public async Task DeleteRegionAfiData(string id)
        {
            var selected = await LogisticDbContext.Region.FirstOrDefaultAsync(Q => Q.RegionCode == id);
            if (selected != null)
            {
                selected.AFIRegionCode = null;
            }
            await LogisticDbContext.SaveChangesAsync();
        }

        public async Task UpdateRegionAfiData(MasterRegionAfiPostViewModel regionAfi)
        {
            var selected = await LogisticDbContext.Region.Where(Q => Q.PostCode == regionAfi.PostCode).ToListAsync();
            var userName = this.WebEnvironmentService.UserHumanName;

            foreach (var data in selected)
            {
                data.PostCode = regionAfi.PostCode;
                data.AFIRegionCode = regionAfi.AFIRegionCode;
                data.UpdatedAt = DateTime.UtcNow;
                data.UpdatedBy = userName;
            }

            LogisticDbContext.Region.UpdateRange(selected);
            await LogisticDbContext.SaveChangesAsync();
        }
    }
}
