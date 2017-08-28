using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Controllers
{
    public class FormAApiController : Controller
    {
        private FormAService formAService;

        public FormAApiController(FormAService formAService)
        {
            this.formAService = formAService;
        }

        // TIE: START
        //[HttpGet("/FormA/GetAll")]
        // public IActionResult GetAll()
        // {
        //     var Data = formAService.GetAll();
        //     return Ok(Data);
        // }

        // [HttpGet("/FormA/GetFrameNumber/{FrameNumber}")]
        // public IActionResult GetFrameNumber(string FrameNumber)
        // {
        //     var Data = formAService.GetFrameNumber(FrameNumber);
        //     return Ok(Data);
        // }

        // [HttpPost("/FormA/edit/{id}")]
        // public async Task<IActionResult> Edit(string id, [FromBody] FormAModel model)
        // {
        //     await formAService.Update(id, model);
        //     return Ok();
        // }
        // TIE: END
    }
}
