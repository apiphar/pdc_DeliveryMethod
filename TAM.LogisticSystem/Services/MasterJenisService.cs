using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class MasterJenisService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;
        public MasterJenisService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironmentService = webEnvironmentService;
        }

        /// <summary>
        /// Get all data from database
        /// </summary>
        /// <returns></returns>
        public async Task<List<MasterJenisViewModel>> GetJenisData()
        {
            var jenisData = await this.LogisticDbContext.AFICarType
                .AsNoTracking()
                .Select(Q => new MasterJenisViewModel
                {
                    AFICarTypeCode = Q.AFICarTypeCode,
                    Jenis = Q.Jenis,
                    Model = Q.Model
                }).ToListAsync();
            return jenisData;
        }

        /// <summary>
        /// Insert data to database
        /// </summary>
        /// <returns></returns>
        public async Task AddJenisData(MasterJenisViewModel model)
        {
            var user = this.WebEnvironmentService.UserHumanName;
            var jenisData = new AFICarType
            {
                AFICarTypeCode = model.AFICarTypeCode.ToUpper(),
                Jenis = model.Jenis.ToUpper(),
                Model = model.Model.ToUpper(),
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = user,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = user
            };
            LogisticDbContext.Add(jenisData);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Delete selected data from database
        /// </summary>
        /// <returns></returns>
        public async Task<int> RemoveJenisData(string code)
        {
            var jenisData = await LogisticDbContext.AFICarType.FirstOrDefaultAsync(Q => Q.AFICarTypeCode == code);
            if (jenisData != null)
            {
                LogisticDbContext.Remove(jenisData);
            }
            var rowsAffected = await LogisticDbContext.SaveChangesAsync();
            return rowsAffected;
        }

        /// <summary>
        /// Update selected data from database
        /// </summary>
        /// <returns></returns>
        public async Task<int> UpdateJenisData(string code, MasterJenisViewModel model)
        {
            var user = this.WebEnvironmentService.UserHumanName;
            var existingJenisData = await LogisticDbContext.AFICarType.FirstOrDefaultAsync(Q => Q.AFICarTypeCode == code);
            var rowsAffected = 0;

            if (existingJenisData != null)
            {
                existingJenisData.Jenis = model.Jenis.ToUpper();
                existingJenisData.Model = model.Model.ToUpper();
                existingJenisData.UpdatedAt = DateTimeOffset.UtcNow;
                existingJenisData.UpdatedBy = user;
                rowsAffected = await LogisticDbContext.SaveChangesAsync();
            }
            return rowsAffected;
        }

        /// <summary>
        /// to validate Kode Jenis (AFICarTypeCode)
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Validate(string code)
        {
            var validateJenisData = await LogisticDbContext.AFICarType.FirstOrDefaultAsync(Q => Q.AFICarTypeCode == code);

            if(validateJenisData != null)
            {
                return true;
            }
            return false;
        }
    }
}
