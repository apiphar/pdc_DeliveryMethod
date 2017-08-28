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
    public class RoutingDictionaryDetailService
    {
        private readonly LogisticDbContext DB;

        public RoutingDictionaryDetailService(LogisticDbContext db)
        {
            this.DB = db;
        }

        // TIE: START
        //        public List<RoutingDictionaryDetailViewModel> GetRoutingDictionaryDetail(int RoutingDictionaryId)
        //        {
        //            var dbconnection = DB.Database.GetDbConnection();
        //            {
        //                string query = @"select 
        //								rdd.RoutingDictionaryDetailId,
        //								rdd.DeliveryMethodCode,
        //								rdd.Ordering,
        //								rd.RoutingDictionaryId,
        //								rm.RoutingMasterCode,
        //								rm.Name as RoutingMasterName,
        //                                br.BranchCode,
        //                                br.Name as BranchName,
        //                                ct.Katashiki,
        //                                ct.Suffix,
        //                                lc.LocationCode,
        //                                lc.Name as LocationName,
        //                                dl.DealerCode,
        //                                dl.Name as DealerName,
        //                                cm.CompanyCode,
        //                                cm.Name as CompanyName,
        //                                dm.Name as DeliveryMethodName
        //                                from RoutingDictionaryDetail rdd
        //								left join RoutingDictionary rd on rd.RoutingDictionaryId = rdd.RoutingDictionaryId
        //								left join RoutingMaster rm on rm.RoutingMasterCode = rdd.RoutingMasterCode
        //                                left join DeliveryMethod dm on dm.DeliveryMethodCode = rdd.DeliveryMethodCode
        //                                left join Branch br on br.BranchCode = rd.BranchCode
        //                                left join Company cm on cm.CompanyCode = br.CompanyCode
        //                                left join Dealer dl on dl.DealerCode = cm.DealerCode
        //                                left join Location lc on lc.LocationCode = br.LocationCode
        //                                left join CarType ct on ct.Katashiki = rd.Katashiki and ct.Suffix = rd.Suffix
        //where rd.RoutingDictionaryId = @RoutingDictionaryId ";

        //                var result = dbconnection.Query<RoutingDictionaryDetailViewModel>(query, new
        //                {
        //                    RoutingDictionaryId = RoutingDictionaryId
        //                }).ToList();

        //                return result;
        //            }
        //        }
        // TIE: END

        // TIE: START
        //public List<RoutingDictionaryDetailViewModel> GetRoutingDictionary()
        //{
        //    var dbconnection = DB.Database.GetDbConnection();
        //    {
        //        string query = @"select 
        //rd.RoutingDictionaryId,
        //                        br.BranchCode,
        //                        br.Name as BranchName,
        //                        ct.Katashiki,
        //                        ct.Suffix,
        //                        dl.DealerCode,
        //                        dl.Name as DealerName,
        //                        cm.CompanyCode,
        //                        cm.Name as CompanyName
        //                        from RoutingDictionaryDetail rdd
        //left join RoutingDictionary rd on rd.RoutingDictionaryId = rdd.RoutingDictionaryId
        //left join RoutingMaster rm on rm.RoutingMasterCode = rdd.RoutingMasterCode
        //                        left join Branch br on br.BranchCode = rd.BranchCode
        //                        left join Company cm on cm.CompanyCode = br.CompanyCode
        //                        left join Dealer dl on dl.DealerCode = cm.DealerCode
        //                        left join Location lc on lc.LocationCode = br.LocationCode
        //                        left join CarType ct on ct.Katashiki = rd.Katashiki and ct.Suffix = rd.Suffix";

        //        var result = dbconnection.Query<RoutingDictionaryDetailViewModel>(query, new
        //        {
        //        }).ToList();

        //        return result;
        //    }
        //}
        // TIE: END

        public List<Location> GetLocation()
        {
            var items = DB.Location.OrderBy(x => x.LocationCode).Select(x => new Location()
            {
                LocationCode = x.LocationCode,
                Name = x.Name
            }).ToList();

            items.Insert(0, new Location()
            {
                LocationCode = "",
                Name = ""
            });

            return items;
        }

        public List<DeliveryMethod> GetDeliveryMethod()
        {
            var items = DB.DeliveryMethod.OrderBy(x => x.DeliveryMethodCode).Select(x => new DeliveryMethod()
            {
                DeliveryMethodCode = x.DeliveryMethodCode,
                Name = x.Name
            }).ToList();

            items.Insert(0, new DeliveryMethod()
            {
                DeliveryMethodCode = "",
                Name = ""
            });

            return items;
        }

        public List<ProcessMaster> GetProcessMaster()
        {
            var items = DB.ProcessMaster.OrderBy(x => x.ProcessMasterCode).Select(x => new ProcessMaster()
            {
                ProcessMasterCode = x.ProcessMasterCode,
                Name = x.Name
            }).ToList();

            items.Insert(0, new ProcessMaster()
            {
                ProcessMasterCode = "",
                Name = ""
            });

            return items;
        }

        public async Task<int> Add(RoutingDictionaryDetailViewModel model)
        {
            //model.CreatedAt = DateTime.Now;
            //model.CreatedBy = "SYSTEM";
            //model.UpdatedAt = DateTime.Now;
            //model.UpdatedBy = "SYSTEM";
            var entity = new ProcessDictionaryDetail();
            {
                entity.ProcessDictionaryId = model.RoutingDictionaryId;
                entity.LocationCode = model.LocationCode;
                entity.ProcessMasterCode = model.RoutingMasterCode;
                entity.DeliveryMethodCode = model.DeliveryMethodCode;
                entity.Ordering = model.Ordering;

            }
            DB.Add(entity);
            return await DB.SaveChangesAsync();
        }

        // TIE: START
        //public async Task<ProcessDictionaryDetail> Get(int id)
        //{
        //    return await DB.ProcessDictionaryDetail.FirstOrDefaultAsync(m => m.ProcessDictionaryDetailId == id);
        //}

        //public async Task<int> Update(int id, RoutingDictionaryDetailViewModel model)
        //{
        //    var data = await DB.ProcessDictionaryDetail.Where(x => x.ProcessDictionaryDetailId == id).FirstOrDefaultAsync();
        //    int rowsAffected = 0;

        //    if (data != null)
        //    {
        //        //data.RoutingDictionaryId = model.RoutingDictionaryId;
        //        data.LocationCode = model.LocationCode;
        //        data.RoutingMasterCode = model.RoutingMasterCode;
        //        data.DeliveryMethodCode = model.DeliveryMethodCode;
        //        data.Ordering = model.Ordering;

        //        rowsAffected = await DB.SaveChangesAsync();
        //    }
        //    return rowsAffected;
        //}
        // TIE: END

        public async Task<int> Remove(int id)
        {
            var data = await DB.ProcessDictionaryDetail.Where(x => x.ProcessDictionaryId == id).FirstOrDefaultAsync();
            if (data != null)
            {
                DB.Remove(data);
            }
            return await DB.SaveChangesAsync();
        }
    }
}
