using System.Threading.Tasks;
using Backend.Entities;

namespace Backend.Repositories
{
    public interface IShowRepository
    {
        Task<Show> AddAsync(Show show);
        Task<Show> UpdateAsync(Show show);
        Task<Show> GetAsync(int id);
        Task<Show> GetAsync(string movie);
        Task<bool> DeleteAsync(int id);
    }
}