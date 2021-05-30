using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public interface IMovieService
    {
        public Task<IActionResult> CreateAsync(string title, int length, string description);
        public Task<IActionResult> GetAsync(int id);
        public Task<IActionResult> UpdateAsync(int id, string newTitle, int newLength, string newDescription);
        public Task<IActionResult> DeleteAsync(int id);
    }
}