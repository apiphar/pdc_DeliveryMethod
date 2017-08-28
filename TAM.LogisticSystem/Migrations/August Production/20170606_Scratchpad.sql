-- Defer: Phase 2
CREATE TABLE PlantProductionTime
(
	PlantProductionTimeId INT PRIMARY KEY IDENTITY,

	PlantCode VARCHAR(8) FOREIGN KEY REFERENCES Plant NOT NULL UNIQUE,
	
	IsDailyConvertProductionTime BIT NOT NULL DEFAULT 1,
	IsEndOfMonthConvertProductionTime BIT NOT NULL DEFAULT 1,

	CustomProductionTimeFrom DATETIME2 NOT NULL,
	CustomProductionTimeTo DATETIME2 NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- Defer: Not used
CREATE TABLE Sourcing
(
	SourcingId INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(255) NOT NULL,	-- CKD, CBU

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- Defer: Probably not used.
CREATE TABLE CarTypeColor
(
	CarTypeColorCode VARCHAR(16) PRIMARY KEY,

	CarModelCode VARCHAR(8) FOREIGN KEY REFERENCES [CarModel] NOT NULL,
	ExteriorColorCode VARCHAR(4) FOREIGN KEY REFERENCES ExteriorColor NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- Defer: CBU December
CREATE TABLE Currency
(
	CurrencySymbol VARCHAR(8) PRIMARY KEY, -- e.g. USD, GBP, JPY.
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part 2: Location Master
---------------------------------------------------------------------------------------------------------------------------------

-- Defer: Not yet used
CREATE TABLE Cluster
(
	ClusterCode VARCHAR(16) PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE UNIQUE NONCLUSTERED INDEX idx_MDMDealerId on Dealer(MDMDealerId) WHERE MDMDealerId IS NOT NULL
CREATE UNIQUE NONCLUSTERED INDEX idx_MDMCompanyId ON Company(MDMCompanyId) WHERE MDMCompanyId IS NOT NULL

-- Deleted: Absorbed into SalesArea
CREATE TABLE [Zone]
(
	ZoneId INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE UNIQUE NONCLUSTERED INDEX idx_BranchLocationCode ON Branch(LocationCode) WHERE LocationCode IS NOT NULL
CREATE UNIQUE NONCLUSTERED INDEX idx_BranchAS400 ON Branch(BranchAS400) WHERE BranchAS400 IS NOT NULL

-- Defer: Desember? Dan belom jelas
CREATE TABLE FTZLookup
(
	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
	FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType,

	DealerCode VARCHAR(16) FOREIGN KEY REFERENCES Dealer NOT NULL,

	PRIMARY KEY(Katashiki, Suffix, DealerCode),
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: Delivery Leg
---------------------------------------------------------------------------------------------------------------------------------

-- Defer: masih rancu
CREATE TABLE DeliveryLegSplit
(
	DeliveryLegCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryLeg NOT NULL,
	DeliveryVendorCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryVendor NOT NULL,
	PRIMARY KEY (DeliveryLegCode, DeliveryVendorCode),

	HeadDeliveryLegCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryLeg NOT NULL,
	TailDeliveryLegCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryLeg NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- Deleted: absorbed delivery method
CREATE TABLE DeliveryVendorVehicleCostType	-- ENUMERABLE
(
	DeliveryVendorVehicleCostTypeId INT PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,	-- Unit, Ritase - Single Trip, Ritase - Round Trip

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: Vehicle Import
---------------------------------------------------------------------------------------------------------------------------------

-- defer: CBU december
CREATE TABLE ExchangeRate
(
	ExchangeRateId INT PRIMARY KEY IDENTITY,

	CurrencySymbol VARCHAR(8) FOREIGN KEY REFERENCES Currency NOT NULL,

	ToRupiah DECIMAL(19,4) NOT NULL,
	ValidFrom DATETIME2 NOT NULL,
	ValidUntil DATETIME2 NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- Engine number & frame number is being checked against engine & frame code which depending on the car type.
-- Apparently 1 Car Type can have multiple Engine Codes + Frame Code! (paired)
-- defer: CBU december
CREATE TABLE KatashikiValidation
(
	KatashikiValidationId INT PRIMARY KEY IDENTITY,

	Katashiki VARCHAR(32) NOT NULL,

	EnginePrefix VARCHAR(8) NOT NULL,
	FrameCode VARCHAR(8) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: CBU december
CREATE TABLE PersetujuanImportBarang -- PIB
(
	NomorAju VARCHAR(32) PRIMARY KEY,
	CurrencySymbol VARCHAR(8) FOREIGN KEY REFERENCES Currency NOT NULL, -- User must select when uploading new excel. e.g. USD, GBP, JPY.

	HarmonizeSchema VARCHAR(4) NOT NULL,
	HarmonizeSchemaFinal VARCHAR(4) NOT NULL,

	TanggalAju DATETIME2 NOT NULL,
	TanggalAjuApproved DATETIME2 NOT NULL,

	NomorPIB VARCHAR(32) NULL,
	TanggalPIB DATETIME2 NULL,
	
	CurrencyRate DECIMAL(19,4) NOT NULL,
	CurrencyRateFinal DECIMAL(19,4) NOT NULL,
	
	ClearanceAt DATETIME2 NULL,
	FinalizedAt DATETIME2 NULL,

	PortArrivalDate DATETIME2 NULL,	-- NULLABLE

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- CarTypeConverter --> CarType's HarmonizeCode --> HarmonizeTariff
-- defer: CBU december
CREATE TABLE ShipmentInvoice -- Invoices are grouped by Vehicle Type apparently
(
	InvoiceNumber VARCHAR(32) PRIMARY KEY, -- ????
	InvoiceDate DATETIME2 NOT NULL,
	
	NomorAju VARCHAR(32) FOREIGN KEY REFERENCES PersetujuanImportBarang NULL,	-- NULLABLE, karna invoice terbit blm tntu nomor aju terbit -Hartato
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: CBU december
CREATE TABLE ShipmentInvoiceDetail
(
	ShipmentInvoiceDetailId INT PRIMARY KEY IDENTITY,
	InvoiceNumber VARCHAR(32) FOREIGN KEY REFERENCES ShipmentInvoice NOT NULL,

	FrameNumber VARCHAR(32) NOT NULL,
	EnginePrefix VARCHAR(8) NULL,	-- For PDI Completion
	EngineNumber VARCHAR(32) NULL,	-- For PDI Completion

	FormARequestNumber VARCHAR(32) NULL,	-- NULLABLE, Formatnya follow hartato
	FormARequestDate DATETIME2 NULL,

	FormANumber VARCHAR(32) NULL,
	FormADate DATETIME2 NULL,
	
	Katashiki VARCHAR(32) NOT NULL,
	SuffixOriginal VARCHAR(4) NOT NULL,
	Suffix VARCHAR(4) NOT NULL, 	-- Equals to SuffixOriginal or new Suffix after converted.
	FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType,

	SellerCode VARCHAR(8) NOT NULL,
	SenderCode VARCHAR(8) NOT NULL,
	ReceiverCode VARCHAR(8) NOT NULL,	-- Mungkin Company Code

	EstimatedShipmentDeparture DATETIME2 NOT NULL,
	EstimatedShipmentArrival DATETIME2 NOT NULL,
	ActualShippingDate DATETIME2 NOT NULL,	-- CBU Shipment

	VesselName VARCHAR(255) NOT NULL,
	EDNumber VARCHAR(16) NOT NULL,	-- Unique Key from Manufacturer, Which manufacturer ?
	OrderNumber VARCHAR(16) NOT NULL,

	ProductionMonth DATETIME2 NOT NULL,
	Insurance DECIMAL(19,4) NOT NULL,
	Freight DECIMAL(19,4) NOT NULL,

	PortDischarge VARCHAR(8) NOT NULL,
	
	-- Frame Number = Country Code + VDS Code + VISCode
	VDSCode VARCHAR(16) NOT NULL,
	VISCode VARCHAR(16) NOT NULL,
	
	ExteriorColorCode VARCHAR(4) FOREIGN KEY REFERENCES ExteriorColor NOT NULL,
	InteriorColorCode VARCHAR(4) FOREIGN KEY REFERENCES InteriorColor NOT NULL,

	Price DECIMAL(19,4) NOT NULL,

	PriceRupiah DECIMAL(19,4) NOT NULL,
	BeaMasuk DECIMAL(19,4) NOT NULL,
	ImportValue DECIMAL(19,4) NOT NULL,
	PPH DECIMAL(19,4) NOT NULL,
	PPN DECIMAL(19,4) NOT NULL,
	PPNBM DECIMAL(19,4) NOT NULL,

	PriceRupiahFinal DECIMAL(19,4) NOT NULL,
	BeaMasukFinal DECIMAL(19,4) NOT NULL,
	ImportValueFinal DECIMAL(19,4) NOT NULL,
	PPHFinal DECIMAL(19,4) NOT NULL,
	PPNFinal DECIMAL(19,4) NOT NULL,
	PPNBMFinal DECIMAL(19,4) NOT NULL,

	Flag BIT NOT NULL,	-- Gatau buat apaan, simpen aja kbutuhan PIB
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: CBU december
CREATE TABLE Permit
(
	PermitId INT PRIMARY KEY IDENTITY,
	
	TPTNumber VARCHAR(16) NOT NULL,

	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
	FOREIGN KEY(Katashiki, Suffix) REFERENCES CarType,

	Quota INT NOT NULL,
	ValidFrom DATETIME2 NOT NULL,
	ValidTo DATETIME2 NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: CBU december
CREATE TABLE CarTypeConverter
(
	CarTypeConverterId INT PRIMARY KEY IDENTITY,

	Katashiki VARCHAR(32) NOT NULL,
	SuffixFrom VARCHAR(4) NOT NULL,
	SuffixTo VARCHAR(4) NOT NULL,						
	FOREIGN KEY (Katashiki, SuffixTo) REFERENCES CarType,

	[Description] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- Defer: Phase 2
CREATE TABLE RoutingDictionaryProduction	-- Routing for Jig In, Welding, Tosho, Assembly
(
	RoutingDictionaryProductionId INT PRIMARY KEY IDENTITY,

	LocationCode VARCHAR(8) REFERENCES [Location] NOT NULL,

	Katashiki VARCHAR(32) NULL,	-- Nullable, if NULL it is default
	Suffix VARCHAR(4) NULL,	-- Nullable, if NULL it is default
	FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType,
	
	RoutingMasterCode VARCHAR(8) REFERENCES RoutingMaster NOT NULL,

	Ordering INT NOT NULL,
	LeadMinutes INT NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- Defer: December?
CREATE TABLE ProcessForLine	-- ENUMERABLE
(
	ProcessForLineId INT PRIMARY KEY,
	[Name] VARCHAR(16) NOT NULL,	-- Incomplete, Repair
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- Defer: December?
CREATE TABLE LineDefaultLeadTimeConfiguration	-- Default Lead Time Configuration for PIO and SPU
(
	[RoutingMasterCode] VARCHAR(8) FOREIGN KEY REFERENCES RoutingMaster NOT NULL,	-- PIO, SPU
	[LocationCode] VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,
	ProcessForLineId INT FOREIGN KEY REFERENCES ProcessForLine NOT NULL,
	PRIMARY KEY(RoutingMasterCode, LocationCode, ProcessForLineId),

	LeadMinutes INT NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)


---------------------------------------------------------------------------------------------------------------------------------
-- Part X: DCCP
---------------------------------------------------------------------------------------------------------------------------------

-- Defer: skip
CREATE TABLE DailyCarCarrierPlan
(
	DailyCarCarrierPlanId INT PRIMARY KEY IDENTITY,

	TransInOutDate DATETIME2 NOT NULL,

	LocationFrom VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,
	LocationTo VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,

	Trip INT NOT NULL,
	[Load] INT NOT NULL,
	[ShiftCode] VARCHAR(16) FOREIGN KEY REFERENCES [Shift] NOT NULL,

	UnitReadyQuantity INT NOT NULL,
	UnitReadyAdjusted INT NULL,	-- Nullable, Human Judgement Input

	EstimatedUnit INT NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- Defer: skip
CREATE TABLE DailyCarCarrierPlanExcelConfiguration
(
	DailyCarCarrierPlanExcelConfigurationId INT PRIMARY KEY IDENTITY,

	[Description] VARCHAR(255) NOT NULL,
	[RangeStart] VARCHAR(4) NOT NULL,
	[RangeEnd] VARCHAR(4) NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: MCCP Configuration
---------------------------------------------------------------------------------------------------------------------------------

-- Defer: skip
CREATE TABLE MonthlyCarCarrierPlanConfiguration	-- Modul Pak Landu, sangat tidak conventional
(
	[ConfigurationRow] INT PRIMARY KEY,

	LocationFrom VARCHAR(8) FOREIGN KEY REFERENCES [Location] NULL,	-- NULLABLE
	LocationTo VARCHAR(8) FOREIGN KEY REFERENCES [Location] NULL,	-- NULLABLE

	WordToSearch VARCHAR(255) NULL,	-- NULLABLE
	DealerCode VARCHAR(16) FOREIGN KEY REFERENCES Dealer NULL,	-- NULLABLE
	BranchCode VARCHAR(16) FOREIGN KEY REFERENCES [Branch] NULL,	-- NULLABLE

	ResultQuery VARCHAR(MAX) NULL,	-- NULLABLE
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: Maintenance Configuration Planning
---------------------------------------------------------------------------------------------------------------------------------

-- Defer: skip
CREATE TABLE MaintenanceConfigurationPlanning
(
	MaintenanceConfigurationPlanningId INT PRIMARY KEY IDENTITY,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- Defer: Redis
CREATE TABLE [PlafondHistory]	-- Plafond Usage History
(
	PlafondHistoryId INT PRIMARY KEY IDENTITY,

	PlafondMasterId INT FOREIGN KEY REFERENCES PlafondMaster NOT NULL,

	Plafond DECIMAL(19,4) NOT NULL,	-- Outstanding + Balance
	Outstanding DECIMAL(19,4) NOT NULL,
	Balance DECIMAL(19,4) NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- Defer: Not Sure
CREATE TABLE DeliveryOrderDutyFreeRequest
(
	DeliveryOrderDutyFreeRequestNumber VARCHAR(32) PRIMARY KEY,	-- Special Request

	[Date] DATETIME2 NOT NULL,
	VehicleId INT FOREIGN KEY REFERENCES [Vehicle] NOT NULL,

	IsDutyFree BIT NOT NULL DEFAULT 0,	-- Default Duty Paid
	ReferenceNumber VARCHAR(64) NULL,	-- NULLABLE

	NormalPrice DECIMAL(19,4) NOT NULL,
	DiscountPrice DECIMAL(19,4) NOT NULL,
	WPBT DECIMAL(19,4) NOT NULL,
	ValueAddedTax DECIMAL(19,4) NOT NULL,
	LuxuryTax DECIMAL(19,4) NOT NULL,
	PPH22 DECIMAL(19,4) NOT NULL,
	InvoicedPrice DECIMAL(19,4) NOT NULL,

	ConfirmedAt DATETIME2 NULL,	-- NULLABLE
	CancelledAt DATETIME2 NULL,	-- NULLABLE
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: Tax History
---------------------------------------------------------------------------------------------------------------------------------

-- defer: december
CREATE TABLE WPBTHistory	-- untuk report tapi gajelas kapan diupdateny
(
	[WPBTHistoryId] INT PRIMARY KEY IDENTITY,

	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
	FOREIGN KEY(Katashiki, Suffix) REFERENCES CarType,

	OldWPBT DECIMAL(19,4) NOT NULL,
	NewWPBT DECIMAL (19,4) NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: SJKB
---------------------------------------------------------------------------------------------------------------------------------

-- defer: december
CREATE TABLE SJKBConfirmation
(
	SJKBConfirmationCode VARCHAR(16) PRIMARY KEY,	-- Generated to be confirmed later before user can create SJKB

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),	-- Date Generated
	ValidUntil DATETIME2 NOT NULL,	-- Valid Until (still not know for how long)

	ConfirmedAt DATETIME2 NULL,	-- Nullable

	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: december
CREATE TABLE SJKB -- Used when a CarCarrier carried 1 or more vehicles out from location to location
(
	SJKBNumber VARCHAR(16) PRIMARY KEY,
	SJKBConfirmationCode VARCHAR(16) FOREIGN KEY REFERENCES SJKBConfirmation NULL,	-- Cuma beberapa DR yang memakai self pick yg perlu confirmation code

	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES Location NOT NULL,	-- Lokasi tempat pembuatan SJKB

	DeliveryVendorVehicleId INT FOREIGN KEY REFERENCES DeliveryVendorVehicle NOT NULL,	-- 
	DeliveryDriverCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryDriver NULL,	-- NULLABLE

	DeliveryLegCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryLeg NULL, -- Nullable, SJKB can have either registered or custom destination

	CustomDestinationName VARCHAR(255) NULL,							-- Nullable, Custom Destination Name
	CustomDestinationCityLocation VARCHAR(16) FOREIGN KEY REFERENCES CityLocation NULL,		-- Nullable, Custom Destination City
	CustomDestinationAddress VARCHAR(255) NULL,						-- Nullable, Custom Destination Address
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),	-- Created Time
	ValidatedAt DATETIME2 NULL,		-- Nullable, Validated SJKB can be used for Gate Out
	UsedAt DATETIME2 NULL,			-- Nullable, SJKB has been used for Gate Out
	ReceivedAt DATETIME2 NULL,		-- Nullable, GR, this SJKB is finished properly. End Point for SJKB.

	CancelledAt DATETIME2 NULL,		-- Nullable, Cancelled SJKB can occured anytime. End Point for SJKB.
	
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: december
CREATE TABLE SJKBDetail -- Map 1 SJKB to many Vehicle
(	
	SJKBNumber VARCHAR(16) FOREIGN KEY REFERENCES SJKB NOT NULL,
	VehicleId INT FOREIGN KEY REFERENCES Vehicle NOT NULL,
	PRIMARY KEY(SJKBNumber, VehicleId),

	Cost DECIMAL(19,4) NULL,	-- NULLABLE, for ritase

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part 4: Vehicle Parking Master
---------------------------------------------------------------------------------------------------------------------------------

-- defer: december
CREATE TABLE [Yard]
(
	YardCode VARCHAR(16) PRIMARY KEY,

	[LocationCode] VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,

	[Name] VARCHAR(255) NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: december
CREATE TABLE [YardDetail]
(
	YardDetailId INT PRIMARY KEY IDENTITY,
	
	YardCode VARCHAR(16) FOREIGN KEY REFERENCES [Yard] UNIQUE NOT NULL,
	
	ParkingRowTotal INT NOT NULL,
	ParkingColumnTotal INT NOT NULL,
	YardManagementLayoutJSON VARCHAR(MAX) NOT NULL,
	StallManagementLayoutJSON VARCHAR(MAX) NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- 0: Freestyle
-- 1: Car Type
-- 2: Next Location
-- 3: Next Line
-- 4: DealerId
-- defer: december
CREATE TABLE ParkingRule
(
	ParkingRuleId INT PRIMARY KEY, -- ENUMERABLE
	[Name] VARCHAR(64) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: december
CREATE TABLE [ParkingException]
(
	ParkingExceptionCode VARCHAR(16) PRIMARY KEY,

	CarSeriesCode VARCHAR(16) FOREIGN KEY REFERENCES [CarSeries] NOT NULL,

	[Quantity] INT NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

/*
LocationName : CCY

   1 2 3 4 5 6 7 8
 1 B B       C C C
 2 B B       C C C
 3 B B B B B C C C
 4 B B B B B
 5           
 6 A A A A   
 7 A A A A
  
 A-C : Parking Areas, e.g. Before Receiving, Before Gate Out, Before PIO, etc.
 _ (Blank) : UnAssigned Parking Cells, can be assigned anytime such as Before Receiving 2

 ADDITIONAL NOTE (DESMOND) : ParkingArea can be not square
*/

-- defer: december
CREATE TABLE ParkingArea	-- Contains Parking Restriction as Area Name for Car to be parked before every process in CY (Before Receiving, Before PIO, etc)
(
	ParkingAreaId INT PRIMARY KEY IDENTITY,

	YardCode VARCHAR(16) FOREIGN KEY REFERENCES [Yard] NOT NULL,
	AreaCode VARCHAR(16) NOT NULL,	-- Label (Prefix) for Parking Slot Name, e.g. A B C
	UNIQUE(YardCode, AreaCode),

	[Name] VARCHAR(255) NOT NULL,	-- Before PIO

	RoutingMasterCode VARCHAR(8) FOREIGN KEY REFERENCES RoutingMaster NOT NULL, -- For What Process
	ParkingRuleId INT FOREIGN KEY REFERENCES ParkingRule NOT NULL,
	
	IsParkingException BIT NOT NULL DEFAULT 0,
	[Priority] VARCHAR(255) NOT NULL, -- ???

	Katashiki VARCHAR(32) NULL,
	Suffix VARCHAR(4) NULL,						
	FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType, -- NULLABLE
	NextLocation VARCHAR(8) FOREIGN KEY REFERENCES [Location] NULL, -- NULLABLE
	NextLineNumber VARCHAR(16) NULL, -- NULLABLE
	DealerCode VARCHAR(16) FOREIGN KEY REFERENCES Dealer NULL, -- NULLABLE

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

/*
LocationName : CCY
ParkingAreaName : BeforeReceiving 1
ParkingAreaBlockName : B

   1  2   3   4   5
 1 B1 B5      
 2 B2 B6      
 3 B3 B7 B9  B11 B13
 4 B4 B8 B10 B12 B14
  
 B1-B14 : Parking Slots
 
 Numbering for Parking Slot is not fixed yet
*/

-- defer: december
CREATE TABLE ParkingSlot -- Contains slot information for every parking Areas, every ParkingArea has x ParkingSlot where x is Vehicle, selection in UI displayed based on ParkingArea (Katsu always Active)
(
	ParkingAreaId INT FOREIGN KEY REFERENCES ParkingArea NOT NULL, -- This ParkingSlot belongs to Which ParkingArea
	Number INT NOT NULL, -- 1, 2, 3, 4, 5, ...
	PRIMARY KEY(ParkingAreaId, Number),

	VehicleId INT FOREIGN KEY REFERENCES Vehicle NULL, -- Nullable, Currently occupied by vehicle or not

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: december
CREATE TABLE ParkingSlotHistory	-- Duplicate Value for ParkingSlot
(
	--ParkingSlotHistoryId INT PRIMARY KEY IDENTITY,

	ParkingAreaId INT NOT NULL, -- This ParkingSlot belongs to Which ParkingArea
	Number INT NOT NULL, -- 1, 2, 3, 4, 5, ...
	PRIMARY KEY(ParkingAreaId, Number),
	FOREIGN KEY(ParkingAreaId, Number) REFERENCES ParkingSlot,

	VehicleId INT FOREIGN KEY REFERENCES Vehicle NULL,	-- Nullable, If NULL the slot is empty
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: december
CREATE TABLE ParkingMovementConfiguration
(
	YardCode VARCHAR(16) FOREIGN KEY REFERENCES [Yard] NOT NULL,
	CurrentRoutingCode VARCHAR(8) FOREIGN KEY REFERENCES [RoutingMaster] NOT NULL,
	PRIMARY KEY(YardCode, CurrentRoutingCode),

	PreviousRoutingCode VARCHAR(8) FOREIGN KEY REFERENCES [RoutingMaster] NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part 5: Vehicle Accessory
---------------------------------------------------------------------------------------------------------------------------------

-- defer: phase 2
CREATE TABLE AccessoryVendor
(
	AccessoryVendorId INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: phase 2
CREATE TABLE AccessoryMaster
(
	PartNumber VARCHAR(16) PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,

	AccessoryVendorId INT FOREIGN KEY REFERENCES AccessoryVendor NOT NULL,
	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
	FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType,
	ExteriorColorCode VARCHAR(4) FOREIGN KEY REFERENCES ExteriorColor NOT NULL,

	IsDefault BIT NOT NULL,			-- Will be installed to vehicle by default regardless of Branch
	DefaultQuantity INT NOT NULL,	-- Will be used when automatically installed via PIO (IsDefault) or SpecUp (Branch mapped)

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: phase 2
CREATE TABLE BranchAccessory
(
	BranchCode VARCHAR(16) FOREIGN KEY REFERENCES Branch NOT NULL,
	PartNumber VARCHAR(16) FOREIGN KEY REFERENCES AccessoryMaster NOT NULL,
	PRIMARY KEY (BranchCode, PartNumber),

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: phase 2
CREATE TABLE VehicleAccessory
(
	VehicleId INT FOREIGN KEY REFERENCES Vehicle NOT NULL,
	PartNumber VARCHAR(16) FOREIGN KEY REFERENCES AccessoryMaster NOT NULL,
	PRIMARY KEY (VehicleId, PartNumber),

	Quantity INT NOT NULL,
	IsCustomerRequest BIT NOT NULL,		-- DIO

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part 6: Inspection Masters
---------------------------------------------------------------------------------------------------------------------------------

-- defer: december
CREATE TABLE InspectionCategory	-- ENUMERABLE
(
	InspectionCategoryId INT PRIMARY KEY,
	[Name] VARCHAR(16) NOT NULL,	-- Interior/Exterior
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

/*
Exterior Side : Front, Top, Rear, LH, RH
Interior Side : Front, Middle, Rear, LH, RH
*/
-- defer: december
CREATE TABLE InspectionSide	-- ENUMERABLE
(
	InspectionSideId INT PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,			-- e.g. Front Side, Top Side, Rear Side, LH Side, RH Side, Middle Side

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: december
CREATE TABLE InspectionPart
(
	InspectionPartCode VARCHAR(16) PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,			-- e.g. Fender, Front Door, Rear Door
	
	InspectionCategoryId INT FOREIGN KEY REFERENCES InspectionCategory NOT NULL,
	InspectionSideId INT FOREIGN KEY REFERENCES InspectionSide NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: december
CREATE TABLE InspectionItem
(
	InspectionItemCode VARCHAR(16) PRIMARY KEY,

	InspectionPartCode VARCHAR(16) FOREIGN KEY REFERENCES InspectionPart NOT NULL,
	[Name] VARCHAR(255) NOT NULL,			-- e.g. Door Glass FR LH, Quarter Panel LH, Quarter Trimboard RH-LH, Fender RH Appearance, etc.

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: december
CREATE TABLE Defect
(
	DefectCode VARCHAR(16) PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: december
CREATE TABLE InspectionMaster -- This list is for which vehicle type?
(
	InspectionMasterId INT PRIMARY KEY IDENTITY,

	RoutingMasterCode VARCHAR(8) FOREIGN KEY REFERENCES RoutingMaster NOT NULL,

	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,						
	FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType,

	UNIQUE (RoutingMasterCode, Katashiki, Suffix),

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: december
CREATE TABLE InspectionMasterDetail -- Daftar isi dari inspection untuk tipe kendaraan tersebut
(
	InspectionMasterDetailId INT PRIMARY KEY IDENTITY,

	InspectionMasterId INT FOREIGN KEY REFERENCES InspectionMaster NOT NULL,
	InspectionItemCode VARCHAR(16) FOREIGN KEY REFERENCES InspectionItem NOT NULL,
	DefectCode VARCHAR(16) FOREIGN KEY REFERENCES Defect NOT NULL,

	UNIQUE (InspectionMasterId, InspectionItemCode, DefectCode),

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

/*
Rear | Front | Left | Right + Interior | Exterior

Left: Door
	Panel
	Handle
	Window

Panel: Burem, Baret
Handle: Hilang, Baret
Panel: Bonyok, Baret
*/

---------------------------------------------------------------------------------------------------------------------------------
-- Part 6: Vehicle Inspection Transaction
---------------------------------------------------------------------------------------------------------------------------------

-- defer: december
CREATE TABLE VehicleInspection
(
	VehicleInspectionId INT PRIMARY KEY IDENTITY,

	VehicleRoutingId INT FOREIGN KEY REFERENCES VehicleRouting NOT NULL, -- At which process is this inspection is being performed?

	PerformedAt DATETIME2 NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: december
CREATE TABLE VehicleInspectionPhoto
(
	VehicleInspectionPhotoId UNIQUEIDENTIFIER ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
	VehicleInspectionId INT FOREIGN KEY REFERENCES VehicleInspection NOT NULL,
	Blob VARBINARY(MAX) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: december
CREATE TABLE VehicleInspectionDetail -- Checkboxes, lots of checkboxes. Values are historical; copied from VehicleInspectionMasterDetail.
(
	VehicleInspectionDetailId INT PRIMARY KEY IDENTITY,

	VehicleInspectionId INT FOREIGN KEY REFERENCES VehicleInspection NOT NULL,
	InspectionMasterDetailId INT FOREIGN KEY REFERENCES InspectionMasterDetail NOT NULL,

	IsDefect BIT NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: december
CREATE TABLE VehicleInspectionIncompleteDetail
(
	VehicleInspectionIncompleteDetailId INT PRIMARY KEY IDENTITY,

	VehicleInspectionId INT FOREIGN KEY REFERENCES VehicleInspection NOT NULL,

	[Note] VARCHAR(255) NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: december
CREATE TABLE VehicleCripple
(
	VehicleInspectionId INT FOREIGN KEY REFERENCES VehicleInspection NOT NULL,
	PartNumber VARCHAR(16) FOREIGN KEY REFERENCES AccessoryMaster NOT NULL,
	PRIMARY KEY (VehicleInspectionId, PartNumber),

	Quantity INT NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- defer: december
CREATE TABLE VehiclePendingCripple -- Copy values then erase them when the said vehicle enters quarantine.
(
	VehicleId INT FOREIGN KEY REFERENCES Vehicle NOT NULL,
	PartNumber VARCHAR(16) FOREIGN KEY REFERENCES AccessoryMaster NOT NULL,
	PRIMARY KEY (VehicleId, PartNumber),

	Quantity INT NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: DIO (Dealer Installation Option)
---------------------------------------------------------------------------------------------------------------------------------

-- defer: phase 2
CREATE TABLE DealerInstallationOption
(
	DealerInstallationOptionId INT PRIMARY KEY IDENTITY,

	[Name] VARCHAR(255) NOT NULL,	-- Door Sticker, Car Jack, Fender FR LH
	Quantity INT NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part 7: Vehicle Workshop + Quarantine
---------------------------------------------------------------------------------------------------------------------------------

-- deleted
CREATE TABLE VehicleOnWorkshop
(
	VehicleWorkshopId INT PRIMARY KEY IDENTITY,
	
	VehicleRoutingId INT FOREIGN KEY REFERENCES VehicleRouting NOT NULL, -- From which process did the vehicle took a detour?
	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,

	InTime DATETIME2 NOT NULL,
	OutTime DATETIME2 NULL,		-- NULLABLE when still stuck in quarantine!
	LeadMinutes INT NOT NULL,

	Remark VARCHAR(255) NULL,	-- NULLABLE if nothing to say. e.g. NG Heavy, NG Light, etc.

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part 8: AFI (Application For Invoice)
---------------------------------------------------------------------------------------------------------------------------------

-- Deleted: because Michael
CREATE TABLE Faktur
(
	FakturNumber VARCHAR(32) PRIMARY KEY,
	FakturDate DATETIME2 NOT NULL,
	EffectiveDate DATETIME2 NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

-- Defer: not sure
CREATE TABLE AFIRestrictionCity
(
	AFIRestrictionId INT PRIMARY KEY IDENTITY,

	CityCode VARCHAR(16) FOREIGN KEY REFERENCES [City] NULL,	-- NULLABLE

	IsLocked BIT NOT NULL DEFAULT 0,

	ValidFrom DATETIME2 NOT NULL,
	ValidTo DATETIME2 NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: Planning
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE MaintenanceProcess
(
	ProcessCode VARCHAR(16) PRIMARY KEY,

	[Description] VARCHAR(255) NOT NULL,
	[EmailTemplateCode] VARCHAR(16) NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE ErrorMessage
(
	ErrorMessageCode VARCHAR(8) PRIMARY KEY,

	[Description] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part 10: Log Upload Download
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE LogUploadDownloadFile
(
	LogUploadDownloadId INT PRIMARY KEY FOREIGN KEY REFERENCES [LogUploadDownload] ON DELETE CASCADE,

	[Blob] VARBINARY(MAX) NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)
