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
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService movieService;

        public MoviesController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(typeof(Movie), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto movieDto)
        {
            return await movieService.CreateAsync(movieDto.Title, movieDto.Length, movieDto.Description, movieDto.PosterFile, Request);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetAsync(int id)
        {
            return await movieService.GetAsync(id);
        }
        
        [HttpPost("page")]
        [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetAsyncPage([FromBody] SearchDto searchDto)
        {
            return await movieService.GetPageAsync(searchDto.Page, searchDto.ItemsPerPage, searchDto.SearchString);
        }
        
        [HttpPost("page/count")]
        [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetAsyncCount([FromBody] SearchDto searchDto)
        {
            return await movieService.GetPageCountAsync(searchDto.ItemsPerPage, searchDto.SearchString);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            return await movieService.DeleteAsync(id);
        }
    }
}