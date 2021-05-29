using System.Linq;
using System.Threading.Tasks;
using Backend.Entities;
using Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DataContext context;

        public MovieRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Movie> AddAsync(Movie movie)
        {
            var result = await context.Movies.AddAsync(movie);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Movie> UpdateAsync(Movie movie)
        {
            var result = context.Movies.Update(movie);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Movie> GetAsync(int id)
        {
            return await context.Movies.FindAsync(id);
        }

        public async Task<Movie> GetAsync(string title)
        {
            return await context.Movies.FirstOrDefaultAsync(m=>m.Title == title);
        }

        public async Task<Movie[]> GetContainsAsync(string title)
        {
            return await context.Movies.Where(m => m.Title.Contains(title)).ToArrayAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var movieToDelete = await context.Movies.FindAsync(id);
            if (movieToDelete == null) return false;
            context.Movies.Remove(movieToDelete);
            await context.SaveChangesAsync();
            return true;
        }
    }
}