using System.Threading.Tasks;
using Backend.Entities;

namespace Backend.Repositories
{
    public interface IHallRepository
    {
        Task<Hall> GetAsync(int id);
        Task<Hall> GetAsync(string letterCode);
    }
}