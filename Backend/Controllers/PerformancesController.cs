using System.Threading.Tasks;
using Backend.DTO;
using Backend.Entities;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerformancesController: ControllerBase
    {
        private readonly IPerformanceService performanceService;

        public PerformancesController(IPerformanceService performanceService)
        {
            this.performanceService = performanceService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(typeof(Performance), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreatePerformance(PerformanceDto performanceDto)
        {
            return await performanceService.CreateAsync(performanceDto.Date,
                performanceDto.NormalPrice, performanceDto.DiscountedPrice,performanceDto.Hall, performanceDto.Movie);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Performance), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetPerformance(int id)
        {
            return await performanceService.GetAsync(id);
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeletePerformance(int id)
        {
            return await performanceService.DeleteAsync(id);
        }
    }
}