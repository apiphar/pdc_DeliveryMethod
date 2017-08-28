using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.TANGO.Entities;
using TAM.TANGO.Models;
using Dapper;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace TAM.TANGO.Services
{
    public class PDIService
    {
        private TangoDbContext tangoDbContext;

        public PDIService(TangoDbContext tangoDbContext)
        {
            this.tangoDbContext = tangoDbContext;
        }

        public async Task<List<InspectionMasterChecklist>> GetInspectionMasterChecklist(InspectionParameter inspectionMasterParameters)
        {
            List<InspectionMasterChecklist> inspectionMasterCheclist;

            using (var dbConnection = tangoDbContext.Database.GetDbConnection())
            {
                string query = @"
                    select a.InspectionMasterId
                         , b.InspectionMasterDetailId
	                     , a.Katashiki
	                     , a.Suffix
	                     , b.Name as Name
	                     , b.Description as Description
	                     , b.InspectionAreaId
	                     , c.Name as Area
                         , b.InspectionMasterDetailId
                      from InspectionMaster a
                     inner join InspectionMasterDetail b
                        on b.InspectionMasterId = a.InspectionMasterId
                     inner join InspectionArea c
                        on c.InspectionAreaId = b.InspectionAreaId
                     where b.InspectionAreaId = @AreaId
                       and substring(b.name, 0, 3) = @Prefix; 
                ";
                var queryParameters = new
                {
                    AreaId = inspectionMasterParameters.AreaId,
                    Prefix = inspectionMasterParameters.Prefix
                };
                var data = await dbConnection.QueryAsync<InspectionMasterChecklist>(query, queryParameters);
                inspectionMasterCheclist = data.ToList();
            }

            return inspectionMasterCheclist;
        }

        public async Task<VehicleData> GetVehicleData(string frameNumber)
        {
            VehicleData vehicleData = null;

            var dbConnection = tangoDbContext.Database.GetDbConnection();
            string query = @"
                select d.CarModelId as VehicleModel
                        , d.CarSeriesId as VehicleSeries
	                    , c.Name as VehicleType
	                    , e.Name as VehicleColor
	                    , h.Name as VehicleOutlet
                    from Vehicle a
                    inner join CarColor b
                    on b.CarColorId = a.CarColorId
                    left join CarType c
                    on c.Katashiki = b.Katashiki
                    and c.Suffix = b.Suffix
                    left join CarSeries d
                    on d.CarSeriesId = c.CarSeriesId
                    left join ColorMaster e
                    on e.ColorMasterId = b.ColorMasterId
                    inner join RoutingDictionary f
                    on f.Katashiki = c.Katashiki
                    and f.Suffix = c.Suffix
                    left join Dealer g
                    on g.DealerId = f.DealerId
                    left join Outlet h
                    on h.OutletId = f.OutletId
                    where a.FrameNumber = @FrameNumber
            ";

            var queryParams = new
            {
                FrameNumber = frameNumber
            };

            vehicleData = await dbConnection.QueryFirstOrDefaultAsync<VehicleData>(query, queryParams);

            return vehicleData;
        }

        public async Task<PDIParkingData> SubmitInspectionData(InspectionData inspectionData)
        {
            if (inspectionData != null)
            {
                inspectionData = ExtractParameters(inspectionData);
            }

            await UpdateInspectionData(inspectionData);

            // Fake parking data, because there is no service provided for suggesting parking area
            PDIParkingData parkingData = await PDIParkingSuggestion();
            return parkingData;
        }

        private InspectionData ExtractParameters(InspectionData inspectionData)
        {
            List<InspectionMasterChecklist> formattedDatas = JsonConvert.DeserializeObject<List<InspectionMasterChecklist>>(inspectionData.InspectionChecklist);
            inspectionData.FormattedInspectionChecklist = formattedDatas.ToDictionary(x => x.InspectionMasterDetailId, x=> x.IsChecked);

            return inspectionData;
        }

        private async Task<int> UpdateInspectionData(InspectionData inspectionData)
        {
            var vehicleData = await tangoDbContext.Vehicle.Where(x => x.FrameNumber == inspectionData.FrameNumber).FirstOrDefaultAsync();
            if (vehicleData != null)
            {
                vehicleData.EngineNumber = inspectionData.EngineNumber;
                vehicleData.KeyNumber = inspectionData.KeyNumber;

                var vehicleRouting = await tangoDbContext.VehicleRouting.Where(x => x.VehicleId == vehicleData.VehicleId).FirstOrDefaultAsync();
                var vehicleInspection = await tangoDbContext.VehicleInspection.Where(x => x.VehicleRoutingId == vehicleRouting.VehicleRoutingId).FirstOrDefaultAsync();
                var vehicleInspectionDetails = await tangoDbContext.VehicleInspectionDetail.Where(x => x.VehicleInspectionId == vehicleInspection.VehicleInspectionId).ToListAsync();

                foreach (var vehicleInspectionDetail in vehicleInspectionDetails)
                {
                    bool isChecked = false;
                    inspectionData.FormattedInspectionChecklist.TryGetValue(vehicleInspectionDetail.InspectionMasterDetailId, out isChecked);
                    vehicleInspectionDetail.IsChecked = isChecked;
                }
            }

            return await tangoDbContext.SaveChangesAsync();
        }

        private async Task UpdateVehicleData(InspectionData inspectionData)
        {
            var vehicleData = await tangoDbContext.Vehicle.Where(x => x.FrameNumber == inspectionData.FrameNumber).FirstOrDefaultAsync();


            await tangoDbContext.SaveChangesAsync();
        }

        public async Task<PDIParkingData> PDIParkingSuggestion()
        {
            PDIParkingData parkingData = new PDIParkingData()
            {
                ParkingLineOptional = "A1",
                ParkingLineSuggestion = "A1"
            };

            return parkingData;
        }
    }
}
