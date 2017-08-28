using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class KonfigurasiGesekanService
    {
        private readonly LogisticDbContext tangoDBContext;

        public KonfigurasiGesekanService(LogisticDbContext tangoDBContext)
        {
            this.tangoDBContext = tangoDBContext;
        }

        // TIE: START
        //public int SaveConfiguration(ScratchConfigInsertData Data)
        //{
        //    int row = 0;
        //    tangoDBContext.Database.CreateExecutionStrategy().Execute(() =>
        //    {
        //        using (var transaction = tangoDBContext.Database.BeginTransaction())
        //        {
        //            int count = 0;
        //            if (Data.InsertData.Count!=0)
        //            {
        //                List<ScratchConfiguration> scratchInsertList = new List<ScratchConfiguration>();
        //                Data.InsertData.ForEach(
        //                    Q =>
        //                    {
        //                        ScratchConfiguration scratchInsert = new ScratchConfiguration()
        //                        {
        //                            NumberOfScratch = Data.jumlahGesekan,
        //                            BranchCode = Q[0],
        //                            CarModelCode = Q[1]
        //                        };
        //                        scratchInsertList.Add(scratchInsert);
        //                    }
        //                );
        //                tangoDBContext.AddRange(scratchInsertList);
        //                count += Data.InsertData.Count;
        //            }
        //            if (Data.UpdateData.Count != 0)
        //            {
        //                Data.UpdateData.ForEach(
        //                    Q =>
        //                    {
        //                        ScratchConfiguration scratch = tangoDBContext.ScratchConfiguration.FirstOrDefault(i => i.BranchCode == Q[0] && i.CarModelCode == Q[1]);
        //                        scratch.NumberOfScratch = Data.jumlahGesekan;
        //                    }
        //                );
        //                count += Data.UpdateData.Count;
        //            }
        //            row = tangoDBContext.SaveChanges() == count ? 1 : 0 ;
        //            transaction.Commit();
        //        }
        //    });
        //    return row;
        //}

        //public async Task<List<DealerListViewModel>> GetDealerBranch(string locationCode)
        //{
        //    var con = tangoDBContext.Database.GetDbConnection();
        //    {
        //        string query = @"SELECT a.DealerCode ,[DealerName] = a.Name,c.BranchCode,[BranchName] = c.Name
        //                        FROM Dealer a
        //                        JOIN Company b ON b.DealerCode = a.DealerCode
        //                        JOIN Branch c ON c.CompanyCode = b.CompanyCode
        //                        WHERE c.LocationCode = @locationCode";
        //        var result = await con.QueryAsync<DealerBranchViewModel>(query, new { locationCode = locationCode });

        //        var Dealer = result.ToList().GroupBy(Q => Q.DealerCode).Select(i => i.FirstOrDefault());
        //        List<DealerListViewModel> DealerList = new List<DealerListViewModel>();
        //        Dealer.ToList().ForEach(
        //            Q =>
        //            {
        //                DealerListViewModel dl = new DealerListViewModel()
        //                {
        //                    DealerName = Q.DealerName,
        //                    DealerCode = Q.DealerCode,
        //                    Branch = result.Where(i => i.DealerCode == Q.DealerCode).ToList()
        //                };
        //                DealerList.Add(dl);
        //            }
        //        );
        //        return DealerList;
        //    }
        //}

        //public async Task<List<CarModelViewModel>> GetCarModel(string locationCode)
        //{
        //    var con = tangoDBContext.Database.GetDbConnection();
        //    {
        //        string query = @"SELECT e.CarModelCode,e.Name
        //                        FROM RoutingDictionary a
        //                        JOIN RoutingDictionaryDetail b ON b.RoutingDictionaryId = a.RoutingDictionaryId
        //                        JOIN CarType c ON c.Katashiki = a.Katashiki AND c.Suffix = a.Suffix
        //                        JOIN CarSeries d ON d.CarSeriesCode = c.CarSeriesCode
        //                        JOIN CarModel e ON e.CarModelCode = d.CarModelCode
        //                        WHERE b.LocationCode = @locationCode
        //                        ORDER BY e.Name";
        //        var result = await con.QueryAsync<CarModelViewModel>(query, new { locationCode = locationCode });
        //        return result.ToList();
        //    }

        //}

        //public List<ScratchConfiguration> GetScratchByBranchCode(string branchCode,string locationCode)
        //{
        //    var con = tangoDBContext.Database.GetDbConnection();
        //    {
        //        string query = @"SELECT a.*
        //                        FROM ScratchConfiguration a
        //                        JOIN CarModel b ON b.CarModelCode = a.CarModelCode
        //                        WHERE BranchCode = @branch
        //                        AND a.CarModelCode IN (
        //                          SELECT e.CarModelCode
        //                          FROM RoutingDictionary a
        //                          JOIN RoutingDictionaryDetail b ON b.RoutingDictionaryId = a.RoutingDictionaryId
        //                          JOIN CarType c ON c.Katashiki = a.Katashiki AND c.Suffix = a.Suffix
        //                          JOIN CarSeries d ON d.CarSeriesCode = c.CarSeriesCode
        //                          JOIN CarModel e ON e.CarModelCode = d.CarModelCode
        //                          WHERE b.LocationCode = @locationCode
        //                        )
        //                        ORDER BY b.Name";
        //        return con.Query<ScratchConfiguration>(query, new { branch = branchCode , locationCode = locationCode }).ToList();
        //    }
        //}

        //public async Task<DataTable> GetKonfigurasiGesekanDt(string locationCode)
        //{
        //    var Branch = tangoDBContext.Branch.Where(Q => Q.LocationCode == locationCode).ToList();
        //    var CarModel = await GetCarModel(locationCode);
        //    DataTable dataTable = new DataTable();
        //    dataTable.Columns.Add("Dealer",typeof(string));
        //    foreach(var item in CarModel)
        //    {
        //        dataTable.Columns.Add(item.Name,typeof(int));
        //    }
        //    object[] values = new object[CarModel.Count+1];
        //    Branch.ForEach(branch=> {
        //        values[0] = branch.Name;
        //        var ScracthConfig = GetScratchByBranchCode(branch.BranchCode,locationCode).ToList();
        //        ScracthConfig.ForEach(scratch => {
        //            var index = CarModel.Select(s => s.CarModelCode).ToList().IndexOf(scratch.CarModelCode);
        //            if (index >= 0)
        //            {
        //                values[index + 1] = scratch.NumberOfScratch;
        //            }
        //        });
        //        dataTable.Rows.Add(values);
        //    });
        //    return dataTable;
        //}

        //public async Task<List<DealerBranchViewModel>> GetExistsScratch()
        //{
        //    var con = tangoDBContext.Database.GetDbConnection();
        //    {
        //        string query = @"SELECT c.DealerCode,[DealerName]=c.Name,b.CompanyCode,a.BranchCode,[BranchName]=a.Name,
        //                        d.CarModelCode,[CarModelName]=d.Name,[JumlahGesek]=e.NumberOfScratch
        //                        FROM Branch a
        //                        LEFT JOIN Company b ON a.CompanyCode = b.CompanyCode
        //                        LEFT JOIN Dealer c ON c.DealerCode = b.DealerCode
        //                        CROSS APPLY
        //                                (
        //                                SELECT CarModelCode,Name
        //                          FROM CarModel
        //                                ) d
        //                        LEFT JOIN ScratchConfiguration e ON e.BranchCode = a.BranchCode AND d.CarModelCode = e.CarModelCode
        //                        ORDER BY c.DealerCode";
        //        var result = await con.QueryAsync<DealerBranchViewModel>(query, new { });
        //        return result.ToList();
        //    }
        //}
        // TIE: END
    }
}
