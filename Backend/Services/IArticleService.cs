using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public interface IArticleService
    {
        Task<IActionResult> CreateAsync(string title, string articleAbstract, IFormFile file, HttpRequest request);
        Task<IActionResult> GetAsync(int id);
        Task<IActionResult> GetAllAsync();
    }
}