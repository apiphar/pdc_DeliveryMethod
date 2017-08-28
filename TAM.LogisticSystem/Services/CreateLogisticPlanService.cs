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
    public class CreateLogisticPlanService
    {
        private readonly LogisticDbContext context;

        public CreateLogisticPlanService(LogisticDbContext context)
        {
            this.context = context;
        }
        public async Task<List<CreateLogisticPlanModel>> GetModel()
        {
            var createLogisticPlanModel = (await this.context.Database.GetDbConnection().QueryAsync<CreateLogisticPlanModel>($@"SELECT V.VehicleId,
		           V.Katashiki,
		           V.Suffix,
		           RDP.RoutingMasterCode,
		           RDP.LeadMinutes,
		           ISNULL(RDP.Ordering,0) as Ordering,
		           V.PhysicalLocationName,
		           --di tabel routing ada branch
		           BRANCHCODE = (SELECT DISTINCT RD.BRANCHCODE
                                   FROM RoutingDictionary RD
                                  WHERE RD.Katashiki = V.Katashiki
                                    AND RD.Suffix    = V.Suffix
                                ),
                   ROUTINGLEADTIMEBYID = (SELECT RM.RoutingLeadTimeById
                                            FROM RoutingMaster RM
			                               WHERE RM.RoutingMasterCode = RDP.RoutingMasterCode
                                         )
              FROM Vehicle V
			  JOIN ROUTINGDICTIONARYPRODUCTION RDP
			    ON V.PhysicalLocationName = RDP.LocationCode
			   AND V.Katashiki            = RDP.Katashiki
			   AND V.Suffix               = RDP.Suffix
			 UNION
			SELECT DISTINCT V.VehicleId,
			       V.Katashiki,
		           V.Suffix,
		           RDP.RoutingMasterCode,
		           NULL,
		           NULL,
		           V.PhysicalLocationName,
		           BranchCode = (SELECT DISTINCT RD.BRANCHCODE
                                   FROM RoutingDictionary RD
                                  WHERE RD.Katashiki = V.Katashiki
                                    AND RD.Suffix    = V.Suffix
                                ),
                   RoutingLeadTimeById = (SELECT RM.RoutingLeadTimeById
                                            FROM RoutingMaster RM
			                               WHERE RM.RoutingMasterCode = RDP.RoutingMasterCode
                                         )
		      FROM Vehicle V
			  JOIN ROUTINGDICTIONARYPRODUCTION RDP
			    ON V.PhysicalLocationName = RDP.LocationCode
			 WHERE V.VehicleId NOT IN (SELECT V.VehicleId
			                             FROM Vehicle V
			 		                     JOIN ROUTINGDICTIONARYPRODUCTION RDP
										   ON V.PhysicalLocationName = RDP.LocationCode
										  AND V.Katashiki            = RDP.Katashiki
										  AND V.Suffix               = RDP.Suffix)
			")).ToList();
            return createLogisticPlanModel;
        }
        public async Task<List<TemporalLogisticPlanModel>> GetTemporalModel(List<CreateLogisticPlanModel> model)
        {
            var tempLogisticPlanModel = new List<TemporalLogisticPlanModel>();
            foreach (var item in model)
            {
                if (Convert.ToInt32(item.RoutingLeadTimeById) == 1)
                {
                    var tempModelTime = (await this.context.Database.GetDbConnection().QueryAsync<TemporalLogisticPlanModel>($@"
SELECT TaktSeconds = ISNULL(PKD.TaktSeconds,0),
			    	       Post        = ISNULL(PKD.Post,0),
                           LeadMinutes = TaktSeconds*Post
			    	  FROM PDIKATSUDICTIONARY PKD
			         WHERE PKD.Katashiki = @Katashiki
			           AND PKD.Suffix    = @Suffix
", new { Katashiki = item.Katashiki, Suffix = item.Suffix })).FirstOrDefault();
                    var lineNumber = (await this.context.Database.GetDbConnection().QueryAsync<TemporalLogisticPlanModel>($@"
			        SELECT LineNumber = PLD.LINENUMBER
			          FROM PDILineDictionary PLD
			         WHERE PLD.LocationCode = @LocationCode
", new { LocationCode = item.PhysicalLocationName })).FirstOrDefault();
                    var tempModelPdi = new TemporalLogisticPlanModel();
                    tempModelPdi.TaktSeconds = tempModelTime.TaktSeconds;
                    tempModelPdi.Post = tempModelTime.Post;
                    tempModelPdi.LeadMinutes = tempModelTime.LeadMinutes;
                    tempModelPdi.LineNumber = lineNumber.LineNumber;
                    tempModelPdi.VehicleId = item.VehicleId;
                    tempModelPdi.RoutingMasterCode = item.RoutingMasterCode;
                    tempModelPdi.LocationCode = item.PhysicalLocationName;
                    tempModelPdi.Ordering = item.Ordering;
                    tempLogisticPlanModel.Add(tempModelPdi);
                }
                else if (Convert.ToInt32(item.RoutingLeadTimeById) == 2)
                {
                    var PioId = (await this.context.Database.GetDbConnection().QueryAsync<string>($@"
SELECT PIOLINEDICTIONARYID = PLDD.PIOLineDictionaryId
				    	  FROM PIOLineDictionaryDetail PLDD
				         WHERE PLDD.Katashiki = @Katashiki
				           AND PLDD.Suffix    = @Suffix
				        ", new { Katashiki = item.Katashiki, Suffix = item.Suffix })).FirstOrDefault();
                    var tempTime = (await this.context.Database.GetDbConnection().QueryAsync<TemporalLogisticPlanModel>($@"
                        SELECT TaktSeconds = ISNULL(PLD.TaktSeconds,0),
				    	Post= ISNULL(PLD.Post,0),
				    	LineNumber= PLD.LINENUMBER,
                        LeadMinutes= TaktSeconds * Post
				    	FROM PIOLineDictionary PLD
				    	WHERE PLD.PIOLineDictionaryId = @id
", new { id = PioId })).FirstOrDefault();
                    var tempModelPIO = new TemporalLogisticPlanModel();
                    tempModelPIO.TaktSeconds = tempTime.TaktSeconds;
                    tempModelPIO.Post = tempTime.Post;
                    tempModelPIO.LineNumber = tempTime.LineNumber;
                    tempModelPIO.LeadMinutes = tempTime.LeadMinutes;
                    tempModelPIO.VehicleId = item.VehicleId;
                    tempModelPIO.RoutingMasterCode = item.RoutingMasterCode;
                    tempModelPIO.LocationCode = item.PhysicalLocationName;
                    tempModelPIO.Ordering = item.Ordering;
                    tempLogisticPlanModel.Add(tempModelPIO);
                }
                else if ((Convert.ToInt32(item.RoutingLeadTimeById) == 3))
                {
                    var SPUId = (await this.context.Database.GetDbConnection().QueryAsync<string>($@"
SELECT SPULINEDICTIONARYID = SLDD.SPULineDictionaryId
			    	  FROM SPULineDictionaryDetail SLDD
			         WHERE SLDD.Katashiki  = @Katashiki
			           AND SLDD.Suffix     = @Suffix
			           AND SLDD.BranchCode = @BranchCode
				        ", new { Katashiki = item.Katashiki, Suffix = item.Suffix, BranchCode = item.BranchCode })).FirstOrDefault();
                    var tempTime3 = (await this.context.Database.GetDbConnection().QueryAsync<TemporalLogisticPlanModel>($@"
                        SELECT TaktSeconds = ISNULL(SLD.TaktSeconds,0),
			    	       Post= ISNULL(SLD.Post,0),
			    	       LineNumber= SLD.LINENUMBER,
			    	       LeadMinutes = TaktSeconds * Post
			    	  FROM SPULineDictionary SLD
			    	 WHERE SLD.SPULineDictionaryId = @SPULINEDICTIONARYID
", new { SPULINEDICTIONARYID = SPUId })).FirstOrDefault();
                    var tempModelSPU = new TemporalLogisticPlanModel();
                    tempModelSPU.TaktSeconds = tempTime3.TaktSeconds;
                    tempModelSPU.Post = tempTime3.Post;
                    tempModelSPU.LineNumber = tempTime3.LineNumber;
                    tempModelSPU.LeadMinutes = tempTime3.LeadMinutes;
                    tempModelSPU.VehicleId = item.VehicleId;
                    tempModelSPU.RoutingMasterCode = item.RoutingMasterCode;
                    tempModelSPU.LocationCode = item.PhysicalLocationName;
                    tempModelSPU.Ordering = item.Ordering;
                    tempLogisticPlanModel.Add(tempModelSPU);

                }
                else if ((Convert.ToInt32(item.RoutingLeadTimeById) == 4))
                {
                    var tempTime4 = (await this.context.Database.GetDbConnection().QueryAsync<TemporalLogisticPlanModel>($@"
                        SELECT LeadMinutes = A.LeadMinutes
			    	  FROM RoutingLocationLeadTime A
			         WHERE A.LOCATIONCODE      = @LOCATIONCODE
			           AND A.ROUTINGMASTERCODE = @ROUTINGMASTERCODE   
", new { LOCATIONCODE = item.PhysicalLocationName, ROUTINGMASTERCODE = item.RoutingMasterCode })).FirstOrDefault();
                    var tempModelLocation = new TemporalLogisticPlanModel();
                    tempModelLocation.LeadMinutes = tempTime4.LeadMinutes;
                    tempModelLocation.VehicleId = item.VehicleId;
                    tempModelLocation.RoutingMasterCode = item.RoutingMasterCode;
                    tempModelLocation.LocationCode = item.PhysicalLocationName;
                    tempModelLocation.Ordering = item.Ordering;
                    tempLogisticPlanModel.Add(tempModelLocation);
                }
                else if ((Convert.ToInt32(item.RoutingLeadTimeById) == 5))
                {
                    var tempTime5 = (await this.context.Database.GetDbConnection().QueryAsync<TemporalLogisticPlanModel>($@"
                       SELECT EstimatedTimeInitial= A.EstimatedShipmentArrival
			    	  FROM ShipmentInvoiceDetail A
			         WHERE A.Katashiki = @Katashiki
			           AND A.Suffix    = @Suffix
", new { Katashiki = item.Katashiki, Suffix = item.Suffix })).FirstOrDefault();
                    var tempModelVessel = new TemporalLogisticPlanModel();
                    tempModelVessel.EstimatedTimeInitial = tempTime5.EstimatedTimeInitial;
                    tempModelVessel.VehicleId = item.VehicleId;
                    tempModelVessel.RoutingMasterCode = item.RoutingMasterCode;
                    tempModelVessel.LocationCode = item.PhysicalLocationName;
                    tempModelVessel.Ordering = item.Ordering;
                    tempLogisticPlanModel.Add(tempModelVessel);
                }
                else if ((Convert.ToInt32(item.RoutingLeadTimeById) == 6))
                {
                    var DELIVERYLEGCODE = (await this.context.Database.GetDbConnection().QueryAsync<string>($@"
SELECT DELIVERYLEGCODE = A.DELIVERYLEGCODE
			    	  FROM DELIVERYLEG A
			         WHERE A.LocationFrom = @LOCATIONCODE
			           AND A.LocationTo   = @LOCATIONCODE
				        ", new { LOCATIONCODE = item.PhysicalLocationName })).FirstOrDefault();

                    var BufferMinutes = (await this.context.Database.GetDbConnection().QueryAsync<int>($@"
SELECT BUFFERMINUTES   =  A.BufferMinutes
			    	  FROM DELIVERYLEG A
			         WHERE A.LocationFrom = @LOCATIONCODE
			           AND A.LocationTo   = @LOCATIONCODE
				        ", new { LOCATIONCODE = item.PhysicalLocationName })).FirstOrDefault();
                    var LeadMinutes = (await this.context.Database.GetDbConnection().QueryAsync<int>($@"
SELECT LEADMINUTES = A.LeadMinutes
			    	  FROM DeliveryLeadTime A
			    	 WHERE A.DeliveryLegCode = @delivery", new { delivery = DELIVERYLEGCODE })).FirstOrDefault();

                    var tempModelDelivery = new TemporalLogisticPlanModel();
                    tempModelDelivery.LeadMinutes = LeadMinutes;
                    tempModelDelivery.BufferMinutes = BufferMinutes;
                    tempModelDelivery.VehicleId = item.VehicleId;
                    tempModelDelivery.RoutingMasterCode = item.RoutingMasterCode;
                    tempModelDelivery.LocationCode = item.PhysicalLocationName;
                    tempModelDelivery.Ordering = item.Ordering;
                    tempLogisticPlanModel.Add(tempModelDelivery);
                }
                else if ((Convert.ToInt32(item.RoutingLeadTimeById) == 7))
                {
                    var LeadMinutess = (await this.context.Database.GetDbConnection().QueryAsync<int>($@"
			    	SELECT LEADMINUTES = A.LeadMinutes
			    	  FROM Dwelling A
			         WHERE A.LocationFrom = @locationCode
			           AND A.LocationTo   = @locationCode", new { locationCode = item.PhysicalLocationName })).FirstOrDefault();

                    var tempModelDwelling = new TemporalLogisticPlanModel();
                    tempModelDwelling.LeadMinutes = LeadMinutess;
                    tempModelDwelling.VehicleId = item.VehicleId;
                    tempModelDwelling.RoutingMasterCode = item.RoutingMasterCode;
                    tempModelDwelling.LocationCode = item.PhysicalLocationName;
                    tempModelDwelling.Ordering = item.Ordering;
                    tempLogisticPlanModel.Add(tempModelDwelling);
                }
                else
                {
                    var tempModelKosong = new TemporalLogisticPlanModel();
                    tempModelKosong.LeadMinutes = 0;
                    tempModelKosong.VehicleId = 0;
                    tempModelKosong.RoutingMasterCode = "";
                    tempModelKosong.LocationCode = "";
                    tempModelKosong.Ordering = 0;
                    tempLogisticPlanModel.Add(tempModelKosong);
                }
            }
            return tempLogisticPlanModel;
        }
        // TIE: START
        //public async Task InsertVehicleRouting(List<TemporalLogisticPlanModel> listTemporal)
        //{
        //    var listRoutingVehicle = new List<VehicleRouting>();
        //    foreach (var item in listTemporal)
        //    {
        //        var vehicleRouting = new VehicleRouting()
        //        {
        //            VehicleId = item.VehicleId,
        //            RoutingMasterCode = item.RoutingMasterCode,
        //            BufferMinutes = item.BufferMinutes,
        //            EstimatedTimeInitial = item.EstimatedTimeInitial,
        //            LeadMinutes = item.LeadMinutes,
        //            LineNumber = item.LineNumber,
        //            Ordering = item.Ordering,
        //            LocationCode = item.LocationCode,
        //            EstimatedTimeAdjusted = Convert.ToDateTime("01/01/1900"),
        //            CreatedBy = "SYSTEM",
        //            UpdatedBy = "SYSTEM",
        //            CreatedAt = DateTime.Now,
        //            UpdatedAt = DateTime.Now
        //        };
        //        this.context.VehicleRouting.Add(vehicleRouting);

        //    }
        //    await this.context.SaveChangesAsync();
        //}
        // TIE: END
    }
}
