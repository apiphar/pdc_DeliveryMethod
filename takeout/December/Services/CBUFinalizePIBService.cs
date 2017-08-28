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
    public class CBUFinalizePIBService
    {
        private readonly LogisticDbContext LogisticDbContext;

        public CBUFinalizePIBService(LogisticDbContext logisticDbContext)
        {
            this.LogisticDbContext = logisticDbContext;
        }
        
        //get all data for import
        public async Task<List<CBUImportFinalizePIBViewModel>> GetAllData()
        {
            var cbuImportFinalizePIBViewModel = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<CBUImportFinalizePIBViewModel>(@"
                SELECT 
	                pib.NomorAju AS noAju,
	                pib.TanggalAju AS ajuDate,
	                COUNT(DISTINCT side.frameNumber) AS totalQty,
	                pib.CurrencySymbol,
	                pib.HarmonizeSchema AS [Schema],
	                pib.CurrencyRate
                FROM ShipmentInvoice si
                    JOIN ShipmentInvoiceDetail side ON si.InvoiceNumber = side.InvoiceNumber
	                JOIN CarType ct ON ct.Katashiki = side.Katashiki AND ct.Suffix = side.Suffix
	                JOIN Harmonize h ON h.HSCode = ct.HSCode
	                JOIN HarmonizeTariff ht ON ht.HSCode = h.HSCode
                    JOIN PersetujuanImportBarang pib ON si.NomorAju = pib.NomorAju
				WHERE pib.FinalizedAt IS NULL				
                GROUP BY pib.NomorAju, pib.TanggalAju, pib.CurrencySymbol, pib.HarmonizeSchema,	pib.CurrencyRate
            ")).ToList();
            return cbuImportFinalizePIBViewModel;     
        }

        //Get data that will be finalize
        public async Task<List<CBUFinalizePIBViewModel>> GetAllPreFinalizeData(string noAju)
        {
            var cbuFinalizePIBViewModel = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<CBUFinalizePIBViewModel>(@"
                SELECT DISTINCT
	                side.InvoiceNumber, 
	                side.FrameNumber, 
	                side.EngineNumber,  
	                side.Katashiki,
	                side.SuffixOriginal AS GlobalSuffix,
	                side.Suffix AS LocalSuffix, 
	                ct.HSCode AS HSCode,
	                side.EDNumber,
	                side.Price AS Price, 
	                side.PriceRupiah AS PriceRupiah, 
	                side.BeaMasuk AS BM, 
	                side.ImportValue AS ImportValue, 
	                side.PPH AS PPH, 
	                side.PPN AS PPN,
	                side.PPNBM AS PPNBM
                FROM ShipmentInvoice si
                    JOIN ShipmentInvoiceDetail side ON si.InvoiceNumber = side.InvoiceNumber
	                JOIN CarType ct ON ct.Katashiki = side.Katashiki AND ct.Suffix = side.Suffix
	                JOIN Harmonize h ON h.HSCode = ct.HSCode		
	                JOIN HarmonizeTariff ht ON ht.HSCode = h.HSCode
	                JOIN PersetujuanImportBarang pib ON si.NomorAju = pib.NomorAju
                WHERE si.NomorAju = @noAju
            ", new { noAju = noAju })).ToList();
            return cbuFinalizePIBViewModel;
        }

        //Get Currency and Rupiah Value
        public async Task<List<CurrencyViewModel>> GetCurrency()
        {
            var currency = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<CurrencyViewModel>(@"
                SELECT 
                    CurrencySymbol AS Currency, 
                    ToRupiah AS NDPBM 
                FROM ExchangeRate
            ")).ToList();
            return currency;
        }

        //Get Harmonize Percentage
        public async Task<List<PercentageViewModel>> GetPercentage()
        {
            var percentage = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<PercentageViewModel>(@"
                SELECT 
	                ht.BeaMasukPercentage,
	                ht.HSCode,
	                ht.[Schema],
	                h.PphPercentage,
	                h.PpnPercentage,
	                h.PpnBmPercentage
                FROM Harmonize h 
	                JOIN HarmonizeTariff ht ON ht.HSCode = h.HSCode
            ")).ToList();
            return percentage;
        }

        //Finalize PIB ke Database
        public async Task FinalizePIB(FinalizePIBViewModel finalizePIBViewModel)
        {
            await LogisticDbContext.Database.CreateExecutionStrategy().Execute(async () =>
            {
                using (var transaction = await LogisticDbContext.Database.BeginTransactionAsync())
                {
                    var updatePIBModel = await LogisticDbContext.PersetujuanImportBarang
                        .FirstOrDefaultAsync(Q => Q.NomorAju == finalizePIBViewModel.FinalizeInfo.NomorAju);
                    updatePIBModel.HarmonizeSchemaFinal = finalizePIBViewModel.FinalizeInfo.SchemaFinal;
                    updatePIBModel.TanggalAjuApproved = finalizePIBViewModel.FinalizeInfo.TanggalAjuApproved;
                    updatePIBModel.CurrencyRateFinal = finalizePIBViewModel.FinalizeInfo.CurrencyRateFinal;
                    updatePIBModel.FinalizedAt = DateTime.UtcNow;
                    //symbol bisa di ganti?

                    this.LogisticDbContext.PersetujuanImportBarang.Update(updatePIBModel);
                    await this.LogisticDbContext.SaveChangesAsync();

                    foreach (var item in finalizePIBViewModel.FinalizeTable)
                    {
                        var updateShipmentInvoiceDetail = await LogisticDbContext.ShipmentInvoiceDetail
                            .FirstOrDefaultAsync(Q => Q.InvoiceNumber == item.InvoiceNumber && Q.FrameNumber == item.FrameNumber);
                        updateShipmentInvoiceDetail.PriceRupiahFinal = item.PriceRupiah;
                        updateShipmentInvoiceDetail.BeaMasukFinal = item.BM;
                        updateShipmentInvoiceDetail.PPHFinal = item.PPH;
                        updateShipmentInvoiceDetail.PPNFinal = item.PPN;
                        updateShipmentInvoiceDetail.PPNBMFinal = item.PPNBM;
                        updateShipmentInvoiceDetail.ImportValueFinal = item.ImportValue;


                        this.LogisticDbContext.ShipmentInvoiceDetail.Update(updateShipmentInvoiceDetail);
                        await this.LogisticDbContext.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
            });
            
        }
    }
}
