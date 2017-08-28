using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class MCCPService
    {
        private readonly LogisticDbContext logisticDbContext;

        public MCCPService(LogisticDbContext tangoDb)
        {
            this.logisticDbContext = tangoDb;
        }

        public List<MCCPViewModel> GetAllData()
        {
            var data = logisticDbContext.MonthlyCarCarrierPlanConfiguration.Join(
                logisticDbContext.Branch,
                MCCP => MCCP.BranchCode,
                branch => branch.BranchCode,
                (MCCP, branch) => new MCCPViewModel
                {
                    Baris = MCCP.ConfigurationRow,
                    LokAsal = MCCP.LocationFrom,
                    LokTujuan = MCCP.LocationTo,
                    Keterangan = MCCP.WordToSearch,
                    BranchCode = MCCP.BranchCode,
                    DealerCode = MCCP.DealerCode,
                    SqlField = MCCP.ResultQuery,
                    BranchName = branch.Name
                }).Join(
                logisticDbContext.Dealer,
                MCCP => MCCP.DealerCode,
                Dealer => Dealer.DealerCode,
                (MCCP, Dealer) => new MCCPViewModel
                {
                    Baris = MCCP.Baris,
                    LokAsal = MCCP.LokAsal,
                    LokTujuan = MCCP.LokTujuan,
                    Keterangan = MCCP.Keterangan,
                    BranchCode = MCCP.BranchCode,
                    DealerCode = MCCP.DealerCode,
                    SqlField = MCCP.SqlField,
                    BranchName = MCCP.BranchName,
                    DealerName = Dealer.Name
                }).Join(
                logisticDbContext.Location,
                MCCP => MCCP.LokAsal,
                location => location.LocationCode,
                (MCCP, location) => new MCCPViewModel
                {
                    Baris = MCCP.Baris,
                    LokAsal = MCCP.LokAsal,
                    LokTujuan = MCCP.LokTujuan,
                    Keterangan = MCCP.Keterangan,
                    BranchCode = MCCP.BranchCode,
                    DealerCode = MCCP.DealerCode,
                    SqlField = MCCP.SqlField,
                    BranchName = MCCP.BranchName,
                    DealerName = MCCP.DealerName,
                    LokAsalName = location.Name
                }).Join(
                logisticDbContext.Location,
                MCCP => MCCP.LokTujuan,
                location => location.LocationCode,
                (MCCP, location) => new MCCPViewModel
                {
                    Baris = MCCP.Baris,
                    LokAsal = MCCP.LokAsal,
                    LokTujuan = MCCP.LokTujuan,
                    Keterangan = MCCP.Keterangan,
                    BranchCode = MCCP.BranchCode,
                    DealerCode = MCCP.DealerCode,
                    SqlField = MCCP.SqlField,
                    BranchName = MCCP.BranchName,
                    DealerName = MCCP.DealerName,
                    LokAsalName = MCCP.LokAsalName,
                    LokTujuanName = location.Name
                }).ToList();

            return data;
        }


        public List<Branch> GetBranchData()
        {
            var branch = logisticDbContext.Branch.OrderBy(x => x.BranchCode).Select(x => new Branch()
            {
                BranchCode = x.BranchCode,
                Name = x.Name
            }).ToList();

            return branch;
        }
        public List<Dealer> GetDealerData()
        {
            var dealer = logisticDbContext.Dealer.OrderBy(x => x.DealerCode).Select(x => new Dealer()
            {
                DealerCode = x.DealerCode,
                Name = x.Name
            }).ToList();

            return dealer;
        }
        public List<Location> GetLocationData()
        {
            var location = logisticDbContext.Location.OrderBy(x => x.LocationCode).Select(x => new Location()
            {
                LocationCode = x.LocationCode,
                Name = x.Name
            }).ToList();

            return location;
        }
        public async Task<int> Add(MCCPViewModel model)
        {
            var entity = new MonthlyCarCarrierPlanConfiguration();
            {
                entity.ConfigurationRow = model.Baris;
                entity.BranchCode = model.BranchCode;
                entity.DealerCode = model.DealerCode;
                entity.LocationFrom = model.LokAsal;
                entity.LocationTo = model.LokTujuan;
                entity.WordToSearch = model.Keterangan;
                entity.ResultQuery = model.SqlField;
            };

            logisticDbContext.Add(entity);
            return await logisticDbContext.SaveChangesAsync();
        }

        public async Task<int> Remove(int baris)
        {
            var data = await logisticDbContext.MonthlyCarCarrierPlanConfiguration.Where(x => x.ConfigurationRow==baris).FirstOrDefaultAsync();
            if (data != null)
            {
                logisticDbContext.Remove(data);
            }

            int rowsAffected = await logisticDbContext.SaveChangesAsync();
            return rowsAffected;
        }
        public async Task<int> Update(int baris, MCCPViewModel model)
        {
            var data = await logisticDbContext.MonthlyCarCarrierPlanConfiguration.Where(x => x.ConfigurationRow == baris).FirstOrDefaultAsync();
            int rowsAffected = 0;

            if (data != null)
            {
                data.BranchCode = model.BranchCode;
                data.DealerCode = model.DealerCode;
                data.LocationFrom = model.LokAsal;
                data.LocationTo = model.LokTujuan;
                data.WordToSearch = model.Keterangan;
                data.ResultQuery = model.SqlField;
                rowsAffected = await logisticDbContext.SaveChangesAsync();
            }

            return rowsAffected;
        }


    }
}
