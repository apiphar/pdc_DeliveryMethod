--------------------------------------------------------------------------------------------------------------------------------
-- Send Status MDP
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE VehicleMDPStatus
(
	RRN VARCHAR(8) NOT NULL,
	DTPLOD DATETIME2 NOT NULL,
	PRIMARY KEY(RRN, DTPLOD),

	[Status] BIT NOT NULL DEFAULT 1,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

--------------------------------------------------------------------------------------------------------------------------------
-- Send Revisi PDD from TLS
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE VehicleRevisePDDStatus
(
	[RRN] VARCHAR(8) NOT NULL,
	[DTPLOD] DATETIME2 NOT NULL,
	PRIMARY KEY(RRN, DTPLOD),

	[Status] BIT NOT NULL DEFAULT 1,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

--------------------------------------------------------------------------------------------------------------------------------
-- Send Get Delivery Info Status from TLS
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE DeliveryInfoStatus
(
	[RRN] VARCHAR(8) NOT NULL,
	[DTPLOD] DATETIME2 NOT NULL,
	PRIMARY KEY(RRN, DTPLOD),

	[FrameNumber] VARCHAR(32) NULL UNIQUE,

	[Status] BIT NOT NULL DEFAULT 1,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE UNIQUE NONCLUSTERED INDEX idx_DeliveryFrameNumber ON DeliveryInfoStatus(FrameNumber) WHERE FrameNumber IS NOT NULL

--------------------------------------------------------------------------------------------------------------------------------
-- Send Good Received
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE GoodReceived
(
	[FrameNumber] VARCHAR(32) PRIMARY KEY,
	[GRDate] DATETIME2 NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

--------------------------------------------------------------------------------------------------------------------------------
-- Send TAM Distribusi
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE SuratPengantarFakturStatus
(
	[FrameNumber] VARCHAR(32) PRIMARY KEY,
	[Status] BIT NOT NULL DEFAULT 1,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

--------------------------------------------------------------------------------------------------------------------------------
-- Status Penerimaan data Info STNK
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE STNKStatus
(
	[FrameNumber] VARCHAR(32) PRIMARY KEY,
	[Status] BIT NOT NULL DEFAULT 1,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

--------------------------------------------------------------------------------------------------------------------------------
-- PDD Versus ADD Report
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE PDDVsADDReport
(
	[FrameNumber] VARCHAR(32) PRIMARY KEY,
	[ActualDeliveryCustomer] DATETIME2 NOT NULL,
	[DECDate] DATETIME2 NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)