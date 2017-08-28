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
    public class ClusterService
    {
        private readonly LogisticDbContext logisticDbContext;
        private readonly WebEnvironmentService WebEnvService;

        public ClusterService(LogisticDbContext db, WebEnvironmentService webEnvService)
        {
            this.logisticDbContext = db;
            this.WebEnvService = webEnvService;
        }

        public async Task<List<ClusterViewModel>> GetDataCluster()
        {
            var clusters = await this.logisticDbContext.AS400Cluster
                .Select(Q => new ClusterViewModel
                {
                    AS400ClusterCode = Q.AS400ClusterCode,
                    Name = Q.Name
                })
                .ToListAsync();
            return clusters;
        }

        public async Task<int> Create(ClusterViewModel model)
        {
            var username = WebEnvService.UserHumanName;
            var rowsAffected = 2;
            var existingCluster = await this.logisticDbContext.AS400Cluster.Where(x => x.AS400ClusterCode == model.AS400ClusterCode).FirstOrDefaultAsync();
            if (existingCluster != null)
            {
                return 0;
            }
            if (existingCluster == null)
            {
                var entity = new AS400Cluster();
                {
                    entity.AS400ClusterCode = model.AS400ClusterCode.ToUpper();
                    entity.Name = model.Name.ToUpper();
                    entity.CreatedAt = DateTimeOffset.UtcNow;
                    entity.UpdatedAt = DateTimeOffset.UtcNow;
                    entity.CreatedBy = username;
                    entity.UpdatedBy = username;
                };
                this.logisticDbContext.Add(entity);
            }
            rowsAffected = await this.logisticDbContext.SaveChangesAsync();
            return rowsAffected;
        }

        public async Task<int> Update(string id, ClusterViewModel model)
        {
            var username = WebEnvService.UserHumanName;
            var rowsAffected = 0;
            var existingCluster = await this.logisticDbContext.AS400Cluster.Where(x => x.AS400ClusterCode == id).FirstOrDefaultAsync();
            if (existingCluster != null)
            {
                existingCluster.Name = model.Name.ToUpper();
                existingCluster.UpdatedAt = DateTimeOffset.UtcNow;
                existingCluster.UpdatedBy = username;
            }
            rowsAffected = await this.logisticDbContext.SaveChangesAsync();
            return rowsAffected;
        }

        public async Task<int> Remove(string id)
        {
            var existingCluster = await this.logisticDbContext.AS400Cluster.Where(x => x.AS400ClusterCode == id).FirstOrDefaultAsync();
            var rowsAffected = 0;
            if (existingCluster != null)
            {
                this.logisticDbContext.Remove(existingCluster);
            }
            rowsAffected = await this.logisticDbContext.SaveChangesAsync();
            return rowsAffected;
        }
    }
}
