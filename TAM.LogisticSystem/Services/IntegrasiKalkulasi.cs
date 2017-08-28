using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Services
{
    public class IntegrasiKalkulasi
    {
        private readonly LogisticDbContext _context;
        private readonly SupplyRoutingService _supplyRoutingService;
        private readonly WebEnvironmentService _envService;

        public IntegrasiKalkulasi(LogisticDbContext c, SupplyRoutingService s, WebEnvironmentService e)
        {
            this._context = c;
            this._supplyRoutingService = s;
            this._envService = e;
        }

        public async Task CalculateRoutingVehicle(Vehicle vehicle)
        {
            //supply the Routing
            await _supplyRoutingService.SupplyRouting(vehicle);
            
            //filter the vehicle routing based on the vehicle id and order by ordering to know the flow process
            var VehicleRouting = await this._context.VehicleRouting.Where(Q => Q.VehicleId == vehicle.VehicleId)
                .OrderBy(Q => Q.Ordering).ToListAsync();

            //Create dict to store all the RoutingLeadID and Buffer Time with key from LeadId.. use this because MasterCode somehow still duplicate
            var ProcessLeadTimeByEnumId = this._context.Query<LeadTimeBy>(@"
                                        SELECT 
	                                        VR.VehicleRoutingId AS RoutingId, 
	                                        PLTBE.ProcessLeadTimeByEnumId AS Leadid, 
	                                        PM.BufferMinutes AS Buffer              
                                        FROM 
	                                        ProcessMaster PM JOIN ProcessLeadTimeByEnum PLTBE ON PM.ProcessLeadTimeByEnumId = PLTBE.ProcessLeadTimeByEnumId
					                                         JOIN VehicleRouting VR on VR.ProcessMasterCode = PM.ProcessMasterCode
                                        WHERE VR.VehicleId = @id
                                    ", new { id = vehicle.VehicleId }).ToDictionary(Q => Q.RoutingId, Q => new LeadTimeBy { Buffer = Q.Buffer, Leadid = Q.Leadid });

            for (var i = 0; i < VehicleRouting.Count; i++)
            {
                switch(ProcessLeadTimeByEnumId[VehicleRouting[i].VehicleRoutingId].Leadid)
                {
                    case 1: 
                        await this.CalculatePDILeadTime(VehicleRouting[i], ProcessLeadTimeByEnumId[VehicleRouting[i].VehicleRoutingId].Buffer);
                        break;
                    case 2:
                        await this.CalculatePIOLeadTime(VehicleRouting[i], ProcessLeadTimeByEnumId[VehicleRouting[i].VehicleRoutingId].Buffer);
                        break;
                    case 3: 
                        await this.CalculateSPULeadTime(VehicleRouting[i], ProcessLeadTimeByEnumId[VehicleRouting[i].VehicleRoutingId].Buffer);
                        break;
                    case 4:
                        await this.CalculateLocationLeadTime(VehicleRouting[i], ProcessLeadTimeByEnumId[VehicleRouting[i].VehicleRoutingId].Buffer);
                        break;
                    case 5:
                        await this.CalculateKapalLeadTime(VehicleRouting[i - 1], VehicleRouting[i]);
                        break;
                    case 6: 
                        await this.CalculateDeliveryLeadTime(VehicleRouting[i - 1], VehicleRouting[i], ProcessLeadTimeByEnumId[VehicleRouting[i].VehicleRoutingId].Buffer);
                        break;
                    case 7:
                       await this.CalculateDwellingLeadTime(VehicleRouting[i - 1], VehicleRouting[i], ProcessLeadTimeByEnumId[VehicleRouting[i].VehicleRoutingId].Buffer);
                        break;
                    default:
                        break;
                }   
            }
            await this._context.SaveChangesAsync();
            
            await this.CalculatePDC(vehicle, VehicleRouting.Last());
        }

        private async Task CalculatePDC(Vehicle vehicle, VehicleRouting LastVehicleRouting)
        {
            //var PDCConfig = await this._context.PDCConfig.FirstOrDefaultAsync(Q => Q.LocationCode == LastVehicleRouting.LocationCode);
            vehicle.EstimatedPDCIn = LastVehicleRouting.EstimatedTimeAdjusted;
            //PDC IN + Hasil DLO [this.CalculateDeliveryLeadTime(LastVehicleRouting, LastVehicleRouting)]
            //vehicle.EstimatedPDCOut = vehicle.EstimatedPDCIn.Value.AddDays(PDCConfig.LeadDayPreDeliveryService);
            vehicle.EstimatedArrivalBranch = vehicle.EstimatedPDCOut.Value.AddMinutes(await this.CalculateDeliveryLeadTimeForArrivalBranch(LastVehicleRouting, LastVehicleRouting));
            //vehicle.EstimatedDeliveryTime = vehicle.EstimatedArrivalBranch.Value.AddDays(PDCConfig.LeadDayPreDeliveryService);

            this._context.Vehicle.Update(vehicle);
            await this._context.SaveChangesAsync();
        }

        #region All The Calculate Function
        private async Task CalculatePDILeadTime(VehicleRouting vehicleRouting, int bufferTime)
        {
            var leadMinutePDI = (await this._context.QueryAsync<int>(@"
            SELECT 
                ((PLT.TaktSeconds/60)*PLT.Post) + PM.BufferMinutes AS PDILeadMinute
            FROM Vehicle V JOIN VehicleRouting VR ON V.VehicleId = VR.VehicleId
			               JOIN PDILeadTime PLT ON PLT.LocationCode = VR.LocationCode 
			               JOIN ProcessMaster PM ON PM.ProcessMasterCode = VR.ProcessMasterCode
			               JOIN ProcessLeadTimeByEnum PLTBE ON PLTBE.ProcessLeadTimeByEnumId = PM.ProcessLeadTimeByEnumId 
            WHERE PLT.Katashiki = V.Katashiki AND PLT.Suffix = V.Suffix
	              AND V.VehicleId = @VehicleId", new { VehicleId = vehicleRouting.VehicleId }
)).FirstOrDefault();
            
            //Re-Adjust the new ETA
            vehicleRouting.EstimatedTimeInitial = vehicleRouting.EstimatedTimeInitial.AddMinutes(leadMinutePDI + bufferTime);
            vehicleRouting.EstimatedTimeAdjusted = vehicleRouting.EstimatedTimeAdjusted.AddMinutes(leadMinutePDI + bufferTime);

            //Update
            vehicleRouting.UpdatedAt = DateTimeOffset.UtcNow;
            vehicleRouting.UpdatedBy = "SYSTEM";
            this._context.VehicleRouting.Update(vehicleRouting);
          
        }
        private async Task CalculatePIOLeadTime(VehicleRouting vehicleRouting, int bufferTime)
        {
            var leadMinutePIO = (await this._context.QueryAsync<int>(@"
           SELECT 
                ((PL.TaktSeconds/60)*PL.Post) + PM.BufferMinutes  AS PIOLeadMinute
            FROM VehicleRouting VR join Vehicle V ON V.VehicleId = VR.VehicleId 
			               JOIN PIOLineDetail PLD ON PLD.Katashiki = V.Katashiki and PLD.Suffix = V.Suffix 
			               JOIN PIOLine PL ON PL.PIOLineId = PLD.PIOLineId 
			               JOIN ProcessMaster PM ON PM.ProcessMasterCode = VR.ProcessMasterCode
			               JOIN ProcessLeadTimeByEnum PLTBE ON PLTBE.ProcessLeadTimeByEnumId = PM.ProcessLeadTimeByEnumId
            WHERE PL.LineNumber = VR.LineNumber AND PL.LocationCode = VR.LocationCode
	              AND V.VehicleId = @VehicleId", new { VehicleId = vehicleRouting.VehicleId }
)).FirstOrDefault();

            //Re-Adjust the new ETA
            vehicleRouting.EstimatedTimeInitial = vehicleRouting.EstimatedTimeInitial.AddMinutes(leadMinutePIO + bufferTime);
            vehicleRouting.EstimatedTimeAdjusted = vehicleRouting.EstimatedTimeAdjusted.AddMinutes(leadMinutePIO + bufferTime);

            //Update
            vehicleRouting.UpdatedAt = DateTimeOffset.UtcNow;
            vehicleRouting.UpdatedBy = "SYSTEM";
            this._context.VehicleRouting.Update(vehicleRouting);

        }
        private async Task CalculateSPULeadTime(VehicleRouting vehicleRouting, int bufferTime)
        {
            var leadMinuteSPU = (await this._context.QueryAsync<int>(@"
            SELECT 
		            ((SL.TaktSeconds/60)*SL.Post) + PM.BufferMinutes  AS SPULeadMinute
            FROM Vehicle V JOIN VehicleRouting VR ON V.VehicleId = VR.VehicleId
			   JOIN SPULineDetail SLD ON SLD.Katashiki = V.Katashiki and SLD.Suffix = V.Suffix and SLD.BranchCode = V.BranchCode
			   JOIN SPULine SL ON SL.SPULineId = SLD.SPULineId
			   JOIN ProcessMaster PM ON PM.ProcessMasterCode = VR.ProcessMasterCode
			   JOIN ProcessLeadTimeByEnum PLTBE ON PLTBE.ProcessLeadTimeByEnumId = PM.ProcessLeadTimeByEnumId
            WHERE SL.LineNumber = VR.LineNumber AND SL.LocationCode = VR.LocationCode
	              AND V.VehicleId = @VehicleId", new { VehicleId = vehicleRouting.VehicleId }
)).FirstOrDefault();

            //Re-Adjust the new ETA
            vehicleRouting.EstimatedTimeInitial = vehicleRouting.EstimatedTimeInitial.AddMinutes(leadMinuteSPU + bufferTime);
            vehicleRouting.EstimatedTimeAdjusted = vehicleRouting.EstimatedTimeAdjusted.AddMinutes(leadMinuteSPU + bufferTime);

            //Update
            vehicleRouting.UpdatedAt = DateTimeOffset.UtcNow;
            vehicleRouting.UpdatedBy = "SYSTEM";
            this._context.VehicleRouting.Update(vehicleRouting);
        }
        private async Task CalculateLocationLeadTime(VehicleRouting vehicleRouting, int bufferTime)
        {
            var leadMinutes = await this._context.ProcessLeadTimeForLocation.
                    Where(Q => Q.LocationCode == vehicleRouting.LocationCode && Q.ProcessMasterCode == vehicleRouting.ProcessMasterCode)
                    .Select(Q=>Q.LeadMinutes).FirstOrDefaultAsync();

            //Re-Adjust the new ETA
            vehicleRouting.EstimatedTimeInitial = vehicleRouting.EstimatedTimeInitial.AddMinutes(leadMinutes + bufferTime);
            vehicleRouting.EstimatedTimeAdjusted = vehicleRouting.EstimatedTimeAdjusted.AddMinutes(leadMinutes + bufferTime);

            //Update
            vehicleRouting.UpdatedAt = DateTimeOffset.UtcNow;
            vehicleRouting.UpdatedBy = "SYSTEM";
            this._context.VehicleRouting.Update(vehicleRouting);
        }
        private async Task CalculateKapalLeadTime(VehicleRouting firstVehicleRouting, VehicleRouting secondVehicleRouting)
        {
            //Departure
            #region Re-Adjust first.ETA
            firstVehicleRouting.EstimatedTimeAdjusted = await this._context.Voyage.Where(Q => Q.DepartureDate > firstVehicleRouting.EstimatedTimeAdjusted && Q.DepartureLocationCode == firstVehicleRouting.LocationCode)
                .OrderBy(Q => Q.DepartureDate).Select(Q => Q.DepartureDate).FirstOrDefaultAsync();

            this._context.VehicleRouting.Update(firstVehicleRouting);
            #endregion
            
            //Arrival
        }
        private async Task CalculateDeliveryLeadTime(VehicleRouting firstVehicleRouting, VehicleRouting secondVehicleRouting, int bufferTime)
        {
            var leadMinutes = await _context.DeliveryLeg.Where(Q => Q.LocationFrom == firstVehicleRouting.LocationCode
            && Q.LocationTo == secondVehicleRouting.LocationCode).Select(Q => Q.BufferMinutes).FirstOrDefaultAsync();

            //Re-Adjust the new ETA
            secondVehicleRouting.EstimatedTimeInitial = secondVehicleRouting.EstimatedTimeInitial.AddMinutes(leadMinutes + bufferTime);
            secondVehicleRouting.EstimatedTimeAdjusted = secondVehicleRouting.EstimatedTimeAdjusted.AddMinutes(leadMinutes + bufferTime);

            //Update
            secondVehicleRouting.UpdatedAt = DateTimeOffset.UtcNow;
            secondVehicleRouting.UpdatedBy = "SYSTEM";
            this._context.VehicleRouting.Update(secondVehicleRouting);
        }
        private async Task CalculateDwellingLeadTime(VehicleRouting firstVehicleRouting, VehicleRouting secondVehicleRouting, int bufferTime)
        {
            var leadMinutes = await _context.Dwelling.Where(Q => Q.LocationFrom == firstVehicleRouting.LocationCode &&
            Q.LocationTo == secondVehicleRouting.LocationCode).Select(Q => Q.LeadMinutes).FirstOrDefaultAsync();

            //Re-Adjust the new ETA
            secondVehicleRouting.EstimatedTimeInitial = secondVehicleRouting.EstimatedTimeInitial.AddMinutes(leadMinutes + bufferTime);
            secondVehicleRouting.EstimatedTimeAdjusted = secondVehicleRouting.EstimatedTimeAdjusted.AddMinutes(leadMinutes + bufferTime);

            //Update
            secondVehicleRouting.UpdatedAt = DateTimeOffset.UtcNow;
            secondVehicleRouting.UpdatedBy = "SYSTEM";
            this._context.VehicleRouting.Update(secondVehicleRouting);
        }

        //This func For The EstimatedArrivalBranch, return int. different behaviour from CalculateDeliveryLeadTime becasue this func use last routing
        private async Task<int> CalculateDeliveryLeadTimeForArrivalBranch(VehicleRouting firstVehicleRouting, VehicleRouting secondVehicleRouting)
        {
            return await _context.DeliveryLeg.Where(Q => Q.LocationFrom == firstVehicleRouting.LocationCode
            && Q.LocationTo == secondVehicleRouting.LocationCode).Select(Q => Q.BufferMinutes).FirstOrDefaultAsync();
        }

        #endregion

        public async Task<Vehicle> GetSelectedVehicle(string frameNo)
        {
            return await this._context.Vehicle.Where(Q => Q.FrameNumber == frameNo).FirstOrDefaultAsync();
        }

        private class LeadTimeBy
        {
            public long RoutingId { get; set; }
            public int Buffer { get; set; }
            public int Leadid { get; set; }
        }
    }
}
