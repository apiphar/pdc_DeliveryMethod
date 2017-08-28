using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace TAM.LogisticSystem.Services
{
    public class DeliveryUnitAdvanceService
    {
        private readonly LogisticDbContext LogisticDbContext;

        public DeliveryUnitAdvanceService(LogisticDbContext logisticDbContext)
        {
            this.LogisticDbContext = logisticDbContext;
        }

        /// <summary>
        /// get semua data dari db
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeliveryUnitAdvanceViewModel>> GetUnitAdvanceData()
        {
            _ = nameof(Vehicle.FrameNumber);
            _ = nameof(Vehicle.Suffix);
            _ = nameof(Vehicle.Katashiki);
            _ = nameof(CarType.Name);
            _ = nameof(Branch.Name);
            _ = nameof(Vehicle.RequestedDeliveryTime);
            _ = nameof(CarModel.Name);
            _ = nameof(ExteriorColor.IndonesianName);
            _ = nameof(Vehicle.HasCustomer);
            var deliveryUnitAdvanceModel = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<DeliveryUnitAdvanceViewModel>(@"
                SELECT 
                    V.FrameNumber AS FrameNumber,
                    V.Suffix AS Suffix,
                    V.Katashiki AS Katashiki,
                    CT.[Name] AS Tipe,
                    Br.[Name] AS Branch,
                    V.RequestedDeliveryTime AS RequestedPDD,
                    CM.[Name] AS Model,
                    EC.IndonesianName AS Warna,
                    V.HasCustomer AS CustomerAssign
                FROM Vehicle V 
                    JOIN Branch Br ON V.BranchCode = Br.BranchCode 
                    JOIN CarType CT ON CT.Katashiki = V.Katashiki AND CT.Suffix = V.Suffix
                    JOIN CarSeries CS ON CS.CarSeriesCode = CT.CarSeriesCode
                    JOIN CarModel CM ON CM.CarModelCode = CS.CarModelCode
                    JOIN ExteriorColor EC ON EC.ExteriorColorCode = V.ExteriorColorCode")).ToList();
            return deliveryUnitAdvanceModel;
        }

        /// <summary>
        /// mengubah IsAdvanceUnit didalam db menjadi true
        /// </summary>
        /// <param name="deliveryUnitAdvanceViewModel"></param>
        /// <returns></returns>
        public async Task SubmitUnitAdvanceData(DeliveryUnitAdvanceViewModel deliveryUnitAdvanceViewModel)
        {
            var updateUnitAdvance = await LogisticDbContext.Vehicle.FirstOrDefaultAsync(Q => Q.FrameNumber == deliveryUnitAdvanceViewModel.FrameNumber);
            updateUnitAdvance.IsAdvanceUnit = true;
            LogisticDbContext.Vehicle.Update(updateUnitAdvance);
            await LogisticDbContext.SaveChangesAsync();
        }
    }
}
