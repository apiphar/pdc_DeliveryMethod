/*
SQL Server index maximum size is 900 bytes

int: 4 bytes
	When used as PRIMARY KEY,
		IDENTITY for auto-incrementing key (surrogate key).
		not-IDENTITY if you plan to use the table as relational constraints to C# Enum. We DO NOT INDEX FK to these kinds of keys.

varchar: 2 + n bytes
	Field length consistency rule:
		ID / Code: 16 unless exact length known
		Phone: 32 because Apparently the ITU recommendation E.164 limits telephone numbers to 15 digits, including the international prefix, which can be up to 3 digits. 
			That does not include the international access code, e.g. 00 from most countries, 011 from North America, 0011 from Australia, 
			and also four digits from some other countries such as Brazil and Bolivia.
		NPWP: 32, just to be safe (my card's length is 20)
		Human Name / Username / Email: 255 because = the largest numerical value that can be stored in a single-byte unsigned integer 2^8-1, might be more efficient.
		Product Name / Label / Short Description / Postal Address: 255 
		Long Description / JSON: MAX because > 900 cannot be indexed anyway
			
bit: 1 bit for each bit-typed column in the same table. At least 1 byte.

datetime2: 8 bytes for default precision (7)
	Precision: 0 to 7 digits, with an accuracy of 100ns. The default precision is 7 digits.
	6 bytes for precisions less than 3; 7 bytes for precisions 3 and 4. All other precisions require 8 bytes.

decimal: 9 bytes for default precision (18, 0) = (total of 18 digits, 0 after decimal point)
	Precision => Storage bytes
		1 - 9 => 5
		10-19 => 9
		20-28 => 13
		29-38 => 17
	RYAN: It is recommended to set default explicit precision to (19,4) to reduce headache because:
		Using default precision, the DECIMAL data type cannot handle "decimal numbers".
		All major currencies of the world (that I am aware of) only require 2 decimal places, but there are a handful where 4 are required,
		and there are financial transactions such as currency transactions and bond sales where 4 decimal places are mandated by law. 
		Given how few transactions, and corporations, actually require precision greater than 19 digits this seems eminently sensible.
		And for some reason, storing 19 digits require byte space equals to 18 digits. So go for DECIMAL(19,4).
		(19,4) => 999,999,999,999,999.9999
		Which means you can handle any money or percentage under a quadrillion. Should be logical for most of your projects.
*/

/*
22 March Design Notes
- Requires sample data to determine VARCHAR length:
	CarModel.CarModelCode	=	FORT, AVNZ, INVA
	CarSeries.CarSeriesCode	=	FORD, AVNZ, INVD, INVB
	City.CityCode	=	
	Region.RegionCode	=	A,B,C,D,DA,E
	Region.Type	=	?
	Region.RegionCodeAFI	=	3 Digit
	Location.LocationCode	=	BAM7060, BBD1201 (Bandung Suci), BBD1018 (Bandung Pasteur)
	Dealer.DealerCode	=	BJY (Budi Jaya), 3005 (Agung Toyota)	- So far 4 digit
	Company.CompanyCode	=	AKJM (Akita Jaya Motor), BUJM (Budi Jaya Mobilindo), TAM (Toyota Astra Motor)
	Branch.BranchCode	=	1028, 1055, WM, AAL, 1044
	Branch.BranchCodeAFI	=	4 Digit			<-- kenapa namanya gak AFI aja langsung? Ini apaan btw?
	PricingComponentLookup.Category
	CityLeg.CityLegCode	=	CITY0001, CITY0002
	DeliveryVendor.[Account]
	DeliveryVendor.PaymentTerm
	Harmonize.HSCode
	KatashikiValidation.EnginePrefixCode
	KatashikiValidation.FrameCode
	PersetujuanImportBarang.HarmonizeSchema & PersetujuanImportBarang.HarmonizeSchemaFinal
	PersetujuanImportBarang.NomorPIB
	PersetujuanImportBarang.FormARequestNumber
	PersetujuanImportBarang.FormANumber
	ShipmentInvoice.InvoiceNumber
	ShipmentInvoiceDetail.EnginePrefix
	ShipmentInvoiceDetail.EngineNumber
	ShipmentInvoiceDetail.SellerCode
	ShipmentInvoiceDetail.SenderCode
	ShipmentInvoiceDetail.ReceiverCode
	ShipmentInvoiceDetail.EDNumber <= ED apa?
	ShipmentInvoiceDetail.OrderNo
	ShipmentInvoiceDetail.PortDischarge
	ShipmentInvoiceDetail.VDSCode
	ShipmentInvoiceDetail.VISCode
	Vehicle.DANo <-- DA itu apa????
	Vehicle.DONo <-- kalo 1 vehicle bisa punya banyak DO number berarti gak flat field dong...?
	Vehicle.EnginePrefix
	Vehicle.EngineNumber
	Vehicile.KeyNumber
	SuratJalanKeluarBarang.SuratJalanKeluarBarangNumber
	VesselStatusType.Name 
	AccessoryMaster.PartNumber
	Faktur.FakturNumber	=	20 digit
	AFIApplication.ApplicationNo
	AFIApplication.ChassisModel
	AFIApplication.TipePengajuan
	DeliveryOrder.DeliveryOrderNumber vs DeliveryOrder.ReferenceNumber

- Butuh dijelasin (ulang) tabel-tabel berikut: (dan ditulis biar gak lupa)
	Zone vs Region apa bedanya		- March 23 Currently Unknown for what
	SalesArea						- March 23 Currently Unknown for what
	Plafond		=	Jumlah Plafond (Limit Kredit) yang diberikan Company kepada Dealer
	DiscountConfiguration's Amount vs AmountRunning	=	Total Diskon yang diberikan vs Total Diskon yang sudah terpakai
	Kenapa DeliveryLeg jadi FK ke CityLegCode yah?	=	CityLeg itu Grouping dari beberapa delivery leg yg berada di kota yang sama or something like that. Tujuannya untuk hitung cost
	CityLegRitaseCost	=	CityLegCost itu untuk ngehitung cost per leg, Ritase itu pengiriman yang perhitungan costnya pake pertrip, bukan dari jumlah mobil yang diangkut
	DeliveryVendorVehicle kenapa FK ke DeliveryVendorVehicleCostType?	=	
	ShipmentSchedule	=	Jadwal Shipment untuk Delivery yang memakai shipment, ga ada hubnya sama CBU
	Relationshipnya tuh ShipmentInvoice 1-* PIB atau PIB 1-* ShipmentInvoice atau ShipmentInvoice 1-1 PIB?	=	PIB 1-* ShipmentInvoice (1 pengajuan bisa banyak Invoice)
	PDCConfig = Sekarang ditake out lagi, gatau nanti masuk lagi atau ngga
	RoutingLocationLeadTime	= Untuk Perhitungan lead time yang make metode Location, jadi mappingan routingmaster untuk lokasi mana leadtimenya berbeda
	Kenapa urusan shift jadi ribet banget yah...?	= gw belom berani ngmg dh untuk ini
	DailyCarCarrierPlan	= Untuk nyimpen info DCCP, data dari excel

- Does CarType.EngineVolume & CarType.WheelDiameter requires decimal point? Can you use INT instead? Or must you use VARCHAR?
	 If you need more than 8 digits (2+8 bytes), might as well use decimal (5 or 9 bytes)
- ShipmentInvoiceDetail.Insurance dan ShipmentInvoiceDetail.Freight, itu uang ato berat ato apaan?
- ShipmentInvoiceDetail.Flag <-- WHAT FLAG????
- By the way, kenapa RoutingDictionary (head/tail) kita PKnya pake VARCHAR bukan INT autoincrement?
- Currency kalo emang gak nempel di Shipment (excel upload), dan Name memang nullable, kenapa gak didelete aja?
- RoutingDictionary can be safely deleted because no more business function
- EstimatedPDDTime in Vehicle table can cause locking (performance penalty)
- DailyCarCarrierPlan.TransInOutDate = Transfer In or Out?
- SuratJalanKeluarBarang.LocationCode = From atau To?
- ParkingArea.IsFTZ apaan?
- TANYA PAK SANTO PASTIKAN AUDIT LIST ITU APAKAH BERBEDA TERGANTUNG KATSU & LOKASI?

*/

---------------------------------------------------------------------------------------------------------------------------------
-- Part 1: Car Master
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Plant
(
	PlantCode VARCHAR(8) PRIMARY KEY, -- ADM, HMI, TMMIN
	[Name] VARCHAR(255) NOT NULL,		
	[Country] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

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

CREATE TABLE [Shift]
(
	ShiftCode VARCHAR(16) PRIMARY KEY,
	[Description] VARCHAR(255),
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Brand
(
	BrandCode VARCHAR(16) PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,		-- Toyota, Daihatsu, Mitsubishi, gitu"

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Sourcing
(
	SourcingId INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(255) NOT NULL,	-- CKD, CBU

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Harmonize
(
	HSCode VARCHAR(16) PRIMARY KEY,

	PphPercentage DECIMAL(19,4) NOT NULL,
	PpnPercentage DECIMAL(19,4) NOT NULL,
	PpnbmPercentage DECIMAL(19,4) NOT NULL,
	
	ValidFrom DATETIME2 NOT NULL,
	ValidUntil DATETIME2 NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE HarmonizeTariff
(
	HarmonizeTariffId INT PRIMARY KEY IDENTITY,
	
	HSCode VARCHAR(16) FOREIGN KEY REFERENCES Harmonize NOT NULL,
	[Schema] VARCHAR(4) NOT NULL,

	BeaMasukPercentage DECIMAL(19,4) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Currency
(
	CurrencySymbol VARCHAR(8) PRIMARY KEY, -- e.g. USD, GBP, JPY.
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE CarModel
(
	CarModelCode VARCHAR(8) PRIMARY KEY NOT NULL, -- FORT, CLYA, UIMV, YRIS, AGYA, RUSH
	BrandCode VARCHAR(16) FOREIGN KEY REFERENCES Brand NULL,	-- NULLABLE for initial data
	PlantCode VARCHAR(8) FOREIGN KEY REFERENCES Plant NULL,		-- NULLABLE for initial data

	[Name] VARCHAR(255) NOT NULL, -- e.g. Fortuner, Avanza

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE CarSeries
(
	CarSeriesCode VARCHAR(16) PRIMARY KEY NOT NULL,	-- FORTB, FORTD ????
	CarModelCode VARCHAR(8) FOREIGN KEY REFERENCES CarModel NOT NULL, 
	
	[Name] VARCHAR(255) NOT NULL, -- FORT Diesel, Fort Bensin

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE CarCategory
(
	CarCategoryCode VARCHAR(16) PRIMARY KEY,

	[Name] VARCHAR(255) NOT NULL,	-- e.g. Mobil Barang, Mobil Penumpang, etc
	[Model] VARCHAR(255) NOT NULL,	-- e.g. Minibus, Chassis, Pick-up, Double Cabin, etc

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)	

CREATE TABLE CarType
(
	Katashiki VARCHAR(32) NOT NULL, -- B100RA-GMQFJ, NGX50L-AHXGXX, F653RM-GMMFJ
	Suffix VARCHAR(4) NOT NULL, -- 11, 62, 00
	PRIMARY KEY (Katashiki, Suffix), -- Natural Superkey that is unique for each Katashiki / Model defined by TAM.

	HSCode VARCHAR(16) FOREIGN KEY REFERENCES Harmonize NULL, -- NULLABLE if not imports
	
	[Name] VARCHAR(255) NOT NULL, -- e.g. FORTUNER SRZ A/T
	EngineDescription VARCHAR(255) NOT NULL,	-- Bensin/4 Silinder Sejajar
	EngineVolume VARCHAR(8) NOT NULL,			-- 1496
	SteerPosition CHAR(1) NOT NULL,				-- L | R
	WheelDiameter VARCHAR(8) NOT NULL,			-- 2750
	WheelSize VARCHAR(32) NOT NULL,				-- 185/60 R15
	[Assembly] VARCHAR(8) NULL,					-- NULLABLE, contoh data : CBU
	
	CarSeriesCode VARCHAR(16) FOREIGN KEY REFERENCES CarSeries NULL,	-- NULLABLE for initial data
	CarCategoryCode VARCHAR(16) FOREIGN KEY REFERENCES CarCategory NULL,	-- NULLABLE for initial data

	IsFTZ BIT NOT NULL DEFAULT 0,	-- Dipakai apabila Katsu ini dipakai untuk batam

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE ExteriorColor
(
	ExteriorColorCode VARCHAR(4) PRIMARY KEY, -- e.g. 4R8
	[IndonesianName] VARCHAR(255) NOT NULL, -- e.g. Putih
	[EnglishName] VARCHAR(255) NOT NULL, -- e.g. Super White

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE InteriorColor
(
	InteriorColorCode VARCHAR(4) PRIMARY KEY, -- e.g. 4R8
	[IndonesianName] VARCHAR(255) NOT NULL, -- e.g. Putih
	[EnglishName] VARCHAR(255) NOT NULL, -- e.g. Super White

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

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

CREATE TABLE EngineMaster
(
	EngineNumber VARCHAR(32) PRIMARY KEY,
	EnginePrefix VARCHAR(8) NOT NULL,
	StatusEngine INT NULL,	-- NULLABLE, Buat TMMIN
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE FrameNumberMaster
(
	FrameNumber VARCHAR(32) PRIMARY KEY,

	IDNumber INT NULL,		-- NULLABLE
	RRN VARCHAR(8) NULL,	-- NULLABLE,
	
	Katashiki VARCHAR(32) NULL,	-- NULLABLE
	Suffix VARCHAR(4) NULL,		-- NULLABLE
	FOREIGN KEY(Katashiki, Suffix) REFERENCES CarType,

	ExteriorColorCode VARCHAR(4) FOREIGN KEY REFERENCES ExteriorColor NULL,	-- NULLABLE
	DTPLOS DATETIME2 NULL,	-- NULLABLE
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part 2: Location Master
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Cluster
(
	ClusterCode VARCHAR(16) PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE CostingCluster
(
	CostingClusterCode VARCHAR(16) PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE City	-- City For Shipment
(
	CityCode VARCHAR(16) PRIMARY KEY, -- ????
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE CityLocation	-- City For CityLeg
(
	CityLocationCode VARCHAR(16) PRIMARY KEY, -- ????
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Region
(
	RegionCode VARCHAR(16) PRIMARY KEY, -- ????
	[Type] VARCHAR(4) NOT NULL, -- KEL
	[Name] VARCHAR(255) NOT NULL,
	
	ParentRegionCode VARCHAR(16) FOREIGN KEY REFERENCES Region NULL, -- NULLABLE
	PostCode VARCHAR(8) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE LocationType
(
	LocationTypeCode VARCHAR(8) PRIMARY KEY, -- CY, BRCH, DD, HO, PDCR, PDCN, PR, Karoseri, Body Paint
	[Name] VARCHAR(255) NOT NULL,

	HasResponsibility BIT NOT NULL DEFAULT 1, -- If TRUE, then this location has responsibility over the vehicle received on this location
	NeedSJKBTarikan BIT NOT NULL DEFAULT 0,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE [Location]
(
	LocationCode VARCHAR(8) PRIMARY KEY, -- ????
	
	[Name] VARCHAR(255) NOT NULL,
	[Address] VARCHAR(255) NULL,	-- Nullable
	
	LocationTypeCode VARCHAR(8) FOREIGN KEY REFERENCES LocationType NOT NULL,
	CityLocationCode VARCHAR(16) FOREIGN KEY REFERENCES CityLocation NULL, 	-- NULLABLE, for initial release because of different sources of master
	City VARCHAR(16) FOREIGN KEY REFERENCES City NULL,	-- NULLABLE, for initial release because of different sources of master

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DealerType
(
	DealerTypeCode VARCHAR(16) PRIMARY KEY,

	[Name] VARCHAR(255) NOT NULL, -- Founder AI, Founder Non AI, Ex Indirect AI, Ex Indirect Non AI
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Dealer -- Will be used for Dealer-specific Accessory Mapping.
(
	DealerCode VARCHAR(16) PRIMARY KEY, -- ????
	MDMDealerId INT NULL,	-- NULLABLE

	DealerTypeCode VARCHAR(16) FOREIGN KEY REFERENCES DealerType NULL,	-- NULLABLE, for initial data

	[Name] VARCHAR(255) NOT NULL,
	[Address] VARCHAR(255) NULL,	-- Nullable

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE UNIQUE NONCLUSTERED INDEX idx_MDMDealerId on Dealer(MDMDealerId) WHERE MDMDealerId IS NOT NULL

CREATE TABLE Company
(
	CompanyCode VARCHAR(16) PRIMARY KEY, -- ????
	MDMCompanyId INT NULL,	-- NULLABLE

	DealerCode VARCHAR(16) FOREIGN KEY REFERENCES Dealer NOT NULL,

	[Name] VARCHAR(255) NOT NULL,
	[NPWPAddress] VARCHAR(255) NULL, -- NULLABLE
	[SAPCode] VARCHAR(8) NULL,	-- NULLABLE, for initial data
	[Phone] VARCHAR(32) NULL, -- NULLABLE
	[Fax] VARCHAR(32) NULL, -- NULLABLE
	[Email] VARCHAR(255) NULL, -- NULLABLE
	[TradeName] VARCHAR(255) NULL, -- NULLABLE
	[NPWP] VARCHAR(32) NULL, -- NULLABLE
	[TermOfPaymentDay] INT NULL,	-- NULLABLE, for initial data

	IsDealerFinancing BIT NOT NULL DEFAULT 0,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE UNIQUE NONCLUSTERED INDEX idx_MDMCompanyId ON Company(MDMCompanyId) WHERE MDMCompanyId IS NOT NULL

CREATE TABLE [Zone]
(
	ZoneId INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE [Destination]
(
	DestinationCode VARCHAR(16) PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE SalesArea
(
	SalesAreaCode VARCHAR(16) PRIMARY KEY,
	[Description] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Branch -- Will be used for ordering a car. (MDP)
(
	BranchCode VARCHAR(16) PRIMARY KEY, -- ????

	SalesAreaCode VARCHAR(16) FOREIGN KEY REFERENCES SalesArea NOT NULL,
	CompanyCode VARCHAR(16) FOREIGN KEY REFERENCES Company NOT NULL, -- Who owns this Branch?
	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES [Location] NULL,	-- NULLABLE, for initial data
	DestinationCode VARCHAR(16) FOREIGN KEY REFERENCES [Destination] NULL,	-- NULLABLE, ga tau gunanya apa, tapi diminta dimasukin dulu sama Pak Yovan + Dasu, Beda dengan branchId diatas	
	RegionCode VARCHAR(16) FOREIGN KEY REFERENCES [Region] NULL,	-- NULLABLE, for initial data
	CostingClusterCode VARCHAR(16) FOREIGN KEY REFERENCES [CostingCluster] NULL,	--	NULLABLE, for initial data
	
	[Name] VARCHAR(255) NOT NULL, -- e.g. AUTO2000
	[Phone] VARCHAR(32) NULL,	-- NULLABLE
	[Fax] VARCHAR(32) NULL,	-- NULLABLE
	
	[BranchAFICode] VARCHAR(4) NOT NULL UNIQUE,
	[BranchAS400] VARCHAR(16) NULL,	-- NULLABLE, for initial data
	[KabupatenCode] VARCHAR(16) NULL,		-- NULLABLE, for initial data
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE UNIQUE NONCLUSTERED INDEX idx_BranchLocationCode ON Branch(LocationCode) WHERE LocationCode IS NOT NULL
CREATE UNIQUE NONCLUSTERED INDEX idx_BranchAS400 ON Branch(BranchAS400) WHERE BranchAS400 IS NOT NULL

CREATE TABLE RegionAFI
(
	RegionAFICode VARCHAR(4) PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

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
-- Part X: Plafond
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [PlafondMaster]
(
	PlafondMasterId INT PRIMARY KEY IDENTITY,
	
	CompanyCode VARCHAR(16) FOREIGN KEY REFERENCES Company NOT NULL,

	Plafond DECIMAL(19,4) NOT NULL,	-- Outstanding + Balance
	Outstanding DECIMAL(19,4) NOT NULL,
	Balance DECIMAL(19,4) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

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

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: Pricing Master
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE PricingType -- ENUMERABLE
(
	PricingTypeId INT PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,	-- Dealer-Type, Cluster-Model Series, Cluster

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE PricingComponent
(
	PricingComponentCode VARCHAR(8) PRIMARY KEY, -- OPEX, SRUT, UP, LOG, SPU
	
	[Name] VARCHAR(255) NOT NULL,
	[PricingTypeId] INT FOREIGN KEY REFERENCES PricingType NOT NULL,

	[ParentPricingComponent] VARCHAR(8) FOREIGN KEY REFERENCES PricingComponent NULL,	-- NULLABLE

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE PricingComponentLookup
(
	PricingComponentLookupId INT PRIMARY KEY IDENTITY,
	
	[PricingComponentCode] VARCHAR(8) FOREIGN KEY REFERENCES PricingComponent NOT NULL,
	
	[Katashiki] VARCHAR(32) NULL,										-- NULLABLE
	[Suffix] VARCHAR(4) NULL,											-- NULLABLE
	FOREIGN KEY(Katashiki, Suffix) REFERENCES CarType,

	CarSeriesCode VARCHAR(16) FOREIGN KEY REFERENCES CarSeries NULL,	-- NULLABLE

	CostingClusterCode VARCHAR(16) FOREIGN KEY REFERENCES CostingCluster NULL,	-- NULLABLE

	[Category] VARCHAR(16) NOT NULL,
	[Price] DECIMAL(19,4) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)
-- OPEX untuk Katsu A
-- OPEX untuk Katsu B
-- OPEX untuk Series A
-- SRUT untuk Series A
-- etc.

CREATE TABLE BranchPricingComponent
(
	BranchPricingComponentId INT PRIMARY KEY IDENTITY,

	BranchCode VARCHAR(16) FOREIGN KEY REFERENCES Branch NOT NULL,
	PricingComponentLookupId INT FOREIGN KEY REFERENCES PricingComponentLookup NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: Discount Master
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE DiscountConfiguration
(
	DiscountConfigurationNumber INT PRIMARY KEY,

	IsBudget BIT NOT NULL DEFAULT 0,			-- Checkbox
	IsPeriod BIT NOT NULL DEFAULT 0,			-- Checkbox
	[Budget] DECIMAL(19,4) NULL,			-- Only If IsBudget
	[StartPeriod] DATETIME2 NULL,	-- Only if IsPeriod
	[EndPeriod] DATETIME2 NULL,		-- Only if IsPeriod
	
	[Amount] DECIMAL(19,4) NOT NULL,
	[AmountRunning] DECIMAL(19,4) NOT NULL,

	DealerCode VARCHAR(16) FOREIGN KEY REFERENCES Dealer NOT NULL,
	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
	FOREIGN KEY(Katashiki, Suffix) REFERENCES CarType,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: Delivery Leg
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE CityLeg
(
	CityLocationLegCode VARCHAR(16) PRIMARY KEY, -- ????

	[Name] VARCHAR(255) NOT NULL,

	CityLocationFrom VARCHAR(16) FOREIGN KEY REFERENCES CityLocation NULL,	-- NULLABLE, for initial release because of different sources of master
	CityLocationTo VARCHAR(16) FOREIGN KEY REFERENCES CityLocation NULL, 	-- NULLABLE, for initial release because of different sources of master

	CalculatingSwappingCost BIT NOT NULL DEFAULT 0,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryMethod
(
	DeliveryMethodCode VARCHAR(16) PRIMARY KEY,	-- CC, SC, SD, SH (tpi takutnya ad yg 3-4 digit, jd buffer aja). SH = Shipment
	[Name] VARCHAR(255) NOT NULL,
	NeedSJKBValidation BIT NOT NULL DEFAULT 0,
	
	ParentDeliveryMethodCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryMethod NULL,	-- NULLABLE, CC Single trip parent = CC

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryLeg
(
	DeliveryLegCode VARCHAR(16) PRIMARY KEY,
	
	[Name] VARCHAR(255) NOT NULL,

	LocationFrom VARCHAR(8) FOREIGN KEY REFERENCES Location NOT NULL,
	LocationTo VARCHAR(8) FOREIGN KEY REFERENCES Location NOT NULL,
	CityLegCode VARCHAR(16) FOREIGN KEY REFERENCES CityLeg NOT NULL,

	BufferMinutes INT NOT NULL,
	NeedSJKB BIT NOT NULL DEFAULT 1,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryVendor
(
	DeliveryVendorCode VARCHAR(16) PRIMARY KEY,

	[Name] VARCHAR(255) NOT NULL,
	[Address] VARCHAR(255) NULL,	-- NULLABLE
	[SAPCode] VARCHAR(8) NULL,		-- NULLABLE
	[Account] VARCHAR(16) NULL,		-- NULLABLE
	
	[LocationCode] VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryLegVendorMappingForAdditionalSJKB
(
	DeliveryLegCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryLeg NOT NULL,
	DeliveryVendorCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryVendor NOT NULL,
	PRIMARY KEY(DeliveryLegCode, DeliveryVendorCode),

	HeadDeliveryLegCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryLeg NOT NULL,
	TailDeliveryLegCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryLeg NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE CityLegCost
(
	CityLegCostCode VARCHAR(16) PRIMARY KEY,

	DeliveryVendorCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryVendor NOT NULL,
	CityLegCode VARCHAR(16) FOREIGN KEY REFERENCES CityLeg NOT NULL,
	DeliveryMethodCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryMethod NOT NULL,
	CarSeriesCode VARCHAR(16) FOREIGN KEY REFERENCES CarSeries NULL,	-- NULLABLE, used if city leg by unit
	[ShiftCode] VARCHAR(16) FOREIGN KEY REFERENCES [Shift] NULL,	-- NULLABLE, used if city leg by ritase,
	[Capacity] INT NULL,	-- NULLABLE, used if city leg by ritase

	ValidDate DATETIME2 NOT NULL,
	Currency VARCHAR(16) NOT NULL,
	Nominal DECIMAL(19,4) NOT NULL,

	NeedAdditionalCityLegCostCode VARCHAR(16) FOREIGN KEY REFERENCES CityLegCost NULL,	-- Nullable. A certain CityLeg with certain DeliveryVendor must create additional Additional.

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryLeadTime
(
	DeliveryLeadTimeId INT PRIMARY KEY IDENTITY,

	DeliveryLegCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryLeg NOT NULL,
	DeliveryMethodCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryMethod NOT NULL,

	--BufferMinutes INT NOT NULL,
	LeadMinutes INT NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryVendorVehicleCostType	-- ENUMERABLE
(
	DeliveryVendorVehicleCostTypeId INT PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,	-- Unit, Ritase - Single Trip, Ritase - Round Trip

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryVendorVehicle -- Kita maintain no polisi mobil-mobil driver car carrier, UI Dropdown - Dasu
(
	DeliveryVendorVehicleId INT PRIMARY KEY IDENTITY,
	
	DeliveryVendorCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryVendor NOT NULL,
	DeliveryMethodCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryMethod NOT NULL,

	[Capacity] INT NULL,	-- NULLABLE, Shipment bisa null dari contoh
	[PoliceNumberOrVesselName] VARCHAR(255) NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryDriver -- Kita maintain nama driver car carrier, UI Dropdown - Dasu
(
	DeliveryDriverCode VARCHAR(16) PRIMARY KEY,
	
	DeliveryVendorCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryVendor NOT NULL,
	DeliveryMethodCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryMethod NOT NULL,

	[Name] VARCHAR(255) NOT NULL,

	[HasSIMA] BIT NOT NULL DEFAULT 0,
	[HasSIMB] BIT NOT NULL DEFAULT 0,
	
	[SIMNumber] VARCHAR(32) NOT NULL,
	[SIMExpirationDate] DATETIME2 NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: Vehicle Import
---------------------------------------------------------------------------------------------------------------------------------
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

---------------------------------------------------------------------------------------------------------------------------------
-- Part 3: Vehicle Routing Master
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE RoutingLeadTimeBy -- ENUMERABLE
(
	RoutingLeadTimeById INT PRIMARY KEY,
	[Name] VARCHAR(32) NOT NULL,	-- PDI, PIO, SPU, Location, Kapal, Leg, Dwell, Nol

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE RoutingMaster
(
	RoutingMasterCode VARCHAR(8) PRIMARY KEY,	-- Can be anything, decided by Accelist. Nanti di C#: if CODE == ... then ...
	[Name] VARCHAR(255) NOT NULL,	-- PIO In, PIO Out

	IsScan BIT NOT NULL,
	RoutingLeadTimeById INT FOREIGN KEY REFERENCES RoutingLeadTimeBy NOT NULL,

	BufferMinutes INT NOT NULL,
	SwappingPoint BIT NOT NULL DEFAULT 0,	-- Flag to indicate whether this routing need its pair

	DoPreDeliveryCenterIn BIT NOT NULL DEFAULT 0,		-- Flag to indicate whether this table will be used to obtain the Predicted PDC In Date
	DoPreDeliveryCenterOut BIT NOT NULL DEFAULT 0,	-- Flag to indicate whether this table will be used to obtain the Predicted PDC Out Date
	DoETABranch BIT NOT NULL DEFAULT 0,				-- Flag to indicate whether this table will be used to obtain the Estimated Branch Date
	DoPredictDelivery BIT NOT NULL DEFAULT 0,			-- Flag to indicate whether this table will be used to obtain the Predicted Delivery Date
	DoMonthlyCarCarrierPlan BIT NOT NULL DEFAULT 0,	-- Flag to indicate whether this table will be used to obtain the Number of vehicle for Monthly Car Carrier Plan
	DoDailyCarCarrierPlan BIT NOT NULL DEFAULT 0,		-- Flag to indicate whether this table will be used to obtain the Number of vehicle for Daily Car Carrier Plan

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

/*
	ProcessOrder column is crucial because process can be amended later but it is the Primary Key must NOT change.
	Therefore, the order of working is sorted ASCENDING in accordance to this column.
	For example, if the current process is:
	1 | 1 | Jig IN	
	2 | 2 | Welding
	3 | 3 | Tosho	
	4 | 4 | Assembly
	5 | 5 | Line-off
	6 | 6 | Delivery

	And then, for example, I want to put a new process between Line-off and Delivery: Cleaning, 
	I can change the working order without modifying the Primary Key!

	1 | 1 | Jig IN	
	2 | 2 | Welding
	3 | 3 | Tosho	
	4 | 4 | Assembly
	5 | 5 | Line-off
	6 | 7 | Delivery
	7 | 6 | Cleaning
*/
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

CREATE TABLE RoutingDictionaryHead
(
	RoutingDictionaryHeadCode VARCHAR(16) PRIMARY KEY,
	[Description] VARCHAR(255) NULL, -- NULLABLE

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE RoutingDictionaryHeadDetail
(
	RoutingDictionaryHeadCode VARCHAR(16) FOREIGN KEY REFERENCES RoutingDictionaryHead NOT NULL,
	Ordering INT NOT NULL,
	PRIMARY KEY (RoutingDictionaryHeadCode, Ordering),
	
	RoutingMasterCode VARCHAR(8) FOREIGN KEY REFERENCES RoutingMaster NOT NULL,

	DeliveryMethodCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryMethod NULL,	-- Nullable
	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE RoutingDictionaryHeadVehicleMapping
(
	RoutingDictionaryHeadVehicleMappingId INT PRIMARY KEY IDENTITY,

	RoutingDictionaryHeadCode VARCHAR(16) FOREIGN KEY REFERENCES RoutingDictionaryHead NOT NULL,

	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
	FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE RoutingDictionaryTail
(
	RoutingDictionaryTailCode VARCHAR(16) PRIMARY KEY,
	[Description] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE RoutingDictionaryTailDetail
(
	RoutingDictionaryTailCode VARCHAR(16) FOREIGN KEY REFERENCES RoutingDictionaryTail NOT NULL,
	Ordering INT NOT NULL,
	PRIMARY KEY (RoutingDictionaryTailCode, Ordering),
	
	RoutingMasterCode VARCHAR(8) FOREIGN KEY REFERENCES RoutingMaster NOT NULL,
	
	DeliveryMethodCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryMethod NULL,	-- Nullable
	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE RoutingDictionaryTailVehicleMapping
(
	RoutingDictionaryTailVehicleMappingId INT PRIMARY KEY IDENTITY,

	RoutingDictionaryTailCode VARCHAR(16) FOREIGN KEY REFERENCES RoutingDictionaryTail NOT NULL,

	BranchCode VARCHAR(16) FOREIGN KEY REFERENCES Branch NOT NULL,

	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
	FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE RoutingDictionary
(
	RoutingDictionaryId INT PRIMARY KEY IDENTITY,
	
	BranchCode VARCHAR(16) FOREIGN KEY REFERENCES Branch NOT NULL,

	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
	FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType,

	ValidFrom DATETIME2 NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE RoutingDictionaryDetail
(
	RoutingDictionaryDetailId INT PRIMARY KEY IDENTITY,

	RoutingDictionaryId INT FOREIGN KEY REFERENCES RoutingDictionary NOT NULL,

	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES Location NOT NULL,
	RoutingMasterCode VARCHAR(8) FOREIGN KEY REFERENCES RoutingMaster NOT NULL,
	DeliveryMethodCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryMethod NULL,	-- NULLABLE, Not all Routing needed Delivery
	
	Ordering INT NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

/*
PDI: Location ini, untuk proses itu, punya line untuk katsu apa saja.
PIO: Location ini, untuk proses itu, punya line dengan nomor sekian untuk katsu apa saja.
Spec Up: Location ini, untuk proses itu, punya line dengan nomor sekian, untuk katsu dari branch apa.

Semua butuh TaktMinutes dan Post tentunya.
*/

CREATE TABLE PDILineDictionary	-- PDI configuration is not per line
(
	PDILineDictionaryId INT PRIMARY KEY IDENTITY,

	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,

	LineNumber VARCHAR(16) NOT NULL UNIQUE,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE PDIKatsuDictionary	-- Katsu configuration for PDI is per location
(
	PDIKatsuDictionaryDetailId INT PRIMARY KEY IDENTITY,

	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,

	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
	FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType,

	TaktSeconds INT NOT NULL,
	Post INT NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE PIOLineDictionary -- What line number for a specific car type, a specific location on PIO
(
	PIOLineDictionaryId INT PRIMARY KEY IDENTITY,

	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,
	
	LineNumber VARCHAR(16) NOT NULL UNIQUE,

	TaktSeconds INT NOT NULL,
	Post INT NOT NULL,

	IsLocked BIT NOT NULL DEFAULT 1,	-- If Locked it will use it configuration, if unlocked it is free for all vehicle

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE PIOLineDictionaryDetail
(
	PIOLineDictionaryDetailId INT PRIMARY KEY IDENTITY,

	PIOLineDictionaryId INT FOREIGN KEY REFERENCES PIOLineDictionary NOT NULL,

	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
	FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE SPULineDictionary -- What line number for a specific car type, a specific location, a specific branch on Spec Up
(
	SPULineDictionaryId INT PRIMARY KEY IDENTITY,

	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,
	
	LineNumber VARCHAR(16) NOT NULL UNIQUE,

	TaktSeconds INT NOT NULL,
	Post INT NOT NULL,

	IsLocked BIT NOT NULL DEFAULT 1,	-- If Locked it will use it configuration, if unlocked it is free for all vehicle

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE SPULineDictionaryDetail
(
	SPULineDictionaryDetailId INT PRIMARY KEY IDENTITY,

	SPULineDictionaryId INT FOREIGN KEY REFERENCES SPULineDictionary NOT NULL,

	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
	FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType,

	BranchCode VARCHAR(16) FOREIGN KEY REFERENCES Branch NOT NULL,	-- NULLABLE, PIO does not use branch

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE ProcessForLine	-- ENUMERABLE
(
	ProcessForLineId INT PRIMARY KEY,
	[Name] VARCHAR(16) NOT NULL,	-- Incomplete, Repair
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

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
	
CREATE TABLE RoutingLocationLeadTime
(
	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES Location NOT NULL,
	RoutingMasterCode VARCHAR(8) FOREIGN KEY REFERENCES RoutingMaster NOT NULL,
	PRIMARY KEY(LocationCode, RoutingMasterCode),

	LeadMinutes INT NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Dwelling	-- Waktu Nginap Kendaraan sebelum dan setelah ke/dari Port
(
	DwellingId INT PRIMARY KEY IDENTITY,

	LocationFrom VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,
	LocationTo VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,

	LeadMinutes INT NOT NULL,	-- Absolute, Not Affected by Idle Time

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: Shift Master
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE WorkingDictionary
(
	WorkingDictionaryCode VARCHAR(16) PRIMARY KEY,
	[Description] VARCHAR(255),
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE WorkingDictionaryDetail
(
	WorkingDictionaryDetailId INT PRIMARY KEY IDENTITY,
	WorkingDictionaryCode VARCHAR(16) FOREIGN KEY REFERENCES WorkingDictionary NOT NULL,

	ShiftCode VARCHAR(16) FOREIGN KEY REFERENCES [Shift] NOT NULL,

	TimeStart DATETIME2 NOT NULL,
	TimeFinish DATETIME2 NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE WorkingDictionaryDayForDetail
(
	WorkingDictionaryDetailId INT FOREIGN KEY REFERENCES [WorkingDictionaryDetail] NOT NULL,
	[Day] INT NOT NULL,	-- 1,2,3,4,5,6,7 Monday to Sunday
	PRIMARY KEY(WorkingDictionaryDetailId, [Day]),
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE WorkingTime
(
	WorkingDictionaryCode VARCHAR(16) FOREIGN KEY REFERENCES WorkingDictionary NOT NULL,
	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES Location NOT NULL,
	PRIMARY KEY(WorkingDictionaryCode, LocationCode),

	ValidFrom DATETIME2 NOT NULL,
	ValidTo DATETIME2 NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE WorkingTimeCustom
(
	WorkingTimeCustomId INT PRIMARY KEY IDENTITY,
	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,	-- Accomodate different operation time for different location.
	
	ShiftCode VARCHAR(16) FOREIGN KEY REFERENCES [Shift] NOT NULL,

	[DateFrom] DATETIME2 NOT NULL,
	[DateTo] DATETIME2 NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE IdleDictionary
(
	IdleDictionaryCode VARCHAR(8) PRIMARY KEY,
	[Description] VARCHAR(128),
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE IdleDictionaryDetail
(
	IdleDictionaryDetailId INT PRIMARY KEY IDENTITY,
	IdleDictionaryCode VARCHAR(8) FOREIGN KEY REFERENCES IdleDictionary NOT NULL,

	ShiftCode VARCHAR(16) FOREIGN KEY REFERENCES [Shift] NOT NULL,

	TimeStart DATETIME2 NOT NULL,
	TimeFinish DATETIME2 NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE IdleDictionaryDayForDetail
(
	IdleDictionaryDetailId INT FOREIGN KEY REFERENCES [IdleDictionaryDetail] NOT NULL,
	[Day] INT NOT NULL, -- 1,2,3,4,5,6,7 Monday to Sunday
	PRIMARY KEY(IdleDictionaryDetailId, [Day]),
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE IdleTime
(
	IdleDictionaryCode VARCHAR(8) FOREIGN KEY REFERENCES IdleDictionary NOT NULL,
	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES Location NOT NULL,
	PRIMARY KEY(IdleDictionaryCode, LocationCode),

	ValidFrom DATETIME2 NOT NULL,
	ValidTo DATETIME2 NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE IdleTimeCustom
(
	IdleTimeCustomId INT PRIMARY KEY IDENTITY,
	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,	-- Accomodate different operation time for different location.
	
	ShiftCode VARCHAR(16) FOREIGN KEY REFERENCES [Shift] NOT NULL,

	[DateFrom] DATETIME2 NOT NULL,
	[DateTo] DATETIME2 NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Holiday
(
	HolidayDate DATETIME2, -- Just store the 'Date' part in the .NET, zero out the time.
	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,
	PRIMARY KEY(HolidayDate, LocationCode),
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part 4: Vehicle Planning & Routing
---------------------------------------------------------------------------------------------------------------------------------
-- Is a blended table of MDP and real physical Vehicle / Car store.
-- This is done because RRN is not unique and requires to be paired with DTPLOD for lookup!
-- FrameNumber is truly unique but does not exist until later on in the process!
-- Therefore vehicle can be looked up using 2 parameters: DTPLOD+RRN or FrameNumber

CREATE TABLE Vehicle
(
	VehicleId INT PRIMARY KEY IDENTITY,

	Katashiki VARCHAR(32) NOT NULL,
	Suffix VARCHAR(4) NOT NULL,
	FOREIGN KEY (Katashiki, Suffix) REFERENCES CarType,
	ExteriorColorCode VARCHAR(4) FOREIGN KEY REFERENCES ExteriorColor NOT NULL,
	InteriorColorCode VARCHAR(4) FOREIGN KEY REFERENCES InteriorColor NOT NULL,
	
	BranchCode VARCHAR(16) FOREIGN KEY REFERENCES Branch NULL,	-- NULLABLE, Branch yg kosong itu punya TAM Stock. Dipake untuk DO Manual. Which Branch orders this car?
	
	Responsibility VARCHAR(8) FOREIGN KEY REFERENCES [Location] NULL,		    -- Where is the current location responsible for this car? Is NULLABLE because when the vehicle first registered PLANT is not responsible, it needs to be scanned on PDI first for the location to be responsible.
	PhysicalLocationCode VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,		-- Physical Name of the Vehicle

	ProductionMonthYear DATETIME2 NOT NULL,	-- Vehicle year MDP, different from DTPLOD.
	
	DTPLOD DATETIME2 NOT NULL,			-- DTPLOD = vehicle's birthday (Estimated initial process start time)
	RRN VARCHAR(8) NOT NULL,			-- This is like a temporary Frame Number. But not unique and can be reused!
	UNIQUE(DTPLOD, RRN),				-- You can use MDPYear and RRN to query vehicles 
	
	FrameNumber VARCHAR(32) NULL,	-- NULLABLE when vehicle has no frame number yet! (Only DTPLOD + RRN) MHFGB8EM0G0408581
	NomorIndukKendaraan VARCHAR(32) NULL,	-- NULLABLE, Nomor Induk Kendaraan

	REVPLOD DATETIME2 NULL,			-- NULLABLE, REVPLOD = Delivery Date. Vehicle's time when finish Assembly Route, used for calculate PDI Completion
	
	TotalLossAt DATETIME2 NULL,		-- NULLABLE: the time when vehicle been written off as total loss due to heavy damage.
	SetUsedAt DATETIME2 NULL,		-- NULLABLE: the time when vehicle has been written off as used due to heavy damage.

	EstimatedPDCIn DATETIME2 NULL,	-- NULLABLE: the time when PDC Receive, latest vehicle routing from planning
	EstimatedPDCOut DATETIME2 NULL,	-- NULLABLE: the time when PDC Out, PDC Receive + PDCConfig.Maintenance Day
	
	EstimatedArrivalBranch DATETIME2 NULL,	-- NULLABLE, From Planning
	ActualArrivalBranch DATETIME2 NULL,		-- NULLABLE, GR from DMS	

	RequestedDeliveryTime DATETIME2 NULL,	-- NULLABLE, Requested PDD from DMS. Harus diatas EstimatedPDCOut
	
	ActualDeliveryTime DATETIME2 NULL,		-- NULLABLE, ADD from DMS (ADD vs PDD)
	DECDate DATETIME2 NULL,					-- NULLABLE, DEC Date from DMS (ADD vs PDD)

	PaketAksesorisTAM VARCHAR(32) NULL,	-- NULLABLE, kiriman DIO dari DMS. Diminta simpan aj

	EngineNumber VARCHAR(32) NULL,	--NULLABLE, For PDI Completion
	EnginePrefix VARCHAR(8) NULL,	--NULLABLE, For PDI Completion
	KeyNumber VARCHAR(32) NULL,		--NULLABLE, For PDI Completion

	SpecialVehicleSign VARCHAR(1) NULL,	-- Nullable, Kebutuhan AS400 untuk force match vehicle asli dgn mdp ? Confirm ulang dengan Pak Santo

	HasCustomer BIT NOT NULL DEFAULT 0,

	IsAdvanceUnit BIT NOT NULL DEFAULT 0,	-- Diflag apabila kendaraan ini sebenernya diplanning untuk MDP hari/bulan setelah"nya namun di hasilkan hari ini
	IsUrgentDeliveryRequest BIT NOT NULL DEFAULT 0,	-- Kalau PDC tempat mesannya melebihi kapasitas untuk hari itu. Maka akan dikirim keesokan harinya
	IsHold BIT NOT NULL DEFAULT 0,
	IsInAudit BIT NOT NULL DEFAULT 0,
	IsInWorkshop BIT NOT NULL DEFAULT 0,

	IsSentMDPToDMS BIT NOT NULL DEFAULT 0,	-- Sent after dms give back confirmation
	IsSentRevisedPDDToDMS BIT NOT NULL DEFAULT 0,
	IsSentDeliveryInfoToDMS BIT NOT NULL DEFAULT 0,
	IsSentBoardPDSToDMS BIT NOT NULL DEFAULT 0,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE UNIQUE NONCLUSTERED INDEX idx_VehicleFrameNumber ON Vehicle(FrameNumber) WHERE FrameNumber IS NOT NULL

CREATE TABLE VehicleRouting
(
	VehicleRoutingId INT PRIMARY KEY IDENTITY,

	VehicleId INT FOREIGN KEY REFERENCES Vehicle NOT NULL,
	RoutingMasterCode VARCHAR(8) FOREIGN KEY REFERENCES RoutingMaster NOT NULL,
	DeliveryMethodCode VARCHAR(16) FOREIGN KEY REFERENCES DeliveryMethod NULL,
	
	LeadMinutes INT NOT NULL,
	Ordering INT NOT NULL,
	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,
	[ShiftCode] VARCHAR(16) FOREIGN KEY REFERENCES [Shift] NOT NULL,
	LineNumber VARCHAR(16) NULL,			-- 0 = no line!

	EstimatedTimeInitial DATETIME2 NOT NULL,	-- Initial process value is copied from DTPLOD. Next process value = previous process value + LeadMinutes
	EstimatedTimeAdjusted DATETIME2 NOT NULL,
	ScanTime DATETIME2 NULL,					-- NULLABLE This is the real time when the process is executed. NULL = not yet scanned

	BufferMinutes INT NOT NULL,
	--DoPredictDelivery BIT NOT NULL, -- Flag to indicate whether this table will be used to obtain the Predicted Delivery Date

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE VehicleHold
(
	VehicleHoldId INT PRIMARY KEY IDENTITY,

	VehicleId INT FOREIGN KEY REFERENCES Vehicle NOT NULL,

	VehicleRoutingId INT FOREIGN KEY REFERENCES VehicleRouting NULL,	-- NULLABLE, Di hold antara routing apa atau location apa
	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES Location NULL,	-- NULLABLE, Di hold antara routing apa atau location apa

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE VehicleLost
(
	VehicleLostId INT PRIMARY KEY IDENTITY,

	VehicleId INT FOREIGN KEY REFERENCES Vehicle NOT NULL UNIQUE,

	IsClaimLostToInsurance BIT NOT NULL,	-- NULLABLE, If False then claim lost to Vendor
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: DCCP
---------------------------------------------------------------------------------------------------------------------------------

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

CREATE TABLE MaintenanceConfigurationPlanning
(
	MaintenanceConfigurationPlanningId INT PRIMARY KEY IDENTITY,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: DeliveryOrder
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [DebitAdvice]	-- DA
(
	DebitAdviceNumber VARCHAR(16) PRIMARY KEY,

	FakturPajakNumber VARCHAR(32) NULL,	-- NULLABLE, Faktur pajak keluarnya belakangan

	IsSentToDMS BIT NOT NULL DEFAULT 0,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryOrder	-- DO
(
	DeliveryOrderNumber VARCHAR(16) PRIMARY KEY,	-- Seharusnya AutoGenerated tapi belom dibongkar, int ampe 9.999.999 mnurut felix
	
	ReferenceNumber VARCHAR(64) NULL,	-- Nullable
	IsManualDO BIT NOT NULL DEFAULT 0,

	DebitAdviceNumber VARCHAR(16) FOREIGN KEY REFERENCES [DebitAdvice] NULL,	-- NULLABLE, Awal" masih null
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryOrderDetail
(
	DeliveryOrderDetailId INT PRIMARY KEY IDENTITY,

	DeliveryOrderNumber VARCHAR(16) FOREIGN KEY REFERENCES DeliveryOrder NOT NULL,
	
	Waiver BIT NOT NULL DEFAULT 0,	-- Buat Cek Plafon, Dasu
	[Date] DATETIME2 NOT NULL,
	
	VehicleId INT FOREIGN KEY REFERENCES Vehicle NOT NULL,
	
	IsDutyFree BIT NOT NULL DEFAULT 0,	-- Default Duty Paid

	NormalPrice DECIMAL(19,4) NOT NULL,
	DiscountPrice DECIMAL(19,4) NOT NULL,
	WPBT DECIMAL(19,4) NOT NULL,
	ValueAddedTax DECIMAL(19,4) NOT NULL,
	LuxuryTax DECIMAL(19,4) NOT NULL,

	PPH22 DECIMAL(19,4) NOT NULL,
	IsPPH22BarangMewah BIT DEFAULT 0 NOT NULL,	-- True, if WPBT > 2.000.000.000 or CC > 3000

	InvoicedPrice DECIMAL(19,4) NOT NULL,

	PlafondId INT FOREIGN KEY REFERENCES PlafondMaster NULL,	-- Nullable. If Delivery Order cancelled, plafond must be returned to how it was

	DOCancellationNumber VARCHAR(16) NULL,	-- Nullable
	DACancellationNumber VARCHAR(16) NULL,	-- Nullable
	CancelledAt DATETIME2 NULL,			-- Nullable

	IsSentToDMS BIT NOT NULL DEFAULT 0,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryOrderPriceDetail	-- Historical DO Price Break Down, Data Duplicated
(
	DeliveryOrderPriceDetailId INT PRIMARY KEY IDENTITY,

	DeliveryOrderDetailId INT FOREIGN KEY REFERENCES DeliveryOrderDetail NOT NULL,

	PricingComponentCode VARCHAR(8) FOREIGN KEY REFERENCES PricingComponent NOT NULL,

	Category VARCHAR(255) NOT NULL,
	Price DECIMAL(19,4) NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

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
-- Part X: "D-X" 1 Master
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE PDCConfig	-- Perlakuan untuk Routing setelah PDC Regional In (to be confirmed buat pemakaianny)
(
	LocationCode VARCHAR(8) PRIMARY KEY FOREIGN KEY REFERENCES Location NOT NULL,	-- PDC nya

	MaintenanceDay INT NOT NULL,
	CarCarrierQuotaPerDay INT NOT NULL,
	NonCarCarrierQuotaPerDay INT NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE PDCConfigDeliveryMethod
(
	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES PDCConfig NOT NULL,
	BranchCode VARCHAR(16) FOREIGN KEY REFERENCES Branch NOT NULL,

	DeliveryMethodCode VARCHAR(16) FOREIGN KEY REFERENCES [DeliveryMethod] NOT NULL,
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: Unit Booking Vessel
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE PreBookVesselLocationMapping
(
	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,
	RoutingMasterCode VARCHAR(8) FOREIGN KEY REFERENCES [RoutingMaster] NOT NULL,
	PRIMARY KEY(LocationCode, RoutingMasterCode),
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE VoyageStatus	-- ENUMERABLE
(
	VoyageStatusId INT PRIMARY KEY,
	[Name] VARCHAR(32),	-- Prebook, Assign, Loading, Depart, Arrival
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Voyage
(
	VoyageNumber VARCHAR(16) PRIMARY KEY,

	DeliveryVendorVehicleId INT FOREIGN KEY REFERENCES DeliveryVendorVehicle NOT NULL,	-- For 2 Dropdown, Only IsShipment
	
	DepartureLocation VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL,
	DepartureDate DATETIME2 NOT NULL,

	VoyageStatusId INT FOREIGN KEY REFERENCES VoyageStatus NOT NULL,

	CancelledAt DATETIME2 NULL,	-- Nullable
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE VoyageDestination
(
	VoyageDestinationId INT PRIMARY KEY IDENTITY,

	VoyageNumber VARCHAR(16) FOREIGN KEY REFERENCES Voyage NOT NULL,
	DestinationCity VARCHAR(16) FOREIGN KEY REFERENCES City NOT NULL,

	EstimatedTimeArrivalDate DATETIME2 NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE VoyageCapacityPerDestination
(
	VoyageCapacityPerDestinationId INT PRIMARY KEY IDENTITY,

	VoyageDestinationId INT FOREIGN KEY REFERENCES VoyageDestination,

	SourceLocationCode VARCHAR(8) FOREIGN KEY REFERENCES Location NOT NULL,
	Capacity INT NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE UnitStatusType	-- ENUMERABLE
(
	UnitStatusTypeId INT PRIMARY KEY,
	[Name] VARCHAR(64) NOT NULL,	-- Planned, Prebook, Ported, Assigned, Loading
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE VehicleAssignmentPerVoyage
(
	VehicleId INT FOREIGN KEY REFERENCES Vehicle NOT NULL,
	VoyageNumber VARCHAR(16) FOREIGN KEY REFERENCES Voyage NOT NULL,
	PRIMARY KEY(VehicleId, VoyageNumber),

	UnitStatusTypeId INT FOREIGN KEY REFERENCES UnitStatusType NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: Swapping
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Swapping
(
	SwappingId INT PRIMARY KEY IDENTITY,

	VehicleId INT FOREIGN KEY REFERENCES Vehicle NOT NULL,

	FromBranch VARCHAR(16) FOREIGN KEY REFERENCES Branch NOT NULL,
	ToBranch VARCHAR(16) FOREIGN KEY REFERENCES Branch NOT NULL,

	[Cost] DECIMAL(19,4) NOT NULL,
	[EstimatedPDD] DATETIME2 NOT NULL,

	[IsApproved] BIT NOT NULL DEFAULT 0,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part 4: Vehicle Parking Master
---------------------------------------------------------------------------------------------------------------------------------

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
CREATE TABLE ParkingRule
(
	ParkingRuleId INT PRIMARY KEY, -- ENUMERABLE
	[Name] VARCHAR(64) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

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

CREATE TABLE AccessoryVendor
(
	AccessoryVendorId INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

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
CREATE TABLE InspectionSide	-- ENUMERABLE
(
	InspectionSideId INT PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,			-- e.g. Front Side, Top Side, Rear Side, LH Side, RH Side, Middle Side

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

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

CREATE TABLE Defect
(
	DefectCode VARCHAR(16) PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

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
-- Part X: Delivery Request
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE DeliveryRequestType	-- ENUMERABLE
(
	DeliveryRequestTypeId INT PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,	-- Normal, Self-pick, Direct Delivery, Transit To Others
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryRequest
(
	DeliveryRequestNumber VARCHAR(32) PRIMARY KEY,	-- 32 Permintaan Pak Yovan, tidak ada example	

	[Date] DATETIME2 NOT NULL,

	VehicleId INT FOREIGN KEY REFERENCES Vehicle NOT NULL,
	DeliveryRequestTypeId INT FOREIGN KEY REFERENCES DeliveryRequestType NOT NULL,
	
	CancelledAt DATETIME2 NULL,	-- NULLABLE
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryRequestTransitType	-- ENUMERABLE
(
	DeliveryRequestTransitTypeId INT PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,	-- Normal, Self-pick to others
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryRequestDetail
(
	DeliveryRequestDetailId INT PRIMARY KEY IDENTITY,

	DeliveryRequestNumber VARCHAR(32) FOREIGN KEY REFERENCES DeliveryRequest NOT NULL,

	EstimatedTimeArrivalBranch DATETIME2 NULL,	-- NULLABLE, filled when deliveryrequesttype is normal

	[PickUpDate] DATETIME2 NULL,		-- NULLABLE, filled when deliveryrequesttype is Self-pick
	[DriverIdIsKTP] BIT NULL,			-- NULLABLE, filled when deliveryrequesttype is Self-pick
	[DriverIdNumber] VARCHAR(32) NULL,	-- NULLABLE, filled when deliveryrequesttype is Self-pick
	[DriverName] VARCHAR(255) NULL,	-- NULLABLE, filled when deliveryrequesttype is Self-pick
	SJKBConfirmationCode VARCHAR(16) FOREIGN KEY REFERENCES [SJKBConfirmation] NULL,	-- NULLABLE, filled when deliveryrequesttype is Self-pick

	EstimatedPDCOut DATETIME2 NULL,	-- NULLABLE, filled when deliveryrequesttype is Direct Delivery
	CustomerName VARCHAR(255) NULL,	-- NULLABLE, filled when deliveryrequesttype is Direct Delivery
	CustomerAddress VARCHAR(MAX) NULL,	-- NULLABLE, filled when deliveryrequesttype is Direct Delivery
	CustomerCity VARCHAR(255) NULL,	-- NULLABLE, filled when delivery	requesttype is Direct Delivery
	SalesmanName VARCHAR(255) NULL,	-- NULLABLE, filled when deliveryrequesttype is Direct Delivery
	SalesmanContactNumber VARCHAR(255) NULL,	-- NULLABLE, filled when deliveryrequesttype is Direct Delivery

	[WorkshopCode] VARCHAR(8) FOREIGN KEY REFERENCES [Location] NULL, -- NULLABLE, filled when deliveryrequesttype is Transit To Others. Only Karoseri, BodyPaint, Borrowing
	[LeadMinutes] INT NULL, -- NULLABLE, filled when deliveryrequesttype is Transit To Others
	DeliveryRequestTransitTypeId INT FOREIGN KEY REFERENCES DeliveryRequestTransitType NULL, -- NULLABLE, filled when deliveryrequesttype is Transit To Others
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryRequestTransitDetailType	-- ENUMERABLE
(
	DeliveryRequestTransitDestinationTypeId INT PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,	-- Normal - Re	turn To PDC, Normal - Return To Other PDC, Normal - Self-pick From others, Self-pick to others
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE DeliveryRequestTransitDetail
(
	DeliveryRequestTransitDetailId INT PRIMARY KEY IDENTITY,

	DeliveryRequestDetailId INT FOREIGN KEY REFERENCES [DeliveryRequestDetail] NOT NULL,
	
	DeliveryRequestTransitDetailTypeId INT FOREIGN KEY REFERENCES [DeliveryRequestTransitDetailType] NOT NULL,

	[OtherPDCLocationCode] VARCHAR(8) FOREIGN KEY REFERENCES [Location] NULL,	-- NULLABLE, filled when DeliveryRequestTransitDestinationType is Normal - Return to Other PDC

	[PickUpDate] DATETIME2 NULL,		-- NULLABLE, filled when deliveryrequesttype is Normal - Self-pick To Others
	[DriverIdIsKTP] BIT NULL,			-- NULLABLE, filled when deliveryrequesttype is Normal - Self-pick To Others
	[DriverIdNumber] VARCHAR(32) NULL,	-- NULLABLE, filled when deliveryrequesttype is Normal - Self-pick To Others
	[DriverName] VARCHAR(255) NULL,	-- NULLABLE, filled when deliveryrequesttype is Self-pick and Transit To Others
	SJKBConfirmationCode VARCHAR(16) FOREIGN KEY REFERENCES [SJKBConfirmation] NULL,	-- NULLABLE, filled when DeliveryRequestType is Self-pick To Other
	ReturnPDCDate DATETIME2 NULL,	-- NULLABLE, filled when DeliveryRequestType is Self-pick To Other
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part 8: AFI (Application For Invoice)
---------------------------------------------------------------------------------------------------------------------------------

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

CREATE TABLE AFIApplicationProcess -- ENUMERABLE
(
	AFIApplicationProcessId INT PRIMARY KEY,
	[Name] VARCHAR(255), -- Sebelum pengajuan bea cukai, etc (Aju Baru, Revisi AFI, Batal AFI, AJu Eks Batal)

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Customer
(
	CustomerId INT PRIMARY KEY IDENTITY,

	[Name] VARCHAR(255) NOT NULL,
	[KTP] VARCHAR(32) NOT NULL,
	[Address1] VARCHAR(255) NOT NULL,	-- Alamat + RT/RW
	[Address2] VARCHAR(255) NOT NULL,	-- Kelurahan
	[Address3] VARCHAR(255) NOT NULL,	-- Kecamatan
	City VARCHAR(32) NOT NULL,
	Province VARCHAR(32) NOT NULL,
	PostalCode VARCHAR(8) NOT NULL,

	RegionAFICode VARCHAR(4) FOREIGN KEY REFERENCES RegionAFI NOT NULL,	-- Use Field RegionCodeAFI

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE AFISubmissionType	-- ENUMERABLE
(
	AFISubmissionTypeCode VARCHAR(16) PRIMARY KEY,	-- Normal, REVA, REVB, REVC, REVD, REVE, REVF, Cancelled
	[Name] VARCHAR(255) NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE AFIApplication
(
	AFIApplicationId INT PRIMARY KEY IDENTITY,

	AFIApplicationNumber VARCHAR(32) NULL,	-- NULLABLE, Sewaktu di create tidak langsung terbit Application Numbernya
	AFIReferenceNumber VARCHAR(32) NULL,	-- NULLABLE, bisa ada bisa tidak
	AFIApplicationProcessId INT FOREIGN KEY REFERENCES AFIApplicationProcess NOT NULL,

	[Timestamp] DATETIME2 NOT NULL,
	[EffectiveDate] DATETIME2 NOT NULL,
	[Description] VARCHAR(255) NOT NULL,

	FakturNumber VARCHAR(32) FOREIGN KEY REFERENCES Faktur NULL, -- NULLABLE

	BranchAFICode VARCHAR(4) FOREIGN KEY REFERENCES Branch(BranchAFICode) NOT NULL,
	CustomerId INT FOREIGN KEY REFERENCES Customer NOT NULL,
	VehicleId INT FOREIGN KEY REFERENCES Vehicle NOT NULL,
	
	AFISubmissionTypeCode VARCHAR(16) FOREIGN KEY REFERENCES AFISubmissionType NOT NULL,
	
	[ChassisModel] VARCHAR(16) NULL,	-- NULLABLE, Only filled when CarCategory is Chassis
	[CarColor] VARCHAR(32) NULL,		-- NULLABLE, Only filled when AFISubmissionTypeCode is Revision for Color (tanya tato)
	[CarCategory] VARCHAR(32) NULL,		-- NULLABLE, Only filled when AFISubmissionTypeCode is Revision for Jenis Mobil (tanya tato)

	DocumentSentAt DATETIME2 NULL,		-- NULLABLE, Info will be received later from DMS
	DocumentReceivedAt DATETIME2 NULL,	-- NULLABLE, Info will be received later from DMS
	STNKAjuAt DATETIME2 NULL,			-- NULLABLE, Info will be received later from DMS
	STNKReceivedAt DATETIME2 NULL,		-- NULLABLE, Info will be received later from DMS
	
	TamReceivedAt DATETIME2 NULL,	-- NULLABLE
	IsLatestSubmission BIT NOT NULL DEFAULT 1,	-- Submission can be revised, revised submission create new submission. Old Submission become false but needed as history

	IsSTNKInfoSentToDMS BIT NOT NULL DEFAULT 0,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE AFIApplicationNumberSequential
(
	BranchAFICode VARCHAR(4) FOREIGN KEY REFERENCES Branch(BranchAFICode) NOT NULL,
	[Year] INT NOT NULL,
	PRIMARY KEY(BranchAFICode, [Year]),

	SequentialNumber INT NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE AFIRestrictionRegion
(
	AFIRestrictionId INT PRIMARY KEY IDENTITY,

	RegionAFICode VARCHAR(4) FOREIGN KEY REFERENCES [RegionAFI] NULL,	-- NULLABLE

	IsLocked BIT NOT NULL DEFAULT 0,

	ValidFrom DATETIME2 NOT NULL,
	ValidTo DATETIME2 NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

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

CREATE TABLE SuratPengantarFaktur
(
	NomorSuratPengantarFaktur VARCHAR(32) PRIMARY KEY,
	[ProcessDate] DATETIME2 NOT NULL,	-- Process Date dari WXDATA1
	[OutDate] DATETIME2 NOT NULL,		-- Out Date dari WXDATA1

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE SuratPengantarFakturDetail	-- 1 SPF punya banyak Frame Number
(
	NomorSuratPengantarFaktur VARCHAR(32) FOREIGN KEY REFERENCES SuratPengantarFaktur,
	VehicleId INT FOREIGN KEY REFERENCES Vehicle,
	PRIMARY KEY(NomorSuratPengantarFaktur, VehicleId),

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part X: Scratch
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE ScratchConfiguration
(
	BranchCode VARCHAR(16) FOREIGN KEY REFERENCES Branch NOT NULL,
	CarModelCode VARCHAR(8) FOREIGN KEY REFERENCES CarModel NOT NULL,
	
	PRIMARY KEY(BranchCode, CarModelCode),

	[NumberOfScratch] INT NOT NULL,	-- Jumlah "gesek/scratch" yang dibutuhkan untuk Mobil Impor/Lokal dengan Branch Auto2000/NonAuto2000
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE ScratchHandOver
(
	ScratchHandOverNumber VARCHAR(16) PRIMARY KEY,

	[Date] DATETIME2 NOT NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Scratch
(
	ScratchId INT PRIMARY KEY IDENTITY,

	VehicleId INT FOREIGN KEY REFERENCES Vehicle NOT NULL,				-- Which Frame Number to be scratched
	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES Location NOT NULL,	-- Scratch Location

	ScratchedAt DATETIME2 NOT NULL, --the time when vehicle frame number has been "gesek/scratch" for STNK, on last PDC before sent to Branch.

	ScratchHandOverNumber VARCHAR(16) FOREIGN KEY REFERENCES ScratchHandOver NULL,	-- Nullable, is not filled until later
	
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
-- Part 9: Others
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE UserMapping 
(
	Username VARCHAR(256) PRIMARY KEY, -- Fuckyeah TAM Passport! o/**\o User's Roles will be received from TAM Passport.
	LocationCode VARCHAR(8) FOREIGN KEY REFERENCES [Location] NOT NULL, -- When user is signing into Mobile App, send his location. (Deny login when data not available)

	ShiftCode VARCHAR(16) FOREIGN KEY REFERENCES [Shift] NULL,	-- NULLABLE

	PDILineNumber VARCHAR(16) FOREIGN KEY REFERENCES PDILineDictionary(LineNumber) NULL,	-- NULLABLE
	PIOLineNumber VARCHAR(16) FOREIGN KEY REFERENCES PIOLineDictionary(LineNumber) NULL,	-- NULLABLE
	SPULineNumber VARCHAR(16) FOREIGN KEY REFERENCES SPULineDictionary(LineNumber) NULL,	-- NULLABLE
	
	DeliveryVendorCode VARCHAR(16) FOREIGN KEY REFERENCES [DeliveryVendor] NULL,	-- NULLABLE

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE AppRole -- Hard defined in accordance to Role value obtained from PASSSPORT
(
	AppRoleName VARCHAR(16) PRIMARY KEY,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE AppMenu -- Hard defined in accordance to value in MVC [Authorize] Attribute, Role Property
(
	AppMenuName VARCHAR(16) PRIMARY KEY,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE AppRoleMenuMapping
(
	AppRoleName VARCHAR(16) FOREIGN KEY REFERENCES AppRole NOT NULL,
	AppMenuName VARCHAR(16) FOREIGN KEY REFERENCES AppMenu NOT NULL,
	PRIMARY KEY(AppMenuName, AppRoleName),

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

---------------------------------------------------------------------------------------------------------------------------------
-- Part 10: Log Upload Download
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE LogUploadDownload
(
	LogUploadDownloadId INT PRIMARY KEY IDENTITY,

	[JobId] INT NULL,
	[Module] VARCHAR(32) NULL,	-- MASTER, DELIVERY
	[Menu] VARCHAR(255) NULL,	-- Download Branch, Upload Branch
	[IsUploadProcess] BIT NOT NULL DEFAULT 0,	-- If false, process is download
	[StartTime] DATETIME2 NULL,
	[EndTime] DATETIME2 NULL,
	[FileName] VARCHAR(255) NULL,
	[Status] VARCHAR(16) NULL,	-- On Process, Success, Failed
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE LogUploadDownloadFile
(
	LogUploadDownloadId INT PRIMARY KEY FOREIGN KEY REFERENCES [LogUploadDownload] ON DELETE CASCADE,

	[Blob] VARBINARY(MAX) NULL,
	
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)
