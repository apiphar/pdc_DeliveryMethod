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
    public class MaintenanceShiftKerjaService
    {
        private readonly LogisticDbContext logisticDbContext;
        private readonly WebEnvironmentService env;

        public MaintenanceShiftKerjaService(LogisticDbContext logisticDbContext,WebEnvironmentService env)
        {
            this.logisticDbContext = logisticDbContext;
            this.env = env;
        }

        public async Task<MaintenaceShiftKerjaViewModel> GetMaintenanceShiftKerja()
        {
            var allView = await this.logisticDbContext.LocationWorkHour.ToListAsync();
            foreach (var item in allView)
            {
                item.Start = item.Start.ToLocalTime();
                item.Finish = item.Finish.ToLocalTime();
            }

            var allShift = await this.logisticDbContext.Shift.ToListAsync();
            var allLocation = await this.logisticDbContext.Location.Select(Q => new MaintenaceShiftKerja_LocationModel
            {
                Nama = Q.Name,
                LocationCode = Q.LocationCode
            }).ToListAsync(); ;
            var viewModel = new MaintenaceShiftKerjaViewModel();
            viewModel.MaintenanceShiftKerjaFullModel = allView;
            viewModel.ShiftModel = allShift;
            viewModel.LokasiModel = allLocation;
            return viewModel;
        }


        public async Task<int> PostData(MaintenaceShiftKerjaPostModel postModel)
        {
            var viewIrisan = await this.logisticDbContext.LocationWorkHour.Where(Q => Q.LocationCode == postModel.LocationCode && Q.ShiftCode == postModel.ShiftCode).ToListAsync();
            if (IrisanJam(postModel.Start.ToUniversalTime(), postModel.Finish.ToUniversalTime(), viewIrisan) > 0)
            {
                return 0;
            }
            else
            {
                var newShiftKerja = new LocationWorkHour
                {
                    ShiftCode = postModel.ShiftCode,
                    Start = postModel.Start.ToUniversalTime(),
                    Finish = postModel.Finish.ToUniversalTime(),
                    LocationCode = postModel.LocationCode,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    CreatedBy = env.UserHumanName,
                    UpdatedBy= env.UserHumanName
                };
                this.logisticDbContext.LocationWorkHour.Add(newShiftKerja);
                return await this.logisticDbContext.SaveChangesAsync();
            }
        }
        public async Task<int> UpdateData(MaintenaceShiftKerjaPostModel postModel)
        {
            var viewIrisan = await this.logisticDbContext.LocationWorkHour.Where(Q => Q.LocationCode == postModel.LocationCode && Q.ShiftCode == postModel.ShiftCode && Q.LocationWorkHourId != postModel.LocationWorkHourId).ToListAsync();
            if (IrisanJam(postModel.Start.ToUniversalTime(), postModel.Finish.ToUniversalTime(), viewIrisan) > 0)
            {
                return 0;
            }
            else
            {
                var selectedData = await this.logisticDbContext.LocationWorkHour.Where(Q => Q.LocationWorkHourId == postModel.LocationWorkHourId).FirstOrDefaultAsync();
                selectedData.Start = postModel.Start.ToUniversalTime();
                selectedData.Finish = postModel.Finish.ToUniversalTime();
                selectedData.LocationCode = postModel.LocationCode;
                selectedData.ShiftCode = postModel.ShiftCode;
                selectedData.UpdatedBy = env.UserHumanName;
                selectedData.UpdatedAt = DateTimeOffset.UtcNow;
                this.logisticDbContext.LocationWorkHour.Update(selectedData);
                return await this.logisticDbContext.SaveChangesAsync();
            }
        }
        public async Task<int> DeleteData(int id)
        {
            var selectedData = await this.logisticDbContext.LocationWorkHour.Where(Q => Q.LocationWorkHourId == id).FirstOrDefaultAsync();
            this.logisticDbContext.Remove(selectedData);
            return await this.logisticDbContext.SaveChangesAsync();
        }
        public int IrisanJam(DateTimeOffset jamMulai, DateTimeOffset jamSelesai, List<LocationWorkHour> listData)
        {
            var flagintersection = 0;
            foreach (var jam in listData)
            {
                if (DateTimeOffset.Compare(jam.Finish, jamMulai) < 0 && DateTimeOffset.Compare(jamMulai, jam.Start) < 0)
                {
                    flagintersection++;
                }
                else if (DateTimeOffset.Compare(jam.Finish, jamMulai) > 0 && DateTimeOffset.Compare(jam.Finish, jamSelesai) < 0)
                {
                    flagintersection++;
                }
                else if (DateTimeOffset.Compare(jam.Start, jamMulai) > 0 && DateTimeOffset.Compare(jam.Start, jamSelesai) < 0)
                {
                    flagintersection++;
                }
            }
            return flagintersection;
        }
    }
}
