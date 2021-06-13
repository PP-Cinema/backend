using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public interface IShowService
    {
        Task<IActionResult> CreateAsync(DateTime time, decimal price, string hall, string movie);
        Task<IActionResult> GetAsync(int id);
        public Task<IActionResult> DeleteAsync(int id);
    }
}