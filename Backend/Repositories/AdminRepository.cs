using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext context;

        public AdminRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Admin> AddAsync(Admin admin)
        {
            var result = await context.Admins.AddAsync(admin);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Admin> UpdateAsync(Admin admin)
        {
            var result = context.Admins.Update(admin);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Admin> GetAsync(int id)
        {
            return await context.Admins.FindAsync(id);
        }

        public async Task<Admin> GetAsync(string login)
        {
            return await context.Admins.FirstOrDefaultAsync(a =>
                a.Employee.Id == context.Employees.FirstOrDefault(e => e.Login == login).Id
            );
        }
    }
}