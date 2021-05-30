using System.Threading.Tasks;
using Backend.Entities;

namespace Backend.Repositories
{
    public interface IHallsRepository
    {
        Task<Hall> GetAsync(int id);
        Task<Hall> GetAsync(string letterCode);
    }
}