using System.Threading.Tasks;
using Backend.Entities;

namespace Backend.Repositories
{
    public interface IPerformanceRepository
    {
        Task<Performance> AddAsync(Performance performance);
        Task<Performance> UpdateAsync(Performance performance);
        Task<Performance> GetAsync(int id);
        Task<Performance> GetAsync(string movie);
        Task<bool> DeleteAsync(int id);
    }
}