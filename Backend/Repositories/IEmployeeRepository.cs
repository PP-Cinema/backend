using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Entities;

namespace Backend.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> AddAsync(Employee employee);
        Task<Employee> UpdateAsync(Employee employee);
        Task<Employee> GetAsync(int id);
        Task<Employee> GetAsync(string login);

        Task<ICollection<Employee>> GetAllAsync();

        Task<bool> DeleteAsync(int id);
    }
}