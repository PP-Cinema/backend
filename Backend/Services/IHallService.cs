using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public interface IHallService
    {
        Task<IActionResult> GetAllAsync();
    }
}