using System.Threading.Tasks;
using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class ShowRepository: IShowRepository
    {
        private readonly DataContext context;

        public ShowRepository(DataContext context) => this.context = context;
            
        public async Task<Show> AddAsync(Show show)
        {
            var result = await context.Shows.AddAsync(show);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Show> UpdateAsync(Show show)
        {
            var result = context.Shows.Update(show);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Show> GetAsync(int id)
        {
            return await context.Shows.FindAsync(id);
        }

        public async Task<Show> GetAsync(string movie)
        {
            return await context.Shows.FirstOrDefaultAsync(show => show.Movie.Title == movie);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var showToDelete = await context.Shows.FindAsync(id);
            if (showToDelete == null) return false;
            context.Shows.Remove(showToDelete);
            await context.SaveChangesAsync();
            return true;
        }
    }
}