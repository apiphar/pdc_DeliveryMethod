using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Services
{
    public class RoleMenuService
    {
        private readonly LogisticDbContext DB;
        private readonly WebEnvironmentService Env;

        public RoleMenuService(LogisticDbContext db, WebEnvironmentService env)
        {
            this.DB = db;
            this.Env = env;
        }

        public async Task<List<AppRoleMenuMapping>> GetAll()
        {
            return await DB.AppRoleMenuMapping.ToListAsync();
        }

        public async Task<AppRoleMenuMapping> Get(string role, string menu)
        {
            return await DB.AppRoleMenuMapping.FirstOrDefaultAsync(Q => Q.AppMenuName == menu && Q.AppRoleName == role);
        }

        public async Task Delete(AppRoleMenuMapping e)
        {
            DB.Remove(e);
            await DB.SaveChangesAsync();
        }

        // TIE: START
        //public async Task CreateNew(string role, string menu)
        //{
        //    DB.AppRoleMenuMapping.Add(new AppRoleMenuMapping
        //    {
        //        AppMenuName = menu,
        //        AppRoleName = role,
        //        CreatedAt = DateTime.UtcNow,
        //        UpdatedAt = DateTime.UtcNow,
        //        CreatedBy = Env.Username,
        //        UpdatedBy = Env.Username
        //    });

        //    await DB.SaveChangesAsync();
        //}
        // TIE: END

        public async Task<List<AppRole>> GetAllRoles()
        {
            return await DB.AppRole.ToListAsync();
        }

        public async Task<List<AppMenu>> GetAllMenu()
        {
            return await DB.AppMenu.ToListAsync();
        }
    }
}
