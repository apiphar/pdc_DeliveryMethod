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
    public class MasterModelSeriesService
    {
        private LogisticDbContext logisticDbContext;
        private readonly WebEnvironmentService WebEnvService;


        public MasterModelSeriesService(LogisticDbContext logisticDbContext,WebEnvironmentService webEnvService)
        {
            this.logisticDbContext = logisticDbContext;
            this.WebEnvService = webEnvService;
        }

      
        public List<MasterModelSeriesSearchResult> MasterSeriesGetData()
        {
            using (var connectionQ = logisticDbContext.Database.GetDbConnection())
            {
                var query = @" select cS.CarSeriesCode
                                     , cS.Name CarSeriesName
									 , cs.CarModelCode
                                     , cM.Name carModelName				                     
									from CarSeries cS
									inner join CarModel cM on cs.CarModelCode = cM.CarModelCode
                                ";
                var resulta = connectionQ.Query<MasterModelSeriesSearchResult>(query, new
                {
                   
                }).ToList();

                return resulta;
            }
        }

        public int Add(MasterModelSeriesCreateOrUpdate model)
        {
            var username = WebEnvService.UserHumanName;

            var existSeriesCOde = logisticDbContext.CarSeries.Where(s => s.CarSeriesCode == model.CarSeriesCode).FirstOrDefault();
            if (existSeriesCOde != null)
            {
                return 0;
            }
            else
            {
                var insert = new CarSeries
                {
                    CarSeriesCode = model.CarSeriesCode.ToUpper(),
                    Name = model.Name.ToUpper(),
                    CarModelCode = model.carModelCode.ToUpper(),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = username,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = username
                };

                logisticDbContext.Add(insert);
                logisticDbContext.SaveChanges();
            }

            return 1; 
        }

        public CarSeries Get(string id)
        {
            return logisticDbContext.CarSeries.Find(id);
        }

        public MasterModelSeriesCreateOrUpdate GetDataById(string id)
        {
            using (var connectionQ = logisticDbContext.Database.GetDbConnection())
            {
                var query = @" select cS.CarSeriesCode
                                     , cS.Name CarSeriesName
									 , cs.CarModelCode
                                     , cM.Name carModelName				                     
									from CarSeries cS
									inner join CarModel cM on cs.CarModelCode = cM.CarModelCode
                                 Where cS.CarSeriesCode = @carSeriesId";
                var resulta = connectionQ.Query<MasterModelSeriesCreateOrUpdate>(query, new
                {
                    carSeriesId = id
                }).FirstOrDefault();

                return resulta;
            }
        }

        public  int Remove(CarSeries entity)
        {
            //var exixtCode = logisticDbContext.CarType.Where()
            logisticDbContext.Remove(entity);
            return logisticDbContext.SaveChanges();
        }

        public int Update(CarSeries entity, string name, string carModelCode)
        {
            var username = WebEnvService.UserHumanName;
            entity.Name = name.ToUpper();
            entity.CarModelCode = carModelCode.ToUpper();
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = username;
            return logisticDbContext.SaveChanges();
        }
        public MasterModelSeriesDelete MasterModelGetDelete(string id)
        {
            using (var connectionQ = logisticDbContext.Database.GetDbConnection())
            {
                var query = @" select cS.CarSeriesCode
                                     , cS.Name CarSeriesName
									 , cs.CarModelCode
                                     , cM.Name carModelName				                     
									from CarSeries cS
									inner join CarModel cM on cs.CarModelCode = cM.CarModelCode
                                 Where cS.CarSeriesCode = @carSeriesId";
                var resulta = connectionQ.Query<MasterModelSeriesDelete>(query, new
                {
                    carSeriesId = id
                }).FirstOrDefault();

                return resulta;
            }
        }

        public List<CarModel> GetCarModel()
        {
            var carModel = logisticDbContext.CarModel.OrderBy(m => m.Name).Select(m => new CarModel()
            {
                Name = m.Name,
                CarModelCode = m.CarModelCode
            }).ToList();

           

            return carModel;
        }


        public int CekModelCode(string modelCode)
        {
            var tidak = 0;
            var codeExist = logisticDbContext.CarSeries.Where(x => x.CarSeriesCode == modelCode).FirstOrDefault();

            if (codeExist != null)
            {
                tidak = 1;
            }

            return tidak;
        }

    }
}
