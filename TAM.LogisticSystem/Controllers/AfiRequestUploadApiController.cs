//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using TAM.LogisticSystem.Services;
//using OfficeOpenXml;
//using System.Data;
//using TAM.LogisticSystem.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Authorization;

//// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

//namespace TAM.LogisticSystem.Controllers
//{
//    [Authorize]
//    [Route("api/v1/[controller]")]
//    public class AfiRequestUploadApiController : Controller
//    {

//        public AfiRequestUploadService AfiRequestUploadService;
//        public AfiRequestUploadApiController(AfiRequestUploadService afiRequestUploadService)
//        {
//            this.AfiRequestUploadService = afiRequestUploadService;
//        }
//        /// <summary>
//        /// Download Template
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        [Route("GetFileTemplate")]
//        [HttpGet]
//        public IActionResult GetFileTemplate()
//        {
//            var header = new List<string>() { "Frame Number", "Nama Customer", "No Identitas", "Alamat1", "Alamat2", "Alamat3", "Provinsi", "Kota", "Kode Pos", "Region AFI", "Tanggal Efektif", "Warna", "Chassis" };
//            var fileresult = this.AfiRequestUploadService.ExportExcelFromList(header);
//            return File(fileresult, "application/vnd.ms-excel", string.Format("AFIUpload_{0:yyyMMdd_HHmm}.xlsx", DateTime.Now));
//        }
//        [Route("Upload")]
//        [HttpPost]
//        public async Task<IActionResult> Upload(IFormFile file)
//        {
//            if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
//            {
//                return BadRequest("Silahkan Upload file .xlsx");
//            }
//            if (file.Length > 10000000)
//            {
//                return BadRequest("File harus dibawah 10 Mb");
//            }
//            var package = new ExcelPackage(file.OpenReadStream());
//            if (this.AfiRequestUploadService.IsTableTemplateTrue(package) == false)
//            {
//                return BadRequest("Silahkan Upload file sesuai template");
//            }
//            if (this.AfiRequestUploadService.IsRowUnderLimit(package) == false)
//            {
//                return BadRequest(this.AfiRequestUploadService.GetMaxRowMessage());
//            }
//            var afirequestdata = this.AfiRequestUploadService.ImportExcelToList(package);
//            return Ok(afirequestdata);
//        }
//        [Route("SaveAfiRequest")]
//        [HttpPost]
//        public async Task<IActionResult> SaveAfiRequest([FromBody] List<AfiRequestUploadViewModel> model)
//        {
//            var row = await this.AfiRequestUploadService.InsertAfiRequest(model);
//            if (row == false)
//            {
//                return BadRequest("Data gagal disimpan");
//            }
//            return Ok("Data berhasil disimpan");
//        }
//    }
//}
