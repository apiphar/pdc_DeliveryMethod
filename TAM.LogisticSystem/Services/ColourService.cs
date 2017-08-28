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
    public class ColourService
    {
        private readonly LogisticDbContext DB;
        private readonly WebEnvironmentService environment;

        public ColourService(LogisticDbContext db, WebEnvironmentService env)
        {
            this.DB = db;
            this.environment = env;

        }


        internal async Task<int> AddInterior(string colorcode, string indonesianame, string englishname)
        {
            var checkDuplicate = await this.DB.InteriorColor.FirstOrDefaultAsync(Q => Q.InteriorColorCode == colorcode);
            if (checkDuplicate != null)
            {
                return 0;
            }
            var checkDuplicate2 = await this.DB.ExteriorColor.FirstOrDefaultAsync(Q => Q.ExteriorColorCode == colorcode);
            if (checkDuplicate2 != null)
            {
                return 0;
            }
            var insert = new InteriorColor
            {
                InteriorColorCode = colorcode.ToUpper(),
                IndonesianName = indonesianame,
                EnglishName = englishname,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = environment.UserHumanName,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = environment.UserHumanName
            };

            DB.Add(insert);

            return await DB.SaveChangesAsync();
        }

        internal async Task<int> AddExterior(string colorcode, string indonesianame, string englishname)
        {
            var checkDuplicate = await this.DB.InteriorColor.FirstOrDefaultAsync(Q => Q.InteriorColorCode == colorcode);
            if (checkDuplicate != null)
            {
                return 0;
            }
            var checkDuplicate2 = await this.DB.ExteriorColor.FirstOrDefaultAsync(Q => Q.ExteriorColorCode == colorcode);
            if (checkDuplicate2 != null)
            {
                return 0;
            }
            var insert = new ExteriorColor
            {
                ExteriorColorCode = colorcode,
                IndonesianName = indonesianame,
                EnglishName = englishname,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = environment.UserHumanName,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = environment.UserHumanName
            };

            DB.Add(insert);

            return await DB.SaveChangesAsync();
        }

        internal async Task<InteriorColor> GetInterior(string id)
        {
            return await DB.InteriorColor.Where(x => x.InteriorColorCode.Equals(id)).FirstOrDefaultAsync();
        }

        internal async Task<ExteriorColor> GetExterior(string id)
        {
            return await DB.ExteriorColor.Where(x => x.ExteriorColorCode.Equals(id)).FirstOrDefaultAsync();
        }

        internal async Task<int> RemoveInterior(InteriorColor entity)
        {
            DB.Remove(entity);
            return await DB.SaveChangesAsync();
        }

        internal async Task<int> RemoveExterior(ExteriorColor entity)
        {
            DB.Remove(entity);
            return await DB.SaveChangesAsync();
        }

        internal async Task UpdateInterior(InteriorColor entity, string indonesianame, string englishname)
        {
            entity.IndonesianName = indonesianame;
            entity.EnglishName = englishname;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            entity.UpdatedBy = environment.UserHumanName;
            await DB.SaveChangesAsync();
        }

        internal async Task UpdateExterior(ExteriorColor entity, string indonesianame, string englishname)
        {
            entity.IndonesianName = indonesianame;
            entity.EnglishName = englishname;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            entity.UpdatedBy = environment.UserHumanName;
            await DB.SaveChangesAsync();
        }

        public async Task<List<ColourCreateOrUpdateRequest>> GetAll()
        {
            var dbConnection = DB.Database.GetDbConnection();
            var query_raw = @"WITH table1 AS (
	                                SELECT 
		                                ExteriorColorCode AS ColorCode,
		                                IndonesianName,
		                                EnglishName,
		                                'Exterior' AS ColorType
	                                FROM ExteriorColor
                                ),
                                table2 AS (
	                                SELECT 
		                                InteriorColorCode AS ColorCode,
		                                IndonesianName,
		                                EnglishName,
		                                'Interior' AS ColorType
	                                FROM InteriorColor
                                )
                                SELECT * FROM table1
                                UNION
                                SELECT * FROM table2
                                order by IndonesianName";
            var queryParams = new { };
            var query = (await dbConnection.QueryAsync<ColourCreateOrUpdateRequest>(query_raw, queryParams)).ToList();
            return query;
        }
    }
}
