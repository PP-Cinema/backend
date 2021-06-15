using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public interface IPerformanceService
    {
        Task<IActionResult> CreateAsync(
            DateTime time, float normalPrice, float discountedPrice, int length, string hall, string movie);
        Task<IActionResult> GetAsync(int id);
        public Task<IActionResult> DeleteAsync(int id);
    }
}