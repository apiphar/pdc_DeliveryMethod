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
    public class BranchService
    {
        private readonly LogisticDbContext logisticDbContext;
        private readonly WebEnvironmentService webEnvironmentService;

        public BranchService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService)
        {
            this.logisticDbContext = logisticDbContext;
            this.webEnvironmentService = webEnvironmentService;
        }

        public async Task<List<BranchModel>> getDataBranch()
        {
            var dbconnection = logisticDbContext.Database.GetDbConnection();

            _ = nameof(Branch.BranchCode);
            _ = nameof(Branch.Name);
            _ = nameof(Branch.AS400BranchCode);
            _ = nameof(Branch.MasterDataPrimaryKey);
            _ = nameof(Branch.Phone);
            _ = nameof(Branch.Fax);
            _ = nameof(Branch.KabupatenCode);
            _ = nameof(Destination.DestinationCode);
            _ = nameof(Destination.Name);
            _ = nameof(Region.RegionCode);
            _ = nameof(Region.Name);
            _ = nameof(Company.CompanyCode);
            _ = nameof(Company.Name);
            _ = nameof(AS400Cluster.AS400ClusterCode);
            _ = nameof(AS400Cluster.Name);
            _ = nameof(SalesArea.SalesAreaCode);
            _ = nameof(SalesArea.Description);
            _ = nameof(AFIBranch.AFIBranchCode);
            _ = nameof(Location.LocationCode);
            _ = nameof(Location.Name);
            _ = nameof(BranchLocationMapping.LocationCode);

            var query = @"select Branch.BranchCode
                                , Branch.Name
                                , Branch.AS400BranchCode
                                , Branch.MasterDataPrimaryKey
                                , Branch.Phone
                                , Branch.Fax
                                , Branch.KabupatenCode
                                , Destination.DestinationCode
                                , Destination.Name as DestinationName
                                , Region.RegionCode
                                , Region.Name as RegionName
                                , Company.CompanyCode
                                , Company.Name as CompanyName
                                , AS400Cluster.AS400ClusterCode
                                , AS400Cluster.Name as ClusterName
                                , SalesArea.SalesAreaCode
                                , SalesArea.description as SalesAreaName
                                , AfiBranch.AFIBranchCode
                                , BranchLocationMapping.locationcode
                                , Location.Name as LocationName
                            from branch
                            left outer join Destination on Destination.DestinationCode = branch.DestinationCode
                            left outer join Region on Region.RegionCode = branch.RegionCode
                            left outer join Company on Company.CompanyCode = branch.CompanyCode
                            left outer join AS400Cluster on AS400Cluster.AS400ClusterCode = branch.AS400ClusterCode
                            left outer join SalesArea on SalesArea.SalesAreaCode = branch.SalesAreaCode
                            left outer join AfiBranch on AfiBranch.BranchCode = branch.BranchCode
                            left outer join BranchLocationMapping on BranchLocationMapping.BranchCode = branch.BranchCode
                            left outer join Location on Location.LocationCode = BranchLocationMapping.LocationCode";

            var dataBranch = (await dbconnection.QueryAsync<BranchModel>(query)).ToList();

            return dataBranch;
        }

        public async Task<List<SalesAreaModel>> getDataSalesArea()
        {
            //var dbconnection = logisticDbContext.Database.GetDbConnection();

            //_ = nameof(SalesArea.SalesAreaCode);
            //_ = nameof(SalesArea.Description);

            //var query = @"select a.SalesAreaCode
            //                  , a.description as SalesAreaName
            //                from SalesArea a";

            //var result = dbconnection.Query<SalesAreaModel>(query, new { }).ToList();
            //return result;

            var salesAreas = await this.logisticDbContext.SalesArea
                .Select(Q => new SalesAreaModel
                {
                    SalesAreaCode = Q.SalesAreaCode,
                    SalesAreaName = Q.Description
                })
                .ToListAsync();
            return salesAreas;
        }

        public async Task<List<DestinationModel>> getDataDestination()
        {
            //var dbconnection = logisticDbContext.Database.GetDbConnection();
            //var query = @"select a.DestinationCode
            //                  , a.Name as DestinationName
            //                from Destination a";

            //var result = dbconnection.Query<DestinationModel>(query, new { }).ToList();
            //return result;

            var destinations = await this.logisticDbContext.Destination
                .Select(Q => new DestinationModel
                {
                    DestinationCode = Q.DestinationCode,
                    DestinationName = Q.Name
                })
                .ToListAsync();
            return destinations;
        }

        public async Task<List<RegionModel>> getDataRegion()
        {
            //var dbconnection = logisticDbContext.Database.GetDbConnection();
            //var query = @"select a.RegionCode
            //                  , a.Name as RegionName
            //                from Region a";

            //var result = dbconnection.Query<RegionModel>(query, new { }).ToList();
            //return result;

            var regions = await this.logisticDbContext.Region
                .Select(Q => new RegionModel
                {
                    RegionCode = Q.RegionCode,
                    RegionName = Q.Name
                })
                .ToListAsync();
            return regions;
        }

        public async Task<List<CompanyModel>> getDataCompany()
        {
            //var dbconnection = logisticDbContext.Database.GetDbConnection();
            //var query = @"select a.CompanyCode
            //                  , a.Name as CompanyName
            //                from Company a";

            //var result = dbconnection.Query<CompanyModel>(query, new { }).ToList();
            //return result;

            var companys = await this.logisticDbContext.Company
                .Select(Q => new CompanyModel
                {
                    CompanyCode = Q.CompanyCode,
                    CompanyName = Q.Name
                })
                .ToListAsync();
            return companys;
        }

        public async Task<List<LocationModel>> getDataLocation()
        {
            var dbconnection = logisticDbContext.Database.GetDbConnection();

            _ = nameof(Location.LocationCode);
            _ = nameof(Location.Name);
            _ = nameof(LocationType.LocationTypeCode);

            var query = @"select Location.locationcode
		                            , Location.Name as LocationName
                            from Location
                            left outer join LocationType on LocationType.LocationTypecode = Location.LocationTypecode
                            where Location.LocationTypecode = 'BRCH'";

            var result = (await dbconnection.QueryAsync<LocationModel>(query)).ToList();
            return result;

            //var locations = await this.logisticDbContext.Location
            //    .Select(Q => new LocationModel
            //    {
            //        LocationCode = Q.LocationCode,
            //        LocationName = Q.Name
            //    })
            //    .ToListAsync();
            //return locations;
        }

        //public List<LocationModel> getDataLocation()
        //{
        //    var dbconnection = logisticDbContext.Database.GetDbConnection();
        //    string query = @"select a.locationcode
        //                      , a.Name as LocationName
        //                    from location a
        //                    left outer join branch b on  b.locationcode = a.locationcode
        //                    where a.LocationCode not in(select a.locationcode from branch a)";

        //    var result = dbconnection.Query<LocationModel>(query, new { }).ToList();
        //    return result;
        //}

        //public List<LocationModel> getDataLocation2(string locationcode)
        //{
        //    var dbconnection = logisticDbContext.Database.GetDbConnection();
        //    string query = @"select a.locationcode
        //                      , a.Name as LocationName
        //                    from location a
        //                    left outer join branch b on  b.locationcode = a.locationcode
        //                    where a.LocationCode not in(select a.locationcode from branch a)
        //                    union 
        //                    select a.locationcode
        //                      , a.Name as LocationName
        //                    from location a
        //                    left outer join branch b on  b.locationcode = a.locationcode
        //                    where a.locationcode = @locationcode ";

        //    var result = dbconnection.Query<LocationModel>(query, new
        //    {
        //        locationcode = locationcode
        //    }).ToList();
        //    return result;
        //}

        public async Task<List<ClusterModel>> getDataCluster()
        {
            //var dbconnection = logisticDbContext.Database.GetDbConnection();

            //_ = nameof(AS400Cluster.AS400ClusterCode);
            //_ = nameof(AS400Cluster.Name);

            //var query = @"select a.ClusterCode
            //                  , a.Name as ClusterName
            //                from AS400Cluster a";

            //var result = dbconnection.Query<ClusterModel>(query, new { }).ToList();
            //return result;

            var clusters = await this.logisticDbContext.AS400Cluster
                .Select(Q => new ClusterModel
                {
                    AS400ClusterCode = Q.AS400ClusterCode,
                    ClusterName = Q.Name
                })
                .ToListAsync();
            return clusters;
        }

        internal async Task<int> Add(BranchModel model)
        {
            var userName = webEnvironmentService.UserHumanName;
            var existingBranch = await logisticDbContext.Branch.Where(x => x.BranchCode == model.BranchCode).FirstOrDefaultAsync();
            var rowsAffected = 3;
            if (existingBranch != null)
            {
                return 0;
            }
            if (existingBranch == null)
            {
                var entity = new Branch
                {
                    BranchCode = model.BranchCode,
                    SalesAreaCode = model.SalesAreaCode,
                    CompanyCode = model.CompanyCode,
                    //LocationCode = model.LocationCode,
                    DestinationCode = model.DestinationCode,
                    RegionCode = model.RegionCode,
                    AS400ClusterCode = model.AS400ClusterCode,
                    Name = model.Name,
                    Phone = model.Phone,
                    Fax = model.Fax,
                    //BranchCodeAFI = model.BranchCodeAFI,
                    AS400BranchCode = model.AS400BranchCode,
                    KabupatenCode = model.KabupatenCode,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    CreatedBy = userName,
                    UpdatedBy = userName
                };
                logisticDbContext.Add(entity);
            };
            rowsAffected = await logisticDbContext.SaveChangesAsync();

            var existingAFIBranch = await logisticDbContext.AFIBranch.Where(x => x.AFIBranchCode == model.AFIBranchCode).FirstOrDefaultAsync();

            if (existingAFIBranch != null)
            {
                return 2;
            }
            if (existingAFIBranch == null)
            {
                var entity = new AFIBranch
                {
                    BranchCode = model.BranchCode,
                    AFIBranchCode = model.AFIBranchCode,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    CreatedBy = userName,
                    UpdatedBy = userName
                };
                logisticDbContext.Add(entity);
            };
            rowsAffected = await logisticDbContext.SaveChangesAsync();

            var existingBranchLocation = await logisticDbContext.BranchLocationMapping.Where(x => x.BranchCode == model.BranchCode && x.LocationCode == model.LocationCode).FirstOrDefaultAsync();

            if (existingBranchLocation == null)
            {
                var entity = new BranchLocationMapping
                {
                    BranchCode = model.BranchCode,
                    LocationCode = model.LocationCode,
                    CreatedAt = DateTimeOffset.UtcNow,
                    CreatedBy = userName,
                };
                logisticDbContext.Add(entity);
            };
            rowsAffected = await logisticDbContext.SaveChangesAsync();
            return rowsAffected;
        }

        internal async Task<int> Update(string id, BranchModel model)
        {
            var userName = webEnvironmentService.UserHumanName;
            var rowsAffected = 0;

            var existingBranch = await logisticDbContext.Branch.Where(x => x.BranchCode == id).FirstOrDefaultAsync();
            var existingAFIBranch = await logisticDbContext.AFIBranch.Where(x => x.BranchCode == id).FirstOrDefaultAsync();
            var existingBranchLocationMapping = await logisticDbContext.BranchLocationMapping.Where(x => x.BranchCode == id).FirstOrDefaultAsync();
            if (existingBranch != null)
            {
                await this.logisticDbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
                {
                    using (var transaction = await logisticDbContext.Database.BeginTransactionAsync())
                    {
                        existingBranch.SalesAreaCode = model.SalesAreaCode.ToUpper();
                        existingBranch.CompanyCode = model.CompanyCode.ToUpper();
                        existingBranch.DestinationCode = model.DestinationCode.ToUpper();
                        existingBranch.RegionCode = model.RegionCode.ToUpper();
                        existingBranch.AS400ClusterCode = model.AS400ClusterCode.ToUpper();
                        existingBranch.Name = model.Name.ToUpper();
                        existingBranch.Phone = model.Phone;
                        existingBranch.Fax = model.Fax;
                        existingBranch.AS400BranchCode = model.AS400BranchCode.ToUpper();
                        existingBranch.KabupatenCode = model.KabupatenCode.ToUpper();
                        existingBranch.UpdatedAt = DateTimeOffset.UtcNow;
                        existingBranch.UpdatedBy = userName.ToUpper();

                        //existingAFIBranch.AFIBranchCode = model.AFIBranchCode;
                        existingAFIBranch.UpdatedAt = DateTimeOffset.UtcNow;
                        existingAFIBranch.UpdatedBy = userName;

                        existingBranchLocationMapping.LocationCode = model.LocationCode;

                        rowsAffected = await logisticDbContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                });
            }
            return rowsAffected;
        }

        internal async Task<int> Remove(string id)
        {
            var existingBranch = await logisticDbContext.Branch.Where(x => x.BranchCode == id).FirstOrDefaultAsync();
            if (existingBranch != null)
            {
                logisticDbContext.Remove(existingBranch);
            }
            return await logisticDbContext.SaveChangesAsync();
        }
    }
}
