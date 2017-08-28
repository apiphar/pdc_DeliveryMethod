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
    public class RoutingDictionaryService
    {
        private readonly LogisticDbContext DB;

        public RoutingDictionaryService(LogisticDbContext db)
        {
            this.DB = db;
        }

        public List<RoutingDictionaryViewModel> GetRoutingDictionary()
        {
            var dbconnection = DB.Database.GetDbConnection();
            {
                string query = @"select 
                                rd.RoutingDictionaryId,
                                br.BranchCode,
                                br.Name as BranchName,
                                ct.Katashiki,
                                ct.Suffix,
                                cs.CarModelCode,
                                cs.Name as CarModelName,
                                dl.DealerCode,
                                dl.Name as DealerName,
                                cm.CompanyCode,
                                cm.Name as CompanyName
                                from RoutingDictionary rd
                                left join Branch br on br.BranchCode = rd.BranchCode
                                left join Company cm on cm.CompanyCode = br.CompanyCode
                                left join Dealer dl on dl.DealerCode = cm.DealerCode
                                left join CarType ct on ct.Katashiki = rd.Katashiki and ct.Suffix = rd.Suffix
								left join CarSeries cs on cs.CarSeriesCode = ct.CarSeriesCode";

                var result = dbconnection.Query<RoutingDictionaryViewModel>(query, new
                {
                }).ToList();

                return result;
            }
        }

        public List<RoutingDictionaryViewModel> GetVehicle()
        {
            var dbconnection = DB.Database.GetDbConnection();
            string query = @"select 
                            a.Katashiki,
                            a.Suffix,
                            b.CarModelCode,
                            b.Name as CarModelName
                            from CarType a
                            left join CarSeries b on b.CarSeriesCode = a.CarSeriesCode
                            left join CarModel c on c.CarModelCode = b.CarModelCode";

            var result = dbconnection.Query<RoutingDictionaryViewModel>(query, new
            {
            }).ToList();

            return result;
        }

        public List<RoutingDictionaryViewModel> GetSuffix(string Katashiki)
        {
            var dbconnection = DB.Database.GetDbConnection();
            string query = @"select a.Katashiki
	                          ,a.Suffix
	                          from CarType a
	                          where a.Katashiki = @Katashiki";

            var result = dbconnection.Query<RoutingDictionaryViewModel>(query, new
            {
                Katashiki = Katashiki
            }).ToList();
            return result;
        }

        public List<Branch> GetBranch()
        {
            var items = DB.Branch.OrderBy(x => x.BranchCode).Select(x => new Branch()
            {
                BranchCode = x.BranchCode,
                Name = x.Name
            }).ToList();

            items.Insert(0, new Branch()
            {
                BranchCode = "",
                Name = ""
            });

            return items;
        }

        public List<RoutingDictionaryViewModel> GetDealer()
        {
            var dbconnection = DB.Database.GetDbConnection();
            {
                string query = @"select
                                dl.DealerCode,
                                dl.Name as DealerName
								from RoutingDictionary rd
                                left join Branch br on br.BranchCode = rd.BranchCode
                                left join Company cm on cm.CompanyCode = br.CompanyCode
                                left join Dealer dl on dl.DealerCode = cm.DealerCode";

                var result = dbconnection.Query<RoutingDictionaryViewModel>(query, new
                {
                }).ToList();

                return result;
            }
        }

        public async Task<int> Add(RoutingDictionaryViewModel model)
        {
            model.ValidFrom = DateTime.Now;

            var entity = new ProcessDictionary();
            {
                entity.BranchCode = model.BranchCode;
                entity.Katashiki = model.Katashiki;
                entity.Suffix = model.Suffix;    
            }
            DB.Add(entity);
            return await DB.SaveChangesAsync();
        }

        public async Task<ProcessDictionary> Get(int id)
        {
            return await DB.ProcessDictionary.FirstOrDefaultAsync(m => m.ProcessDictionaryId == id);
        }

        public async Task<int> Update(int id, RoutingDictionaryViewModel model)
        {
            var data = await DB.ProcessDictionary.Where(x => x.ProcessDictionaryId == id).FirstOrDefaultAsync();
            int rowsAffected = 0;

            if (data != null)
            {
                data.BranchCode = model.BranchCode;
                data.Katashiki = model.Katashiki;
                data.Suffix = model.Suffix;

                rowsAffected = await DB.SaveChangesAsync();
            }
            return rowsAffected;
        }


            public async Task<int> Remove(int id)
        {
            var data = await DB.ProcessDictionary.Where(x => x.ProcessDictionaryId == id).FirstOrDefaultAsync();
            if (data != null)
            {
                DB.Remove(data);
            }
            return await DB.SaveChangesAsync();
        }
    }
}
