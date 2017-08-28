using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Entities;
using Microsoft.AspNetCore.Authorization;

namespace TAM.LogisticSystem.Services
{
    [Authorize]
    public class AfiDownloadService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvService;

        public AfiDownloadService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvService = webEnvService;
        }
        
        public async Task<List<AfiBranchViewModel>> GetAllBranch()
        {
            _ = nameof(AFIBranch.AFIBranchCode);
            _ = nameof(Branch.BranchCode);
            _ = nameof(Branch.Name);
            var query = @"SELECT [AFIBranchCode] = a.AFIBranchCode,[BranchCode]=b.BranchCode,[Name]=b.Name
                            FROM AFIBranch a
                            JOIN Branch b ON a.BranchCode = b.BranchCode";
            var result = await this.LogisticDbContext.QueryAsync<AfiBranchViewModel>(query);
            return result;
        }
        public string GenerateApplicationNumberRevisi(int nomorUrut,string tipePengajuan, string frameNo)
        {
            string[] romawi = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII" };
            return string.Format("{0:D3}/{1}/{2}/{3:yyyy}", nomorUrut, tipePengajuan, romawi[DateTime.Now.Month - 1], DateTime.Now);

        }
        public string GenerateApplicationNumberExCancel(int nomorUrut, string BranchAS400, string frameNo)
        {
            var nomorRangka = frameNo.Substring(4, 4);
            return string.Format("{0,4}/{1:D5}.R/{2}/{3:yyyy}", BranchAS400, nomorUrut, nomorRangka, DateTime.Now);

        }
        public string GenerateApplicationNumberNormal(int nomorUrut, string BranchAS400, string frameNo)
        {
            var nomorRangka = frameNo.Substring(4, 4);
            return string.Format("{0,4}/{1:D5}/{2}/{3:yyyy}", BranchAS400, nomorUrut, nomorRangka, DateTime.Now);
        }

        public int GetNomorUrut(string branchCodeAFI)
        {
            var sequential = this.LogisticDbContext.AFIApplicationNumber.LastOrDefault(Q => Q.AFIBranchCode == branchCodeAFI && Q.Year == DateTimeOffset.UtcNow.Year);
            if (sequential == null)
            {
                InsertSequential(branchCodeAFI, 1);
                return 1;
            }
            sequential.SequentialNumber = sequential.SequentialNumber + 1;
            this.LogisticDbContext.SaveChanges();
            return sequential.SequentialNumber+1;
        }

        public void InsertSequential(string afibranchcode,int sequentialNumber)
        {
            var sequential = new AFIApplicationNumber()
            {
                AFIBranchCode = afibranchcode,
                Year = DateTimeOffset.UtcNow.Year,
                SequentialNumber = sequentialNumber,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = WebEnvService.UserHumanName,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = WebEnvService.UserHumanName
            };
            this.LogisticDbContext.Add(sequential);
            this.LogisticDbContext.SaveChanges();
        }

        public int GetApplicationNomorUrut(string AFIBranchCode)
        {
            _ = nameof(AFIApplication.AFIApplicationId);
            var query = @"SELECT COUNT(a.AFIApplicationId) + 1
                        FROM AFIApplication a
                        WHERE YEAR(a.[Timestamp]) = YEAR(GETUTCDATE()) AND MONTH(a.[TimeStamp]) = MONTH(GETUTCDATE())
                        AND a.AFISubmissionTypeEnumId IN (2,3,4,5,6,7) AND a.AFIBranchCode = @AFIBranchCode";
            return this.LogisticDbContext.Query<int>(query,new { AFIBranchCode = AFIBranchCode }).FirstOrDefault();
        }

        public string GenerateFakturNumber(int nomorUrut,string branchCodeAFI, string frameNo)
        {   
            var nomorRangka = frameNo.Substring(4, 4);
            return string.Format("{0,4}/{1:D5}/{2}/{3:yyyy}", branchCodeAFI, nomorUrut, nomorRangka, DateTime.Now);
        }
        public string GetGenerateTextResult(List<AfiGridDownload> model)
        {
            var text = "";
            foreach (var data in model)
            {
                if (data.TipePengajuanName.ToUpper()== "REV.A")
                {
                    text += string.Format("{0,-22}{1:ddMMyyyy}{2,-20}{3:ddMMyyyy}", data.FrameNumber, data.TanggalEfektif, data.ReferenceNumber, data.TanggalAjuAFI);
                }
                else
                {
                    var branchafi = data.Branch.Split('-')[0].TrimEnd();
                    var regionafi = data.Region.Split('-')[0].TrimEnd();
                    text += string.Format("{0,-15}{1,-22}{2:ddMMyyyy}{3:ddMMyyyy}{4,-30}{5,-30}{6,-30}{7,-30}{8,-30}{9,-30}{10,-42}{11,-22}{12,-7}{13,-20}{14,-4}{15,-10}{16:ddMMyyyy}{17,-3}{18,-50}{19,-30}{20,-30}{21,-5}{22}",
                                    data.Katashiki, data.ApplicationNumber, data.TanggalAjuAFI, data.TanggalEfektif, data.CustomerName, data.Address1, data.Address2, data.Address3, data.City, data.Province, data.ReferenceNumber, data.FrameNumber, data.EngineNumber, data.Color, branchafi, data.DONumber, data.DODate, regionafi, data.KTP, data.ModelName, data.Model,data.PostalCode, Environment.NewLine);
                }
            }
            return text;
        }

        public int ProcessTAM(ref List<AfiGridDownload> Submission)
        {
            var row = 0;
            var Sub = Submission;
            this.LogisticDbContext.Database.CreateExecutionStrategy().Execute(()=>
            {
                var trans = this.LogisticDbContext.Database.BeginTransaction();
                {
                    Sub.ForEach(
                        data => {
                            var existingSubmission = this.LogisticDbContext.AFIApplication.Find(data.AfiApplicationId);
                            existingSubmission.AFIApplicationProcessEnumId = data.TipePengajuanName.ToUpper() == "NORMAL" ? 4 : (data.TipePengajuanName.ToUpper().Contains("REV") ? 8 : (data.TipePengajuanName.ToUpper() == ".R" ? 12 : existingSubmission.AFIApplicationProcessEnumId));
                            data.ReferenceNumber = existingSubmission.FakturNumber;
                            var appNumber = "";
                            var nomorUrut = 0;
                            var branchAFI = data.Branch.Split('-')[0].TrimEnd();
                            if (data.TipePengajuanName.ToUpper() == "NORMAL")
                            {
                                nomorUrut = GetNomorUrut(branchAFI);
                                var fakturNumber = GenerateFakturNumber(nomorUrut, branchAFI, data.FrameNumber);
                                appNumber = GenerateApplicationNumberNormal(nomorUrut, data.BranchAS400, data.FrameNumber);
                                existingSubmission.FakturNumber = fakturNumber;
                                existingSubmission.EffectiveUntil = data.TanggalEfektif;
                                existingSubmission.ApplicationNumber = appNumber;
                                //Save fakturNo & AppNumber to data for generate text
                                data.ReferenceNumber = fakturNumber;
                                data.ApplicationNumber = appNumber;
                            }else if(data.TipePengajuanName.ToUpper().Contains("REV"))
                            {
                                nomorUrut = GetApplicationNomorUrut(branchAFI);
                                appNumber = GenerateApplicationNumberRevisi(nomorUrut, data.TipePengajuanName, data.FrameNumber);
                                existingSubmission.ApplicationNumber = appNumber;
                                data.ApplicationNumber = appNumber;
                            }
                            else if(data.TipePengajuanName.ToUpper() == ".R")
                            {
                                var nomor = data.ReferenceNumber.Split('/');
                                nomorUrut = int.Parse(nomor[1].TrimStart(new char[] { '0' }));
                                appNumber = GenerateApplicationNumberExCancel(nomorUrut, data.BranchAS400, data.FrameNumber);
                                existingSubmission.ApplicationNumber = appNumber;
                                data.ApplicationNumber = appNumber;
                            }
                            existingSubmission.UpdatedAt = DateTimeOffset.UtcNow;
                            existingSubmission.UpdatedBy = WebEnvService.UserHumanName;
                            row += this.LogisticDbContext.SaveChanges();
                        }
                    );
                    trans.Commit();
                }
            });
            return row == Submission.Count ? 1 : 0;
            
        }

        public List<AfiGridDownload> GetSubmission(AfiDownloadSearch model)
        {
            var query = "";
            model.FrameNo = model.FrameNo != null ?( model.FrameNo.Trim() != "" ? model.FrameNo :null ) : null;
            _ = nameof(AFIApplication.AFIApplicationId);
            _ = nameof(DeliveryOrderDetail.IssuedDate);
            _ = nameof(DeliveryOrderDetail.VehicleId);
            _ = nameof(AFIApplication.VehicleId);
            _ = nameof(Vehicle.FrameNumber);
            _ = nameof(CarModel.Name);
            _ = nameof(AFIApplication.Warna);
            _ = nameof(AFIApplication.Jenis);
            _ = nameof(AFIApplication.Model);
            _ = nameof(AFIApplication.ChassisModel);
            _ = nameof(AFIApplication.ApplicationNumber);
            _ = nameof(AFIApplication.AFIBranchCode);
            _ = nameof(Branch.Name);
            _ = nameof(Branch.AS400BranchCode);
            _ = nameof(AFIApplication.Name);
            _ = nameof(AFIApplication.KTP);
            _ = nameof(AFIApplication.Address1);
            _ = nameof(AFIApplication.Address2);
            _ = nameof(AFIApplication.Address3);
            _ = nameof(AFIApplication.City);
            _ = nameof(AFIApplication.Province);
            _ = nameof(AFIApplication.PostalCode);
            _ = nameof(AFIApplication.EffectiveUntil);
            _ = nameof(AFIRegion.AFIRegionCode);
            _ = nameof(AFIRegion.Name);
            _ = nameof(AFIApplication.FakturNumber);
            _ = nameof(AFIApplication.Timestamp);
            _ = nameof(AFISubmissionTypeEnum.AFISubmissionTypeEnumId);
            _ = nameof(AFISubmissionTypeEnum.Name);
            _ = nameof(Vehicle.EngineNumber);
            _ = nameof(Vehicle.Katashiki);
            var con = this.LogisticDbContext.Database.GetDbConnection();
            {
                if (model.FrameNo != null)
                {
                    query = @"SELECT [AfiApplicationId] = a.AfiApplicationId,
                                [DODate]=h.IssuedDate,
                                [DONumber] =h.DeliveryOrderNumber,
	                            [VehicleId] = a.VehicleId,
                                [FrameNumber]=c.FrameNumber,
                                [ModelName]=f.Name,
                                [Color]=a.Warna,
                                [Jenis]=a.Jenis,
                                [Model]=a.Model,
                                [Chassis] = a.ChassisModel,
	                            [ApplicationNumber] = a.ApplicationNumber,
                                [Branch] = (a.AFIBranchCode+' - '+l.Name), 
                                [BranchAS400] = l.AS400BranchCode, 
	                            [CustomerName] = a.Name,
                                [KTP]=a.KTP,
                                [Address1]=a.Address1,
                                [Address2]=a.Address2,
                                [Address3]=a.Address3,
                                [City]=a.City,
                                [Province]=a.Province,
                                [PostalCode]=a.PostalCode,
                                [TanggalEfektif]=a.EffectiveUntil,
                                [Region]= (i.AFIRegionCode+' - '+i.Name)
	                            ,[ReferenceNumber]= a.FakturNumber,
	                            [TanggalAjuAFI]=a.Timestamp,
	                            [TipePengajuan] = k.AFISubmissionTypeEnumId,
                                [TipePengajuanName]= k.Name,
                                [EngineNumber]=c.EngineNumber,
                                [Katashiki] = c.Katashiki
                            FROM AfiApplication a
                            LEFT JOIN AFIApplication b ON(a.AFIApplicationId < b.AFIApplicationId AND a.VehicleId = b.VehicleId)
                            JOIN Vehicle c ON c.VehicleId = a.VehicleId
                            JOIN CarType d ON d.Katashiki = c.Katashiki AND d.Suffix = c.Suffix
                            JOIN CarSeries e ON e.CarSeriesCode = d.CarSeriesCode
                            JOIN CarModel f ON f.CarModelCode = e.CarModelCode
                            JOIN DeliveryOrderDetail h ON h.VehicleId = a.VehicleId
                            JOIN AFIRegion i ON i.AFIRegionCode = a.AFIRegionCode
                            JOIN AFIBranch j ON j.AFIBranchCode = a.AFIBranchCode
                            JOIN AFISubmissionTypeEnum k ON k.AFISubmissionTypeEnumId = a.AFISubmissionTypeEnumId
                            JOIN Branch l ON l.BranchCode = j.BranchCode
                                WHERE c.FrameNumber = @FrameNumber AND a.AFIApplicationProcessEnumId IN (3,7,11)
                                AND h.CancelledAt IS NOT NULL AND b.AFIApplicationId IS NULL";
                    return con.Query<AfiGridDownload>(query, new { FrameNumber = model.FrameNo }).ToList();
                }
                else
                {
                    query = @"SELECT [AfiApplicationId] = a.AfiApplicationId,
                                    [DODate]=h.IssuedDate,
                                    [DONumber] =h.DeliveryOrderNumber,
	                                [VehicleId] = a.VehicleId,
                                    [FrameNumber]=c.FrameNumber,
                                    [ModelName]=f.Name,
                                    [Color]=a.Warna,
                                    [Jenis]=a.Jenis,
                                    [Model]=a.Model,
                                    [Chassis] = a.ChassisModel,
	                                [ApplicationNumber] = a.ApplicationNumber,
                                    [Branch] = (a.AFIBranchCode+' - '+l.Name), 
                                    [BranchAS400] = l.AS400BranchCode, 
	                                [CustomerName] = a.Name,
                                    [KTP]=a.KTP,
                                    [Address1]=a.Address1,
                                    [Address2]=a.Address2,
                                    [Address3]=a.Address3,
                                    [City]=a.City,
                                    [Province]=a.Province,
                                    [PostalCode]=a.PostalCode,
                                    [TanggalEfektif]=a.EffectiveUntil,
                                    [Region]= (i.AFIRegionCode+' - '+i.Name)
	                                ,[ReferenceNumber]= a.FakturNumber,
	                                [TanggalAjuAFI]=a.Timestamp,
	                                [TipePengajuan] = k.AFISubmissionTypeEnumId,[TipePengajuanName]= k.Name,[EngineNumber]=c.EngineNumber,[Katashiki] = c.Katashiki
                                FROM AfiApplication a
                                LEFT JOIN AFIApplication b ON(a.AFIApplicationId < b.AFIApplicationId AND a.VehicleId = b.VehicleId)
                                JOIN Vehicle c ON c.VehicleId = a.VehicleId
                                JOIN CarType d ON d.Katashiki = c.Katashiki AND d.Suffix = c.Suffix
                                JOIN CarSeries e ON e.CarSeriesCode = d.CarSeriesCode
                                JOIN CarModel f ON f.CarModelCode = e.CarModelCode
                                JOIN DeliveryOrderDetail h ON h.VehicleId = a.VehicleId
                                JOIN AFIRegion i ON i.AFIRegionCode = a.AFIRegionCode
                                JOIN AFIBranch j ON j.AFIBranchCode = a.AFIBranchCode
                                JOIN AFISubmissionTypeEnum k ON k.AFISubmissionTypeEnumId = a.AFISubmissionTypeEnumId
                                JOIN Branch l ON l.BranchCode = j.BranchCode
                            WHERE  (@BranchCode IS NULL OR (a.AFIBranchCode = @BranchCode))
                            AND ((@TipePengajuan = 'Normal' AND a.AFISubmissionTypeEnumId  = 1)
	                            OR
	                            (@TipePengajuan = 'Revisi' AND a.AFISubmissionTypeEnumId IN(2,3,4,5,6,7))
	                            OR
	                            (@TipePengajuan = 'Ex-Cancel' AND a.AFISubmissionTypeEnumId = 8))
                            AND ((@type = 'CBU' AND (FrameNumber NOT LIKE '%MHK%' AND FrameNumber NOT LIKE '%MHF%'))
	                            OR
	                            (@type = 'MHK' AND FrameNumber LIKE '%MHK%')
	                            OR
	                            (@type = 'CKD' AND FrameNumber LIKE '%MHF%'))
                            AND ((@tanggalPengajuan IS NULL AND @sampai IS NULL)  
                            OR 
                            ((@tanggalPengajuan IS NULL AND @sampai IS NOT NULL) AND (a.Timestamp < @sampai))
                            OR
                            ((@tanggalPengajuan IS NOT NULL AND @sampai IS NULL) AND (a.Timestamp > @tanggalPengajuan))
                            OR
                            ((@tanggalPengajuan IS NOT NULL AND @sampai IS NOT NULL) AND (a.Timestamp BETWEEN @tanggalPengajuan AND @sampai)))
                            AND a.AFIApplicationProcessEnumId IN (3,7,11)
                            AND h.CancelledAt IS NOT NULL AND b.AFIApplicationId IS NULL";
                    var result =  con.Query<AfiGridDownload>(query,
                        new {
                                BranchCode =model.Branch?.AFIBranchCode,
                                tanggalPengajuan = model.TanggalPengajuan ?? null,
                                sampai = model.Sampai ?? null,
                                TipePengajuan = model.StatusPengajuan,
                                type = model.Type
                        }).Select(Q => { Q.TanggalAjuAFI = Q.TanggalAjuAFI.ToLocalTime(); return Q; }).ToList();
                    if (model.StatusPengajuan == "Revisi" && model.Revisi == "REV.A")
                       result = result.Where(i => i.TipePengajuanName == "REV.A").ToList();
                    else if (model.StatusPengajuan == "Revisi" && model.Revisi == "NonREV.A")
                        result = result.Where(i => i.TipePengajuanName != "REV.A").ToList();
                    if (model.Quantity == null)
                        return result.OrderBy(Q => Q.TanggalAjuAFI).ToList();

                    return result.OrderByDescending(Q=>Q.TanggalAjuAFI).Take((int)model.Quantity).ToList();
                }
            }
        }
    }
}
