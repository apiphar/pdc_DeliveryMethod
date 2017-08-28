using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TAM.LogisticSystem.Services
{
    public class ExchangeRateService
    {
        private readonly LogisticDbContext logisticDbContext;

        public ExchangeRateService(LogisticDbContext tangodb)
        {
            this.logisticDbContext = tangodb;
        }

        public List<ExchangeRateViewModel> GetExchangeRates()
        {
            List<ExchangeRateViewModel> lists = new List<ExchangeRateViewModel>();
            var dbConnection = logisticDbContext.Database.GetDbConnection();
   
                string query = @"
                        select ExchangeRateId, Torupiah, CurrencySymbol, validfrom, validuntil
                        from ExchangeRate       
                ";

                var data = dbConnection.Query<ExchangeRateViewModel>(query);
                lists = data.ToList();

                return lists;
            }
        
        public List<SelectListItem> GetCurrencys()
        {
            var countries = logisticDbContext.Currency.OrderBy(x => x.CurrencySymbol).Select(x => new SelectListItem()
            {
                Text = x.CurrencySymbol,
                Value = x.CurrencySymbol.ToString(),
                Selected = false
            }).ToList();

            countries.Insert(0, new SelectListItem()
            {
                Value = "0",
                Text = "(Please Choose One)",
                Selected = true
            });

            return countries;
        }

        public async Task<int> Add(string CurrencySymbol, decimal ToRupiah, DateTime ValidFrom, DateTime ValidUntil, DateTime CreatedAt, string CreatedBy, DateTime UpdatedAt, string UpdatedBy )
        {
            logisticDbContext.Add(new ExchangeRate
            {
                CurrencySymbol = CurrencySymbol,
                ToRupiah = ToRupiah,
                ValidFrom = ValidFrom,
                ValidUntil = ValidUntil,
                CreatedAt = CreatedAt,
                CreatedBy = CreatedBy = "", 
                UpdatedAt = UpdatedAt,
                UpdatedBy = UpdatedBy = ""
            });
            return await logisticDbContext.SaveChangesAsync();
        }

        public async Task<ExchangeRate> Get(int id)
        {
            return await logisticDbContext.ExchangeRate.FindAsync(id);
        }

        public async Task<int> Update(int id, ExchangeRateViewModel model)
        {
            var entity = await logisticDbContext.ExchangeRate.Where(x => x.ExchangeRateId == id).FirstOrDefaultAsync();
            int rowsAffected = 0;

            if (entity != null)
            {
                entity.CurrencySymbol = model.CurrencySymbol;
                entity.ValidFrom = model.ValidFrom;
                entity.ValidUntil = model.ValidUntil;
                entity.ToRupiah = model.ToRupiah;

                rowsAffected = await logisticDbContext.SaveChangesAsync();
            }

            return rowsAffected;
        }

        public async Task<int> Remove(int id)
        {
            var entity = await logisticDbContext.ExchangeRate.Where(x => x.ExchangeRateId == id).FirstOrDefaultAsync();
            if (entity != null)
            {
                logisticDbContext.Remove(entity);
            }

            int rowsAffected = await logisticDbContext.SaveChangesAsync();
            return rowsAffected;
        }

    }
}
