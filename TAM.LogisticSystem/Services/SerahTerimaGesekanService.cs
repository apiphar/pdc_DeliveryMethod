using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace TAM.LogisticSystem.Services
{
    public class SerahTerimaGesekanService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;

        public SerahTerimaGesekanService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironmentService = webEnvironmentService;
        }

        public async Task<List<SerahTerimaGesekanViewModel>> GetAllSerahTerimaGesekan()
        {
            _ = nameof(Scratch.CreatedAt);
            _ = nameof(Scratch.ScratchId);
            _ = nameof(Vehicle.FrameNumber);
            _ = nameof(Vehicle.VehicleId);
            _ = nameof(ScratchConfiguration.NumberOfScratch);
            _ = nameof(Location.LocationCode);
            _ = nameof(Location.Name);
            _ = nameof(Vehicle.Katashiki);
            _ = nameof(Vehicle.Suffix);
            _ = nameof(Location.LocationCode);
            _ = nameof(CarModel.Name);
            _ = nameof(ExteriorColor.IndonesianName);
            _ = nameof(Branch.BranchCode);
            _ = nameof(Branch.Name);
            _ = nameof(Vehicle.HasCustomer);
            _ = nameof(Vehicle.RequestedDeliveryTime);
            _ = nameof(CarSeries.CarSeriesCode);
            _ = nameof(CarType.Katashiki);
            _ = nameof(CarType.Suffix);
            _ = nameof(CarType.CarSeriesCode);
            _ = nameof(CarModel.CarModelCode);
            _ = nameof(CarSeries.CarModelCode);
            _ = nameof(ScratchConfiguration.CarModelCode);
            _ = nameof(Vehicle.ExteriorColorCode);
            _ = nameof(Scratch.LocationCode);
            _ = nameof(Vehicle.BranchCode);
            var allData = (await LogisticDbContext.Database.GetDbConnection()
                           .QueryAsync<SerahTerimaGesekanViewModel>($@"
                                SELECT  a.ScratchId, 
		                                a.VehicleId,
                                        b.FrameNumber,
		                                [TanggalGesek] = a.CreatedAt,
		                                [JumlahGesek] = f.NumberOfScratch,
		                                [Lokasi] =h.LocationCode +' - '+h.Name,
		                                b.Katashiki,
		                                b.Suffix,
		                                [ModelName] = e.Name,
		                                [Color] = g.IndonesianName,
										[Branch] = i.BranchCode + ' - ' + i.Name,
										[CustomerAssign] = b.HasCustomer,
										[RequestedPdd] = b.RequestedDeliveryTime
                                FROM Scratch a
                                JOIN Vehicle b ON a.VehicleId = b.VehicleId
                                JOIN CarType c ON c.Katashiki = b.Katashiki AND c.Suffix = b.Suffix
                                JOIN CarSeries d ON c.CarSeriesCode = d.CarSeriesCode
                                JOIN CarModel e ON e.CarModelCode = d.CarModelCode
                                JOIN ScratchConfiguration f ON f.CarModelCode = d.CarModelCode
                                JOIN ExteriorColor g ON g.ExteriorColorCode = b.ExteriorColorCode
                                JOIN [Location] h ON h.LocationCode = a.LocationCode
								JOIN Branch i ON  b.BranchCode = i.BranchCode
                                WHERE a.ScratchHandOverNumber IS NULL
")).ToList();
            return allData;

        }
        /// <summary>
        /// Insert to ScratchHandOver and Update HandOver Column in table Scratch
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public async Task<bool> InsertAndUpdateScratchHandOver(SerahTerimaGesekanInputViewModel data)
        {
            var checkedData = await LogisticDbContext.ScratchHandOver
                                       .FirstOrDefaultAsync(q => q.ScratchHandOverNumber == data.NoSurat);
            if (checkedData != null)
            {
                return false;
            }
            else
            {
                await LogisticDbContext.Database.CreateExecutionStrategy().Execute(async () =>
                {
                    using (var transaction = await LogisticDbContext.Database.BeginTransactionAsync())
                    {
                        var newScratchHandOver = new ScratchHandOver()
                        {
                            ScratchHandOverNumber = data.NoSurat,
                            Date = data.Tanggal,
                            CreatedAt = DateTimeOffset.UtcNow,
                            CreatedBy = WebEnvironmentService.UserHumanName,
                            UpdatedAt = DateTimeOffset.UtcNow,
                            UpdatedBy = WebEnvironmentService.UserHumanName

                        };
                        LogisticDbContext.ScratchHandOver.Add(newScratchHandOver);
                        await LogisticDbContext.SaveChangesAsync();
                        var scratchListUpdated = new List<Scratch>();
                        foreach (var row in data.VehicleId)
                        {
                            var selectedRow = await LogisticDbContext.Scratch.AsNoTracking().FirstOrDefaultAsync(q => q.VehicleId == row);
                            selectedRow.ScratchHandOverNumber = data.NoSurat;
                            scratchListUpdated.Add(selectedRow);
                        }
                        LogisticDbContext.Scratch.UpdateRange(scratchListUpdated);
                        await LogisticDbContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                });
                return true;
            }

        }

        /// <summary>
        /// Generate File Excel
        /// </summary>
        /// <param name="package"></param>
        /// <param name="dataModel"></param>
        /// <param name="WorksheetTitle"></param>
        public void GenerateWorkSheet(ExcelPackage package, List<SerahTerimaGesekanExcelViewModel> dataModel, string WorksheetTitle)
        {
            var worksheet = package.Workbook.Worksheets.Add(WorksheetTitle);
            //var startColFrom = 1;
            var heading = "Serah Terima Gesekan";
            var headerTitle = new List<string>() {
                "Frame Number","Tanggal Gesek","Jumlah Gesek",
                "Lokasi","Katashiki","Suffix","Model","Warna",
                "Branch","Customer Assign","Requested PDD"
            };


            // add the content into the Excel file
            //foreach (var item in headerTitle)
            //{
            //    worksheet.Cells[3, startColFrom].Value=item;
            //    startColFrom++;
            //}           
            worksheet.Cells[3,1].LoadFromCollection(dataModel, true);
            // autofit width of cells with small content 
            var columnIndex = 1;
            foreach (var dataColumn  in headerTitle)
            {
                //ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                var maxLength = dataColumn.Length;
                /*columnCells.Max(cell => cell.Value.ToString().Count());*/
                if (maxLength < 150)
                {
                    worksheet.Column(columnIndex).AutoFit();
                }
                columnIndex++;
            }

            // format header - bold, white on black 
            using (var r = worksheet.Cells[3, 1, 3, headerTitle.Count])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.Font.Bold = true;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.Black);
            }

            // format cells - add borders 
            using (var r = worksheet.Cells[3, 1, 3 + dataModel.Count, headerTitle.Count])
            {
                r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                r.Style.Border.Top.Color.SetColor(Color.Black);
                r.Style.Border.Bottom.Color.SetColor(Color.Black);
                r.Style.Border.Left.Color.SetColor(Color.Black);
                r.Style.Border.Right.Color.SetColor(Color.Black);
            }

            if (!string.IsNullOrEmpty(heading))
            {
                worksheet.Cells[1,1].Value = heading;
                worksheet.Cells[1,1].Style.Font.Size = 20;

                worksheet.InsertColumn(1, 1);
                worksheet.InsertRow(1, 1);
                worksheet.Column(1).Width = 5;
            }
        }

        /// <summary>
        /// Export Excel File
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public byte[] ExportExcel(List<SerahTerimaGesekanExcelViewModel> dataModel)
        {
            byte[] result = null;
            using (var package = new ExcelPackage())
            {
                GenerateWorkSheet(package, dataModel, "Serah Terima Gesekan");
                result = package.GetAsByteArray();
            }

            return result;
        }
    }
}
