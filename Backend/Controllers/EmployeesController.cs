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
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }


        [HttpPost]
        [Authorize(Roles = "AdminEmployee")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateAsync([FromBody] EmployeeDto employeeDto)
        {
            return await employeeService.CreateAsync(employeeDto.Login, employeeDto.Password, employeeDto.IsAdmin);
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthenticationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AuthenticateAsync([FromBody] LoginDto loginDto)
        {
            return await employeeService.AuthenticateAsync(loginDto.Login, loginDto.Password);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthenticationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshDto refreshDto)
        {
            return await employeeService.RefreshAsync(refreshDto.AccessToken, refreshDto.RefreshToken);
        }
    }
}