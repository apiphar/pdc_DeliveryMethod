INSERT INTO [dbo].[ProcessLeadTimeByEnum]
           ([ProcessLeadTimeByEnumId],[Name])
     VALUES
           (9,'Shipment Arrival'),
           (10,'PDC Configuration')
GO

INSERT INTO [dbo].[ProcessMaster]
           ([ProcessMasterCode],[Name],[IsScan],[ProcessLeadTimeByEnumId],[BufferMinutes],[SwappingPoint])
     VALUES
           ('PDI','PDI Completion',1,1,0,0),
           ('RCI','Receiving In',1,6,0,0),
           ('DLO','Delivery Out',1,4,0,1),
           ('PIOIN','PIO In',1,4,0,0),
           ('PIOC','PIO Completion',1,2,0,0),
           ('SPUIN','SPU In',1,4,0,0),
           ('SPUC','SPU Completion',1,3,0,0),
           ('PORTI','Port In',1,6,0,0),
           ('SHASSIGN','Shipment Assign',0,8,0,0),
           ('SHLDNG','Shipment Loading',1,8,0,0),
           ('SHDPRT','Shipment Depart',1,5,0,0),
           ('SHARRVL','Shipment Arrival',1,9,0,0)
GO

INSERT INTO [dbo].[DeliveryLeg]
           ([DeliveryLegCode],[Name],[LocationFrom],[LocationTo],[CityLegCode],[BufferMinutes],[NeedSJKB])
     VALUES
           ('HOATRTYCJK0SCY','HO Astrido Toyota - Sunter Common Yard','HOATRTY','CJK0SCY','CITY0814',0,0),
           ('PMEBLWNNBIGLMN','Port Belawan - PDC Gilimanuk','PMEBLWN','NBIGLMN','CITY0032',0,0),
           ('NBIGLMNNSSDAYA','PDC Gilimanuk - PDC Daya','NBIGLMN','NSSDAYA','CITY0064',0,0),
           ('NSSDAYABJKARM','PDC Daya - Astrido Toyota Balikpapan','NSSDAYA','BJKARM','CITY0086',0,0)
GO

INSERT INTO [dbo].[DeliveryLeadTime]
           ([DeliveryLegCode],[DeliveryMethodCode],[LeadMinutes])
     VALUES
           ('HOATRTYCJK0SCY','SC',150),
           ('PMEBLWNNBIGLMN','SC',270),
           ('NBIGLMNNSSDAYA','SC',210),
           ('NSSDAYABJKARM','SC',120)
GO

INSERT INTO [dbo].[DeliveryVendorVehicle]
           ([DeliveryVendorCode],[DeliveryMethodCode],[PoliceNumberOrVesselName],[Capacity])
     VALUES
           ('ANG','SH','Titanic',250)
GO

INSERT INTO [dbo].[CityForShipment]
           ([CityForShipmentCode],[Name])
     VALUES
           ('CTSH0001','Balikpapan')
GO

UPDATE [dbo].[Location]
   SET [CityForShipmentCode] = 'CTSH0001'
 WHERE LocationCode = 'PMEBLWN'
GO

INSERT INTO [dbo].[Voyage]
           ([VoyageNumber],[DeliveryVendorVehicleId],[DepartureLocationCode],[DepartureDate],[VoyageStatusEnumId],[CancelledAt])
     VALUES
           ('VYG001',
		   (SELECT [DeliveryVendorVehicleId] FROM [dbo].[DeliveryVendorVehicle] WHERE [DeliveryVendorCode] = 'ANG' AND [DeliveryMethodCode] = 'SH'),
		   'PJKTJPR','20170617 09:00',1,NULL)
GO

INSERT INTO [dbo].[VoyageNode]
           ([VoyageNumber],[CityForShipmentCode],[EstimatedTimeOfArrival])
     VALUES
           ('VYG001','CTSH0001','20170619 07:00')
GO

INSERT INTO [dbo].[ProcessDictionary]
           ([BranchCode],[Katashiki],[Suffix],[ValidFrom])
     VALUES
           ('AST003075','B100RA-GQSFJ','00',GETDATE())
GO

INSERT INTO [dbo].[PDILeadTime]
           ([Katashiki],[Suffix],[LocationCode],[TaktSeconds],[Post])
     VALUES
           ('B100RA-GQSFJ','00','HOATRTY',120,10)
GO

INSERT INTO [dbo].[PIOLine]
           ([LocationCode],[LineNumber],[TaktSeconds],[Post],[IsLocked])
     VALUES
           ('CJK0SCY','1',240,8,1)
GO

INSERT INTO [dbo].[PIOLineDetail]
           ([PIOLineId],[Katashiki],[Suffix])
     VALUES
           ((SELECT [PIOLineId] FROM [dbo].[PIOLine] WHERE [LineNumber] = '1'),'B100RA-GQSFJ','00')
GO

INSERT INTO [dbo].[SPULine]
           ([LocationCode],[LineNumber],[TaktSeconds],[Post],[IsLocked])
     VALUES
           ('NSSDAYA','2',300,12,1)
GO

INSERT INTO [dbo].[SPULineDetail]
           ([SPULineId],[Katashiki],[Suffix],[BranchCode])
     VALUES
           ((SELECT [SPULineId] FROM [dbo].[SPULine] WHERE [LineNumber] = '2'),'B100RA-GQSFJ','00','AST003075')
GO

INSERT INTO [dbo].[ProcessDictionaryDetail]
           ([ProcessDictionaryId],[Ordering],[ProcessMasterCode],[DeliveryMethodCode],[LocationCode])
     VALUES
           ((SELECT [ProcessDictionaryId] FROM [dbo].[ProcessDictionary] WHERE [BranchCode] = 'AST003075'),1,'PDI',NULL,'HOATRTY'),
           ((SELECT [ProcessDictionaryId] FROM [dbo].[ProcessDictionary] WHERE [BranchCode] = 'AST003075'),2,'DLO',NULL,'HOATRTY'),
           ((SELECT [ProcessDictionaryId] FROM [dbo].[ProcessDictionary] WHERE [BranchCode] = 'AST003075'),3,'RCI','SC','CJK0SCY'),
           ((SELECT [ProcessDictionaryId] FROM [dbo].[ProcessDictionary] WHERE [BranchCode] = 'AST003075'),4,'PIOIN',NULL,'CJK0SCY'),
           ((SELECT [ProcessDictionaryId] FROM [dbo].[ProcessDictionary] WHERE [BranchCode] = 'AST003075'),5,'PIOC',NULL,'CJK0SCY'),
           ((SELECT [ProcessDictionaryId] FROM [dbo].[ProcessDictionary] WHERE [BranchCode] = 'AST003075'),6,'DLO',NULL,'CJK0SCY'),
           ((SELECT [ProcessDictionaryId] FROM [dbo].[ProcessDictionary] WHERE [BranchCode] = 'AST003075'),7,'PORTI','SC','PJKTJPR'),
           ((SELECT [ProcessDictionaryId] FROM [dbo].[ProcessDictionary] WHERE [BranchCode] = 'AST003075'),8,'SHASSIGN',NULL,'PJKTJPR'),
           ((SELECT [ProcessDictionaryId] FROM [dbo].[ProcessDictionary] WHERE [BranchCode] = 'AST003075'),9,'SHLDNG',NULL,'PJKTJPR'),
           ((SELECT [ProcessDictionaryId] FROM [dbo].[ProcessDictionary] WHERE [BranchCode] = 'AST003075'),10,'SHDPRT',NULL,'PJKTJPR'),
           ((SELECT [ProcessDictionaryId] FROM [dbo].[ProcessDictionary] WHERE [BranchCode] = 'AST003075'),11,'SHARRVL',NULL,'PMEBLWN'),
           ((SELECT [ProcessDictionaryId] FROM [dbo].[ProcessDictionary] WHERE [BranchCode] = 'AST003075'),12,'RCI','SC','NBIGLMN'),
           ((SELECT [ProcessDictionaryId] FROM [dbo].[ProcessDictionary] WHERE [BranchCode] = 'AST003075'),13,'DLO',NULL,'NBIGLMN'),
           ((SELECT [ProcessDictionaryId] FROM [dbo].[ProcessDictionary] WHERE [BranchCode] = 'AST003075'),14,'RCI','SC','NSSDAYA'),
           ((SELECT [ProcessDictionaryId] FROM [dbo].[ProcessDictionary] WHERE [BranchCode] = 'AST003075'),15,'SPUIN',NULL,'NSSDAYA'),
           ((SELECT [ProcessDictionaryId] FROM [dbo].[ProcessDictionary] WHERE [BranchCode] = 'AST003075'),16,'SPUC',NULL,'NSSDAYA')
GO

INSERT INTO [dbo].[Vehicle]
           ([Katashiki],[Suffix],[ExteriorColorCode],[InteriorColorCode],[BranchCode],[Responsibility],[PhysicalLocationCode],[DTPLOD],[ProductionYear]
           ,[RRN],[FrameNumber],[NomorIndukKendaraan],[EngineNumber],[EnginePrefix],[KeyNumber],[PaketAksesorisTAM],[REVPLOD],[TotalLossAt],[IsClaimLostToInsurance]
           ,[SetUsedAt],[EstimatedPDCIn],[EstimatedPDCOut],[EstimatedArrivalBranch],[ActualArrivalBranch],[RequestedDeliveryTime],[ActualDeliveryTime]
           ,[CustomerReceivedTime],[SpecialVehicleSign],[HasCustomer],[IsAdvanceUnit],[IsUrgentDeliveryRequest],[IsHold],[IsInAudit],[IsInWorkshop]
           ,[IsSentMDPToDMS],[IsSentRevisedPDDToDMS],[IsSentDeliveryInfoToDMS],[IsSentBoardPDSToDMS],[CreatedAt],[CreatedBy],[UpdatedAt],[UpdatedBy])
     VALUES
           ('B100RA-GQSFJ','00','044','044','AST003075',NULL,NULL,GETDATE(),2017,'RRN01','MHFGB8EM0G0408581',NULL,NULL,NULL,NULL,NULL,NULL,NULL,0
           ,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,0,0,0,0,0,0,0,0,GETDATE(),'System',GETDATE(),'System')


--------------- Update with Working Time, Break Time, And Holiday -----------------

INSERT INTO [dbo].[Shift]
           ([ShiftCode],[Description])
     VALUES
           ('SH01','Siang'),
           ('SH02','Malam')
GO

INSERT INTO [dbo].[LocationWorkHour]
           ([LocationCode],[ShiftCode],[Start],[Finish])
     VALUES
           ('HOATRTY','SH01','20170612 10:00','20170612 19:00'),
           ('HOATRTY','SH02','20170612 22:00','20170613 07:00'),
           ('CJK0SCY','SH01','20170612 10:00','20170612 19:00'),
           ('CJK0SCY','SH02','20170612 22:00','20170613 07:00'),
           ('NBIGLMN','SH01','20170612 10:00','20170612 19:00'),
           ('NBIGLMN','SH02','20170612 22:00','20170613 07:00'),
           ('NSSDAYA','SH01','20170612 10:00','20170612 19:00'),
           ('NSSDAYA','SH02','20170612 22:00','20170613 07:00')
GO

INSERT INTO [dbo].[LocationBreakHour]
           ([LocationCode],[ShiftCode],[Start],[Finish])
     VALUES
           ('HOATRTY','SH01','20170612 12:00','20170612 12:30'),
           ('HOATRTY','SH01','20170612 15:00','20170612 15:30'),
           ('HOATRTY','SH02','20170613 00:00','20170613 00:30'),
           ('HOATRTY','SH02','20170613 03:00','20170613 03:30'),
           ('CJK0SCY','SH01','20170612 12:00','20170612 12:30'),
           ('CJK0SCY','SH01','20170612 15:00','20170612 15:30'),
           ('CJK0SCY','SH02','20170613 00:00','20170613 00:30'),
           ('CJK0SCY','SH02','20170613 03:00','20170613 03:30'),
           ('NBIGLMN','SH01','20170612 12:00','20170612 12:30'),
           ('NBIGLMN','SH01','20170612 15:00','20170612 15:30'),
           ('NBIGLMN','SH02','20170613 00:00','20170613 00:30'),
           ('NBIGLMN','SH02','20170613 03:00','20170613 03:30'),
           ('NSSDAYA','SH01','20170612 12:00','20170612 12:30'),
           ('NSSDAYA','SH01','20170612 15:00','20170612 15:30'),
           ('NSSDAYA','SH02','20170613 00:00','20170613 00:30'),
           ('NSSDAYA','SH02','20170613 03:00','20170613 03:30')
GO

INSERT INTO [dbo].[Holiday]
           ([LocationCode],[HolidayDate])
     VALUES
           ('NSSDAYA','20170619')
GO

