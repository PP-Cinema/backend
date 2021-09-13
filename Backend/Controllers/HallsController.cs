using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DTO;
using Backend.Entities;
using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HallsController : ControllerBase
    {
        private readonly IHallService hallService;

        public HallsController(IHallService hallService)
        {
            this.hallService = hallService;
        }
        
        [HttpGet(template:"all")]
        [ProducesResponseType(typeof(IEnumerable<Hall>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetHallsAsync()
        {
            return await hallService.GetAllAsync();
        }
    }
}