-----------------------------------------------------------------
-- Branch
-----------------------------------------------------------------

INSERT INTO Branch (BranchCode, MasterDataPrimaryKey, CompanyCode, SalesAreaCode,  DestinationCode, RegionCode, AS400ClusterCode, AS400BranchCode, [Name], [Phone], [Fax], [KabupatenCode], CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('AGT001262', '16', '3005', '3', '3065', NULL, '21', '3065', 'Agung Toyota Pekanbaru IV (Harapan Raya)', NULL, NULL, NULL,GETDATE(), 'YOVAN', GETDATE(), 'YOVAN') 
INSERT INTO Branch (BranchCode, MasterDataPrimaryKey, CompanyCode, SalesAreaCode,  DestinationCode, RegionCode, AS400ClusterCode, AS400BranchCode, [Name], [Phone], [Fax], [KabupatenCode], CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('AUT001281', '48', '1001', '4', '1516', NULL, '10', '1516', 'AUTO2000 Ahmad Yani Banjarmasin', NULL, NULL, NULL,GETDATE(), 'YOVAN', GETDATE(), 'YOVAN') 
INSERT INTO Branch (BranchCode, MasterDataPrimaryKey, CompanyCode, SalesAreaCode,  DestinationCode, RegionCode, AS400ClusterCode, AS400BranchCode, [Name], [Phone], [Fax], [KabupatenCode], CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('AUT001283', '84', '1001', '2', '1204', NULL, '05', '1204', 'AUTO2000 Jatiwangi Majalengka', NULL, NULL, NULL,GETDATE(), 'YOVAN', GETDATE(), 'YOVAN') 
INSERT INTO Branch (BranchCode, MasterDataPrimaryKey, CompanyCode, SalesAreaCode,  DestinationCode, RegionCode, AS400ClusterCode, AS400BranchCode, [Name], [Phone], [Fax], [KabupatenCode], CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('AUT001280', '104', '1001', '4', '1307', NULL, '07', '1307', 'AUTO2000 Manyar Gresik', NULL, NULL, NULL,GETDATE(), 'YOVAN', GETDATE(), 'YOVAN') 
INSERT INTO Branch (BranchCode, MasterDataPrimaryKey, CompanyCode, SalesAreaCode,  DestinationCode, RegionCode, AS400ClusterCode, AS400BranchCode, [Name], [Phone], [Fax], [KabupatenCode], CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('AUT001284', '134', '1001', '2', '1205', NULL, '03', '1205', 'AUTO2000 Sukabumi', NULL, NULL, NULL,GETDATE(), 'YOVAN', GETDATE(), 'YOVAN') 
INSERT INTO Branch (BranchCode, MasterDataPrimaryKey, CompanyCode, SalesAreaCode,  DestinationCode, RegionCode, AS400ClusterCode, AS400BranchCode, [Name], [Phone], [Fax], [KabupatenCode], CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('AUT001113', '143', '1001', '1', 'TIRA', NULL, '01', '1007', 'AUTO2000 Wahid Hasyim', NULL, NULL, NULL,GETDATE(), 'YOVAN', GETDATE(), 'YOVAN') 
INSERT INTO Branch (BranchCode, MasterDataPrimaryKey, CompanyCode, SalesAreaCode,  DestinationCode, RegionCode, AS400ClusterCode, AS400BranchCode, [Name], [Phone], [Fax], [KabupatenCode], CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('HAK001288', '166', '6010', '5', '6034', NULL, '43', '6034', 'Hadji Kalla Palu II', NULL, NULL, NULL,GETDATE(), 'YOVAN', GETDATE(), 'YOVAN') 
INSERT INTO Branch (BranchCode, MasterDataPrimaryKey, CompanyCode, SalesAreaCode,  DestinationCode, RegionCode, AS400ClusterCode, AS400BranchCode, [Name], [Phone], [Fax], [KabupatenCode], CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('AUT001289', '274', '1001', '3', '1401', NULL, '15', '1401', 'AUTO2000 Tulang Bawang', NULL, NULL, NULL,GETDATE(), 'YOVAN', GETDATE(), 'YOVAN') 
INSERT INTO Branch (BranchCode, MasterDataPrimaryKey, CompanyCode, SalesAreaCode,  DestinationCode, RegionCode, AS400ClusterCode, AS400BranchCode, [Name], [Phone], [Fax], [KabupatenCode], CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('AUT001291', '275', '1001', '3', '1400', NULL, '12', '1400', 'AUTO2000 Binjai', NULL, NULL, NULL,GETDATE(), 'YOVAN', GETDATE(), 'YOVAN') 
INSERT INTO Branch (BranchCode, MasterDataPrimaryKey, CompanyCode, SalesAreaCode,  DestinationCode, RegionCode, AS400ClusterCode, AS400BranchCode, [Name], [Phone], [Fax], [KabupatenCode], CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('ANT001300', '284', 'AAPK', '4', 'AST', NULL, '00', 'AST', 'Anzon Toyota Sintang', NULL, NULL, NULL,GETDATE(), 'YOVAN', GETDATE(), 'YOVAN') 

-----------------------------------------------------------------
-- BranchLocationMapping
-----------------------------------------------------------------

INSERT INTO BranchLocationMapping (BranchCode, LocationCode, CreatedAt, CreatedBy) VALUES('AGT001262', 'BRI3065', GETDATE(), 'YOVAN')
INSERT INTO BranchLocationMapping (BranchCode, LocationCode, CreatedAt, CreatedBy) VALUES('AUT001281', 'BKL1516', GETDATE(), 'YOVAN')
INSERT INTO BranchLocationMapping (BranchCode, LocationCode, CreatedAt, CreatedBy) VALUES('AUT001283', 'BBD1204', GETDATE(), 'YOVAN')
INSERT INTO BranchLocationMapping (BranchCode, LocationCode, CreatedAt, CreatedBy) VALUES('AUT001280', 'BSB1307', GETDATE(), 'YOVAN')
INSERT INTO BranchLocationMapping (BranchCode, LocationCode, CreatedAt, CreatedBy) VALUES('AUT001284', 'BSK1205', GETDATE(), 'YOVAN')
INSERT INTO BranchLocationMapping (BranchCode, LocationCode, CreatedAt, CreatedBy) VALUES('AUT001113', 'BJK1007', GETDATE(), 'YOVAN')
INSERT INTO BranchLocationMapping (BranchCode, LocationCode, CreatedAt, CreatedBy) VALUES('HAK001288', 'BSC6034', GETDATE(), 'YOVAN')
INSERT INTO BranchLocationMapping (BranchCode, LocationCode, CreatedAt, CreatedBy) VALUES('AUT001289', 'BLP1401', GETDATE(), 'YOVAN')
INSERT INTO BranchLocationMapping (BranchCode, LocationCode, CreatedAt, CreatedBy) VALUES('AUT001291', 'BME1400', GETDATE(), 'YOVAN')
INSERT INTO BranchLocationMapping (BranchCode, LocationCode, CreatedAt, CreatedBy) VALUES('ANT001300', 'BKLAST', GETDATE(), 'YOVAN')
INSERT INTO BranchLocationMapping (BranchCode, LocationCode, CreatedAt, CreatedBy) VALUES('HAK001306', 'BSS6035', GETDATE(), 'YOVAN')

-----------------------------------------------------------------
-- AFIBranch
-----------------------------------------------------------------

INSERT INTO AFIBranch (AFIBranchCode, BranchCode, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('3065', 'AGT001262', GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')
INSERT INTO AFIBranch (AFIBranchCode, BranchCode, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('1516', 'AUT001281', GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')
INSERT INTO AFIBranch (AFIBranchCode, BranchCode, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('1204', 'AUT001283', GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')
INSERT INTO AFIBranch (AFIBranchCode, BranchCode, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('1307', 'AUT001280', GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')
INSERT INTO AFIBranch (AFIBranchCode, BranchCode, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('1205', 'AUT001284', GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')
INSERT INTO AFIBranch (AFIBranchCode, BranchCode, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('1007', 'AUT001113', GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')
INSERT INTO AFIBranch (AFIBranchCode, BranchCode, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('6034', 'HAK001288', GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')
INSERT INTO AFIBranch (AFIBranchCode, BranchCode, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('1401', 'AUT001289', GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')
INSERT INTO AFIBranch (AFIBranchCode, BranchCode, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('1400', 'AUT001291', GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')
INSERT INTO AFIBranch (AFIBranchCode, BranchCode, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES ('AST', 'ANT001300', GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')

-----------------------------------------------------------------
-- CityLeg
-----------------------------------------------------------------

INSERT INTO CityLeg (CityLegCode, [Name], CityFrom , CityTo, CalculatingSwappingCost, CreatedAt,CreatedBy,UpdatedAt,UpdatedBy) VALUES ('CITY0734', 'CIKAMPEK - Marunda', 'CIKA', 'MARU', 0, GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')
INSERT INTO CityLeg (CityLegCode, [Name], CityFrom , CityTo, CalculatingSwappingCost, CreatedAt,CreatedBy,UpdatedAt,UpdatedBy) VALUES ('CITY0763', 'Palu Kombos - Dalam Kota', 'PAKO', 'DLKO', 0, GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')
INSERT INTO CityLeg (CityLegCode, [Name], CityFrom , CityTo, CalculatingSwappingCost, CreatedAt,CreatedBy,UpdatedAt,UpdatedBy) VALUES ('CITY0810', 'Port Priok - DUMAI', 'PRIK', 'DUMA', 0, GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')
INSERT INTO CityLeg (CityLegCode, [Name], CityFrom , CityTo, CalculatingSwappingCost, CreatedAt,CreatedBy,UpdatedAt,UpdatedBy) VALUES ('CITY0842', 'Port Priok - Luwuk', 'PRIK', 'LUWU', 0, GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')

-----------------------------------------------------------------
-- DeliveryLeg
-----------------------------------------------------------------

INSERT INTO DeliveryLeg (DeliveryLegCode, [Name], LocationFrom, LocationTo, CityLegCode, BufferMinutes, NeedSJKB, CreatedAt,CreatedBy,UpdatedAt,UpdatedBy) VALUES ('NPUCKMPSJKMRND', 'PDC Cikampek-Storage Marunda', 'NPUCKMP', 'SJKMRND', 'CITY0734', 0, 1, GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')

-----------------------------------------------------------------
-- DeliveryVendor
-----------------------------------------------------------------

INSERT INTO DeliveryVendor (DeliveryVendorCode, [Name], [Address], [SAPCode], [Account], [LocationCode], CreatedAt,CreatedBy,UpdatedAt,UpdatedBy) VALUES ('SMM', 'PT. SUMBER MAS MOTOR', '', '252503', '', 'SPSISTA', GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')

-----------------------------------------------------------------
-- DeliveryLeadTime
-----------------------------------------------------------------

INSERT INTO DeliveryLeadTime (DeliveryLegCode, DeliveryMethodCode, LeadMinutes, CreatedAt,CreatedBy,UpdatedAt,UpdatedBy) VALUES ('NPUCKMPSJKMRND', 'CC', 568, GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')  
INSERT INTO DeliveryLeadTime (DeliveryLegCode, DeliveryMethodCode, LeadMinutes, CreatedAt,CreatedBy,UpdatedAt,UpdatedBy) VALUES ('NPUCKMPSJKCKNG', 'SC', 733, GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')  
INSERT INTO DeliveryLeadTime (DeliveryLegCode, DeliveryMethodCode, LeadMinutes, CreatedAt,CreatedBy,UpdatedAt,UpdatedBy) VALUES ('NPUCKMPSJKCKNG', 'SH', 796, GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')  
INSERT INTO DeliveryLeadTime (DeliveryLegCode, DeliveryMethodCode, LeadMinutes, CreatedAt,CreatedBy,UpdatedAt,UpdatedBy) VALUES ('NPUCKMPSJKCKNG', 'CC', 118, GETDATE(), 'YOVAN', GETDATE(), 'YOVAN') 
INSERT INTO DeliveryLeadTime (DeliveryLegCode, DeliveryMethodCode, LeadMinutes, CreatedAt,CreatedBy,UpdatedAt,UpdatedBy) VALUES ('NPUCKMPSJKCKNG', 'SD', 792, GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')  
INSERT INTO DeliveryLeadTime (DeliveryLegCode, DeliveryMethodCode, LeadMinutes, CreatedAt,CreatedBy,UpdatedAt,UpdatedBy) VALUES ('NPUCKMPSJKMRND', 'SC', 669, GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')
INSERT INTO DeliveryLeadTime (DeliveryLegCode, DeliveryMethodCode, LeadMinutes, CreatedAt,CreatedBy,UpdatedAt,UpdatedBy) VALUES ('NPUCKMPSJKMRND', 'SH', 136, GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')  
INSERT INTO DeliveryLeadTime (DeliveryLegCode, DeliveryMethodCode, LeadMinutes, CreatedAt,CreatedBy,UpdatedAt,UpdatedBy) VALUES ('NPUCKMPSJKMRND', 'SD', 575, GETDATE(), 'YOVAN', GETDATE(), 'YOVAN')
