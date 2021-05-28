using System.Threading.Tasks;
using Backend.Entities;

namespace Backend.Repositories
{
    public interface IAdminRepository
    {
        Task<Admin> AddAsync(Admin admin);
        Task<Admin> UpdateAsync(Admin admin);
        Task<Admin> GetAsync(int id);
        Task<Admin> GetAsync(string login);
    }
}