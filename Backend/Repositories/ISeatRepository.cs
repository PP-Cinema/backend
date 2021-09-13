using System.Threading.Tasks;
using Backend.Entities;

namespace Backend.Repositories
{
    public interface ISeatRepository
    {
        Task<Seat> AddAsync(Seat seat);
        
        Task<Seat> UpdateAsync(Seat seat);
        
        Task<bool> DeleteAsync(int id);
        
        Task<Seat> GetAsync(int id);
    }
}