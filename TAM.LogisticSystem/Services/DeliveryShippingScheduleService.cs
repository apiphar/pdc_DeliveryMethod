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
    public class DeliveryShippingScheduleService
    {
        private readonly LogisticDbContext LogisticDbContext;

        public DeliveryShippingScheduleService(LogisticDbContext LogisticDbContext)
        {
            this.LogisticDbContext = LogisticDbContext;
        }

        public async Task<DeliveryShippingScheduleViewModel> Init()
        {
            var deliveryShippingScheduleViewModel = new DeliveryShippingScheduleViewModel();
            deliveryShippingScheduleViewModel.Vendors = await GetVendors();
            deliveryShippingScheduleViewModel.Vessels = await GetVessels();
            deliveryShippingScheduleViewModel.Ports = await GetPorts();
            deliveryShippingScheduleViewModel.DestinationCities = await GetDestinationCities();
            deliveryShippingScheduleViewModel.SourceLocations = await GetSourceLocations();
            deliveryShippingScheduleViewModel.Voyages = await GetVoyages();
            deliveryShippingScheduleViewModel.VoyagesDestinationCities = await GetVoyagesDestinationCities();
            deliveryShippingScheduleViewModel.VoyagesDestinationsSourceLocations = await GetVoyageNodesSourceLocations();

            return deliveryShippingScheduleViewModel;
        }

        /// <summary>
        /// Get all vendors
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeliveryShippingScheduleVendorModel>> GetVendors()
        {
            var vendors = await this.LogisticDbContext.DeliveryVendor
                .Select(Q => new DeliveryShippingScheduleVendorModel
                {
                    DeliveryVendorCode = Q.DeliveryVendorCode,
                    DeliveryVendorName = Q.Name
                }).ToListAsync();

            return vendors;
        }

        /// <summary>
        /// Get vessels with DeliveryMethodCode SH
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeliveryShippingScheduleVesselModel>> GetVessels()
        {
            var vessels = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<DeliveryShippingScheduleVesselModel>($@"
        SELECT
	        dv.DeliveryVendorCode AS DeliveryVendorCode,
	        dvv.DeliveryVendorVehicleId AS DeliveryVendorVehicleId,
	        dvv.PoliceNumberOrVesselName AS PoliceNumberOrVesselName
        FROM
	        DeliveryVendorVehicle dvv
	        JOIN DeliveryVendor dv ON dv.DeliveryVendorCode = dvv.DeliveryVendorCode
	        JOIN DeliveryMethod dm ON dm.DeliveryMethodCode = dvv.DeliveryMethodCode
        WHERE
	        dvv.DeliveryMethodCode = 'SH'
")).ToList();

            return vessels;
        }

        /// <summary>
        /// Get location with LocationTypeCode Port
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeliveryShippingScheduleLocationModel>> GetPorts()
        {
            var ports = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<DeliveryShippingScheduleLocationModel>($@"
SELECT
	l.LocationCode AS LocationCode,
	l.[Name] AS LocationName
FROM
	[Location] l
	JOIN LocationType lt ON lt.LocationTypeCode = l.LocationTypeCode
WHERE lt.LocationTypeCode = 'PORT'
")).ToList();

            return ports;
        }

        /// <summary>
        /// Get destination cities
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeliveryShippingScheduleDestinationCityModel>> GetDestinationCities()
        {
            var destinationCities = await this.LogisticDbContext.CityForShipment
                .Select(Q => new DeliveryShippingScheduleDestinationCityModel
                {
                    CityForShipmentCode = Q.CityForShipmentCode,
                    Name = Q.Name
                }).ToListAsync();

            return destinationCities;
        }

        public async Task<List<DeliveryShippingScheduleLocationModel>> GetSourceLocations()
        {
            var sourceLocations = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<DeliveryShippingScheduleLocationModel>($@"
SELECT
	l.LocationCode AS LocationCode,
	l.[Name] AS LocationName
FROM
	[Location] l
	JOIN LocationType lt ON lt.LocationTypeCode = l.LocationTypeCode
WHERE lt.LocationTypeCode = 'PDCN'
")).ToList();

            return sourceLocations;
        }

        /// <summary>
        /// Get voyages
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeliveryShippingScheduleVoyageModel>> GetVoyages()
        {
            var voyages = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<DeliveryShippingScheduleVoyageModel>($@"
SELECT
	v.VoyageNumber AS VoyageNumber,
	dv.DeliveryVendorCode AS DeliveryVendorCode,
	dv.Name AS DeliveryVendorName,
	v.DeliveryVendorVehicleId AS DeliveryVendorVehicleId,
	dvv.PoliceNumberOrVesselName AS PoliceNumberOrVesselName,
	v.DepartureLocationCode AS DepartureLocationCode,
	l.Name AS DepartureLocationName,
	v.DepartureDate AS DepartureDate
FROM
	Voyage v
	JOIN DeliveryVendorVehicle dvv ON dvv.DeliveryVendorVehicleId = v.DeliveryVendorVehicleId
	JOIN DeliveryVendor dv ON dv.DeliveryVendorCode = dvv.DeliveryVendorCode
	JOIN [Location] l ON l.LocationCode = v.DepartureLocationCode
")).ToList();

            return voyages;
        }

        public async Task<List<DeliveryShippingScheduleVoyageDestinationCityModel>> GetVoyagesDestinationCities()
        {
            var voyagesDesinationCities = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<DeliveryShippingScheduleVoyageDestinationCityModel>($@"
SELECT
	vd.VoyageNumber AS VoyageNumber,
	vd.VoyageNodeId AS VoyageNodeId,
	vd.CityForShipmentCode AS CityForShipmentCode,
	c.[Name] AS CityName,
	vd.EstimatedTimeOfArrival AS EstimatedTimeOfArrival
FROM
	VoyageNode vd
	JOIN CityForShipment c ON c.CityForShipmentCode = vd.CityForShipmentCode
")).ToList();
            return voyagesDesinationCities;
        }

        public async Task<List<DeliveryShippingScheduleVoyageDestinationSourceLocationModel>> GetVoyageNodesSourceLocations()
        {
            var VoyageNodesSourceLocations = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<DeliveryShippingScheduleVoyageDestinationSourceLocationModel>($@"
SELECT
	vcpd.VoyageNodeId AS VoyageNodeId,
	vcpd.VoyageNodeSourceId AS VoyageNodeSourceId,
	vcpd.LocationCode AS LocationCode,
	l.Name AS LocationName,
	vcpd.Capacity AS Capacity
FROM
	VoyageNodeSource vcpd
	JOIN [Location] l ON l.LocationCode = vcpd.LocationCode
")).ToList();

            return VoyageNodesSourceLocations;
        }

        //public async Task Save(DeliveryShippingScheduleSaveModel deliveryShippingScheduleSaveModel)
        //{
        //    await this.LogisticDbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
        //    {
        //        var trans = await this.LogisticDbContext.Database.BeginTransactionAsync();
        //        {
        //            var voyage = new Voyage
        //            {
        //                VoyageNumber = deliveryShippingScheduleSaveModel.voyageForm.voyageNumber,
        //                DeliveryVendorVehicleId = deliveryShippingScheduleSaveModel.voyageForm.vessel.DeliveryVendorVehicleId,
        //                DepartureLocation = deliveryShippingScheduleSaveModel.voyageForm.port.LocationCode,
        //                DepartureDate = deliveryShippingScheduleSaveModel.voyageForm.estimatedDeparture.ToUniversalTime(),
        //                VoyageStatusId = 20
        //            };
        //            this.LogisticDbContext.Voyage.Add(voyage);

        //            foreach(var destinationCity in deliveryShippingScheduleSaveModel.tempDestinationCities)
        //            {
        //                var newDestinationCity = new VoyageNode
        //                {
        //                    VoyageNumber = deliveryShippingScheduleSaveModel.voyageForm.voyageNumber,
        //                    DestinationCity = destinationCity.CityCode,
        //                    EstimatedTimeArrivalDate = destinationCity.EstimatedArrival.ToUniversalTime()
        //                };
        //                this.LogisticDbContext.VoyageNode.Add(newDestinationCity);
        //                await this.LogisticDbContext.SaveChangesAsync();

        //                foreach(var sourceLocation in deliveryShippingScheduleSaveModel.tempSourceLocations)
        //                {
        //                    if(destinationCity.TempVoyageNodeId == sourceLocation.TempVoyageNodeId)
        //                    {
        //                        var newSourceLocation = new VoyageNodeSource
        //                        {
        //                            VoyageNodeId = newDestinationCity.VoyageNodeId,
        //                            SourceLocationCode = sourceLocation.SourceLocationCode,
        //                            Capacity = sourceLocation.Capacity
        //                        };
        //                        this.LogisticDbContext.VoyageNodeSource.Add(newSourceLocation);
        //                    }
        //                }
        //            }
        //            await this.LogisticDbContext.SaveChangesAsync();
        //            trans.Commit();
        //        }
        //    });
        //}

        public async Task Save(DeliveryShippingScheduleSaveModel deliveryShippingScheduleSaveModel)
        {
            var ListVoyage = await GetListVoyage();
            var ListVoyageNode = await GetListVoyageNode();
            var ListVoyageCapacityDestination = await GetListVoyageCapacityDestinationByFrameNumber();

            await this.LogisticDbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                var trans = await this.LogisticDbContext.Database.BeginTransactionAsync();
                {
                    var CheckVoyage = ListVoyage.Where(x => x.VoyageNumber == deliveryShippingScheduleSaveModel.voyageForm.voyageNumber).FirstOrDefault();

                    if (CheckVoyage == null)
                    {
                        await SaveVoyage(deliveryShippingScheduleSaveModel.voyageForm.voyageNumber, deliveryShippingScheduleSaveModel.voyageForm.vessel.DeliveryVendorVehicleId, deliveryShippingScheduleSaveModel.voyageForm.port.LocationCode, deliveryShippingScheduleSaveModel.voyageForm.estimatedDeparture, 20);
                    }
                    else
                    {
                        var entity = await GetVoyage(deliveryShippingScheduleSaveModel.voyageForm.voyageNumber);
                        await UpdateVoyage(entity, deliveryShippingScheduleSaveModel.voyageForm.vessel.DeliveryVendorVehicleId, deliveryShippingScheduleSaveModel.voyageForm.port.LocationCode, deliveryShippingScheduleSaveModel.voyageForm.estimatedDeparture);
                    }

                    foreach (var destinationCity in deliveryShippingScheduleSaveModel.tempDestinationCities)
                    {
                        ListVoyageNode.RemoveAll(x => x.VoyageNumber != deliveryShippingScheduleSaveModel.voyageForm.voyageNumber);
                        var VoyageNodeId = 0;
                        var CheckVoyageNode = ListVoyageNode.Where(x => x.VoyageNodeId == destinationCity.VoyageNodeId).FirstOrDefault();

                        if (CheckVoyageNode == null)
                        {
                            VoyageNodeId = await SaveVoyageNode(deliveryShippingScheduleSaveModel.voyageForm.voyageNumber, destinationCity.CityForShipmentCode, destinationCity.EstimatedTimeOfArrival);
                        }
                        else
                        {
                            var entity = await GetVoyageNode(destinationCity.VoyageNodeId);
                            await UpdateVoyageNode(entity, deliveryShippingScheduleSaveModel.voyageForm.voyageNumber, destinationCity.CityForShipmentCode, destinationCity.EstimatedTimeOfArrival);
                            VoyageNodeId = destinationCity.VoyageNodeId;
                            ListVoyageNode.RemoveAll(x => x.VoyageNodeId == destinationCity.VoyageNodeId);
                        }
                        var currentSourceLocation = new List<DeliveryShippingScheduleVoyageDestinationSourceLocationModel>();
                        if (destinationCity.TempVoyageNodeId != null && destinationCity.TempVoyageNodeId != 0)
                        {
                            currentSourceLocation = deliveryShippingScheduleSaveModel.tempSourceLocations.Where(x => x.TempVoyageNodeId == destinationCity.TempVoyageNodeId).ToList();
                        }
                        else
                        {
                            currentSourceLocation = deliveryShippingScheduleSaveModel.tempSourceLocations.Where(x => x.VoyageNodeId == destinationCity.VoyageNodeId).ToList();
                        }

                        foreach (var sourceLocation in currentSourceLocation)
                        {
                            ListVoyageCapacityDestination.RemoveAll(x => x.VoyageNumber != deliveryShippingScheduleSaveModel.voyageForm.voyageNumber);
                            var CheckCapacityDestination = ListVoyageCapacityDestination.Where(x => x.VoyageNodeSourceId == sourceLocation.VoyageNodeSourceId).FirstOrDefault();
                            if (CheckCapacityDestination == null)
                            {
                                await SaveVoyageCapacityDestination(VoyageNodeId, sourceLocation.LocationCode, sourceLocation.Capacity);
                            }
                            else
                            {
                                var entity = await GetVoyageCapacityDestination(sourceLocation.VoyageNodeSourceId);
                                await UpdateVoyageCapacityDestination(entity, VoyageNodeId, sourceLocation.LocationCode, sourceLocation.Capacity);
                                ListVoyageCapacityDestination.RemoveAll(x => x.VoyageNodeSourceId == sourceLocation.VoyageNodeSourceId);
                            }
                        }
                    }

                    //Delete By VoyageNodeId
                    foreach (var DestinationCity in ListVoyageNode)
                    {
                        var VoyageCapacityDestination = await GetListVoyageCapacityDestination(DestinationCity.VoyageNodeId);
                        foreach (var data in VoyageCapacityDestination)
                        {
                            var entitycapacity = await GetVoyageCapacityDestination(data.VoyageNodeSourceId);
                            await RemoveVoyageCapacityDestination(entitycapacity);
                        }
                        var entity = await GetVoyageNode(DestinationCity.VoyageNodeId);
                        await RemoveVoyageNode(entity);
                    }
                    //Delete By VoyageNodeSourceId
                    foreach (var CapacityLocation in ListVoyageCapacityDestination)
                    {
                        var entity = await GetVoyageCapacityDestination(CapacityLocation.VoyageNodeSourceId);
                        if (entity != null)
                        {
                            await RemoveVoyageCapacityDestination(entity);
                        }
                    }

                    await this.LogisticDbContext.SaveChangesAsync();
                    trans.Commit();
                }
            });
        }

        //Voyage
        public async Task SaveVoyage(string voyagenumber, int deliveryvendorvehicleid, string DepartureLocationCode, DateTimeOffset departuredate, int VoyageStatusEnumId)
        {
            var add = new Voyage
            {
                VoyageNumber = voyagenumber,
                DeliveryVendorVehicleId = deliveryvendorvehicleid,
                DepartureLocationCode = DepartureLocationCode,
                DepartureDate = departuredate.ToLocalTime(),
                VoyageStatusEnumId = VoyageStatusEnumId,
                CreatedAt = DateTime.Now,
                CreatedBy = "ADMIN",
                UpdatedAt = DateTime.Now,
                UpdatedBy = "ADMIN"
            };
            this.LogisticDbContext.Voyage.Add(add);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        internal async Task UpdateVoyage(Voyage entity, int deliveryvendorvehicleid, string DepartureLocationCode, DateTimeOffset departuredate)
        {
            entity.DeliveryVendorVehicleId = deliveryvendorvehicleid;
            entity.DepartureLocationCode = DepartureLocationCode;
            entity.DepartureDate = departuredate.ToLocalTime();
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = "ADMIN";

            await LogisticDbContext.SaveChangesAsync();
        }

        internal async Task RemoveVoyage(Voyage entity)
        {
            LogisticDbContext.Remove(entity);
            await LogisticDbContext.SaveChangesAsync();
        }

        internal async Task<Voyage> GetVoyage(string id)
        {
            return await LogisticDbContext.Voyage.FindAsync(id);
        }

        internal async Task<List<Voyage>> GetListVoyage()
        {
            return await LogisticDbContext.Voyage.ToListAsync();
        }

        //Voyage Destination
        public async Task<int> SaveVoyageNode(string voyagenumber, string CityForShipmentCode, DateTimeOffset EstimatedTimeOfArrival)
        {
            var newDestinationCity = new VoyageNode
            {
                VoyageNumber = voyagenumber,
                CityForShipmentCode = CityForShipmentCode,
                EstimatedTimeOfArrival = EstimatedTimeOfArrival.ToLocalTime(),
                CreatedAt = DateTime.Now,
                CreatedBy = "ADMIN",
                UpdatedAt = DateTime.Now,
                UpdatedBy = "ADMIN"
            };
            this.LogisticDbContext.VoyageNode.Add(newDestinationCity);
            await this.LogisticDbContext.SaveChangesAsync();
            return newDestinationCity.VoyageNodeId;
        }

        internal async Task UpdateVoyageNode(VoyageNode entity, string voyagenumber, string CityForShipmentCode, DateTimeOffset EstimatedTimeOfArrival)
        {
            entity.VoyageNumber = voyagenumber;
            entity.CityForShipmentCode = CityForShipmentCode;
            entity.EstimatedTimeOfArrival = EstimatedTimeOfArrival.ToLocalTime();
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = "ADMIN";

            await LogisticDbContext.SaveChangesAsync();
        }

        internal async Task RemoveVoyageNode(VoyageNode entity)
        {
            LogisticDbContext.Remove(entity);
            await LogisticDbContext.SaveChangesAsync();
        }

        internal async Task<VoyageNode> GetVoyageNode(int id)
        {
            return await LogisticDbContext.VoyageNode.FindAsync(id);
        }

        internal async Task<List<VoyageNode>> GetListVoyageNode()
        {
            return await LogisticDbContext.VoyageNode.ToListAsync();
        }

        //Voyage Capacity Per Destination
        public async Task SaveVoyageCapacityDestination(int VoyageNodeId, string LocationCode, int capacity)
        {
            var newDestinationCity = new VoyageNodeSource
            {
                VoyageNodeId = VoyageNodeId,
                LocationCode = LocationCode,
                Capacity = capacity,
                CreatedAt = DateTime.Now,
                CreatedBy = "ADMIN",
                UpdatedAt = DateTime.Now,
                UpdatedBy = "ADMIN"
            };
            this.LogisticDbContext.VoyageNodeSource.Add(newDestinationCity);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        internal async Task UpdateVoyageCapacityDestination(VoyageNodeSource entity, int VoyageNodeId, string LocationCode, int capacity)
        {
            entity.VoyageNodeId = VoyageNodeId;
            entity.LocationCode = LocationCode;
            entity.Capacity = capacity;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = "ADMIN";

            await LogisticDbContext.SaveChangesAsync();
        }

        internal async Task RemoveVoyageCapacityDestination(VoyageNodeSource entity)
        {
            LogisticDbContext.Remove(entity);
            await LogisticDbContext.SaveChangesAsync();
        }

        internal async Task<VoyageNodeSource> GetVoyageCapacityDestination(int id)
        {
            return await LogisticDbContext.VoyageNodeSource.Where(x => x.VoyageNodeSourceId == id).FirstOrDefaultAsync();
        }

        internal async Task<List<VoyageNodeSource>> GetListVoyageCapacityDestination()
        {
            return await LogisticDbContext.VoyageNodeSource.ToListAsync();
        }

        internal async Task<List<VoyageNodeSource>> GetListVoyageCapacityDestination(int VoyageNodeId)
        {
            return await LogisticDbContext.VoyageNodeSource.Where(x => x.VoyageNodeId == VoyageNodeId).ToListAsync();
        }

        internal async Task<List<DeliveryShippingScheduleVoyageNodeSourceVoyageNumber>> GetListVoyageCapacityDestinationByFrameNumber()
        {
            var voyages = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<DeliveryShippingScheduleVoyageNodeSourceVoyageNumber>($@"
            select 
	            a.VoyageNodeSourceId ,
	            a.VoyageNodeId,
	            a.LocationCode,
	            a.Capacity,
	            c.VoyageNumber
            from VoyageNodeSource a
            left join VoyageNode b on a.VoyageNodeId = b. VoyageNodeId
            left join Voyage c on c.VoyageNumber = b.VoyageNumber
            ")).ToList();

            return voyages;
        }


    }
}
