CREATE TABLE Blob
(
	BlobId UNIQUEIDENTIFIER 
		CONSTRAINT PK_Blob_BlobId PRIMARY KEY,
	[Path] VARCHAR(255) NOT NULL
		CONSTRAINT UQ_Blob_Path UNIQUE,
	Content VARBINARY(MAX) NOT NULL, -- enable FILESTREAM in ConfigurationManager

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-----------------------------------------------------------------
-- Car Master
-----------------------------------------------------------------

CREATE TABLE Plant
(
	PlantCode VARCHAR(8) 
		CONSTRAINT PK_Plant PRIMARY KEY, -- ADM, HMI, TMMIN
	[Name] VARCHAR(255) NOT NULL,		
	[Country] VARCHAR(255) NOT NULL,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Brand
(
	BrandCode VARCHAR(16) 
		CONSTRAINT PK_Brand PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,		-- Toyota, Daihatsu, Mitsubishi, gitu"

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE CarModel
(
	CarModelCode VARCHAR(8) 											-- FORT, CLYA, UIMV, YRIS, AGYA, RUSH
		CONSTRAINT PK_CarModel PRIMARY KEY,			
	BrandCode VARCHAR(16) NULL 											-- NULLABLE for initial data
		CONSTRAINT FK_CarModel_Brand FOREIGN KEY REFERENCES Brand ON DELETE SET NULL,	
	PlantCode VARCHAR(8) NULL											-- NULLABLE for initial data
		CONSTRAINT FK_CarModel_Plant FOREIGN KEY REFERENCES Plant ON DELETE SET NULL,	

	[Name] VARCHAR(255) NOT NULL, -- e.g. Fortuner, Avanza

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE CarSeries
(
	CarSeriesCode VARCHAR(16) 
		CONSTRAINT PK_CarSeries PRIMARY KEY,	-- FORTB, FORTD ????
	CarModelCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_CarSeries_CarModel FOREIGN KEY REFERENCES CarModel, 
	
	[Name] VARCHAR(255) NOT NULL, -- FORT Diesel, Fort Bensin

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- Formerly CarCategory
CREATE TABLE AFICarType
(
	AFICarTypeCode VARCHAR(16) 
		CONSTRAINT PK_AFICarType PRIMARY KEY,

	[Jenis] VARCHAR(32) NOT NULL,	-- e.g. Mobil Barang, Mobil Penumpang, etc
	[Model] VARCHAR(32) NOT NULL,	-- e.g. Minibus, Chassis, Pick-up, Double Cabin, etc

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE CarType
(
	Katashiki VARCHAR(32) NOT NULL, -- B100RA-GMQFJ, NGX50L-AHXGXX, F653RM-GMMFJ
	Suffix VARCHAR(4) NOT NULL, -- 11, 62, 00
		CONSTRAINT PK_CarType PRIMARY KEY (Katashiki, Suffix), -- Natural Superkey that is unique for each Katashiki / Model defined by TAM.

	CarSeriesCode VARCHAR(16) NULL												-- NULLABLE for initial data
		CONSTRAINT FK_CarType_CarSeries FOREIGN KEY REFERENCES CarSeries,	
	AFICarTypeCode VARCHAR(16) NULL												-- NULLABLE for initial data
		CONSTRAINT FK_CarType_AFICarType FOREIGN KEY REFERENCES AFICarType,	

	[Name] VARCHAR(255) NOT NULL, 				-- e.g. FORTUNER SRZ A/T
	EngineDescription VARCHAR(255) NOT NULL,	-- Bensin/4 Silinder Sejajar
	EngineVolume VARCHAR(8) NOT NULL,			-- 1496
	SteerPosition CHAR(1) NOT NULL,				-- L | R
	WheelDiameter VARCHAR(8) NOT NULL,			-- 2750
	WheelSize VARCHAR(32) NOT NULL,				-- 185/60 R15
	[Assembly] VARCHAR(8) NULL,					-- NULLABLE, contoh data : CBU
	
	IsFreeTaxZone BIT NOT NULL DEFAULT 0,	-- Dipakai apabila Katsu ini dipakai untuk batam

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-----------------------------------------------------------------
-- Car Extra
-----------------------------------------------------------------

CREATE TABLE ExteriorColor
(
	ExteriorColorCode VARCHAR(4) CONSTRAINT PK_ExteriorColor PRIMARY KEY, -- e.g. 4R8
	[IndonesianName] VARCHAR(255) NOT NULL, -- e.g. Putih
	[EnglishName] VARCHAR(255) NOT NULL, -- e.g. Super White

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE InteriorColor
(
	InteriorColorCode VARCHAR(4) CONSTRAINT PK_InteriorColor PRIMARY KEY, -- e.g. 4R8
	[IndonesianName] VARCHAR(255) NOT NULL, -- e.g. Putih
	[EnglishName] VARCHAR(255) NOT NULL, -- e.g. Super White

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- Formerly FrameNumberMaster
-- Todo : NULL on FrameNumber
CREATE TABLE AS400FrameNumber
(
	FrameNumber VARCHAR(32) 
		CONSTRAINT PK_AS400FrameNumber PRIMARY KEY,

	IdNumber INT NULL,					-- NULLABLE
	RRN VARCHAR(8) NULL,				-- NULLABLE,
	
	Katashiki VARCHAR(32) NULL,			-- NULLABLE
	Suffix VARCHAR(4) NULL,				-- NULLABLE
		CONSTRAINT FK_AS400FrameNumber_CarType FOREIGN KEY(Katashiki, Suffix) REFERENCES CarType ON DELETE SET NULL,

	ExteriorColorCode VARCHAR(4) NULL	-- NULLABLE
		CONSTRAINT FK_AS400FrameNumber_ExteriorColor FOREIGN KEY REFERENCES ExteriorColor ON DELETE SET NULL,	
	DTPLOD DATETIMEOFFSET NULL,				-- NULLABLE
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE EngineMaster
(
	EngineNumber VARCHAR(32) PRIMARY KEY,
	EnginePrefix VARCHAR(8) NOT NULL,
	StatusEngine INT NULL,	-- NULLABLE, Buat TMMIN
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-----------------------------------------------------------------
-- Location
-----------------------------------------------------------------

-- Formerly CostingCluster
CREATE TABLE AS400Cluster
(
	AS400ClusterCode VARCHAR(16) 
		CONSTRAINT PK_AS400Cluster PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE CityForShipment
(
	CityForShipmentCode VARCHAR(16) 
		CONSTRAINT PK_CityForShipment PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE CityForLeg
(
	CityForLegCode VARCHAR(16)
		CONSTRAINT PK_CityForLeg PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE AFIRegion
(
	AFIRegionCode VARCHAR(4) CONSTRAINT PK_AFIRegion PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Region
(
	RegionCode VARCHAR(16) 
		CONSTRAINT PK_Region PRIMARY KEY,
	[Type] VARCHAR(4) NOT NULL, -- KEL
	[Name] VARCHAR(255) NOT NULL,
	
	ParentRegionCode VARCHAR(16) NULL -- NULLABLE
		CONSTRAINT FK_Region_ParentRegion FOREIGN KEY REFERENCES Region,
	PostCode VARCHAR(8) NOT NULL,

	AFIRegionCode VARCHAR(4) FOREIGN KEY REFERENCES AFIRegion NULL,	-- NULLABLE

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE LocationType
(
	LocationTypeCode VARCHAR(8) -- CY, BRCH, DD, HO, PDCR, PDCN, PR, Karoseri, BP
		CONSTRAINT PK_LocationType PRIMARY KEY, 
	[Name] VARCHAR(255) NOT NULL,

	HasResponsibility BIT NOT NULL DEFAULT 1, -- If TRUE, then this location has responsibility over the vehicle received on this location
	NeedSJKBTarikan BIT NOT NULL DEFAULT 0,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE [Location]
(
	LocationCode VARCHAR(8) 
		CONSTRAINT PK_Location PRIMARY KEY, -- Dari TLS lama, panjangnya 7
	
	[Name] VARCHAR(255) NOT NULL,
	[Address] VARCHAR(255) NULL,	-- Nullable
	
	LocationTypeCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_Location_LocationType FOREIGN KEY REFERENCES LocationType,
	CityForLegCode VARCHAR(16) NULL -- NULLABLE, for initial release because of different sources of master
		CONSTRAINT FK_Location_CityForLeg FOREIGN KEY REFERENCES CityForLeg,
	CityForShipmentCode VARCHAR(16) NULL -- NULLABLE, for initial release because of different sources of master
		CONSTRAINT FK_Location_CityForShipment FOREIGN KEY REFERENCES CityForShipment,
		
	CanPrintSJKB BIT NOT NULL DEFAULT 0,	

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE [Destination]
(
	DestinationCode VARCHAR(16) 
		CONSTRAINT PK_Destination PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE SalesArea
(
	SalesAreaCode VARCHAR(16) 
		CONSTRAINT PK_SalesArea PRIMARY KEY,
	[Description] VARCHAR(255) NOT NULL,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-----------------------------------------------------------------
-- Organization Structure
-----------------------------------------------------------------

CREATE TABLE DealerType
(
	DealerTypeCode VARCHAR(16) 
		CONSTRAINT PK_DealerType PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL, -- Founder AI, Founder Non AI, Ex Indirect AI, Ex Indirect Non AI
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Dealer
(
	DealerCode VARCHAR(16) 
		CONSTRAINT PK_Dealer PRIMARY KEY,
    MasterDataPrimaryKey INT NOT NULL
		CONSTRAINT UQ_Dealer_MDMPK UNIQUE,

	DealerTypeCode VARCHAR(16) NULL
		CONSTRAINT FK_Dealer_DealerType FOREIGN KEY REFERENCES DealerType,	-- NULLABLE, for initial data

	[Name] VARCHAR(255) NOT NULL,
	[Address] VARCHAR(255) NULL,	-- NULLABLE

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Company
(
	CompanyCode VARCHAR(16) 
		CONSTRAINT PK_Company PRIMARY KEY,
    MasterDataPrimaryKey INT NOT NULL
		CONSTRAINT UQ_Company_MDMPK UNIQUE,
	DealerCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_Company_Dealer FOREIGN KEY REFERENCES Dealer,

	[Name] VARCHAR(255) NOT NULL,
	[NPWPAddress] VARCHAR(255) NULL,    -- NULLABLE
	[SAPCode] VARCHAR(8) NULL,	        -- NULLABLE, for initial data
	[Phone] VARCHAR(32) NULL,           -- NULLABLE
	[Fax] VARCHAR(32) NULL,             -- NULLABLE
	[Email] VARCHAR(255) NULL,          -- NULLABLE
	[TradeName] VARCHAR(255) NULL,      -- NULLABLE
	[NPWP] VARCHAR(32) NULL,            -- NULLABLE
	[TermOfPaymentDay] INT NULL,	    -- NULLABLE, for initial data

	IsDealerFinancing BIT NOT NULL DEFAULT 0,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Branch
(
	BranchCode VARCHAR(16) 
		CONSTRAINT PK_Branch PRIMARY KEY,
    MasterDataPrimaryKey INT NOT NULL
		CONSTRAINT UQ_Branch_MDMPK UNIQUE,

    CompanyCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_Branch_Company FOREIGN KEY REFERENCES Company, -- Who owns this Branch?

	SalesAreaCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_Branch_SalesArea FOREIGN KEY REFERENCES SalesArea,
	DestinationCode VARCHAR(16) NULL												-- NULLABLE, ga tau gunanya apa, tapi diminta dimasukin dulu sama Pak Yovan + Dasu, Beda dengan branchId diatas	
		CONSTRAINT FK_Branch_Destination FOREIGN KEY REFERENCES [Destination],	   
	RegionCode VARCHAR(16) NULL														-- NULLABLE, for initial data
		CONSTRAINT FK_Branch_Region FOREIGN KEY REFERENCES [Region],	                
	
	AS400ClusterCode VARCHAR(16) NULL												-- NULLABLE, for initial data
		CONSTRAINT FK_Branch_AS400Cluster FOREIGN KEY REFERENCES [AS400Cluster],	
	AS400BranchCode VARCHAR(16) NULL,												-- NULLABLE, for initial data

	[Name] VARCHAR(255) NOT NULL,       -- e.g. AUTO2000
	[Phone] VARCHAR(32) NULL,           -- NULLABLE
	[Fax] VARCHAR(32) NULL,             -- NULLABLE
	[KabupatenCode] VARCHAR(16) NULL,   -- NULLABLE, for initial data

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE UNIQUE NONCLUSTERED INDEX UQ_Branch_AS400BranchCode ON Branch(AS400BranchCode) WHERE AS400BranchCode IS NOT NULL

CREATE TABLE BranchLocationMapping
(
    BranchLocationMappingId INT PRIMARY KEY IDENTITY, 
    BranchCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_BranchLocationMapping_Branch FOREIGN KEY REFERENCES Branch
		CONSTRAINT UQ_BranchLocationMapping_BranchCode UNIQUE,
    LocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_BranchLocationMapping_Location FOREIGN KEY REFERENCES [Location] 
		CONSTRAINT UQ_BranchLocationMapping_LocationCode UNIQUE,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Delivery Leg
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Shift]
(
	ShiftCode VARCHAR(16)
		CONSTRAINT PK_Shift PRIMARY KEY,
	[Description] VARCHAR(255),
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE CityLeg
(
	CityLegCode VARCHAR(16)
		CONSTRAINT PK_CityLeg PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,

	CityFrom VARCHAR(16) NOT NULL
		CONSTRAINT FK_CityLeg_CityFrom FOREIGN KEY REFERENCES CityForLeg,
	CityTo VARCHAR(16) NOT NULL
		CONSTRAINT FK_CityLeg_CityTo FOREIGN KEY REFERENCES CityForLeg,
	
	CONSTRAINT UQ_CityLeg_Mapping UNIQUE (CityFrom, CityTo),

	CalculatingSwappingCost BIT NOT NULL DEFAULT 0,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)
-- TODO: Ask CalculatingSwappingCost?

CREATE TABLE DeliveryLeg
(
	DeliveryLegCode VARCHAR(16)
		CONSTRAINT PK_DeliveryLeg PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,

	LocationFrom VARCHAR(8) NOT NULL
		CONSTRAINT FK_DeliveryLeg_LocationFrom FOREIGN KEY REFERENCES Location,
	LocationTo VARCHAR(8) NOT NULL
		CONSTRAINT FK_DeliveryLeg_LocationTo FOREIGN KEY REFERENCES Location,

	CONSTRAINT UQ_DeliveryLeg_Mapping UNIQUE (LocationFrom, LocationTo),

	-- 1 City Leg Code Banyak Delivery Leg, City Leg Code = Grouping buat beberapa Delivery Leg pake 1 harga
	CityLegCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_DeliveryLeg_CityLeg FOREIGN KEY REFERENCES CityLeg,

	BufferMinutes INT NOT NULL,
	NeedSJKB BIT NOT NULL DEFAULT 1,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryVendor
(
	DeliveryVendorCode VARCHAR(16)
		CONSTRAINT PK_DeliveryVendor PRIMARY KEY,

	[Name] VARCHAR(255) NOT NULL,
	[Address] VARCHAR(255) NULL,	-- NULLABLE
	[SAPCode] VARCHAR(8) NULL,		-- NULLABLE
	[Account] VARCHAR(16) NULL,		-- NULLABLE
	
	[LocationCode] VARCHAR(8) NOT NULL
		CONSTRAINT FK_DeliveryVendor_Location FOREIGN KEY REFERENCES [Location],
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryMethod
(
	DeliveryMethodCode VARCHAR(16)
		CONSTRAINT PK_DeliveryMethod PRIMARY KEY,	-- CC, SC, SD, SH (tpi takutnya ad yg 3-4 digit, jd buffer aja). SH = Shipment
	[Name] VARCHAR(255) NOT NULL,

	ParentDeliveryMethodCode VARCHAR(16)
		CONSTRAINT FK_DeliveryMethod_ParentDeliveryMethod FOREIGN KEY REFERENCES DeliveryMethod NULL,	-- NULLABLE, CC Single trip parent = CC
	NeedSJKBValidation BIT NOT NULL DEFAULT 0,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE CityLegCost
(
	CityLegCostCode VARCHAR(16)
		CONSTRAINT PK_CityLegCost PRIMARY KEY,

	CityLegCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_CityLegCost_CityLeg FOREIGN KEY REFERENCES CityLeg ON DELETE CASCADE,
	DeliveryVendorCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_CityLegCost_DeliveryVendor FOREIGN KEY REFERENCES DeliveryVendor,
	DeliveryMethodCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_CityLegCost_DeliveryMethod FOREIGN KEY REFERENCES DeliveryMethod,
	
	-- Parameter A
	CarSeriesCode VARCHAR(16) NULL
		CONSTRAINT FK_CityLegCost_CarSeries FOREIGN KEY REFERENCES CarSeries ON DELETE SET NULL,	-- NULLABLE, read below
	
	-- Parameter B
	[ShiftCode] VARCHAR(16) NULL
		CONSTRAINT FK_CityLegCost_Shift FOREIGN KEY REFERENCES [Shift] ON DELETE SET NULL, 			-- NULLABLE, read below
	[Capacity] INT NOT NULL DEFAULT 1,																			-- NULLABLE, read below

	ValidFrom DATETIMEOFFSET NOT NULL,
	
	-- A certain CityLeg with certain DeliveryVendor has additional CityLegCost
	NeedAdditionalCityLegCostCode VARCHAR(16) FOREIGN KEY REFERENCES CityLegCost NULL,	-- Nullable
	
	-- Result
	Currency VARCHAR(16) NOT NULL,
	Nominal DECIMAL(19,4) NOT NULL,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- TODO: mengapa capacity bisa jadi lookup yah?

/*
Delivery Leg untuk dapet lead time
City Leg untuk dapet cost

Ada kejadian pada 1 Delivery Leg, dengan vendor tertentu, dibelah menjadi 2 Delivery Leg

-- Removed: Karena sudah dibelah
	CityLegCost.NeedAdditionalCityLegCostCode VARCHAR(16) FOREIGN KEY REFERENCES CityLegCost NULL,	-- Nullable. A certain CityLeg with certain DeliveryVendor must create additional Additional.

Delivery method pasti 10. No more, no less.

IF code = SCU / CCU / SD, lookup by CarSeriesCode
IF code = SCST / SCRT / CCST / CCRT, lookup by ShiftCode and Capacity

function(CityLegCode, DeliveryVendorCode, DeliveryMethodCode, [parameters], [time]) => Currency + Nominal
*/

CREATE TABLE DeliveryLeadTime
(
	DeliveryLeadTimeId INT
		CONSTRAINT PK_DeliveryLeadTime PRIMARY KEY IDENTITY,

	DeliveryLegCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_DeliveryLeadTime_DeliveryLeg FOREIGN KEY REFERENCES DeliveryLeg ON DELETE CASCADE,
	DeliveryMethodCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_DeliveryLeadTime_DeliveryMethod FOREIGN KEY REFERENCES DeliveryMethod,
	CONSTRAINT UQ_DeliveryLeadTime_Mapping UNIQUE(DeliveryLegCode, DeliveryMethodCode),

	LeadMinutes INT NOT NULL,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryVendorVehicle
(
	DeliveryVendorVehicleId INT
		CONSTRAINT PK_DeliveryVendorVehicle PRIMARY KEY IDENTITY,
	
	DeliveryVendorCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_DeliveryVendorVehicle_DeliveryVendor FOREIGN KEY REFERENCES DeliveryVendor ON DELETE CASCADE,
	DeliveryMethodCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_DeliveryVendorVehicle_DeliveryMethod FOREIGN KEY REFERENCES DeliveryMethod,

	[PoliceNumberOrVesselName] VARCHAR(255) NOT NULL,
	[Capacity] INT NOT NULL,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- TODO: Confirm driver sama ato nggak kalo SIM beda
CREATE TABLE DeliveryDriver
(
	DeliveryDriverCode VARCHAR(16)
		CONSTRAINT PK_DeliveryDriver PRIMARY KEY,
	
	DeliveryVendorCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_DeliveryDriver_DeliveryVendor FOREIGN KEY REFERENCES DeliveryVendor ON DELETE CASCADE,
	DeliveryMethodCode VARCHAR(16)
		CONSTRAINT FK_DeliveryDriver_DeliveryMethod FOREIGN KEY REFERENCES DeliveryMethod NOT NULL,

	[Name] VARCHAR(255) NOT NULL,

	[HasSIMA] BIT NOT NULL DEFAULT 0,
	[HasSIMB] BIT NOT NULL DEFAULT 0,
	
	[SIMANumber] VARCHAR(32) NOT NULL,
	[SIMAExpiration] DATE NOT NULL,

	[SIMBNumber] VARCHAR(32) NOT NULL,
	[SIMBExpiration] DATE NOT NULL,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Process
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE ProcessLeadTimeByEnum
(
	ProcessLeadTimeByEnumId INT
		CONSTRAINT PK_ProcessLeadTimeByEnum PRIMARY KEY,
	[Name] VARCHAR(32) NOT NULL,    -- PDI, PIO, SPU, Location, Kapal, Leg, Dwell, Nol

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE ProcessMaster
(
	ProcessMasterCode VARCHAR(8)
		CONSTRAINT PK_ProcessMaster PRIMARY KEY,	-- Can be anything, decided by Accelist. Nanti di C#: if CODE == ... then ...
	[Name] VARCHAR(255) NOT NULL,	            -- PIO In, PIO Out

	ProcessLeadTimeByEnumId INT
		CONSTRAINT FK_ProcessMaster_ProcessLeadTimeByEnum FOREIGN KEY REFERENCES ProcessLeadTimeByEnum NOT NULL,

    IsScan BIT NOT NULL,
	BufferMinutes INT NOT NULL,
	SwappingPoint BIT NOT NULL DEFAULT 0,	-- Flag to indicate whether this routing need its pair

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)
-- Do* columns are deprecated in favor of application logic

CREATE TABLE ProcessHeadTemplate
(
	ProcessHeadTemplateCode VARCHAR(16)
		CONSTRAINT PK_ProcessHeadTemplate PRIMARY KEY,
	[Description] VARCHAR(255) NULL, -- NULLABLE

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE ProcessHeadTemplateDetail
(
	ProcessHeadTemplateCode VARCHAR(16) NOT NULL 
		CONSTRAINT FK_ProcessHeadTemplateDetail_Header FOREIGN KEY REFERENCES ProcessHeadTemplate ON DELETE CASCADE,
	Ordering INT NOT NULL,
	CONSTRAINT PK_ProcessHeadTemplateDetail PRIMARY KEY (ProcessHeadTemplateCode, Ordering),
	
	ProcessMasterCode VARCHAR(8)
		CONSTRAINT FK_ProcessHeadTemplateDetail_ProcessMaster FOREIGN KEY REFERENCES ProcessMaster NOT NULL,
	DeliveryMethodCode VARCHAR(16)
		CONSTRAINT FK_ProcessHeadTemplateDetail_DeliveryMethod FOREIGN KEY REFERENCES DeliveryMethod NULL,	-- Nullable
	LocationCode VARCHAR(8)
		CONSTRAINT FK_ProcessHeadTemplateDetail_Location FOREIGN KEY REFERENCES [Location] NOT NULL,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE ProcessHeadTemplateMapping
(
    ProcessHeadTemplateMappingId INT
		CONSTRAINT PK_ProcessHeadTemplateMapping PRIMARY KEY IDENTITY,

    ProcessHeadTemplateCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_ProcessHeadTemplateMapping_ProcessHeadTemplate FOREIGN KEY REFERENCES ProcessHeadTemplate ON DELETE CASCADE,
    Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
		CONSTRAINT FK_ProcessHeadTemplateMapping_CarType FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType ON DELETE CASCADE,
    	
	CONSTRAINT UQ_ProcessHeadTemplateMapping_Mapping UNIQUE(ProcessHeadTemplateCode, Katashiki, Suffix),

    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE ProcessTailTemplate
(
	ProcessTailTemplateCode VARCHAR(16)
		CONSTRAINT PK_ProcessTailTemplate PRIMARY KEY,
	[Description] VARCHAR(255) NULL, -- NULLABLE

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE ProcessTailTemplateDetail
(
	ProcessTailTemplateCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_ProcessTailTemplateDetail_Header FOREIGN KEY REFERENCES ProcessTailTemplate ON DELETE CASCADE,
	Ordering INT NOT NULL,
	CONSTRAINT PK_ProcessTailTemplateDetail PRIMARY KEY (ProcessTailTemplateCode, Ordering),
	
	ProcessMasterCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_ProcessTailTemplateDetail_ProcessMaster FOREIGN KEY REFERENCES ProcessMaster,
	DeliveryMethodCode VARCHAR(16) NULL
		CONSTRAINT FK_ProcessTailTemplateDetail_DeliveryMethod FOREIGN KEY REFERENCES DeliveryMethod,	-- Nullable
	LocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_ProcessTailTemplateDetail_Location FOREIGN KEY REFERENCES [Location],
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE ProcessTailTemplateMapping
(
    ProcessTailTemplateMappingId INT
		CONSTRAINT PK_ProcessTailTemplateMapping PRIMARY KEY IDENTITY,
    
	ProcessTailTemplateCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_ProcessTailTemplateMapping_ProcessTailTemplate FOREIGN KEY REFERENCES ProcessTailTemplate ON DELETE CASCADE,
    Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
		CONSTRAINT FK_ProcessTailTemplateMapping_CarType FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType ON DELETE CASCADE,
    BranchCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_ProcessTailTemplateMapping_Branch FOREIGN KEY REFERENCES Branch ON DELETE CASCADE,

    CONSTRAINT UQ_ProcessTailTemplateMapping_Mapping UNIQUE(ProcessTailTemplateCode, Katashiki, Suffix, BranchCode),

    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE ProcessDictionary
(
	ProcessDictionaryId INT
		CONSTRAINT PK_ProcessDictionary PRIMARY KEY IDENTITY,
	
	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
		CONSTRAINT FK_ProcessDictionary_CarType FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType,
	BranchCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_ProcessDictionary_Branch FOREIGN KEY REFERENCES Branch,
	ValidFrom DATE NOT NULL,
    
	CONSTRAINT UQ_ProcessDictionary_Mapping UNIQUE(Katashiki, Suffix, BranchCode, ValidFrom),
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE ProcessDictionaryDetail
(
	ProcessDictionaryId INT NOT NULL
		CONSTRAINT FK_ProcessDictionaryDetail_Header FOREIGN KEY REFERENCES ProcessDictionary ON DELETE CASCADE,
	Ordering INT NOT NULL,
    CONSTRAINT PK_ProcessDictionaryDetail PRIMARY KEY (ProcessDictionaryId, Ordering),

    ProcessMasterCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_ProcessDictionaryDetail_ProcessMaster FOREIGN KEY REFERENCES ProcessMaster,
	DeliveryMethodCode VARCHAR(16) NULL
		CONSTRAINT FK_ProcessDictionaryDetail_DeliveryMethod FOREIGN KEY REFERENCES DeliveryMethod,	-- Nullable
	LocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_ProcessDictionaryDetail_Location FOREIGN KEY REFERENCES [Location],
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Process Extra
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE ProcessLeadTimeForLocation
(
	LocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_ProcessLeadTimeForLocation_Location FOREIGN KEY REFERENCES Location ON DELETE CASCADE,
	ProcessMasterCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_ProcessLeadTimeForLocation_ProcessMaster FOREIGN KEY REFERENCES ProcessMaster ON DELETE CASCADE,
	CONSTRAINT PK_ProcessLeadTimeForLocation PRIMARY KEY (LocationCode, ProcessMasterCode),

	LeadMinutes INT NOT NULL,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Dwelling	-- Waktu Nginap Kendaraan sebelum dan setelah ke/dari Port
(
	LocationFrom VARCHAR(8) NOT NULL
		CONSTRAINT FK_Dwelling_LocationFrom FOREIGN KEY REFERENCES [Location],
	LocationTo VARCHAR(8) NOT NULL
		CONSTRAINT FK_Dwelling_LocationTo FOREIGN KEY REFERENCES [Location],
	CONSTRAINT PK_Dwelling PRIMARY KEY (LocationFrom, LocationTo),

	LeadMinutes INT NOT NULL,	-- Absolute, Not Affected by Idle Time

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE PreDeliveryCenter
(
	LocationCode VARCHAR(8) NOT NULL
		CONSTRAINT PK_PreDeliveryCenter PRIMARY KEY
		CONSTRAINT FK_PreDeliveryCenter_Location FOREIGN KEY REFERENCES Location ON DELETE CASCADE,

	MaintenanceDay INT NOT NULL,					-- Mobil masuk, PDC In X hari, PDC Out X+N hari
	LeadPreparationDeliveryMinutes INT NOT NULL,	-- Persiapan yang dibutuhkan mobil untuk keluar dari PDC

	CarCarrierQuotaPerDay INT NOT NULL,			
	NonCarCarrierQuotaPerDay INT NOT NULL,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE PreDeliveryCenterDelivery
(
	LocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_PreDeliveryCenterDelivery_PreDeliveryCenter FOREIGN KEY REFERENCES PreDeliveryCenter ON DELETE CASCADE,
	BranchCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_PreDeliveryCenter_Branch FOREIGN KEY REFERENCES Branch ON DELETE CASCADE,
	CONSTRAINT PK_PreDeliveryCenterDelivery PRIMARY KEY (LocationCode, BranchCode),

	DeliveryMethodCode VARCHAR(16)
		CONSTRAINT FK_PreDeliveryCenterDelivery_DeliveryMethod FOREIGN KEY REFERENCES [DeliveryMethod] NOT NULL,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Vehicle
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Vehicle
(
	VehicleId INT
		CONSTRAINT PK_Vehicle PRIMARY KEY IDENTITY,

	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
		CONSTRAINT FK_Vehicle_CarType FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType,

	ExteriorColorCode VARCHAR(4) NOT NULL
		CONSTRAINT FK_Vehicle_ExteriorColor FOREIGN KEY REFERENCES ExteriorColor,
	InteriorColorCode VARCHAR(4) NOT NULL
		CONSTRAINT FK_Vehicle_InteriorColor FOREIGN KEY REFERENCES InteriorColor,
	
	BranchCode VARCHAR(16) NULL
		CONSTRAINT FK_Vehicle_Branch FOREIGN KEY REFERENCES Branch,					-- NULLABLE, Branch yg kosong itu punya TAM Stock. Dipake untuk DO Manual. Which Branch orders this car?
	Responsibility VARCHAR(8) NULL
		CONSTRAINT FK_Vehicle_Responsibility FOREIGN KEY REFERENCES [Location],		    -- NULLABLE, Where is the current location responsible for this car? Is NULLABLE because when the vehicle first registered PLANT is not responsible, it needs to be scanned on PDI first for the location to be responsible.
	PhysicalLocationCode VARCHAR(8) NULL
		CONSTRAINT FK_Vehicle_PhysicalLocation FOREIGN KEY REFERENCES [Location],		-- NULLABLE, Physical Name of the Vehicle

    DTPLOD DATETIMEOFFSET NOT NULL,      -- DTPLOD = vehicle's birthday (Estimated initial process start time)
	ProductionYear INT NOT NULL,
	RRN VARCHAR(8) NOT NULL,        -- This is like a temporary Frame Number. But not unique and can be reused!
		CONSTRAINT UQ_Vehicle_RRN UNIQUE(ProductionYear, RRN),    -- You can use MDPYear and RRN to query vehicles 
	
	FrameNumber VARCHAR(32) NULL,			-- NULLABLE because filled later

	NomorIndukKendaraan VARCHAR(32) NULL,	-- NULLABLE, Nomor Induk Kendaraan
	EngineNumber VARCHAR(32) NULL,	        -- NULLABLE, For PDI Completion
	EnginePrefix VARCHAR(8) NULL,	        -- NULLABLE, For PDI Completion
	KeyNumber VARCHAR(32) NULL,		        -- NULLABLE, For PDI Completion
    PaketAksesorisTAM VARCHAR(32) NULL,	    -- NULLABLE, kiriman DIO dari DMS. Diminta simpan aj

	REVPLOD DATETIMEOFFSET NULL,			        -- NULLABLE, REVPLOD = Delivery Date. Vehicle's time when finish Assembly Route, used for calculate PDI Completion
	TotalLossAt DATETIMEOFFSET NULL,		        -- NULLABLE: the time when vehicle been written off as total loss due to heavy damage.
	IsClaimLostToInsurance BIT NOT NULL DEFAULT 0, 
    SetUsedAt DATETIMEOFFSET NULL,		        -- NULLABLE: the time when vehicle has been written off as used due to heavy damage.

	EstimatedPDCIn DATETIMEOFFSET NULL,	        -- NULLABLE: the time when PDC Receive, latest vehicle routing from planning
	EstimatedPDCOut DATETIMEOFFSET NULL,     	-- NULLABLE: the time when PDC Out, PDC Receive + PDCConfig.Maintenance Day
	
	EstimatedArrivalBranch DATETIMEOFFSET NULL,	-- NULLABLE, From Planning
	ActualArrivalBranch DATETIMEOFFSET NULL,		-- NULLABLE, GR from DMS	

	RequestedDeliveryTime DATETIMEOFFSET NULL,	-- NULLABLE, Requested PDD from DMS. Harus diatas EstimatedPDCOut
	ActualDeliveryTime DATETIMEOFFSET NULL,		-- NULLABLE, ADD from DMS (ADD vs PDD)
	CustomerReceivedTime DATETIMEOFFSET NULL,	-- NULLABLE, DEC Date from DMS (ADD vs PDD)

	SpecialVehicleSign CHAR(1) NULL,	    -- NULLABLE, Kebutuhan AS400 untuk force match vehicle asli dgn mdp ? Confirm ulang dengan Pak Santo
	HasCustomer BIT NOT NULL DEFAULT 0,

	IsAdvanceUnit BIT NOT NULL DEFAULT 0,	        -- Diflag apabila kendaraan ini sebenernya diplanning untuk MDP hari/bulan setelah"nya namun di hasilkan hari ini
	IsUrgentDeliveryRequest BIT NOT NULL DEFAULT 0,	-- Kalau PDC tempat mesannya melebihi kapasitas untuk hari itu. Maka akan dikirim keesokan harinya
	IsHold BIT NOT NULL DEFAULT 0,
	IsInAudit BIT NOT NULL DEFAULT 0,
	IsInWorkshop BIT NOT NULL DEFAULT 0,

	IsSentMDPToDMS BIT NOT NULL DEFAULT 0,	        -- Sent after dms give back confirmation
	IsSentRevisedPDDToDMS BIT NOT NULL DEFAULT 0,
	IsSentDeliveryInfoToDMS BIT NOT NULL DEFAULT 0,
	IsSentBoardPDSToDMS BIT NOT NULL DEFAULT 0,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- Tidak yakin apakah Frame Number PASTI exist di table AS400FrameNumber.
CREATE UNIQUE NONCLUSTERED INDEX UQ_Vehicle_FrameNumber ON Vehicle(FrameNumber) WHERE FrameNumber IS NOT NULL

CREATE TABLE VehicleRouting
(
	VehicleRoutingId BIGINT
		CONSTRAINT PK_VehicleRouting PRIMARY KEY IDENTITY,

	VehicleId INT NOT NULL
		CONSTRAINT FK_VehicleRouting_Vehicle FOREIGN KEY REFERENCES Vehicle ON DELETE CASCADE,
    Ordering INT NOT NULL,
    CONSTRAINT UQ_VehicleRouting_Sequence UNIQUE (VehicleId, Ordering),
	
    ProcessMasterCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_VehicleRouting_ProcessMaster FOREIGN KEY REFERENCES ProcessMaster,
	DeliveryMethodCode VARCHAR(16) NULL
		CONSTRAINT FK_VehicleRouting_DeliveryMethod FOREIGN KEY REFERENCES DeliveryMethod,	-- Nullable
	LocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_VehicleRouting_Location FOREIGN KEY REFERENCES [Location],
	
	[ShiftCode] VARCHAR(16) NULL
		CONSTRAINT FK_VehicleRouting_Shift FOREIGN KEY REFERENCES [Shift],					-- Nullable
	LineNumber VARCHAR(16) NULL,

	EstimatedTimeInitial DATETIMEOFFSET NOT NULL,	-- Initial process value is copied from DTPLOD. Next process value = previous process value + LeadMinutes
	EstimatedTimeAdjusted DATETIMEOFFSET NOT NULL,
	ScanTime DATETIMEOFFSET NULL,					-- NULLABLE This is the real time when the process is executed. NULL = not yet scanned

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE VehicleHold
(
	VehicleHoldId INT
		CONSTRAINT PK_VehicleHold PRIMARY KEY IDENTITY,

	VehicleId INT NOT NULL
		CONSTRAINT FK_VehicleHold_Vehicle FOREIGN KEY REFERENCES Vehicle ON DELETE CASCADE,
	VehicleRoutingId BIGINT NULL
		CONSTRAINT FK_VehicleHold_VehicleRouting FOREIGN KEY REFERENCES VehicleRouting,		-- NULLABLE, Di hold antara routing apa atau location apa
	LocationCode VARCHAR(8) NULL
		CONSTRAINT FK_VehicleHold_Location FOREIGN KEY REFERENCES Location ON DELETE SET NULL,			-- NULLABLE, Di hold antara routing apa atau location apa

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE PortLocationResponsibility
(
	PortLocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_PortLocationResponsibility_PortLocationCode FOREIGN KEY REFERENCES Location,
	ResponsibleLocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_PortLocationResponsibility_ResponsibleLocationCode FOREIGN KEY REFERENCES Location,
	CONSTRAINT PK_PortLocationResponsibility PRIMARY KEY(PortLocationCode,ResponsibleLocationCode),

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Swapping
(
	SwappingId INT
		CONSTRAINT PK_Swapping PRIMARY KEY IDENTITY,

	VehicleId INT NOT NULL
		CONSTRAINT FK_Swapping_VehicleId FOREIGN KEY REFERENCES Vehicle,
	FromBranch VARCHAR(16) NOT NULL
		CONSTRAINT FK_Swapping_FromBranch FOREIGN KEY REFERENCES Branch,
	ToBranch VARCHAR(16) NOT NULL
		CONSTRAINT FK_Swapping_ToBranch FOREIGN KEY REFERENCES Branch,

	[Cost] DECIMAL(19,4) NOT NULL,
	[EstimatedPDD] DATETIMEOFFSET NOT NULL,
	[IsApproved] BIT NOT NULL DEFAULT 0,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Voyage
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE VoyageStatusEnum
(
	VoyageStatusEnumId INT
		CONSTRAINT PK_VoyageStatusEnum PRIMARY KEY,
	[Name] VARCHAR(32),	-- Prebook, Assign, Loading, Depart, Arrival
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Voyage
(
	VoyageNumber VARCHAR(16)
		CONSTRAINT PK_Voyage PRIMARY KEY,

	DeliveryVendorVehicleId INT NOT NULL
		CONSTRAINT FK_Voyage_DeliveryVendorVehicle FOREIGN KEY REFERENCES DeliveryVendorVehicle,	-- For 2 Dropdown, Only IsShipment
	DepartureLocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_Voyage_DepartureLocation FOREIGN KEY REFERENCES [Location],
	DepartureDate DATETIMEOFFSET NOT NULL,

	VoyageStatusEnumId INT NOT NULL
		CONSTRAINT FK_Voyage_VoyageStatusEnum FOREIGN KEY REFERENCES VoyageStatusEnum,
	CancelledAt DATETIMEOFFSET NULL,	-- NULLABLE
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- TODO: VoyageNode ordered by ETA?
CREATE TABLE VoyageNode
(
	VoyageNodeId INT
		CONSTRAINT PK_VoyageNode PRIMARY KEY IDENTITY,

	VoyageNumber VARCHAR(16) NOT NULL
		CONSTRAINT FK_VoyageNode_Voyage FOREIGN KEY REFERENCES Voyage ON DELETE CASCADE,
	CityForShipmentCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_VoyageNode_City FOREIGN KEY REFERENCES CityForShipment,

	EstimatedTimeOfArrival DATETIMEOFFSET NOT NULL,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE VoyageNodeSource
(
	VoyageNodeSourceId INT
		CONSTRAINT PK_VoyageNodeSource PRIMARY KEY IDENTITY,

	VoyageNodeId INT NOT NULL
		CONSTRAINT FK_VoyageNodeSource_Node FOREIGN KEY REFERENCES VoyageNode ON DELETE CASCADE,
	LocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_VoyageNodeSource_Location FOREIGN KEY REFERENCES Location,
	Capacity INT NOT NULL,	
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE VehicleVoyageStatusEnum
(
	VehicleVoyageStatusEnumId INT
		CONSTRAINT PK_VehicleVoyageStatusEnum PRIMARY KEY,
	[Name] VARCHAR(32) NOT NULL,	-- Planned, Prebooked, Ported, Assigned, Loading, Departed
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE PreBookVesselLocationMapping
(
	LocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_PreBookVesselLocationMapping_Location FOREIGN KEY REFERENCES [Location] ON DELETE CASCADE,
	ProcessMasterCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_PreBookVesselLocationMapping_ProcessMaster FOREIGN KEY REFERENCES [ProcessMaster] ON DELETE CASCADE,
	CONSTRAINT PK_PreBookVesselLocationMapping PRIMARY KEY (LocationCode, ProcessMasterCode),
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- TODO: Confirm logic
CREATE TABLE VoyageNodeSourceDetail
(
	VoyageNodeSourceId INT NOT NULL
		CONSTRAINT FK_VoyageNodeSourceDetail_Header FOREIGN KEY REFERENCES VoyageNodeSource ON DELETE CASCADE,
	VehicleId INT NOT NULL
		CONSTRAINT FK_VoyageNodeSourceDetail_Vehicle FOREIGN KEY REFERENCES Vehicle ON DELETE CASCADE,
	CONSTRAINT PK_VoyageNodeSourceDetail PRIMARY KEY (VoyageNodeSourceId, VehicleId),

	VehicleVoyageStatusEnumId INT NOT NULL
		CONSTRAINT FK_VoyageNodeSourceDetail_VehicleVoyageStatusEnum FOREIGN KEY REFERENCES VehicleVoyageStatusEnum,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Pricing Master
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE PricingTypeEnum
(
	PricingTypeEnumId INT
		CONSTRAINT PK_PricingTypeEnum PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,	-- Dealer-Type, Cluster-Model Series, Cluster

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE PricingComponent
(
	PricingComponentCode VARCHAR(8)
		CONSTRAINT PK_PricingComponent PRIMARY KEY, -- OPEX, SRUT, UP, LOG, SPU
	
	[Name] VARCHAR(255) NOT NULL,
	[PricingTypeEnumId] INT NOT NULL
		CONSTRAINT FK_PricingComponent_PricingTypeEnumId FOREIGN KEY REFERENCES PricingTypeEnum,

	[ParentPricingComponentCode] VARCHAR(8) NULL
		CONSTRAINT FK_PricingComponent_ParentPricingComponent FOREIGN KEY REFERENCES PricingComponent,	-- NULLABLE

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE PricingComponentLookup
(
	PricingComponentLookupId INT 
		CONSTRAINT PK_PricingComponentLookup PRIMARY KEY IDENTITY,
	
	-- Parameter 1
	[PricingComponentCode] VARCHAR(8) NOT NULL
		CONSTRAINT FK_PricingComponentLookup_PricingComponent FOREIGN KEY REFERENCES PricingComponent ON DELETE CASCADE,
	
	-- Parameter 2
	[Category] VARCHAR(16) NOT NULL,

	-- Parameter 3A
	[Katashiki] VARCHAR(32) NULL,											-- NULLABLE if PricingComponent's PricingTypeEnum NOT Dealer-Type
	[Suffix] VARCHAR(4) NULL,												-- NULLABLE if PricingComponent's PricingTypeEnum NOT Dealer-Type
		CONSTRAINT FK_PricingComponentLookup_CarType FOREIGN KEY(Katashiki, Suffix) REFERENCES CarType,

	-- Parameter 3B
	CarSeriesCode VARCHAR(16)
		CONSTRAINT FK_PricingComponentLookup_CarSeries FOREIGN KEY REFERENCES CarSeries NULL,		-- NULLABLE if PricingComponent's PricingTypeEnum NOT Cluster-Series
	
	-- Parameter 3C
	AS400ClusterCode VARCHAR(16)
		CONSTRAINT FK_PricingComponentLookup_AS400Cluster FOREIGN KEY REFERENCES AS400Cluster NULL,	-- NULLABLE if PricingComponent's PricingTypeEnum NOT Cluster

	-- Result
	[Price] DECIMAL(19,4) NOT NULL,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

/*
function (PricingComponentCode, [KatSu / Series / Cluster], Category) => Price

Jenis parameter #2 tergantung PricingTypeEnum

-- OPEX untuk Katsu A
-- OPEX untuk Katsu B
-- OPEX untuk Series A
-- SRUT untuk Series A
-- etc.
*/

CREATE TABLE BranchPricingComponent
(
	BranchPricingComponentId INT
		CONSTRAINT PK_BranchPricingComponent PRIMARY KEY IDENTITY,

	BranchCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_BranchPricingComponent_Branch FOREIGN KEY REFERENCES Branch ON DELETE CASCADE,
	PricingComponentLookupId INT NOT NULL
		CONSTRAINT FK_BranchPricingComponent_PricingComponentLookup FOREIGN KEY REFERENCES PricingComponentLookup ON DELETE CASCADE,
	CONSTRAINT UQ_BranchPricingComponent_Mapping UNIQUE (BranchCode, PricingComponentLookupId),

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Delivery Order
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE DiscountConfiguration
(
	NomorSurat VARCHAR(16)
		CONSTRAINT PK_DiscountConfiguration PRIMARY KEY,

	IsBudget BIT NOT NULL DEFAULT 0,	-- Checkbox
	[Budget] DECIMAL(19,4) NULL,		-- Only If IsBudget
	
	IsPeriod BIT NOT NULL DEFAULT 0,	-- Checkbox
	[StartPeriod] DATE NULL,			-- Only if IsPeriod
	[EndPeriod] DATE NULL,				-- Only if IsPeriod
	
	[Amount] DECIMAL(19,4) NOT NULL,
	[AmountRunning] DECIMAL(19,4) NOT NULL,

	DealerCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_DiscountConfiguration_Dealer FOREIGN KEY REFERENCES Dealer ON DELETE CASCADE,
	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
		CONSTRAINT FK_DiscountConfiguration_CarType FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE CompanyPlafond
(
	CompanyCode VARCHAR(16) 
		CONSTRAINT PK_Plafond PRIMARY KEY FOREIGN KEY REFERENCES Company,

	Plafond DECIMAL(19,4) NOT NULL,	-- Outstanding + Balance
	Outstanding DECIMAL(19,4) NOT NULL,
	Balance DECIMAL(19,4) NOT NULL,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE CompanyPlafondMutation
(
	CompanyPlafondMutationId INT PRIMARY KEY IDENTITY,
	CompanyCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_CompanyPlafondMutation_Header FOREIGN KEY REFERENCES CompanyPlafond,

	[Value] DECIMAL(19,4) NOT NULL,
	[Description] VARCHAR(255) NULL,

	Plafond DECIMAL(19,4) NOT NULL,	-- Outstanding + Balance
	Outstanding DECIMAL(19,4) NOT NULL,
	Balance DECIMAL(19,4) NOT NULL,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DebitAdvice
(
	DebitAdviceNumber VARCHAR(16)
		CONSTRAINT PK_DebitAdvice PRIMARY KEY,

	FakturPajakNumber VARCHAR(32) NULL,	-- NULLABLE, Faktur pajak keluarnya belakangan
	IsSentToDMS BIT NOT NULL DEFAULT 0,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryOrder
(
	DeliveryOrderNumber VARCHAR(16)
		CONSTRAINT PK_DeliveryOrder PRIMARY KEY, -- Very big number, unknown length, leading zero
	
	ReferenceNumber VARCHAR(64) NULL,	-- NULLABLE
	IsManual BIT NOT NULL DEFAULT 0,

	DebitAdviceNumber VARCHAR(16) NULL
		CONSTRAINT FK_DeliveryOrder_DebitAdvice FOREIGN KEY REFERENCES [DebitAdvice] ON DELETE SET NULL,	-- NULLABLE, Awal" masih null
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryOrderDetail
(
	DeliveryOrderDetailId INT
		CONSTRAINT PK_DeliveryOrderDetail PRIMARY KEY IDENTITY,
		
	DeliveryOrderNumber VARCHAR(16) NOT NULL
		CONSTRAINT FK_DeliveryOrderDetail_Header FOREIGN KEY REFERENCES DeliveryOrder ON DELETE CASCADE,
	IssuedDate DATETIMEOFFSET NOT NULL,

	VehicleId INT NOT NULL
		CONSTRAINT FK_DeliveryOrderDetail_Vehicle FOREIGN KEY REFERENCES Vehicle,
	Waiver BIT NOT NULL DEFAULT 0,	-- Buat Cek Plafon, Dasu
	IsDutyFree BIT NOT NULL DEFAULT 0,	-- Default Duty Paid	

	NormalPrice DECIMAL(19,4) NOT NULL,
	DiscountPrice DECIMAL(19,4) NOT NULL,
	WholePriceBeforeTax DECIMAL(19,4) NOT NULL,
	ValueAddedTax DECIMAL(19,4) NOT NULL,
	LuxuryTax DECIMAL(19,4) NOT NULL,
	PPH22 DECIMAL(19,4) NOT NULL,
	IsPPH22BarangMewah BIT DEFAULT 0 NOT NULL,	-- True, if WPBT > 2.000.000.000 or CC > 3000
	
	CompanyCode VARCHAR(16) NULL
		CONSTRAINT FK_DeliveryOrderDetail_Company FOREIGN KEY REFERENCES Company,	-- Nullable. If Delivery Order cancelled, plafond must be returned to how it was
	InvoicePrice DECIMAL(19,4) NOT NULL, -- Sepertinya mengurangi balance apabila yang bikin DO memutuskan untuk pakai plafond
	
	CancellationNumber VARCHAR(16) NULL,			-- NULLABLE
	DebitAdviceCancellationNumber VARCHAR(16) NULL,	-- NULLABLE
	CancelledAt DATETIMEOFFSET NULL,			    		-- NULLABLE

	IsSentToDMS BIT NOT NULL DEFAULT 0,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- Data breakdown when we create
CREATE TABLE DeliveryOrderDetailPriceComponent
(
	DeliveryOrderDetailPriceComponentId INT
		CONSTRAINT PK_DeliveryOrderDetailPriceComponent PRIMARY KEY IDENTITY,

	DeliveryOrderDetailId INT NOT NULL
		CONSTRAINT FK_DeliveryOrderDetailPriceComponent_Item FOREIGN KEY REFERENCES DeliveryOrderDetail ON DELETE CASCADE,
	PricingComponentCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_DeliveryOrderDetailPriceComponent_PricingComponent FOREIGN KEY REFERENCES PricingComponent,
	
	Category VARCHAR(255) NOT NULL,
	Price DECIMAL(19,4) NOT NULL,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- PDI / PIO / SPU
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE PDILine
(
	PDILineId INT
		CONSTRAINT PK_PDILine PRIMARY KEY IDENTITY,

	LocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_PDILine_Location FOREIGN KEY REFERENCES [Location] ON DELETE CASCADE,
	LineNumber VARCHAR(8) NOT NULL,
	CONSTRAINT UQ_PDILine_Mapping UNIQUE(LocationCode, LineNumber),
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE PDILeadTime
(
	PDILeadTimeId INT
		CONSTRAINT PK_PDILeadTime PRIMARY KEY IDENTITY,

	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
		CONSTRAINT FK_PDILeadTime_CarType FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType ON DELETE CASCADE,

	LocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_PDILeadTime_Location FOREIGN KEY REFERENCES [Location] ON DELETE CASCADE,
	
	CONSTRAINT UQ_PDILeadTime_Mapping UNIQUE (LocationCode, Katashiki, Suffix),

	TaktSeconds INT NOT NULL,
	Post INT NOT NULL,
	-- Lead Time = TaktSeconds * Post
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE PIOLine
(
	PIOLineId INT
		CONSTRAINT PK_PIOLine PRIMARY KEY IDENTITY,

	LocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_PIOLine_Location FOREIGN KEY REFERENCES [Location] ON DELETE CASCADE,
	LineNumber VARCHAR(8) NOT NULL,
	CONSTRAINT UQ_PIOLine_Mapping UNIQUE (LocationCode, LineNumber),

	TaktSeconds INT NOT NULL,
	Post INT NOT NULL,

	IsLocked BIT NOT NULL DEFAULT 1,	-- If Locked it will use it configuration, if unlocked it is free for all vehicle

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE PIOLineDetail
(
	PIOLineDetailId INT
		CONSTRAINT PK_PIOLineDetail PRIMARY KEY IDENTITY,

	PIOLineId INT NOT NULL
		CONSTRAINT FK_PIOLineDetail_Header FOREIGN KEY REFERENCES PIOLine ON DELETE CASCADE,

	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
		CONSTRAINT FK_PIOLineDetail_CarType FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType,

	CONSTRAINT UQ_PIOLineDetail_Mapping UNIQUE(PIOLineId, Katashiki, Suffix),

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- What line number for a specific car type, a specific location, a specific branch on Spec Up
CREATE TABLE SPULine
(
	SPULineId INT
		CONSTRAINT PK_SPULine PRIMARY KEY IDENTITY,

	LocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_SPULine_Location FOREIGN KEY REFERENCES [Location] ON DELETE CASCADE,
	LineNumber VARCHAR(8) NOT NULL,
	CONSTRAINT UQ_SPULine_Mapping UNIQUE (LocationCode, LineNumber),

	TaktSeconds INT NOT NULL,
	Post INT NOT NULL,

	IsLocked BIT NOT NULL DEFAULT 1,	-- If Locked it will use it configuration, if unlocked it is free for all vehicle

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE SPULineDetail
(
	SPULineDetailId INT
		CONSTRAINT PK_SPULineDetail PRIMARY KEY IDENTITY,
	
	SPULineId INT
		CONSTRAINT FK_SPULineDetail_Header FOREIGN KEY REFERENCES SPULine ON DELETE CASCADE NOT NULL,
	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
		CONSTRAINT FK_SPULineDetail_CarType FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType,
	BranchCode VARCHAR(16) CONSTRAINT FK_SPULineDetail_Branch FOREIGN KEY REFERENCES Branch NOT NULL,
		CONSTRAINT UQ_SPULineDetail_Mapping UNIQUE(SPULineId, Katashiki, Suffix, BranchCode),

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Calendar
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE WorkHourTemplate
(
	WorkHourTemplateCode VARCHAR(16)	
		CONSTRAINT PK_WorkHourTemplate PRIMARY KEY,
	[Description] VARCHAR(255),
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE WorkHourTemplateDetail
(
	WorkHourTemplateDetailId INT
		CONSTRAINT PK_WorkHourTemplateDetail PRIMARY KEY IDENTITY,
	WorkHourTemplateCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_WorkHourTemplateDetail_Header FOREIGN KEY REFERENCES WorkHourTemplate ON DELETE CASCADE,

	ShiftCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_WorkHourTemplateDetail_Shift FOREIGN KEY REFERENCES [Shift]	ON DELETE CASCADE,

	TimeStart TIME NOT NULL,
	TimeFinish TIME NOT NULL,

	IsMonday BIT NOT NULL,
	IsTuesday BIT NOT NULL,
	IsWednesday BIT NOT NULL,
	IsThursday BIT NOT NULL,
	IsFriday BIT NOT NULL,
	IsSaturday BIT NOT NULL,
	IsSunday BIT NOT NULL,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE LocationWorkHour
(
	LocationWorkHourId BIGINT
		CONSTRAINT PK_LocationWorkHour PRIMARY KEY IDENTITY,

	LocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_LocationWorkHour_Location FOREIGN KEY REFERENCES [Location] ON DELETE CASCADE,
	ShiftCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_LocationWorkHour_Shift FOREIGN KEY REFERENCES [Shift] ON DELETE CASCADE,

	Start DATETIMEOFFSET NOT NULL,
	Finish DATETIMEOFFSET NOT NULL,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE BreakHourTemplate
(
	BreakHourTemplateCode VARCHAR(16)
		CONSTRAINT PK_BreakHourTemplate PRIMARY KEY,
	[Description] VARCHAR(255),
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE BreakHourTemplateDetail
(
	BreakHourTemplateDetailId INT 
		CONSTRAINT PK_BreakHourTemplateDetail PRIMARY KEY IDENTITY,
	
	BreakHourTemplateCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_BreakHourTemplateDetail_Header FOREIGN KEY REFERENCES BreakHourTemplate ON DELETE CASCADE,
	ShiftCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_BreakHourTemplateDetail_Shift FOREIGN KEY REFERENCES [Shift] ON DELETE CASCADE,

	TimeStart TIME NOT NULL,
	TimeFinish TIME NOT NULL,

	IsMonday BIT NOT NULL,
	IsTuesday BIT NOT NULL,
	IsWednesday BIT NOT NULL,
	IsThursday BIT NOT NULL,
	IsFriday BIT NOT NULL,
	IsSaturday BIT NOT NULL,
	IsSunday BIT NOT NULL,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE LocationBreakHour
(
	LocationBreakHourId BIGINT
		CONSTRAINT PK_LocationBreakHour PRIMARY KEY IDENTITY,

	LocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_LocationBreakHour_Location FOREIGN KEY REFERENCES [Location] ON DELETE CASCADE,
	ShiftCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_LocationBreakHour_Shift FOREIGN KEY REFERENCES [Shift] ON DELETE CASCADE,

	Start DATETIMEOFFSET NOT NULL,
	Finish DATETIMEOFFSET NOT NULL,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Holiday
(
	LocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_Holiday_Location FOREIGN KEY REFERENCES [Location] ON DELETE CASCADE,
	HolidayDate DATE NOT NULL,
	PRIMARY KEY (LocationCode, HolidayDate),
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM'
)

---------------------------------------------------------------------------------------------------------------------------------
-- Delivery Request
---------------------------------------------------------------------------------------------------------------------------------

/*
mobil udah nyampe PDC
1. kirim ke customer / branch, delivery request (direct delivery)
2. borrowing, mobil dipinjam buat showcase di suatu tempat
3. ketika return bisa pilihan lagi mau ke pdc awal, ato dikirim ke branch tujuan / customer
*/

CREATE TABLE DeliveryRequestTypeEnum
(
	DeliveryRequestTypeEnumId INT
		CONSTRAINT PK_DeliveryRequestTypeEnum PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,	-- Normal, Self-pick, Direct Delivery, Transit To Others
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryRequestTransitTypeEnum
(
	DeliveryRequestTransitTypeEnumId INT
		CONSTRAINT PK_DeliveryRequestTransitTypeEnum PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,	-- Normal - Return To PDC, Normal - Return To Other PDC, Normal - Self-pick From others, Self-pick to others
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryRequest
(
	DeliveryRequestNumber VARCHAR(32)
		CONSTRAINT PK_DeliveryRequest PRIMARY KEY,
		-- Kode Cabang/Tipe DR/Tanggal DR (yyyymmdd)/Sequential No : 1058/NR/20170608/001

	VehicleId INT NOT NULL
		CONSTRAINT FK_DeliveryRequest_Vehicle FOREIGN KEY REFERENCES Vehicle,
	DeliveryRequestTypeEnumId INT NOT NULL
		CONSTRAINT FK_DeliveryRequest_DeliveryRequestTypeEnum FOREIGN KEY REFERENCES DeliveryRequestTypeEnum,
	
	CancelledAt DATETIMEOFFSET NULL,	-- NULLABLE
	ClosedAt DATETIMEOFFSET NULL,	-- NULLABLE

	-- Case A: Normal
	RequestedDeliveryTimeToBranch DATETIMEOFFSET NULL,	-- NULLABLE, filled when deliveryrequesttype is normal

	-- Case B: Self-Pick
	[PickUpDate] DATETIMEOFFSET NULL,					-- NULLABLE, filled when deliveryrequesttype is Self-pick
	[PickUpIdentityIsKtp] BIT NULL,					-- NULLABLE, filled when deliveryrequesttype is Self-pick
	[PickUpIdentityCardNumber] VARCHAR(32) NULL,	-- NULLABLE, filled when deliveryrequesttype is Self-pick
	[PickUpIdentityName] VARCHAR(255) NULL,			-- NULLABLE, filled when deliveryrequesttype is Self-pick
	[PickUpConfirmationCode] VARCHAR(8) NULL,

	-- SELECT WHERE PickUpDate = today && ConfirmationCode = ... && CancelledAt IS NULL && ClosedAt IS NULL

	-- Case C: Direct Delivery
	DirectEstimatedPDCOut DATETIMEOFFSET NULL,			-- NULLABLE, filled when deliveryrequesttype is Direct Delivery
	DirectCustomerName VARCHAR(255) NULL,			-- NULLABLE, filled when deliveryrequesttype is Direct Delivery
	DirectCustomerAddress VARCHAR(255) NULL,		-- NULLABLE, filled when deliveryrequesttype is Direct Delivery
	DirectCustomerCity VARCHAR(255) NULL,			-- NULLABLE, filled when delivery	requesttype is Direct Delivery
	DirectSalesmanName VARCHAR(255) NULL,			-- NULLABLE, filled when deliveryrequesttype is Direct Delivery
	DirectSalesmanContactNumber VARCHAR(255) NULL,	-- NULLABLE, filled when deliveryrequesttype is Direct Delivery

	-- Case D: Transit
	TransitLocationCode VARCHAR(8) NULL
		CONSTRAINT FK_DeliveryRequest_TransitLocation FOREIGN KEY REFERENCES [Location],								-- NULLABLE, filled when deliveryrequesttype is Transit To Others. Only Karoseri, BodyPaint, Borrowing
	TransitReturnDate DATETIMEOFFSET NULL,
	DeliveryRequestTransitTypeEnumId INT NULL
		CONSTRAINT FK_DeliveryRequest_DeliveryRequestTransitTypeEnum FOREIGN KEY REFERENCES DeliveryRequestTransitTypeEnum,	-- NULLABLE, filled when deliveryrequesttype is Transit To Others
	
	-- Case D2: Transit: Normal: Return to Other PDC
	TransitReturnToOtherPdc VARCHAR(8) NULL
		CONSTRAINT FK_DeliveryRequest_TransitReturnToOtherPdc FOREIGN KEY REFERENCES [Location],

	-- Case D3: Transit: Normal: Self-Pick from Other
	-- Use Case B columns...

	-- Case D4: Transit: Self-Pick to Other
	-- Use Case B columns... Without confirmation code

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- AFI (Application for Invoice)
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE AFIRegionRestriction
(
	AFIRegionRestrictionId INT
		CONSTRAINT PK_AFIRegionRestriction PRIMARY KEY IDENTITY,

	RegionCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_AFIRegionRestriction_Region FOREIGN KEY REFERENCES [Region] ON DELETE CASCADE,
	
	IsLocked BIT NOT NULL DEFAULT 0,
	ValidFrom DATETIMEOFFSET NULL,
	ValidTo DATETIMEOFFSET NULL,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE AFIBranch
(
	AFIBranchCode VARCHAR(4) CONSTRAINT PK_AFIBranch PRIMARY KEY,
	BranchCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_AFIBranch_Branch FOREIGN KEY REFERENCES Branch ON DELETE CASCADE
		CONSTRAINT UQ_AFIBranch_BranchCode UNIQUE,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE AFIApplicationProcessEnum
(
	AFIApplicationProcessEnumId INT
		CONSTRAINT PK_AFIApplicationProcessEnum PRIMARY KEY,
	[Name] VARCHAR(255), -- Sebelum pengajuan bea cukai, etc (Aju Baru, Revisi AFI, Batal AFI, AJu Eks Batal)

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE AFISubmissionTypeEnum
(
	AFISubmissionTypeEnumId INT
		CONSTRAINT PK_AFISubmissionTypeEnum PRIMARY KEY,	
	[Name] VARCHAR(255) NOT NULL, -- Normal, REVA, REVB, REVC, REVD, REVE, REVF, Cancelled
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE AFIApplicationNumber
(
	AFIBranchCode VARCHAR(4) NOT NULL
		CONSTRAINT FK_AFIApplicationNumber_AFIBranch FOREIGN KEY REFERENCES AFIBranch,
	[Year] INT NOT NULL,
	CONSTRAINT PK_AFIApplicationNumber PRIMARY KEY(AFIBranchCode, [Year]),

	SequentialNumber INT NOT NULL,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- TODO: TimeStamp dan EffectiveUntil = DATE / DATETIME???
CREATE TABLE AFIApplication
(
	AFIApplicationId INT
		CONSTRAINT PK_AFIApplication PRIMARY KEY IDENTITY,	-- get latest order by desc

	ApplicationNumber VARCHAR(32) NULL,	-- NULLABLE, Sewaktu di create tidak langsung terbit Application Numbernya
	ReferenceNumber VARCHAR(32) NULL,	-- NULLABLE, bisa ada bisa tidak

	FakturNumber VARCHAR(32) NULL, -- NULLABLE
	[Timestamp] DATETIMEOFFSET NOT NULL,
	[EffectiveUntil] DATETIMEOFFSET NOT NULL,
	[Description] VARCHAR(255) NULL, -- NULLABLE

	AFIBranchCode VARCHAR(4) NOT NULL
		CONSTRAINT FK_AFIApplication_AFIBranch FOREIGN KEY REFERENCES AFIBranch,
	AFIApplicationProcessEnumId INT NOT NULL
		CONSTRAINT FK_AFIApplication_AFIApplicationProcessEnum FOREIGN KEY REFERENCES AFIApplicationProcessEnum,
	AFISubmissionTypeEnumId INT NOT NULL
		CONSTRAINT FK_AFIApplication_AFIApplicationTypeEnum FOREIGN KEY REFERENCES AFISubmissionTypeEnum,
	AFIRegionCode VARCHAR(4) NOT NULL
		CONSTRAINT FK_AFIApplication_AFIRegion FOREIGN KEY REFERENCES AFIRegion,
	VehicleId INT NOT NULL
		CONSTRAINT FK_AFIApplication_Vehicle FOREIGN KEY REFERENCES Vehicle,

	-- Customer, flat data juga karena revisi bisa ganti customer, tapi aplikasi baru
	[Name] VARCHAR(32) NOT NULL,
	[KTP] VARCHAR(32) NOT NULL,
	[Address1] VARCHAR(32) NOT NULL,	-- Alamat + RT/RW
	[Address2] VARCHAR(32) NOT NULL,	-- Kelurahan
	[Address3] VARCHAR(32) NOT NULL,	-- Kecamatan
	City VARCHAR(32) NOT NULL,
	Province VARCHAR(32) NOT NULL,
	PostalCode VARCHAR(8) NOT NULL,

	[Warna] VARCHAR(32) NULL,		-- NULLABLE, Only filled when AFISubmissionTypeCode is Revision for Color (tanya tato)
	[Jenis] VARCHAR(32) NULL,		-- e.g. Mobil Barang, Mobil Penumpang, etc
	[Model] VARCHAR(32) NULL,		-- e.g. Minibus, Chassis, Pick-up, Double Cabin, etc
	[ChassisModel] VARCHAR(32) NULL,	-- NULLABLE, Only filled when CarCategory is Chassis
	
	DocumentSentAt DATETIMEOFFSET NULL,		-- NULLABLE, May be not used
	DocumentReceivedAt DATETIMEOFFSET NULL,	-- NULLABLE, Info will be received later from DMS
	STNKAjuAt DATETIMEOFFSET NULL,			-- NULLABLE, Info will be received later from DMS
	STNKReceivedAt DATETIMEOFFSET NULL,		-- NULLABLE, Info will be received later from DMS
	
	TamReceivedAt DATETIMEOFFSET NULL,		-- NULLABLE
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE SuratPengantarFaktur
(
	NomorSuratPengantarFaktur VARCHAR(32)
		CONSTRAINT PK_SuratPengantarFaktur PRIMARY KEY,

	[ProcessDate] DATETIMEOFFSET NOT NULL,	-- Process Date dari WXDATA1
	[OutDate] DATETIMEOFFSET NOT NULL,		-- Out Date dari WXDATA1

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE SuratPengantarFakturDetail	-- 1 SPF punya banyak Frame Number
(
	NomorSuratPengantarFaktur VARCHAR(32) NOT NULL
		CONSTRAINT FK_SuratPengantarFakturDetail_Header FOREIGN KEY REFERENCES SuratPengantarFaktur ON DELETE CASCADE,
	VehicleId INT NOT NULL
		CONSTRAINT FK_SuratPengantarFakturDetail_Vehicle FOREIGN KEY REFERENCES Vehicle,
	CONSTRAINT PK_SuratPengantarFakturDetail PRIMARY KEY(NomorSuratPengantarFaktur, VehicleId),

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE ScratchConfiguration
(
	BranchCode VARCHAR(16) NOT NULL
		CONSTRAINT FK_ScratchConfiguration_Branch FOREIGN KEY REFERENCES Branch ON DELETE CASCADE,
	CarModelCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_ScratchConfiguration_CarModel FOREIGN KEY REFERENCES CarModel ON DELETE CASCADE,
	CONSTRAINT PK_ScratchConfiguration PRIMARY KEY(BranchCode, CarModelCode),

	[NumberOfScratch] INT NOT NULL,	-- Jumlah "gesek/scratch" yang dibutuhkan untuk Mobil Impor/Lokal dengan Branch Auto2000/NonAuto2000
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE ScratchHandOver
(
	ScratchHandOverNumber VARCHAR(16)
		CONSTRAINT PK_ScratchHandOver PRIMARY KEY,
	[Date] DATE NOT NULL,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Scratch
(
	ScratchId INT
		CONSTRAINT PK_Scratch PRIMARY KEY IDENTITY,

	VehicleId INT NOT NULL
		CONSTRAINT FK_Scratch_Vehicle FOREIGN KEY REFERENCES Vehicle,				-- Which Frame Number to be scratched
	LocationCode VARCHAR(8) NOT NULL
		CONSTRAINT FK_Scratch_Location FOREIGN KEY REFERENCES Location,	-- Scratch Location
	
	ScratchHandOverNumber VARCHAR(16) NULL
		CONSTRAINT FK_Scratch_Header FOREIGN KEY REFERENCES ScratchHandOver,	-- Nullable, is not filled until later
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Application Identity
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE AppRole -- Hard defined in accordance to Role value obtained from PASSSPORT
(
	AppRoleName VARCHAR(16)
		CONSTRAINT PK_AppRole PRIMARY KEY,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE AppMenu -- Hard defined in accordance to value in MVC [Authorize] Attribute, Role Property
(
	AppMenuName VARCHAR(16)
		CONSTRAINT PK_AppMenu PRIMARY KEY,
	
	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE AppRoleMenuMapping
(
	AppRoleName VARCHAR(16) NOT NULL
		CONSTRAINT FK_AppRoleMenuMapping_AppRole FOREIGN KEY REFERENCES AppRole,
	AppMenuName VARCHAR(16) NOT NULL
		CONSTRAINT FK_AppRoleMenuMapping_AppMenu FOREIGN KEY REFERENCES AppMenu,
	CONSTRAINT PK_AppRoleMenuMapping PRIMARY KEY(AppMenuName, AppRoleName),

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE UserMapping 
(
	Username VARCHAR(256)
		CONSTRAINT PK_UserMapping PRIMARY KEY, -- Fuckyeah TAM Passport! o/**\o User's Roles will be received from TAM Passport.
	
	-- LocationCode VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL, -- When user is signing into Mobile App, send his location. (Deny login when data not available)
	-- ShiftCode VARCHAR(16) FOREIGN KEY REFERENCES [Shift] NULL,	-- NULLABLE
	-- PDILineNumber VARCHAR(16) FOREIGN KEY REFERENCES PDILineDictionary(LineNumber) NULL,	-- NULLABLE
	-- PIOLineNumber VARCHAR(16) FOREIGN KEY REFERENCES PIOLineDictionary(LineNumber) NULL,	-- NULLABLE
	-- SPULineNumber VARCHAR(16) FOREIGN KEY REFERENCES SPULineDictionary(LineNumber) NULL,	-- NULLABLE
	-- DeliveryVendorCode VARCHAR(16) FOREIGN KEY REFERENCES [DeliveryVendor] NULL,	-- NULLABLE

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- File Job
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE FileJob
(
	FileJobId UNIQUEIDENTIFIER CONSTRAINT PK_FileJob PRIMARY KEY,

	[Module] VARCHAR(32) NULL,	-- MASTER, DELIVERY
	[Menu] VARCHAR(255) NULL,	-- Download Branch, Upload Branch
	[BlobPath] VARCHAR(255) NULL
		CONSTRAINT FK_FileJob_Blob FOREIGN KEY REFERENCES Blob(Path) ON DELETE CASCADE,	-- NULLABLE

	FinishAt DATETIMEOFFSET NULL,	-- NULLABLE
	IsSuccess BIT NOT NULL DEFAULT 0,

	CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)