using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Entities;

namespace Backend.Repositories
{
    public interface IMovieRepository
    {
        Task<Movie> AddAsync(Movie movie);
        Task<Movie> UpdateAsync(Movie movie);
        Task<Movie> GetAsync(int id);
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<Movie> GetAsync(string title);
        Task<IEnumerable<Movie>> GetContainsAsync(string title);
        Task<bool> DeleteAsync(int id);
    }
}