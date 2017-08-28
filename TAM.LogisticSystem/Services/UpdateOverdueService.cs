using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Services
{
    public class UpdateOverdueService
    {
        private readonly LogisticDbContext context;

        public UpdateOverdueService(LogisticDbContext context)
        {
            this.context = context;
        }

        public async Task UpdateOverdue(string frameNo)
        {
            //get the vechile row based on frameNo
            var vehicle = await this.context.Vehicle.Where(Q => Q.FrameNumber == frameNo).FirstOrDefaultAsync();

            //get routing vehicle base on vehicleId
            var vehicleRoutings = await this.context.VehicleRouting.Where(Q => Q.VehicleId == vehicle.VehicleId).OrderBy(Q=>Q.Ordering).ToListAsync();


            var MappingLeadMinuteDictionary = (await this.context.Database.GetDbConnection().QueryAsync<OrderingAndLeadMinute>(@"
                                SELECT  
										PLTFL.LeadMinutes as LeadMinutes,
                                        VR.Ordering as Ordering
                                FROM VehicleRouting VR JOIN ProcessMaster PM ON VR.ProcessMasterCode = PM.ProcessMasterCode 
					                                    JOIN ProcessLeadTimeByEnum PLTBE ON PLTBE.ProcessLeadTimeByEnumId = PM.ProcessLeadTimeByEnumId
														JOIN ProcessLeadTimeForLocation PLTFL on PLTFL.LocationCode = VR.LocationCode AND PLTFL.ProcessMasterCode =VR.ProcessMasterCode
                                WHERE VR.VehicleId = @id
                                ORDER BY VR.Ordering
                                ", new { id = vehicle.VehicleId })).ToDictionary(Q=>Q.Ordering,Q=> Q.LeadMinutes);
            
            var shiftKerjaDictionary = (await this.context.Database.GetDbConnection().QueryAsync<DictionaryModel>(@"
                                 SELECT 
                                    VR.VehicleRoutingId as RoutingId,
                                    a.Start AS TimeStart,
                                    a.Finish AS TimeFinish, 
                                    a.LocationCode AS LocationCode 
                                FROM 
                                    LocationWorkHour a JOIN VehicleRouting VR on VR.LocationCode = a.LocationCode
						        WHERE VR.VehicleId = @id
                                ", new { id = vehicle.VehicleId })).GroupBy(Q => Q.RoutingId).ToDictionary(Q => Q.Key, Q => Q.ToList());

            var breakTimeDictionary = (await this.context.Database.GetDbConnection().QueryAsync<DictionaryModel>(@"
                                SELECT 
                                    a.Start as TimeStart, 
                                    a.Finish as TimeFinish, 
                                    VehicleRoutingId as RoutingId, 
                                    a.LocationCode as LocationCode
                                FROM 
                                    LocationBreakHour a join VehicleRouting b on a.LocationCode = b.LocationCode
						        WHERE b.VehicleId = @id
                                ", new { id = vehicle.VehicleId })).GroupBy(Q => Q.RoutingId).ToDictionary(Q => Q.Key, Q => Q.ToList());

            //get last index of null and then sub with 1 so get the last time scan 
            var indexLastScan = vehicleRoutings.Where(Q => Q.ScanTime == null).OrderBy(Q => Q.Ordering).Select(Q => Q.Ordering).FirstOrDefault()-1;

            //check the routing one by one based on last scan time
            for (var i = indexLastScan; i <= vehicleRoutings.Count; i++)
            {
                //Retrive Holiday Base on Vehicle Routing Location
                var jumlahHoliday = (await this.context.Database.GetDbConnection().QueryAsync<int>($@" 
                                        SELECT count(*)*24 
                                        FROM holiday
                                        WHERE CONVERT(varchar(max),HolidayDate,103) = CONVERT(varchar(max),@date,103) AND LocationCode=@locationcode",
                                            new { locationcode = vehicleRoutings[i + 1].LocationCode, date = vehicleRoutings[i + 1].EstimatedTimeAdjusted })).FirstOrDefault();

                //when scan time is diff from ETA (means late or early)
                if ((vehicleRoutings[i].EstimatedTimeAdjusted.ToString("dd MMM HH:mm") != vehicleRoutings[i].ScanTime.Value.ToString("dd MMM HH:mm")) && vehicleRoutings[i].ScanTime != null)
                {
                    #region Adjusting ETA
                    //Adjust the ETA next
                    vehicleRoutings[i + 1].EstimatedTimeAdjusted = vehicleRoutings[i].ScanTime.Value.AddMinutes(MappingLeadMinuteDictionary[i + 1]);

                    //Re-Adjust ETA after next (domino-effect) 
                    for (var j = i + 2; j < vehicleRoutings.Count; j++)
                    {
                        vehicleRoutings[j].EstimatedTimeAdjusted = vehicleRoutings[j - 1].EstimatedTimeAdjusted.AddMinutes(MappingLeadMinuteDictionary[j]);
                    }

                    #endregion

                    //After Re-adjust, check if holiday
                    this.Check(vehicleRoutings, jumlahHoliday, breakTimeDictionary[i], shiftKerjaDictionary[i], i, MappingLeadMinuteDictionary);
                }
                else //ketika tepat waktu
                {
                    //intersect check
                    this.Check(vehicleRoutings, jumlahHoliday, breakTimeDictionary[i], shiftKerjaDictionary[i], i, MappingLeadMinuteDictionary);
                }
            }
        }

        public async Task DailyOverdue()
        {
            //update all the list vehicle
            var listVehicle = await this.context.Vehicle.ToListAsync();

            //get the date now (23.59)
            var dateNow = DateTime.Now.ToString("yyyy-MM-dd");


            foreach (var aaa in listVehicle)
            {
                var index = 0;
                //get all the routing by vehicle
                var vehicleRouting = await this.context.VehicleRouting.Where(Q => Q.VehicleId == aaa.VehicleId).OrderBy(Q => Q.Ordering).ToListAsync();
                for (var i = 0; i < vehicleRouting.Count; i++)
                {
                    if ((dateNow == vehicleRouting[i].EstimatedTimeAdjusted.ToString("yyyy-MM-dd")) && (vehicleRouting[i].ScanTime == null))
                    {
                        index = i;
                    }
                }
                //domino effect
                for (var j = index; j < vehicleRouting.Count; j++)
                {
                    vehicleRouting[j].EstimatedTimeAdjusted = vehicleRouting[j].EstimatedTimeAdjusted.AddDays(1);
                }
            }
        }

        private void Check(List<VehicleRouting> vehicleRouting, int jumlahHoliday, List<DictionaryModel> BreakTimeList, List<DictionaryModel> ShiftKerja, int i, Dictionary<int,int> MappingLeadMinuteDictionary)
        {
            if (jumlahHoliday > 0)
            {
                //if Match Holiday
                for (var j = i + 2; j < vehicleRouting.Count; j++)
                {
                    vehicleRouting[j].EstimatedTimeAdjusted = vehicleRouting[j].EstimatedTimeAdjusted.AddHours(jumlahHoliday);
                }
            }
            else //if no holiday, check shift kerja and breaktime
            {
                foreach (var breaktime in BreakTimeList)
                {
                    //intersect check
                    //           |----|            |----|                   |----|
                    //      |-------------|        |-----------|     |-----------|
                    if (vehicleRouting[i].ScanTime.Value.TimeOfDay <= breaktime.TimeStart.TimeOfDay && vehicleRouting[i + 1].EstimatedTimeAdjusted.TimeOfDay >= breaktime.TimeStart.TimeOfDay)
                    {
                        //do the domino again to adjust timebreak
                        for (var j = i + 1; j < vehicleRouting.Count; j++)
                        {
                            vehicleRouting[j].EstimatedTimeAdjusted = vehicleRouting[j].EstimatedTimeAdjusted.Add(breaktime.TimeFinish.TimeOfDay.Subtract(breaktime.TimeStart.TimeOfDay));
                        }
                    }
                    //  |------|
                    //      |-------------------------|
                    else if (vehicleRouting[i].ScanTime.Value.TimeOfDay > breaktime.TimeStart.TimeOfDay && vehicleRouting[i].ScanTime.Value.TimeOfDay < breaktime.TimeStart.TimeOfDay)
                    {
                        //do the domino again to adjust timebreak
                        for (var j = i + 1; j < vehicleRouting.Count; j++)
                        {
                            vehicleRouting[j].EstimatedTimeAdjusted = vehicleRouting[j].EstimatedTimeAdjusted.Add(breaktime.TimeStart.TimeOfDay.Subtract(vehicleRouting[i].ScanTime.Value.TimeOfDay));
                        }
                    }
                    //                             |------|
                    //      |-------------------------|
                    else if (vehicleRouting[i].ScanTime.Value.TimeOfDay >= breaktime.TimeStart.TimeOfDay && vehicleRouting[i].ScanTime.Value.TimeOfDay <= breaktime.TimeStart.TimeOfDay)
                    {
                        //do the domino again to adjust timebreak
                        for (var j = i + 1; j < vehicleRouting.Count; j++)
                        {
                            vehicleRouting[j].EstimatedTimeAdjusted = vehicleRouting[j].EstimatedTimeAdjusted.Add(breaktime.TimeFinish.TimeOfDay.Subtract(vehicleRouting[j].EstimatedTimeAdjusted.TimeOfDay));
                        }
                    }
                }
            }

            foreach (var item in ShiftKerja)
            {
                //check if intersect with 
                if (vehicleRouting[i + 1].EstimatedTimeAdjusted.TimeOfDay > item.TimeFinish.TimeOfDay)
                {
                    //itung selisihnya
                    var selisih = vehicleRouting[i].ScanTime.Value.AddMinutes(MappingLeadMinuteDictionary[i + 1]).TimeOfDay.Subtract(item.TimeFinish.TimeOfDay);
                    // misalnya 17/6 jam 17.00, mau diubah jadi 18/6 jam 9
                    vehicleRouting[i + 1].EstimatedTimeAdjusted = vehicleRouting[i + 1].EstimatedTimeAdjusted.AddDays(1);
                    //17/6 jam 17.00 
                    vehicleRouting[i + 1].EstimatedTimeAdjusted = new DateTime(vehicleRouting[i + 1].EstimatedTimeAdjusted.Year, vehicleRouting[i + 1].EstimatedTimeAdjusted.Month, vehicleRouting[i + 1].EstimatedTimeAdjusted.Day, item.TimeStart.AddHours(selisih.Hours).Hour, item.TimeStart.AddMinutes(selisih.Minutes).Minute, item.TimeStart.AddSeconds(selisih.Seconds).Second);
                }
                else if (vehicleRouting[i + 1].EstimatedTimeAdjusted.TimeOfDay < item.TimeStart.TimeOfDay)
                {
                    //itung selisihnya
                    var selisih = vehicleRouting[i].ScanTime.Value.AddMinutes(MappingLeadMinuteDictionary[i + 1]).TimeOfDay.Subtract(vehicleRouting[i + 1].EstimatedTimeAdjusted.TimeOfDay);
                    vehicleRouting[i + 1].EstimatedTimeAdjusted = item.TimeFinish.AddDays(-1);
                    vehicleRouting[i + 1].EstimatedTimeAdjusted = new DateTime(vehicleRouting[i + 1].EstimatedTimeAdjusted.Year, vehicleRouting[i + 1].EstimatedTimeAdjusted.Month, vehicleRouting[i + 1].EstimatedTimeAdjusted.Day, item.TimeStart.AddHours(selisih.Hours).Hour, item.TimeStart.AddMinutes(selisih.Minutes).Minute, item.TimeStart.AddSeconds(selisih.Seconds).Second);
                }
            }
        }


        private class DictionaryModel
        {
            public int RoutingId { get; set; }
            public string LocationCode { get; set; }
            public DateTime TimeStart { get; set; }
            public DateTime TimeFinish { get; set; }
        }

        private class OrderingAndLeadMinute
        {
            public int LeadMinutes { get; set; }
            public int Ordering { get; set; }
        }

        
    }
}
