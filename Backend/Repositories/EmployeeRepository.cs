using System.Threading.Tasks;
using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext context;

        public EmployeeRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            var result = await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Employee> UpdateAsync(Employee employee)
        {
            var result = context.Employees.Update(employee);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Employee> GetAsync(int id)
        {
            return await context.Employees.FindAsync(id);
        }

        public async Task<Employee> GetAsync(string login)
        {
            return await context.Employees.FirstOrDefaultAsync(e =>
                e.Login == login);
        }
    }
}