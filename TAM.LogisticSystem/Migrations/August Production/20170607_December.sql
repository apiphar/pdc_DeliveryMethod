---------------------------------------------------------------------------------------------------------------------------------
-- Import / CBU
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE HarmonizeTariff
(
	HSCode VARCHAR(16) CONSTRAINT FK_HarmonizeTariff_HSCode FOREIGN KEY REFERENCES Harmonize NOT NULL,
	[Schema] VARCHAR(4) NOT NULL,
	CONSTRAINT PK_HarmonizeTariff PRIMARY KEY (HSCode, [Schema]),

	BeaMasukPercentage DECIMAL(19,4) NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
	UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
)

CREATE TABLE Harmonize
(
	HSCode VARCHAR(16) CONSTRAINT PK_Harmonize PRIMARY KEY,

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

ALTER TABLE CarType ADD 
HSCode VARCHAR(16) CONSTRAINT FK_CarType_Harmonize FOREIGN KEY REFERENCES Harmonize NULL, -- NULLABLE if not imports