using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class AfiRequestApiController : Controller
    {

        public AfiRequestService afiRequestService;
        public AfiRequestApiController(AfiRequestService afiRequestService)
        {
            this.afiRequestService = afiRequestService;
        }

        [HttpGet]
        [Route("CheckDataByFrame/{id}")]
        public async Task<IActionResult> CheckDataByFrame(string id)
        {
            var vehicle = afiRequestService.CheckVehicleExists(id);
            if (vehicle==null)
            {
                return BadRequest("Frame Number tidak terdaftar");
            }

            if (afiRequestService.CheckVehicleInAFI(vehicle.VehicleId))
            {
                return BadRequest("Frame Number sudah terpakai. Silahkan pilih Frame Number lain");
            }

            //if (afiRequestService.CheckIsVehicleCBUAndHasFormA(id))
            //{
            //    return BadRequest("Frame Number tidak memiliki Form A");
            //}

            var Data = await afiRequestService.GetVehicle(id);
            return Ok(Data);
        }

        [HttpGet]
        [Route("GetRegionAndRegionAFI")]
        public async Task<IActionResult> GetAllRegion()
        {
            var Data = await afiRequestService.GetRegionAndRegionAFI();
            return Ok(Data);
        }

        [HttpPost]
        [Route("InsertAfi")]
        public async Task<IActionResult> InsertAfi([FromBody] AfiRequestInsertData insertData)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest("Data gagal disimpan");
            }
            var rowAffected = await afiRequestService.insertCustomerAndAFI(insertData);
            if (rowAffected == 0)
            {
                return BadRequest("Data gagal disimpan");
            }
            return Ok("Data berhasil disimpan");
        }
    }
}
