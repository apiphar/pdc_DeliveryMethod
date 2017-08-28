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
    public class MasterModelService
    {
        private LogisticDbContext logisticDbContext;
        private readonly WebEnvironmentService WebEnvService;

        public MasterModelService(LogisticDbContext logisticDbContext, WebEnvironmentService WebEnvService)
        {
            this.logisticDbContext = logisticDbContext;
            this.WebEnvService = WebEnvService;
        }

       

        public List<MasterModelSearchResult> GetMasterModelData()
        {
            var dbconnection = logisticDbContext.Database.GetDbConnection();
            {
                var query = @"
                                SELECT cM.CarModelCode,cM.Name, b.Name BrandName,b.BrandCode , p.PlantCode, p.Name PlantName
                                FROM CarModel cM
                                LEFT OUTER JOIN Brand b on cM.BrandCode = b.BrandCode
                                LEFT OUTER JOIN Plant p on cM.PlantCode = p.PlantCode
                               ";


                var result = dbconnection.Query<MasterModelSearchResult>(query, new
                {

                }).ToList();

                return result;
            }
        }

        public int Add(string carModelCode, string name, string brandId, string plantCode)
        {
          
            var username = WebEnvService.UserHumanName;
            var codeExist = logisticDbContext.CarModel.Where(x => x.CarModelCode == carModelCode).FirstOrDefault();

            if (codeExist != null)
            {
                return 0;
            }
            else
            {
                var insert = new CarModel
                {
                    CarModelCode = carModelCode.ToUpper(),
                    Name = name.ToUpper(),
                    BrandCode = brandId,
                    PlantCode = plantCode,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    CreatedBy = username,
                    UpdatedBy = username
                };

                logisticDbContext.Add(insert);
                logisticDbContext.SaveChanges();
            }

            return 1;
        }

        public CarModel Get(string id)
        {
            return logisticDbContext.CarModel.Find(id);
        }

        public int Remove(CarModel entity)
        {
            logisticDbContext.Remove(entity);
            return logisticDbContext.SaveChanges();
        }

        public int Update(CarModel entity, string name, string brandId, string plantCode)
        {
            var username = WebEnvService.UserHumanName;
            entity.Name = name.ToUpper();
            entity.BrandCode = brandId;
            entity.PlantCode = plantCode;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            entity.UpdatedBy = username;
           
            return logisticDbContext.SaveChanges();
        }

        public MasterModelDelete MasterModelGetDelete(string id)
        {
            using (var connectionQ = logisticDbContext.Database.GetDbConnection())
            {
                var query = @"Select a.CarModelCode, a.Name, a.BrandCode
                                 From CarModel a
                                 Where a.CarModelCode = @carModelId";
                var resulta = connectionQ.Query<MasterModelDelete>(query, new
                {
                    carModelId = id
                }).FirstOrDefault();

                return resulta;
            }
        }

       

        public List<Brand> GetBrand()
        {
            var brand = logisticDbContext.Brand.OrderBy(m => m.Name).Select(m => new Brand()
            {
                Name = m.Name,
                BrandCode = m.BrandCode
            }).ToList();

            return brand;
        }

        public List<Plant> GetManufacturing()
        {
            var plant = logisticDbContext.Plant.OrderBy(m => m.Name).Select(m => new Plant()
            {
                Name = m.Name,
                PlantCode = m.PlantCode
            }).ToList();

            return plant;
        }

        public int CekMOdelCode(string modelCode)
        {
            var tidak = 0;
            var codeExist = logisticDbContext.CarModel.Where(x => x.CarModelCode == modelCode).FirstOrDefault();

            if(codeExist != null)
            {
                tidak = 1;
            }

            return tidak;
        }

    }
}

