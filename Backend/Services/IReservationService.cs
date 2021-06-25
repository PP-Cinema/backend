using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public interface IReservationService
    {
        Task<IActionResult> CreateAsync(
            string email, int normalTickets, int discountedTickets, string FirstName, string lastName, string remarks, int performanceId);

        Task<IActionResult> GetAsync(int id);

        Task<IActionResult> DeleteAsync(int id);
        Task<IActionResult> GetAllUsersReservations(string email, string lastName);

    }
}
