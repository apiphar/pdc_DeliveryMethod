INSERT INTO [dbo].[Dealer]
           ([DealerCode],[MDMDealerId],[DealerTypeCode],[Name],[Address])
     VALUES
           ('DLRMKSR01',NULL,'EXINDNONAI','Dealer Makasar',NULL)
GO

INSERT INTO [dbo].[Company]
           ([CompanyCode],[MDMCompanyId],[DealerCode],[Name],[NPWPAddress],[SAPCode],[Phone],[Fax],[Email],[TradeName],[NPWP],[TermOfPaymentDay],[IsDealerFinancing])
     VALUES
           ('COMP01',NULL,'DLRMKSR01','Company Makassar',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0)
GO

INSERT INTO [dbo].[SalesArea]
           ([SalesAreaCode],[Description])
     VALUES
           ('SLSR01','Sales Area 01')
GO

INSERT INTO [dbo].[Branch]
           ([BranchCode],[SalesAreaCode],[CompanyCode],[LocationCode],[DestinationCode],[RegionCode],[CostingClusterCode],[Name],[Phone],[Fax],[BranchAFICode],[BranchAS400],[KabupatenCode])
     VALUES
           ('MKSR01','SLSR01','COMP01',NULL,NULL,NULL,NULL,'Branch Makassar',NULL,NULL,'1001','SMSTMKSR01',NULL)
GO

INSERT INTO [dbo].[CarType]
           ([Katashiki],[Suffix],[HSCode],[Name],[EngineDescription],[EngineVolume],[SteerPosition],[WheelDiameter],[WheelSize],[Assembly],[CarSeriesCode],[CarCategoryCode],[IsFTZ])
     VALUES
           ('K1','S1',NULL,'Avanza Velos','Mesin Baru','1500','L','2750','185/60 R15',NULL,NULL,NULL,0)
GO

INSERT INTO [dbo].[RoutingDictionary]
           ([BranchCode],[Katashiki],[Suffix],[ValidFrom])
     VALUES
           ('MKSR01','K1','S1',GETDATE())
GO

INSERT INTO [dbo].[ExteriorColor]
           ([ExteriorColorCode],[IndonesianName],[EnglishName])
     VALUES
           ('12E1','Perak','Silver')
GO

INSERT INTO [dbo].[InteriorColor]
           ([InteriorColorCode],[IndonesianName],[EnglishName])
     VALUES
           ('12I1','Perak','Silver')
GO

INSERT INTO [dbo].[RoutingMaster]
           ([RoutingMasterCode],[Name],[IsScan],[RoutingLeadTimeById],[BufferMinutes],[SwappingPoint],[DoPreDeliveryCenterIn],[DoPreDeliveryCenterOut],[DoETABranch],[DoPredictDelivery],[DoMonthlyCarCarrierPlan],[DoDailyCarCarrierPlan])
     VALUES
           ('PDI','PDI Completion',1,1,0,0,0,0,0,0,0,0),
           ('RCI','Receiving In',1,6,0,0,0,0,0,0,0,0),
           ('DLO','Delivery Out',1,4,0,0,0,0,0,0,0,0),
           ('PIOIN','PIO In',1,4,0,0,0,0,0,0,0,0),
           ('PIOC','PIO Completion',1,2,0,0,0,0,0,0,0,0),
           ('SPUIN','SPU In',1,4,0,0,0,0,0,0,0,0),
           ('SPUC','SPU Completion',1,3,0,0,0,0,0,0,0,0),
           ('PORTI','Port In',1,6,0,0,0,0,0,0,0,0),
           ('SHASSIGN','Shipment Assign',0,8,0,0,0,0,0,0,0,0),
           ('SHLDNG','Shipment Loading',1,8,0,0,0,0,0,0,0,0),
           ('SHDPRT','Shipment Depart',1,5,0,0,0,0,0,0,0,0),
           ('SHARRVL','Shipment Arrival',1,8,0,0,0,0,0,0,0,0)
GO

INSERT INTO [dbo].[Location]
           ([LocationCode],[Name],[Address],[LocationTypeCode],[CityLocationCode])
     VALUES
           ('ADMSTR','ADM Sunter',NULL,'BRCH',NULL),
           ('SCY','Sunter Common Yard',NULL,'CY',NULL),
           ('PRIOK','Port Tanjung Priok',NULL,'PORT',NULL),
           ('SOETTA','Port Soetta',NULL,'PORT',NULL),
           ('PDCPTTN','PDC Pattene',NULL,'PDCR',NULL),
           ('PDCDYU','PDC Dayu',NULL,'PDCR',NULL)
GO

INSERT INTO [dbo].[RoutingDictionaryDetail]
           ([RoutingDictionaryId],[LocationCode],[RoutingMasterCode],[DeliveryMethodCode],[Ordering])
     VALUES
           ((SELECT [RoutingDictionaryId] FROM [dbo].[RoutingDictionary] WHERE [BranchCode] = 'MKSR01'),'ADMSTR','PDI',NULL,1),
           ((SELECT [RoutingDictionaryId] FROM [dbo].[RoutingDictionary] WHERE [BranchCode] = 'MKSR01'),'ADMSTR','DLO',NULL,2),
           ((SELECT [RoutingDictionaryId] FROM [dbo].[RoutingDictionary] WHERE [BranchCode] = 'MKSR01'),'SCY','RCI','SC',3),
           ((SELECT [RoutingDictionaryId] FROM [dbo].[RoutingDictionary] WHERE [BranchCode] = 'MKSR01'),'SCY','PIOIN',NULL,4),
           ((SELECT [RoutingDictionaryId] FROM [dbo].[RoutingDictionary] WHERE [BranchCode] = 'MKSR01'),'SCY','PIOC',NULL,5),
           ((SELECT [RoutingDictionaryId] FROM [dbo].[RoutingDictionary] WHERE [BranchCode] = 'MKSR01'),'SCY','DLO',NULL,6),
           ((SELECT [RoutingDictionaryId] FROM [dbo].[RoutingDictionary] WHERE [BranchCode] = 'MKSR01'),'PRIOK','PORTI','SC',7),
           ((SELECT [RoutingDictionaryId] FROM [dbo].[RoutingDictionary] WHERE [BranchCode] = 'MKSR01'),'PRIOK','SHASSIGN',NULL,8),
           ((SELECT [RoutingDictionaryId] FROM [dbo].[RoutingDictionary] WHERE [BranchCode] = 'MKSR01'),'PRIOK','SHLDNG',NULL,9),
           ((SELECT [RoutingDictionaryId] FROM [dbo].[RoutingDictionary] WHERE [BranchCode] = 'MKSR01'),'PRIOK','SHDPRT',NULL,10),
           ((SELECT [RoutingDictionaryId] FROM [dbo].[RoutingDictionary] WHERE [BranchCode] = 'MKSR01'),'SOETTA','SHARRVL',NULL,11),
           ((SELECT [RoutingDictionaryId] FROM [dbo].[RoutingDictionary] WHERE [BranchCode] = 'MKSR01'),'PDCPTTN','RCI','SC',12),
           ((SELECT [RoutingDictionaryId] FROM [dbo].[RoutingDictionary] WHERE [BranchCode] = 'MKSR01'),'PDCPTTN','DLO',NULL,13),
           ((SELECT [RoutingDictionaryId] FROM [dbo].[RoutingDictionary] WHERE [BranchCode] = 'MKSR01'),'PDCDYU','RCI','SC',14),
           ((SELECT [RoutingDictionaryId] FROM [dbo].[RoutingDictionary] WHERE [BranchCode] = 'MKSR01'),'PDCDYU','SPUIN',NULL,15),
           ((SELECT [RoutingDictionaryId] FROM [dbo].[RoutingDictionary] WHERE [BranchCode] = 'MKSR01'),'PDCDYU','SPUC',NULL,16)
GO

INSERT INTO [dbo].[Vehicle]
           ([Katashiki],[Suffix],[ExteriorColorCode],[InteriorColorCode],[BranchCode],[Responsibility],[PhysicalLocationCode],[ProductionMonthYear],[DTPLOD]
           ,[RRN],[FrameNumber],[REVPLOD],[TotalLossAt],[SetUsedAt],[EstimatedPDCIn],[EstimatedPDCOut],[EstimatedArrivalBranch],[EstimatedDeliveryTime]
           ,[RequestedDeliveryTime],[ActualDeliveryTime],[DECDate],[PaketAksesorisTAM],[EngineNumber],[EnginePrefix],[KeyNumber],[SpecialVehicleSign],[HasCustomer]
		   ,[IsAdvanceUnit],[IsUrgentDeliveryRequest],[IsHold],[IsInAudit],[IsInWorkshop],[IsSentMDPToDMS],[IsSentRevisedPDDToDMS],[IsSentDeliveryInfoToDMS],[IsSentBoardPDSToDMS])
     VALUES
           ('K1','S1','12E1','12I1','MKSR01',NULL,'ADMSTR',GETDATE(),GETDATE(),'RRN01','MHFGB8EM0G0408581',GETDATE(),NULL,NULL,NULL,NULL,NULL
           ,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,0,0,0,0,0,0,0,0)
GO

