using System.Collections.Generic;
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
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService reservationService;

        public ReservationsController(IReservationService reservationService) =>
            this.reservationService = reservationService;

        [HttpPost]
        [ProducesResponseType(typeof(Reservation), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateReservation([FromForm]ReservationDto reservationDto)
        {
            return await reservationService.CreateAsync(
                reservationDto.Email, reservationDto.NormalTickets, reservationDto.DiscountedTickets,
                reservationDto.FirstName, reservationDto.LastName, reservationDto.Remarks,
                reservationDto.PerformanceId, reservationDto.Seats);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Reservation), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetReservation(int id)
        {
            return await reservationService.GetAsync(id);
        }
        
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            return await reservationService.DeleteAsync(id);
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(Reservation), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetAllUsersReservations(string email, string lastName)
        {
            return await reservationService.GetAllUsersReservations(email, lastName);
        }

    }
}
