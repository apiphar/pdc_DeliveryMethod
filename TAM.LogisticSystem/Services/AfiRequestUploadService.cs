//using Dapper;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Storage;
//using OfficeOpenXml;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Threading.Tasks;
//using TAM.LogisticSystem.Entities;
//using TAM.LogisticSystem.Models;
//using OfficeOpenXml.Style;
//using System.Drawing;

//namespace TAM.LogisticSystem.Services
//{
//    public class AfiRequestUploadService
//    {
//        private readonly LogisticDbContext LogisticDbContext;
//        private readonly WebEnvironmentService WebEnvironmentService;
//        public delegate bool TryParseHandler<T>(string value, out T result);
//        private AfiUploadHashSet AfiUploadSet { get; set; }
//        private List<string> ExcelFrameList { get; set; }
//        public AfiRequestUploadService(LogisticDbContext logisticDbContext,WebEnvironmentService webEnvironmentService)
//        {
//            this.LogisticDbContext = logisticDbContext;
//            this.WebEnvironmentService = webEnvironmentService;
//        }

//        // TIE: START
//        public async Task<string> GetBranchCodeAFIById(string branchCode)
//        {
//            var branch = await this.LogisticDbContext.Branch.FindAsync(branchCode);
//            return branch.BranchCode;
//        }
//        public byte[] ExportExcelFromList(List<string> header)
//        {
//            byte[] result; 
//            using (var package = new ExcelPackage())
//            {
//                GenerateWorkSheet(package,header);
//                GenerateWorkSheetInformationSchema(package, GetInformationSchema());
//                result = package.GetAsByteArray();
//            }
//            return result;
//        }
//        public void GenerateWorkSheet(ExcelPackage package, List<string> header)
//        {
//            var heading = "AFI Request Upload";
//            var worksheet = package.Workbook.Worksheets.Add(heading);
//            var startRowFrom = 3;
//            var columnIndex = 1;
//            foreach(var column in header)
//            {
//                worksheet.Cells[3, columnIndex++].Value = column;
//            }
//            // add the content into the Excel file 

//            // autofit width of cells with small content 
//            columnIndex = 1;
//            foreach (var column in header)
//            {
//                var maxLength = column.Length;
//                if (maxLength < 150)
//                {
//                    worksheet.Column(columnIndex).AutoFit();
//                }
//                columnIndex++;
//            }

//            // format header - bold, yellow on black 
//            using (var r = worksheet.Cells[startRowFrom, 1, startRowFrom, header.Count])
//            {
//                r.Style.Font.Color.SetColor(Color.White);
//                r.Style.Font.Bold = true;
//                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
//                r.Style.Fill.BackgroundColor.SetColor(Color.Black);
//            }

//            // format cells - add borders 
//            using (var r = worksheet.Cells[startRowFrom + 1, 1, startRowFrom + 1, header.Count])
//            {
//                r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
//                r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//                r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
//                r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

//                r.Style.Border.Top.Color.SetColor(Color.Black);
//                r.Style.Border.Bottom.Color.SetColor(Color.Black);
//                r.Style.Border.Left.Color.SetColor(Color.Black);
//                r.Style.Border.Right.Color.SetColor(Color.Black);
//            }
//            worksheet.Cells["A1"].Value = heading;
//            worksheet.Cells["A1"].Style.Font.Size = 20;

//            worksheet.InsertColumn(1, 1);
//            worksheet.InsertRow(1, 1);
//            worksheet.Column(1).Width = 5;
//        }
//        public void GenerateWorkSheetInformationSchema(ExcelPackage package, List<InformationSchemaModel> schemalist)
//        {
//            var heading = "INFORMATION SCHEMA";
//            var header = new List<string>() {"COLUMN_NAME","IS_NULLABLE","DATA_TYPE", "CHARACTER_MAXIMUM_LENGTH" };
//            var worksheet = package.Workbook.Worksheets.Add(heading);
//            var startRowFrom = 3;
//            var columnIndex = 1;
//            worksheet.Cells[3, 1].LoadFromCollection(schemalist, true);
//            // add the content into the Excel file 

//            // autofit width of cells with small content 
//            columnIndex = 1;
//            foreach (var column in header)
//            {
//                var maxLength = column.Length;
//                if (maxLength < 150)
//                {
//                    worksheet.Column(columnIndex).AutoFit();
//                }
//                columnIndex++;
//            }

//            // format header - bold, yellow on black 
//            using (var r = worksheet.Cells[startRowFrom, 1, startRowFrom, header.Count])
//            {
//                r.Style.Font.Color.SetColor(Color.White);
//                r.Style.Font.Bold = true;
//                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
//                r.Style.Fill.BackgroundColor.SetColor(Color.Black);
//            }

//            // format cells - add borders 
//            using (var r = worksheet.Cells[startRowFrom + 1, 1, startRowFrom + schemalist.Count, header.Count])
//            {
//                r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
//                r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//                r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
//                r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

//                r.Style.Border.Top.Color.SetColor(Color.Black);
//                r.Style.Border.Bottom.Color.SetColor(Color.Black);
//                r.Style.Border.Left.Color.SetColor(Color.Black);
//                r.Style.Border.Right.Color.SetColor(Color.Black);
//            }
//            worksheet.Cells["A1"].Value = heading;
//            worksheet.Cells["A1"].Style.Font.Size = 20;

//            worksheet.InsertColumn(1, 1);
//            worksheet.InsertRow(1, 1);
//            worksheet.Column(1).Width = 5;
//        }
        
//        public HashSet<string> GetAppliedFrame()
//        {
//            var query = @"SELECT UPPER(c.FrameNumber)
//						FROM AFIApplication a
//						LEFT JOIN AFIApplication b ON a.AFIApplicationId < b.AFIApplicationId AND a.VehicleId = b.VehicleId
//						JOIN Vehicle c ON c.VehicleId = a.VehicleId
//						WHERE b.AFIApplicationId IS NULL";
//            var result = this.LogisticDbContext.Query<string>(query).ToList();
//            return new HashSet<string>(result,StringComparer.CurrentCultureIgnoreCase);
//        }
//        public HashSet<string> GetExistedFrame()
//        {
//            var query = @"SELECT UPPER(a.FrameNumber)
//						FROM Vehicle a
//						JOIN DeliveryOrderDetail b ON a.VehicleId = b.VehicleId
//						WHERE b.CancelledAt IS NOT NULL ";
//            var result = this.LogisticDbContext.Query<string>(query).ToList();
//            return new HashSet<string>(result, StringComparer.CurrentCultureIgnoreCase);
//        }
//        public HashSet<string> GetProvinsiSet()
//        {
//            var result = this.LogisticDbContext.Region.Where(Q => Q.Type.ToUpper() == "PRVN")
//                .ToList().Select(Q => { return Q.Name = Q.Name.ToUpper();});
//            return new HashSet<string>(result, StringComparer.CurrentCultureIgnoreCase);
//        }
//        public HashSet<string> GetKotaSet()
//        {
//            var result = this.LogisticDbContext.Region.Where(Q => Q.Type.ToUpper() == "KOTA")
//                .ToList().Select(Q => { return Q.Name = Q.Name.ToUpper(); });
//            return new HashSet<string>(result, StringComparer.CurrentCultureIgnoreCase);
//        }
//        public HashSet<string> GetRegionAFISet()
//        {
//            var result = this.LogisticDbContext.AFIRegion
//                .ToList().Select(Q => { return Q.AFIRegionCode = Q.AFIRegionCode.ToUpper().TrimEnd(); });
//            return new HashSet<string>(result, StringComparer.CurrentCultureIgnoreCase);
//        }
//        public AfiUploadHashSet GetAFIHashSet()
//        {
//            return new AfiUploadHashSet()
//            {
//                AppliedFrameSet = GetAppliedFrame(),
//                ExistedFrameSet = GetExistedFrame(),
//                ProvinsiSet = GetProvinsiSet(),
//                KotaSet = GetKotaSet(),
//                RegionAFISet = GetRegionAFISet()
//            };
//        }
//        public List<string> GetFrameInExcel(List<AfiRequestUploadViewModel> data)
//        {
//            return data.Select(Q => Q.FrameNumber?.ToUpper()).ToList();
//        }
//        public bool IsRowEmpty(AfiRequestUploadViewModel data)
//        {
//            return string.IsNullOrEmpty(data.FrameNumber) && string.IsNullOrEmpty(data.CustomerName) && string.IsNullOrEmpty(data.KTP)
//                && string.IsNullOrEmpty(data.Address1) && string.IsNullOrEmpty(data.Address2) && string.IsNullOrEmpty(data.Address3)
//                && string.IsNullOrEmpty(data.Province) && string.IsNullOrEmpty(data.City) && string.IsNullOrEmpty(data.PostCode)
//                && string.IsNullOrEmpty(data.RegionAFI) && string.IsNullOrEmpty(data.Color) && string.IsNullOrEmpty(data.Chassis)
//                && data.EffectiveDate == null;
//        }
//        public void ValidateByFluent(AfiRequestUploadViewModel data, int count)
//        {
//            var validator = AfiRequestUploadValidator.Instance;
//            validator.AfiUpload = this.AfiUploadSet;
//            validator.AfiUpload.ExcelFrameList = this.ExcelFrameList.Where((v, i) => i != count).ToList();
//            var validationResult = validator.Validate(data);
//            foreach (var item in validationResult.Errors)
//            {
//                data.ErrorDescription += string.Format("-{0}\n",item.ErrorMessage);
//            }
//        }
//        public void ValidateAll(List<AfiRequestUploadViewModel> data)
//        {
//            this.AfiUploadSet = GetAFIHashSet();
//            this.ExcelFrameList = GetFrameInExcel(data);
//            var index = 0;
//            data.ForEach(Q =>
//            {
//                ValidateByFluent(Q, index++);
//            });
//        }
//        public T? TryParse<T>(string key,string value, TryParseHandler<T> handler,ref AfiRequestUploadViewModel model) where T : struct
//        {
//            if (string.IsNullOrEmpty(value))
//            {
//                model.ErrorDescription = "-'" + key + "' harus diisi\n";
//            }
//            else
//            {
//                T result;
//                if (handler(value, out result))
//                {
//                    return result;
//                }
//                else
//                {
//                    model.ErrorDescription = "-'" + key + "' salah format\n";
//                }
//            }
//            return null;
//        }

//        public bool IsTableTemplateTrue(ExcelPackage package)
//        {
//            var worksheet = package.Workbook.Worksheets.First();
//            var header = new HashSet<string>() { "Frame Number", "Nama Customer", "No Identitas", "Alamat1", "Alamat2", "Alamat3", "Provinsi", "Kota", "Kode Pos", "Region AFI", "Tanggal Efektif", "Warna", "Chassis" };
//            for (var i = 2; i <= worksheet.Dimension.End.Column; i++)
//            {
//                if (header.Contains(worksheet.Cells[4, i].Value)==false)
//                {
//                    return false;
//                }
//            }
//            return true;
//        }
//        public bool IsRowUnderLimit(ExcelPackage package)
//        {
//            var workSheet = package.Workbook.Worksheets.First();
//            if (workSheet.Dimension.End.Row > WebEnvironmentService.ExcelMaxRow)
//            {
//                return false;
//            }
//            return true;
//        }
//        public string GetMaxRowMessage()
//        {
//            return string.Format("Upload tidak boleh lebih dari {0} baris", WebEnvironmentService.ExcelMaxRow);
//        }
//        public List<AfiRequestUploadViewModel> ImportExcelToList(ExcelPackage package)
//        {
//            var worksheet = package.Workbook.Worksheets.First();
//            var dataList = new List<AfiRequestUploadViewModel>();
//            for (var i = 5; i <= worksheet.Dimension.End.Row; i++)
//            {
//                var data = new AfiRequestUploadViewModel();
//                data.FrameNumber = worksheet.Cells[i, 2].Value?.ToString()?.ToUpper();
//                data.CustomerName = worksheet.Cells[i, 3].Value?.ToString()?.ToUpper();
//                data.KTP = worksheet.Cells[i, 4].Value?.ToString()?.ToUpper();
//                data.Address1 = worksheet.Cells[i, 5].Value?.ToString()?.ToUpper();
//                data.Address2 = worksheet.Cells[i, 6].Value?.ToString()?.ToUpper();
//                data.Address3 = worksheet.Cells[i, 7].Value?.ToString()?.ToUpper();
//                data.Province = worksheet.Cells[i, 8].Value?.ToString()?.ToUpper();
//                data.City = worksheet.Cells[i, 9].Value?.ToString()?.ToUpper();
//                data.PostCode = worksheet.Cells[i, 10].Value?.ToString()?.ToUpper();
//                data.RegionAFI = worksheet.Cells[i, 11].Value?.ToString()?.ToUpper();
//                data.Chassis = worksheet.Cells[i, 14].Value?.ToString()?.ToUpper();
//                data.EffectiveDate = TryParse<DateTimeOffset>("Tanggal Efektif",worksheet.Cells[i, 12].Value?.ToString(),DateTimeOffset.TryParse,ref data);
//                data.Color = worksheet.Cells[i, 13].Value?.ToString()?.ToUpper();
//                if (IsRowEmpty(data) == false)
//                {
//                    dataList.Add(data);
//                }
//            }
//            //Validation from FluentValidation
//            ValidateAll(dataList);
//            return dataList;
//        }
//        public List<InformationSchemaModel> GetInformationSchema()
//        {
//            var schemalist = new List<InformationSchemaModel>();
//            var schema = new InformationSchemaModel()
//            {
//                COLUMN_NAME = "Frame Number",
//                CHARACTER_MAXIMUM_LENGTH = 30,
//                DATA_TYPE = "VARCHAR",
//                IS_NULLABLE = "NO",
//            };
//            schemalist.Add(schema);

//            schema = new InformationSchemaModel()
//            {
//                COLUMN_NAME = "Nama Customer",
//                CHARACTER_MAXIMUM_LENGTH = 30,
//                DATA_TYPE = "VARCHAR",
//                IS_NULLABLE = "NO",
//            };
//            schema = new InformationSchemaModel()
//            {
//                COLUMN_NAME = "No Identitas",
//                CHARACTER_MAXIMUM_LENGTH = 50,
//                DATA_TYPE = "VARCHAR",
//                IS_NULLABLE = "NO",
//            };
//            schemalist.Add(schema);
//            schema = new InformationSchemaModel()
//            {
//                COLUMN_NAME = "Alamat1",
//                CHARACTER_MAXIMUM_LENGTH = 30,
//                DATA_TYPE = "VARCHAR",
//                IS_NULLABLE = "NO",
//            };
//            schemalist.Add(schema);
//            schema = new InformationSchemaModel()
//            {
//                COLUMN_NAME = "Alamat2",
//                CHARACTER_MAXIMUM_LENGTH = 30,
//                DATA_TYPE = "VARCHAR",
//                IS_NULLABLE = "NO",
//            };
//            schemalist.Add(schema);
//            schema = new InformationSchemaModel()
//            {
//                COLUMN_NAME = "Alamat3",
//                CHARACTER_MAXIMUM_LENGTH = 30,
//                DATA_TYPE = "VARCHAR",
//                IS_NULLABLE = "NO",
//            };
//            schemalist.Add(schema);
//            schema = new InformationSchemaModel()
//            {
//                COLUMN_NAME = "Provinsi",
//                CHARACTER_MAXIMUM_LENGTH = 20,
//                DATA_TYPE = "VARCHAR",
//                IS_NULLABLE = "NO",
//            };
//            schemalist.Add(schema);
//            schema = new InformationSchemaModel()
//            {
//                COLUMN_NAME = "Kota",
//                CHARACTER_MAXIMUM_LENGTH = 30,
//                DATA_TYPE = "VARCHAR",
//                IS_NULLABLE = "NO",
//            };
//            schemalist.Add(schema);
//            schema = new InformationSchemaModel()
//            {
//                COLUMN_NAME = "Kode Pos",
//                CHARACTER_MAXIMUM_LENGTH = 5,
//                DATA_TYPE = "VARCHAR",
//                IS_NULLABLE = "NO",
//            };
//            schemalist.Add(schema);
//            schema = new InformationSchemaModel()
//            {
//                COLUMN_NAME = "Region AFI",
//                CHARACTER_MAXIMUM_LENGTH = 20,
//                DATA_TYPE = "VARCHAR",
//                IS_NULLABLE = "NO",
//            };
//            schemalist.Add(schema);
//            schema = new InformationSchemaModel()
//            {
//                COLUMN_NAME = "Tanggal Efektif",
//                CHARACTER_MAXIMUM_LENGTH = null,
//                DATA_TYPE = "DATETIMEOFFSET",
//                IS_NULLABLE = "NO",
//            };
//            schemalist.Add(schema);
//            schema = new InformationSchemaModel()
//            {
//                COLUMN_NAME = "Warna",
//                CHARACTER_MAXIMUM_LENGTH = 20,
//                DATA_TYPE = "VARCHAR",
//                IS_NULLABLE = "NO",
//            };
//            schemalist.Add(schema);
//            schema = new InformationSchemaModel()
//            {
//                COLUMN_NAME = "Chassis",
//                CHARACTER_MAXIMUM_LENGTH = 30,
//                DATA_TYPE = "VARCHAR",
//                IS_NULLABLE = "YES",
//            };
//            schemalist.Add(schema);
//            return schemalist;

//        }
        
//        public bool CheckRegionCodeAfiExists(string regionCodeAfi)
//        {
//            return this.LogisticDbContext.AFIRegion.FirstOrDefault(i => i.AFIRegionCode.ToLower() == regionCodeAfi.ToLower()) != null;
//        }

//        public bool CheckRegionExists(string regionName)
//        {
//            return this.LogisticDbContext.Region.FirstOrDefault(i => i.Name.ToLower() == regionName.ToLower()) != null;
//        }

//        public Vehicle GetVehicleByFrameNo(string frameNo)
//        {
//            return this.LogisticDbContext.Vehicle.FirstOrDefault(i => i.FrameNumber == frameNo);
//        }
//        public async Task<bool> CheckVehicleExistsAsync(int? vehicleId)
//        {
//            return await this.LogisticDbContext.AFIApplication.Where(i => i.VehicleId == vehicleId).FirstOrDefaultAsync() != null;
//        }
//        public async Task<ModelAndJenisModel> GetJenisAndModel(string FrameNumber)
//        {
//            var query = @"SELECT [Jenis] = c.Jenis, [Model]=c.Model,[VehicleId] = a.VehicleId,[AFIBranchCode] = d.AFIBranchCode
//                            FROM Vehicle a
//                            JOIN CarType b ON a.Katashiki = b.Katashiki AND a.Suffix = b.Suffix
//                            JOIN AFICarType c ON c.AFICarTypeCode = b.AFICarTypeCode
//                            JOIN AFIBranch d ON d.BranchCode = a.BranchCode
//                            WHERE FrameNumber = @FrameNumber";
//            var result = await this.LogisticDbContext.QueryAsync<ModelAndJenisModel>(query,new { FrameNumber = FrameNumber });
//            return result.FirstOrDefault();
//        }
//        public async Task<bool> InsertAfiRequest(List<AfiRequestUploadViewModel> models)
//        {
//            var afirequestlist = new List<AFIApplication>();
//            foreach(var item in models)
//            {
//                var jenismodel = await GetJenisAndModel(item.FrameNumber);
//                var afirequest = new AFIApplication()
//                {
//                    VehicleId = jenismodel.VehicleId,
//                    Name = item.CustomerName.ToUpper(),
//                    KTP = item.KTP.ToUpper(),
//                    Address1 = item.Address1.ToUpper(),
//                    Address2 = item.Address2.ToUpper(),
//                    Address3 = item.Address3.ToUpper(),
//                    Province = item.Province.ToUpper(),
//                    City = item.City.ToUpper(),
//                    PostalCode = item.PostCode,
//                    EffectiveUntil = (DateTimeOffset)item.EffectiveDate,
//                    Timestamp = DateTimeOffset.UtcNow,
//                    AFIApplicationProcessEnumId = 1,
//                    AFISubmissionTypeEnumId = 1,
//                    Warna = item.Color,
//                    Jenis = jenismodel.Jenis,
//                    Model = jenismodel.Model,
//                    AFIBranchCode = jenismodel.AFIBranchCode,
//                    AFIRegionCode = item.RegionAFI,
//                    CreatedAt = DateTimeOffset.UtcNow,
//                    CreatedBy = WebEnvironmentService.UserHumanName,
//                    UpdatedAt = DateTimeOffset.UtcNow,
//                    UpdatedBy = WebEnvironmentService.UserHumanName
//                };
//                if(jenismodel.Model.ToUpper() == "CHASSIS")
//                {
//                    afirequest.ChassisModel = item.Chassis;
//                }
//                this.LogisticDbContext.AFIApplication.Add(afirequest);
//            }
//            await this.LogisticDbContext.AddRangeAsync(afirequestlist);
//            return await this.LogisticDbContext.SaveChangesAsync()==models.Count;
//        }
//    }
//}
