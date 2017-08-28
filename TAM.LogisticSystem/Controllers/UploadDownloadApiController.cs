using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using Hangfire;
using System.Data;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Services
{
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    [Route("api/v1/[controller]")]
    public class UploadDownloadApiController : Controller
    {
       // private readonly UploadDownloadService UploadDownloadService;
       // private readonly WebEnvironmentService Env;
       // private readonly IExcelPackageExtension ExcelPackageExtensionService;
       // private readonly IExcelExportHelperService ExcelExportHelperService;

       // public UploadDownloadApiController(UploadDownloadService UploadDownloadService, WebEnvironmentService Env, IExcelExportHelperService ExcelExportHelperService, IExcelPackageExtension ExcelPackageExtensionService)
       // {
       //     this.UploadDownloadService = UploadDownloadService;
       //     this.Env = Env;
       //     this.ExcelPackageExtensionService = ExcelPackageExtensionService;
       //     this.ExcelExportHelperService = ExcelExportHelperService;
       // }
        
       //[Route("GetColumnDate/{master}")]
       //[HttpGet]
       // public async Task<IActionResult> GetColumnDate(string master)
       // {
       //     var Data = await UploadDownloadService.GetColumnDateAsync(master);
       //     return Ok(Data);
       // }
       // /// <summary>
       // /// Get Template Data by Master
       // /// </summary>
       // /// <returns></returns>
       // [Route("Process/{master}/{title}")]
       // [HttpGet]
       // public FileContentResult Process(string master, string title)
       // {

       //     var Schema = UploadDownloadService.GetSchemaAsync(master).Result;
       //     var Data = ExcelExportHelperService.ListHeaderDataTable(Schema);
       //     var schemaDatatable = ExcelExportHelperService.ListToDataTable(Schema);
       //     var filecontent = ExcelExportHelperService.ExportExcel(Data, schemaDatatable, title);
       //     return File(filecontent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("{0}Template_{1:ddMMyyyyHHmmss}.xlsx", title.Replace(" ", string.Empty), DateTime.Now.ToLocalTime()));
       // }

       // /// <summary>
       // /// 
       // /// </summary>
       // /// <param name="master"></param>
       // /// <returns></returns>
       // [Route("DataByMaster/{master}")]
       // [HttpGet]
       // public IActionResult DataByMaster(string master)
       // {
       //     dynamic Data = UploadDownloadService.GetData(master);
       //     return Ok(Data.Result);
       // }

       // [Route("CheckTable/{master}")]
       // [HttpGet]
       // public IActionResult CheckTable(string master)
       // {
       //     var IsTableExists = UploadDownloadService.CheckTable(master);
       //     return Ok(IsTableExists);
       // }
       // [Route("FilterByDate/{master}")]
       // [HttpPost]
       // public async Task<IActionResult> FilterByDate(string master, [FromBody]FilterDateModel model)
       // {
       //     var Data = await UploadDownloadService.GetDataByFilter(master, model);
       //     return Ok(Data);
       // }

       // /// <summary>
       // /// Bring Data from Master Page to DownloadPage (bring PK)
       // /// </summary>
       // /// <returns></returns>
       // [Route("Filter/{master}")]
       // [HttpPost]
       // public async Task<IActionResult> Filter(string master, [FromBody]UploadDownloadPrimay model)
       // {
       //     var primaryKey = await UploadDownloadService.GetPrimaryKeyByTable(master);
       //     var Data = await UploadDownloadService.GetDataByFilter(master, primaryKey, model);
       //     return Ok(Data);
       // }

       // [Route("DownloadById/{id}/{fileName}")]
       // [HttpGet]
       // public FileResult DownloadById(string id, string fileName)
       // {
       //     var data = UploadDownloadService.GetLogBlob(int.Parse(id));
       //     return File(data, "application/vnd.ms-excel", fileName);
       // }

       // [Route("Download/{master}/{title}")]
       // [HttpPost]
       // public IActionResult Download(string master, string title, [FromBody]JsonStringModel model)
       // {
       //     var logId = 0;
       //     var jsonObject = JsonConvert.DeserializeObject(model.jsonstring);
       //     BackgroundJob.Enqueue(() => UploadDownloadService.DownloadAndInsertLog(jsonObject, master, title, logId));
       //     return Ok("Silahkan Cek Log pada tombol Status Log");
       // }
       // [Route("Upload/{master}")]
       // [HttpPost]
       // public IActionResult Upload(string master, IFormFile file)
       // {
       //     if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
       //     {
       //         return BadRequest("Silahkan Upload file .xls, .xlsx, .csv!");
       //     }
       //     var package = new ExcelPackage(file.OpenReadStream());

       //     var schema = UploadDownloadService.GetSchema(master);

       //     if (!ExcelPackageExtensionService.IsTableTrue(package, schema))
       //     {
       //         return BadRequest("Silahkan Upload file sesuai template");
       //     }
       //     if (!ExcelPackageExtensionService.IsRowUnderLimit(package, Env.ExcelMaxRow))
       //     {
       //         return BadRequest("Tidak dapat Upload lebih dari " + Env.ExcelMaxRow + " baris");
       //     }
       //     if (file.Length > 10000000)
       //     {
       //         return BadRequest("File harus dibawah 10 Mb");
       //     }
       //     var dt = ExcelPackageExtensionService.toDataTable(package, master, schema);
       //     return Ok(dt);

       // }

       // [Route("SaveUpload/{master}/{title}")]
       // [HttpPost]
       // public IActionResult SaveUpload(string master, string title, [FromBody]JsonStringModel model)
       // {
       //     var jsonObject = JsonConvert.DeserializeObject(model.jsonstring);
       //     var logId = 0;
       //     BackgroundJob.Enqueue(() => UploadDownloadService.SaveUpload(master, title, logId, jsonObject));
       //     return Ok("Berhasil menyimpan data");
       // }
    }
}
