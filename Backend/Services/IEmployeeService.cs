using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public interface IEmployeeService
    {
        Task<IActionResult> CreateAsync(string login, string password, bool isAdmin);
        Task<IActionResult> AuthenticateAsync(string login, string password);
        Task<IActionResult> RefreshAsync(string accessToken, string refreshToken);
        Task<string> GetRolesAsync(string login);
    }
}