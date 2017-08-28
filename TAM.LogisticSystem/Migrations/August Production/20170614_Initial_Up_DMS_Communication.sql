--=====================================================--
--untuk dijalankan di Server kita dan di server Vendor --
--=====================================================--

if (select object_id('CVehicle_StockViewMDP_TM_Staging')) is not null
drop table [CVehicle_StockViewMDP_TM_Staging]
GO
CREATE TABLE [dbo].[CVehicle_StockViewMDP_TM_Staging](
	[StockViewMDPId] uniqueidentifier PRIMARY KEY DEFAULT newid(),
	[OutletCode] [varchar](9) NULL,
	[RRNNo] [varchar](20) NULL,
	[KatashikiCode] [varchar](20) NULL,
	[Suffix] [varchar](2) NULL,
	[ColorCode] [varchar](4) NULL,
	[PLOD] [datetime] NULL,
	[Status] INT null, -- null:blank 0:ReadyToCopy 1:Coppied -1:FailedToCopy 2:Processed -2:FailedToProcess 
) ON [PRIMARY]

GO 
  

if (select object_id('CVehicle_RevisiPDD_TM_Staging')) is not null
drop table CVehicle_RevisiPDD_TM_Staging
GO
CREATE TABLE [dbo].[CVehicle_RevisiPDD_TM_Staging](
	[RevisiPDDId]   uniqueidentifier PRIMARY KEY DEFAULT newid(),
	[RRNNo] [varchar](20) NULL,
	[PLOD] [datetime] NULL,
	[FrameNo] [varchar](20) NULL,
	[EarliestPDD] [datetime] NULL,
	[EstimateBranchReceivedDate] [datetime] NULL,
	[ActualLocation] [varchar](20) NULL,
	[Status] INT null, -- null:blank 0:ReadyToCopy 1:Coppied -1:FailedToCopy 2:Processed -2:FailedToProcess 
  ) ON [PRIMARY]

GO
 
 
if (select object_id('SDR_DeliveryInfoFromTLS_TT_Staging')) is not null
drop table SDR_DeliveryInfoFromTLS_TT_Staging
GO 

CREATE TABLE [dbo].[SDR_DeliveryInfoFromTLS_TT_Staging](
	[DeliveryInfoFromTLSId]uniqueidentifier PRIMARY KEY DEFAULT newid(),
	[RRNNo] [varchar](20) NULL,
	[PLOD] [datetime] NULL,
	[FrameNo] [varchar](20) NULL,
	[ActualPDD] [datetime] NULL,
	[EstimatePersiapanPDC] [datetime] NULL,
	[EstimateBranchReceivedDate] [datetime] NULL,
	[ActualLocation] [varchar](20) NULL,
	[Status] INT null, -- null:blank 0:ReadyToCopy 1:Coppied -1:FailedToCopy 2:Processed -2:FailedToProcess 
) ON [PRIMARY]

GO 


if (select object_id('SRegistration_FakturTAMKeCAO_TM_Staging')) is not null
drop table SRegistration_FakturTAMKeCAO_TM_Staging
GO  
CREATE TABLE [dbo].[SRegistration_FakturTAMKeCAO_TM_Staging](
	[FakturTAMKeCAOId]uniqueidentifier PRIMARY KEY DEFAULT newid(),
	[TanggalTAMDistribusiDokumen] [datetime] NULL,
	[NoSuratDistribusi] [varchar](20) NULL,
	[TanggalSuratDistribusi] [datetime] NULL,
	[QtyFrameNo] [int] NULL,
	[FrameNo] [varchar](20) NULL,
	[NoFakturTAM] [varchar](22) NULL,
	[StatusGesek] [bit] NULL,
	[StatusCode] [int] NOT NULL CONSTRAINT [DF__SRegistra__Statu__1281DC21]  DEFAULT ((1)),
	[LastModifiedDate] [datetime] NOT NULL CONSTRAINT [DF__SRegistra__LastM__23AC6823]  DEFAULT (getdate()),
	[LastModifiedUserId] [int] NOT NULL CONSTRAINT [DF__SRegistra__LastM__34D6F425]  DEFAULT ((1)),
 	[Status] INT null, -- null:blank 0:ReadyToCopy 1:Coppied -1:FailedToCopy 2:Processed -2:FailedToProcess 
) ON [PRIMARY]

GO

 
if (select object_id('SRegistration_ReceiveSTNKBPKB_TT_Staging')) is not null
drop table SRegistration_ReceiveSTNKBPKB_TT_Staging
GO  

CREATE TABLE [dbo].SRegistration_ReceiveSTNKBPKB_TT_Staging(
	ReceiveSTNKBPKBId uniqueidentifier PRIMARY KEY DEFAULT newid(),
	FrameNo varchar(20)   null,
	DocumentReceiveDate datetime null,
	STNKRequestDate datetime null, 
	STNKReceiveDate datetime null,
 	[Status] INT null, -- null:blank 0:ReadyToCopy 1:Coppied -1:FailedToCopy 2:Processed -2:FailedToProcess 
) ON [PRIMARY]

 
if (select object_id('SPDD_PDDVersusADDReport_TT_Staging')) IS NOT NULL
drop table SPDD_PDDVersusADDReport_TT_Staging
GO  

CREATE TABLE [dbo].SPDD_PDDVersusADDReport_TT_Staging(
	PDDVersusADDReportId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	FrameNo VARCHAR(20) NULL,
	ETACustomer DATETIMEOFFSET NULL,
	DECDate DATETIMEOFFSET NULL,  
 	[Status] SMALLINT NULL, -- null:blank 0:ReadyToCopy 1:Coppied -1:FailedToCopy 2:Processed -2:FailedToProcess 
) ON [PRIMARY]


if (select object_id('SentGRReceive_TT_Staging')) IS NOT NULL
drop table SentGRReceive_TT_Staging
GO  

CREATE TABLE [dbo].SentGRReceive_TT_Staging(
	SentGRReceiveId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	FrameNo VARCHAR(20) NULL,
	GRDate DATETIMEOFFSET NULL,
	[Status] SMALLINT NULL, -- null:blank 0:ReadyToCopy 1:Coppied -1:FailedToCopy 2:Processed -2:FailedToProcess 
) ON [PRIMARY]


if (select object_id('Cluster_TM_Staging')) IS NOT NULL
drop table Cluster_TM_Staging
GO  

CREATE TABLE [dbo].Cluster_TM_Staging(
	ClusterCode VARCHAR(16) PRIMARY KEY,
	[Name] VARCHAR(255) NULL,
	[Status] SMALLINT NULL, -- null:blank 0:ReadyToCopy 1:Coppied -1:FailedToCopy 2:Processed -2:FailedToProcess 
) ON [PRIMARY]


if (select object_id('AFIRegion_TM_Staging')) IS NOT NULL
drop table AFIRegion_TM_Staging
GO  

CREATE TABLE [dbo].AFIRegion_TM_Staging(
	AFIRegionCode VARCHAR(4) PRIMARY KEY,
	[Name] VARCHAR(255) NULL,
	[PostalCode] VARCHAR (8) NULL,
	[Kota] VARCHAR(255) NULL,
	[Status] SMALLINT NULL, -- null:blank 0:ReadyToCopy 1:Coppied -1:FailedToCopy 2:Processed -2:FailedToProcess 
) ON [PRIMARY]


if (select object_id('AFICarType_TM_Staging')) IS NOT NULL
drop table AFICarType_TM_Staging
GO  

CREATE TABLE [dbo].AFICarType_TM_Staging(
	AFICarTypeCode VARCHAR(16) PRIMARY KEY,
	[Jenis] VARCHAR (32) NULL,
	[Model] VARCHAR (32) NULL,
	[Status] SMALLINT NULL, -- null:blank 0:ReadyToCopy 1:Coppied -1:FailedToCopy 2:Processed -2:FailedToProcess 
) ON [PRIMARY]


if (select object_id('AFISubmissionType_TM_Staging')) IS NOT NULL
drop table AFISubmissionType_TM_Staging
GO  

CREATE TABLE [dbo].AFISubmissionType_TM_Staging(
	AFISubmissionTypeId INT PRIMARY KEY,
	[Name] VARCHAR (16) NULL,
	[Status] SMALLINT NULL, -- null:blank 0:ReadyToCopy 1:Coppied -1:FailedToCopy 2:Processed -2:FailedToProcess 
) ON [PRIMARY]

if (select object_id('RequestInfoAFISTNK_TR_Staging')) IS NOT NULL
drop table RequestInfoAFISTNK_TR_Staging
GO  

CREATE TABLE [dbo].RequestInfoAFISTNK_TR_Staging(
	FrameNumber VARCHAR(20) PRIMARY KEY,
	DocumentReceivedAt DATETIMEOFFSET NULL,
	STNKAjuAt DATETIMEOFFSET NULL,
	STNKReceivedAt DATETIMEOFFSET NULL,
	[Status] SMALLINT NULL, -- null:blank 0:ReadyToCopy 1:Coppied -1:FailedToCopy 2:Processed -2:FailedToProcess 
) ON [PRIMARY]

if (select object_id('NomorFakturPajak_TR_Staging')) IS NOT NULL
drop table NomorFakturPajak_TR_Staging
GO  

CREATE TABLE [dbo].NomorFakturPajak_TR_Staging(
	NomorFakturPajakId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	NomorFakturPajak VARCHAR(20) NULL,
	DebitAdviceNumber VARCHAR(16) NULL,
	[Status] SMALLINT NULL, -- null:blank 0:ReadyToCopy 1:Coppied -1:FailedToCopy 2:Processed -2:FailedToProcess 
) ON [PRIMARY]


if (select object_id('SentMengajukanAFI_TR_Staging')) IS NOT NULL
drop table SentMengajukanAFI_TR_Staging
GO  

CREATE TABLE [dbo].SentMengajukanAFI_TR_Staging(
	SentMengajukanAFIId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	AFIStatus INT NULL,	-- 1: Aju Baru, 2: Revisi AFI, 3: Batal AFI, 4: Aju Eks Batal
	ReferenceNumber	VARCHAR(22) NULL,
	RevisionCode INT NULL,
	Jenis VARCHAR(30) NULL,
	Model VARCHAR(30) NULL,
	Warna VARCHAR(4) NULL,
	FrameNumber VARCHAR(20) NULL,
	NamaCustomer VARCHAR(30) NULL,
	Address1 VARCHAR(30) NULL,
	Address2 VARCHAR(30) NULL,
	Address3 VARCHAR(30) NULL,
	Province VARCHAR(50) NULL,
	AFIRegionCode VARCHAR(4) NULL,
	Kota VARCHAR(50) NULL,
	PostalCode VARCHAR(10) NULL,
	KTP VARCHAR(20) NULL,
	[Timestamp] DATETIMEOFFSET NULL,
	[EffectiveUntil] DATETIMEOFFSET NULL,
	[Status] SMALLINT NULL, -- null:blank 0:ReadyToCopy 1:Coppied -1:FailedToCopy 2:Processed -2:FailedToProcess 
) ON [PRIMARY]