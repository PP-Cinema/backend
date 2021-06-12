using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public interface IMovieService
    {
        public Task<IActionResult> CreateAsync(string title, int length, string description, IFormFile posterFile, HttpRequest request);
        public Task<IActionResult> GetAsync(int id);
        public Task<IActionResult> GetPageAsync(int page, int itemsPerPage, string searchString);
        public Task<IActionResult> GetPageCountAsync(int itemsPerPage, string searchString);
        public Task<IActionResult> UpdateAsync(int id, string newTitle, int newLength, string newDescription);
        public Task<IActionResult> DeleteAsync(int id);
    }
}