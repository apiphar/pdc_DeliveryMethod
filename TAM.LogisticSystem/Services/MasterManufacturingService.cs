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
    public class MasterManufacturingService
    {
        private readonly LogisticDbContext _context;
        private readonly WebEnvironmentService environment;

        public MasterManufacturingService(LogisticDbContext db, WebEnvironmentService env)
        {
            this._context = db;
            this.environment = env;
        }

        /// <summary>
        /// Get All Data Plant
        /// </summary>
        /// <returns></returns>
        public async Task<List<ManufacturingViewModel>> GetAll()
        {
            var data = await this._context.Plant.Select(Q=>  new ManufacturingViewModel
            {
                PlantCode = Q.PlantCode,
                Name = Q.Name,
                Country = Q.Country  
            }).ToListAsync();
           
            return data;
        }

        /// <summary>
        /// Update Data Plant
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Update(ManufacturingUpdateViewModel data)
        {
            var selectedData = await this._context.Plant.FirstOrDefaultAsync(Q=>Q.PlantCode == data.PlantCode);

            selectedData.Name = data.Name.ToUpper();
            selectedData.Country = data.Country.ToUpper();
            selectedData.UpdatedAt = DateTimeOffset.UtcNow;
            selectedData.UpdatedBy = environment.UserHumanName;

            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Insert into Database
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<int> Save(ManufacturingUpdateViewModel data)
        {
            var checkDuplicate = await this._context.Plant.FirstOrDefaultAsync(Q => Q.PlantCode == data.PlantCode);
            if (checkDuplicate != null)
            {
                return 0;
            }

            var saveData = new Plant
            {
                PlantCode = data.PlantCode.ToUpper(),
                Name = data.Name.ToUpper(),
                Country = data.Country.ToUpper(),
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = environment.UserHumanName,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = environment.UserHumanName
            };

            this._context.Plant.Add(saveData);
            return await this._context.SaveChangesAsync();
            
        }

        /// <summary>
        /// Delete The Plant from Database
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<int> Delete(string code)
        {
            var deletedData = await this._context.Plant.FirstOrDefaultAsync(Q => Q.PlantCode == code);

            this._context.Plant.Remove(deletedData);
            return await this._context.SaveChangesAsync();
        }
    }
}
