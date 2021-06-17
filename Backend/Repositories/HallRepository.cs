using System.Threading.Tasks;
using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class HallsRepository : IHallRepository
    {
        private readonly DataContext context;

        public HallsRepository(DataContext context)
        {
            this.context = context;
        }


        public async Task<Hall> UpdateAsync(Hall hall)
        {
            var result = context.Halls.Update(hall);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Hall> GetAsync(int id)
        {
            return await context.Halls.FindAsync(id);
        }

        public async Task<Hall> GetAsync(string letterCode)
        {
            return await context.Halls.Include(h => h.Performances).FirstOrDefaultAsync(h => h.HallLetter == letterCode);
        }
    }
}