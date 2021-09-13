using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public interface IPerformanceService
    {
        Task<IActionResult> CreateAsync(
            DateTime time, float normalPrice, float discountedPrice, string hall, string movie);
        Task<IActionResult> GetAsync(int id);
        Task<IActionResult> GetAllAsync();
        Task<IActionResult> DeleteAsync(int id);
    }
}